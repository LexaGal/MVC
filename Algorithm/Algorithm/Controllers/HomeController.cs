using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Algorithm.DomainModels;
using Algorithm.HelpMethods;
using Algorithm.Models;
using Algorithm.Repository.Abstract;
using Algorithm.Repository.Concrete;
using AntsLibrary.Classes;
using Grsu.Lab.Aoc.Contracts;

namespace Algorithm.Controllers
{
    public class HomeController : Controller
    {
        private IAlgorithm _algorithm;
        private IParametersRepository _parametersRepository;
        private IDistMatricesRepository _distMatricesRepository;
        private IFlowMatricesRepository _flowMatricesRepository;
        private IResultsInfoRepository _resultsInfoRepository;
       
        public HomeController(IAlgorithm algorithm,
            IParametersRepository parametersRepository,
            IDistMatricesRepository distMatricesRepository,
            IFlowMatricesRepository flowMatricesRepository,
            IResultsInfoRepository resultsInfoRepository)
        {
            _algorithm = algorithm;
            _flowMatricesRepository = flowMatricesRepository;
            _distMatricesRepository = distMatricesRepository;
            _parametersRepository = parametersRepository;
            _resultsInfoRepository = resultsInfoRepository;
            Parameters p = _parametersRepository.Get(1);
        }


        public ActionResult Index(InputParametersViewModel inputParameters)
        {
            if (inputParameters == null)
            {
                return View(new InputParametersViewModel());
            }
            return View(inputParameters);
        }

