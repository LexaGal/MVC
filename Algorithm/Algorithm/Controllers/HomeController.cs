using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Algorithm.Models;
using AntsLibrary.Classes;
using Grsu.Lab.Aoc.Contracts;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;

namespace Algorithm.Controllers
{
    public class HomeController : Controller
    {
        private IAlgorithm _algorithm;

        public HomeController(IAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

        public List<List<int>> SplitListIntoLists(List<int> ints, int number)
        {
            return ints.Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index/number)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public int[] ConvertMatrixToArray(int n, int[,] ints)
        {
            List<int> lineInts = new List<int>();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    var elem = ints[i, j];
                    lineInts.Add(elem);
                }
            }
            return lineInts.ToArray();
        }

        public void SetGraphMatrices(IAlgorithm algorithm, Matrix matrix)
        {
            algorithm.Graph.Edges.Clear();
            
            int n = algorithm.Graph.Nodes.Count;
            int pheromone = ((StandartAntAlgorithm) algorithm).Pheromone;

            for (int i = 0; i < matrix.DistGraph.Count(); i++)
            {
                var elem = matrix.DistGraph[i];
                ((Graph) algorithm.Graph).DistanceMatrix[i/n, i%n] = elem;
                algorithm.Graph.Edges.Add(new Edge
                {
                    Begin = i/n,
                    End = i%n,
                    HeuristicInformation = elem,
                    Pheromone = pheromone
                });
            }
            for (int i = 0; i < matrix.FlowGraph.Count(); i++)
            {
                var elem = matrix.FlowGraph[i];
                ((Graph) algorithm.Graph).FlowMatrix[i/n, i%n] = elem;
            }
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
                        StandartAntAlgorithm algorithm = (StandartAntAlgorithm) creator.CreateStandartAlgorithm();
                        Session["algorithm"] = algorithm;
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
        public JsonResult InputMatrix(InputData model)
        {
            if (Session["algorithm"] == null)
            {
                AlgorithmCreator creator = new AlgorithmCreator(new FileStream(
                    ConfigurationManager.AppSettings["Graphs"], FileMode.Open));
                IAlgorithm algorithm = creator.CreateStandartAlgorithm();
                Session["algorithm"] = algorithm;
            }

            else
            {
                int pheromInc;
                int extraPheromInc;
                int numAnts;
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

                Graph graph = new Graph
                {
                    Info = new Tuple<int, int, int, int, int>(pheromInc,
                        extraPheromInc, numAnts, noUpdatesLim, numIter)
                };
                graph.SetValues(numAnts);
                Session["graph"] = graph;

                Matrix matrix = new Matrix(numAnts);
                
                Graph currGraph = ((Graph)((StandartAntAlgorithm) Session["algorithm"]).Graph);
                matrix.DistGraph = ConvertMatrixToArray(numAnts, currGraph.DistanceMatrix);
                matrix.FlowGraph = ConvertMatrixToArray(numAnts, currGraph.FlowMatrix);
                
                return Json(matrix);
            }
            return Json(null);
        }

        [HttpPost]
        public HtmlString ProcessMatrix(Matrix matrix)
        {
            if (matrix == null)
            {
                return null;
            }

            StandartAntAlgorithm algorithm = (StandartAntAlgorithm) Session["algorithm"];
            StandartAntAlgorithm savedAlgorithm = (StandartAntAlgorithm)StandartAntAlgorithm.DeepObjectClone(algorithm);

            algorithm.Graph = (Graph)Session["graph"];
            SetGraphMatrices(algorithm, matrix);
            
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
