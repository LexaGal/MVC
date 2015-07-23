using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Algorithm.HelpMethods;
using Algorithm.Models;
using Algorithm.Repository;
using Algorithm.Unity;
using AntsLibrary.Classes;
using Grsu.Lab.Aoc.Contracts;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;

namespace Algorithm.Controllers
{
    public class HomeController : Controller
    {
        private IAlgorithm _algorithm;
        private IRepository _repository;

        public HomeController(IAlgorithm algorithm, IRepository repository)
        {
            _algorithm = algorithm;
            _repository = repository;
        }


        public ActionResult Index(InputData input)
        {
            if (input == null)
            {
                return View(new InputData());
            }
            return View(input);
        }


        [HttpPost]
        public ActionResult LoadFile(HttpPostedFileBase file)
        {
            InputData input = null;
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        AlgorithmCreator creator = new AlgorithmCreator(file.InputStream);
                        _algorithm = (AntAlgorithm) creator.CreateStandartAlgorithm();
                        AntAlgorithm algorithm = (AntAlgorithm) _algorithm;
                        Session["algorithm"] = _algorithm;
                        input = new InputData
                        {
                            PheromoneIncrement = algorithm.Pheromone.ToString(),
                            ExtraPheromoneIncrement = algorithm.ExtraPheromone.ToString(),
                            AntsNumber = algorithm.Graph.Nodes.Count.ToString(),
                            NoUpdatesLimit = algorithm.MaxIterationsNoChanges.ToString(),
                            IterationsNumber = algorithm.MaxIterations.ToString(),
                        };
                    }
                }
            }
            return RedirectToAction("Index", input);
        }


        [HttpPost]
        public JsonResult GetFiles()
        {
            return Json(new
            {
                names = _repository.GetAll("Names"),
                values = _repository.GetAll("Values")
            });
        }


        [HttpPost]
        public JsonResult InputMatrix(InputData model, string name = null)
        {
            int numAnts = 0;
            AlgorithmCreator creator;
            MemoryStream memoryStream;
            Stream stream;

            if (model.GetType().GetProperties().Any(p => p.GetValue(model) == null) && name == null)
            {
                memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(_repository.GetAll("Values").First()));
                stream = Stream.Synchronized(memoryStream);
                creator = new AlgorithmCreator(stream);
                _algorithm = creator.CreateStandartAlgorithm();
                Session["graph"] = _algorithm.Graph;
                numAnts = _algorithm.Graph.Nodes.Count;
            }

            else if (name != null)
            {
                memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(_repository.Get(name)));
                stream = Stream.Synchronized(memoryStream);
                creator = new AlgorithmCreator(stream);
                _algorithm = creator.CreateStandartAlgorithm();
                Session["graph"] = _algorithm.Graph;
                numAnts = _algorithm.Graph.Nodes.Count;
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
                memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(_repository.GetAll("Values").First()));
                stream = Stream.Synchronized(memoryStream);
                creator = new AlgorithmCreator(stream);
                _algorithm = creator.CreateStandartAlgorithm();

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
            Matrix matrix = new Matrix(numAnts);
            Graph currGraph = ((Graph) ((AntAlgorithm) Session["algorithm"]).Graph);
            matrix.DistGraph = Helper.ConvertMatrixToArray(numAnts, currGraph.DistanceMatrix);
            matrix.FlowGraph = Helper.ConvertMatrixToArray(numAnts, currGraph.FlowMatrix);
            return Json(matrix);
        }


        [HttpPost]
        public HtmlString ProcessMatrix(Matrix matrix)
        {
            if (matrix == null || Session["algorithm"] == null || Session["graph"] == null)
            {
                return new HtmlString(null);
            }

            _algorithm = (AntAlgorithm) Session["algorithm"];
            AntAlgorithm savedAlgorithm = (AntAlgorithm)AntAlgorithm.DeepObjectClone(_algorithm);

            AntAlgorithm algorithm = (AntAlgorithm) _algorithm;
            algorithm.Graph = (Graph)Session["graph"];
            Helper.SetGraphMatrices(algorithm, matrix);
            
            algorithm.CurrentIteration = 0;
            algorithm.CurrentIterationNoChanges = 0;
            algorithm.BestAnt = new Ant {PathCost = int.MaxValue, VisitedNodes = algorithm.Graph.Nodes};
            
            algorithm.Run();
            string result = algorithm.ResultBuilder.ToString();

            Session["algorithm"] = savedAlgorithm;

            return new HtmlString(result);
        }
    }
}
