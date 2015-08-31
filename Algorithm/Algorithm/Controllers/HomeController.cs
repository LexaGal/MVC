using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Algorithm.Authentication;
using Algorithm.Models;
using AntsAlg.AntsAlgorithm.Algorithm;
using AntsAlg.QapAlg;
using Aop.AopAspects;
using Aop.AopAspects.Logging;
using DatabaseAccess.Repository.Abstract;
using Entities.DatabaseModels;
using Path = System.IO.Path;

namespace Algorithm.Controllers
{
    [AuthentificationAspect]
    public class HomeController : Controller
    {
        private QapAntAlgorithm _qapAntAlgorithm;
        private readonly StandartAlgorithmBuilder _standartAlgorithmBuilder;
        private readonly IParametersRepository _parametersRepository;
        private readonly IDistMatricesRepository _distMatricesRepository;
        private readonly IFlowMatricesRepository _flowMatricesRepository;
        private readonly IResultsInfoRepository _resultsInfoRepository;

        public HomeController(StandartAlgorithmBuilder algorithmBuilder,
            IParametersRepository parametersRepository,
            IDistMatricesRepository distMatricesRepository,
            IFlowMatricesRepository flowMatricesRepository,
            IResultsInfoRepository resultsInfoRepository)
        {
            _standartAlgorithmBuilder = algorithmBuilder;
            _flowMatricesRepository = flowMatricesRepository;
            _distMatricesRepository = distMatricesRepository;
            _parametersRepository = parametersRepository;
            _resultsInfoRepository = resultsInfoRepository;
        }    
     
        public ActionResult DatabaseItems(DatabaseViewModel databaseViewModel)
        {
            if (databaseViewModel.GetType().GetProperties().Any(p => p.GetValue(databaseViewModel) == null))
            {
                DatabaseViewModel database = new DatabaseViewModel
                {
                    Parameters = _parametersRepository.GetAll().ToList(),
                    DistMatrices = _distMatricesRepository.GetAll().ToList(),
                    FlowMatrices = _flowMatricesRepository.GetAll().ToList()
                };
                return View(database);
            }
            return View(databaseViewModel);
        }

        [HttpPost]
        public ActionResult LoadFile(HttpPostedFileBase file)
        {
            InputParametersViewModel inputParameters = null;
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                if (fileName != null)
                {
                    _qapAntAlgorithm = (QapAntAlgorithm) _standartAlgorithmBuilder.GetAlgorithm<QapGraph>(1, 1, 100, 50);
                    _qapAntAlgorithm.Calculate(new QapGraph().Load(file.InputStream, _qapAntAlgorithm.NAnts));
                    Session["algorithm"] = _qapAntAlgorithm;
                    inputParameters = new InputParametersViewModel
                    {
                        PheromoneIncrement = _qapAntAlgorithm.PheromoneInc.ToString(),
                        ExtraPheromoneIncrement = _qapAntAlgorithm.ExtraPheromoneInc.ToString(),
                        AntsNumber = _qapAntAlgorithm.NAnts.ToString(),
                        NoUpdatesLimit = _qapAntAlgorithm.MaxIterationsNoChanges.ToString(),
                        IterationsNumber = _qapAntAlgorithm.MaxIterations.ToString()
                    };
                }
            }
            return RedirectToAction("Index", inputParameters);
        }

        [HttpPost]
        public HtmlString ProcessMatrix(GraphViewModel graph)
        {
            if (graph == null)
            {
                return new HtmlString(null);
            }

            _qapAntAlgorithm = (QapAntAlgorithm) Session["algorithm"];

            QapAntAlgorithm savedAlgorithm = (QapAntAlgorithm) QapAntAlgorithm.DeepObjectClone(_qapAntAlgorithm);

            if (_qapAntAlgorithm == null)
            {
                _qapAntAlgorithm = (QapAntAlgorithm) _standartAlgorithmBuilder.GetAlgorithm<QapGraph>(1, 2, 100, 50);
            }

            _qapAntAlgorithm.Graph.SetGraphMatrices(_qapAntAlgorithm.NAnts, graph.DistGraph, graph.FlowGraph);
            _qapAntAlgorithm.Run();

            string result = _qapAntAlgorithm.Result;

            Session["algorithm"] = savedAlgorithm;

            return new HtmlString(result);
        }
    

        public HtmlString GetAllById(string parametersId, string distMatrixId, string flowMatrixId, string type)
        {
            IList<ResultInfo> infos = _resultsInfoRepository.GetAllById(Convert.ToInt32(parametersId),
                Convert.ToInt32(distMatrixId), Convert.ToInt32(flowMatrixId), type);
            return new HtmlString(TypeConverter.GetStringView(infos.GetType().FullName, infos));
        }

        public HtmlString ProcessChoosenItems(string parametersId, string distMatrixId, string flowMatrixId)
        {
            Parameters parameters = _parametersRepository.Get(Convert.ToInt32(parametersId));
            DistMatrix distMatrix = _distMatricesRepository.Get(Convert.ToInt32(distMatrixId));
            FlowMatrix flowMatrix = _flowMatricesRepository.Get(Convert.ToInt32(flowMatrixId));

            Stream paramStream = new MemoryStream(Encoding.UTF8.GetBytes(parameters.StringView));
            string graph = string.Format("{0}\n{1}", distMatrix.MatrixView, flowMatrix.MatrixView);
            Stream graphStream = new MemoryStream(Encoding.UTF8.GetBytes(graph));

            _qapAntAlgorithm = _standartAlgorithmBuilder.GetAlgorithm<QapGraph>(paramStream);
            QapGraph qapGraph = new QapGraph().Load(graphStream, _qapAntAlgorithm.NAnts);
            _qapAntAlgorithm.Calculate(qapGraph);

            HtmlString result = new HtmlString(_qapAntAlgorithm.Result);
            int pathCost = _qapAntAlgorithm.BestAnt.PathCost;

            ResultInfo resultInfo = new ResultInfo
            {
                ParametersId = parameters.Id,
                DistMatrixId = distMatrix.Id,
                FlowMatrixId = flowMatrix.Id,
                Result = result.ToString(),
                PathCost = pathCost
            };

            _resultsInfoRepository.Add(resultInfo);
            return result;
        }

        public HtmlString SaveChoosenItem(string type, string text)
        {
            if (type == "Parameters")
            {
                Parameters parameters = new Parameters(text);
                _parametersRepository.Add(parameters);
                return new HtmlString("Saved");
            }
            if (type == "DistMatrix")
            {
                DistMatrix distMatrix = new DistMatrix(text);
                _distMatricesRepository.Add(distMatrix);
                return new HtmlString("Saved");
            }
            if (type == "FlowMatrix")
            {
                FlowMatrix flowMatrix = new FlowMatrix(text);
                _flowMatricesRepository.Add(flowMatrix);
                return new HtmlString("Saved");
            }
            return new HtmlString("Not saved");
        }

        [LogAspect]
        [AuthentificationAspect(AttributeExclude = true)]
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            if (filterContext.Exception.GetType() == typeof(AuthenticationException))
            {
                Response.RedirectPermanent("~/Auth/LogIn");
            }
        }

        public ActionResult Index(InputParametersViewModel inputParametersViewModel)
        {
            return View();
        }
    }
}