        public ActionResult DatabasePage(GraphViewModel graphViewModel)
        {
            if (graphViewModel == null)
            {
                return View(new GraphViewModel());
            }
            return View(graphViewModel);
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
                    AlgorithmCreator creator = new AlgorithmCreator(file.InputStream);
                    _algorithm = (AntAlgorithm) creator.CreateStandartAlgorithm();
                    AntAlgorithm algorithm = (AntAlgorithm) _algorithm;
                    Session["algorithm"] = _algorithm;
                    inputParameters = new InputParametersViewModel
                    {
                        PheromoneIncrement = algorithm.Pheromone.ToString(),
                        ExtraPheromoneIncrement = algorithm.ExtraPheromone.ToString(),
                        AntsNumber = algorithm.Graph.Nodes.Count.ToString(),
                        NoUpdatesLimit = algorithm.MaxIterationsNoChanges.ToString(),
                        IterationsNumber = algorithm.MaxIterations.ToString(),
                    };
                }
            }
            return RedirectToAction("Index", inputParameters);
        }


        [HttpPost]
        public JsonResult GetParameters()
        {
            Parameters[] parameters = _parametersRepository.GetAll().ToArray();
            return Json(new { parameters });
        }

        [HttpPost]
        public HtmlString SaveGraph(string graphString, string name)
        {
            var jss = new JavaScriptSerializer();
            GraphViewModel graphViewModel = jss.Deserialize<GraphViewModel>(graphString);

            //???

            FileRepository repository = new FileRepository();
            repository.Save(name, graphViewModel.ToString());
            using (StreamWriter writer = new StreamWriter(ConfigurationManager.AppSettings["Names"]))
            {
                writer.BaseStream.Position = writer.BaseStream.Length;
                writer.WriteLine(name);
            }
            return new HtmlString("Saved");
        }


        [HttpPost]
        public JsonResult InputMatrix(InputParametersViewModel model, string name = null)
        {
            int numAnts = 0;
            AlgorithmCreator creator;
            MemoryStream memoryStream;
            Stream stream;
            FileRepository repository = new FileRepository();
            
            if (model.GetType().GetProperties().Any(p => p.GetValue(model) == null) && name == null)
            {
                using (memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(repository.GetAll().Values.First())))
                {
                    using (stream = Stream.Synchronized(memoryStream))
                    {
                        creator = new AlgorithmCreator(stream);
                        _algorithm = creator.CreateStandartAlgorithm();
                        Session["graph"] = _algorithm.Graph;
                        numAnts = _algorithm.Graph.Nodes.Count;
                    }
                }
            }

            else if (name != null)
            {
                using (memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(repository.Get(name))))
                {
                    using (stream = Stream.Synchronized(memoryStream))
                    {
                        creator = new AlgorithmCreator(stream);
                        _algorithm = creator.CreateStandartAlgorithm();
                        Session["graph"] = _algorithm.Graph;
                        numAnts = _algorithm.Graph.Nodes.Count;
                    }
                }
            }

            else if (model.GetType().GetProperties().All(p => p.GetValue(model) != null))
            {
                int pheromInc;
                int extraPheromInc;
                int noUpdatesLim;
                int numIter;
                try
                {
                    pheromInc = Convert.ToInt32(model.PheromoneIncrement);
                    extraPheromInc = Convert.ToInt32(model.ExtraPheromoneIncrement);
                    numAnts = Convert.ToInt32(model.AntsNumber);
                    noUpdatesLim = Convert.ToInt32(model.NoUpdatesLimit);
                    numIter = Convert.ToInt32(model.IterationsNumber);
                }
                catch (FormatException e)
                {
                    return Json(e.Message);
                }

                using (memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(repository.GetAll().Values.First())))
                {
                    using (stream = Stream.Synchronized(memoryStream))
                    {
                        creator = new AlgorithmCreator(stream);
                        _algorithm = creator.CreateStandartAlgorithm();
                    }
                }

                Graph graph = new Graph
                {
                    Info = new Tuple<int, int, int, int, int>(pheromInc,
                        extraPheromInc, numAnts, noUpdatesLim, numIter)
                };
                graph.SetValues(numAnts);
                Session["graph"] = graph;
                numAnts = graph.Nodes.Count;
            }

            Session["algorithm"] = _algorithm;
            GraphViewModel matrix = new GraphViewModel(numAnts);
            Graph currGraph = ((Graph) ((AntAlgorithm) Session["algorithm"]).Graph);
            matrix.DistGraph = Helper.ConvertMatrixToArray(numAnts, currGraph.DistanceMatrix);
            matrix.FlowGraph = Helper.ConvertMatrixToArray(numAnts, currGraph.FlowMatrix);
            return Json(matrix);
        }


        [HttpPost]
        public HtmlString ProcessMatrix(GraphViewModel graph)
        {
            if (graph == null || Session["algorithm"] == null || Session["graph"] == null)
            {
                return new HtmlString(null);
            }

            _algorithm = (AntAlgorithm) Session["algorithm"];
            AntAlgorithm savedAlgorithm = (AntAlgorithm)AntAlgorithm.DeepObjectClone(_algorithm);

            AntAlgorithm algorithm = (AntAlgorithm) _algorithm;
            algorithm.Graph = (Graph)Session["graph"];
            Helper.SetGraphMatrices(algorithm, graph);
            
            algorithm.CurrentIteration = 0;
            algorithm.CurrentIterationNoChanges = 0;
            algorithm.BestAnt = new Ant {PathCost = int.MaxValue, VisitedNodes = algorithm.Graph.Nodes};
            
            algorithm.Run();
            string result = algorithm.ResultBuilder.ToString();

            Session["algorithm"] = savedAlgorithm;

            return new HtmlString(result);
        }

        public HtmlString ProcessChoosenIds(string parametersId, string distMatrixId, string flowMatrixId)
        {
            Parameters parameters = _parametersRepository.Get(Convert.ToInt32(parametersId));
            DistMatrix distMatrix = _distMatricesRepository.Get(Convert.ToInt32(distMatrixId));
            FlowMatrix flowMatrix = _flowMatricesRepository.Get(Convert.ToInt32(flowMatrixId));

            string graph = string.Format("{0}{1}{2}",
                parameters.StringView, distMatrix.MatrixView, flowMatrix.MatrixView);
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(graph));
            
            AlgorithmCreator creator = new AlgorithmCreator(stream);
            _algorithm = (AntAlgorithm)creator.CreateStandartAlgorithm();
            _algorithm.Run();
                    
            HtmlString result = new HtmlString(((AntAlgorithm)_algorithm).ResultBuilder.ToString());
            int pathCost = ((Ant) (((AntAlgorithm) _algorithm).BestAnt)).PathCost;

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
    }
}
