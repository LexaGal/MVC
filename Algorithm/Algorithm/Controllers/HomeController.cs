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

namespace Algorithm.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new Matrix());
        }

        public List<List<int>> SplitList(List<int> ints, int number)
        {
            return ints.Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index/number)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public string JoinIntoString(int numAnts, IAlgorithm algorithm)
        {
            StringBuilder builder = new StringBuilder().AppendFormat("{0} ", numAnts);
            for (int i = 0; i < numAnts; i++)
            {
                for (int j = 0; j < numAnts; j++)
                {
                    var elem = ((Graph)algorithm.Graph).DistanceMatrix[i, j];
                    builder.AppendFormat("{0},", elem);
                }
            }
            builder.Append(" ");
            for (int i = 0; i < numAnts; i++)
            {
                for (int j = 0; j < numAnts; j++)
                {
                    var elem = ((Graph)algorithm.Graph).FlowMatrix[i, j];
                    builder.AppendFormat("{0},", elem);
                }
            }
            return builder.ToString();
        }

        public void SetGraphMatrices(IAlgorithm algorithm, List<List<int>> lists, int n, int pheromone)
        {
            algorithm.Graph.Edges.Clear();
            for (int i = 0; i < lists[0].Count; i++)
            {
                var elem = lists[0][i];
                ((Graph)algorithm.Graph).DistanceMatrix[i / n, i % n] = elem;
                algorithm.Graph.Edges.Add(new Edge
                {
                    Begin = i / n,
                    End = i % n,
                    HeuristicInformation = elem,
                    Pheromone = pheromone
                });
            }
            for (int i = 0; i < lists[1].Count; i++)
            {
                var elem = lists[1][i];
                ((Graph)algorithm.Graph).FlowMatrix[i / n, i % n] = elem;
            }
        }

        [HttpPost]
        public HtmlString InputMatrix(string pherInc, string extPherInc, string nAnts, string nIter, string noUpdLim)
        {
            int pheromInc;
            int extraPheromInc;
            int numAnts;
            int noUpdatesLim;
            int numIter;
            try
            {
                pheromInc = Convert.ToInt32(pherInc);
                extraPheromInc = Convert.ToInt32(extPherInc);
                numAnts = Convert.ToInt32(nAnts);
                noUpdatesLim = Convert.ToInt32(noUpdLim);
                numIter = Convert.ToInt32(nIter);
            }
            catch (FormatException e)
            {
                return new HtmlString(e.Message);
            }

            Matrix matrix = new Matrix(numAnts);
            Session["matrix"] = matrix;
            
            Graph graph = new Graph
            {
                Info = new Tuple<int, int, int, int, int>(pheromInc,
                    extraPheromInc, numAnts, noUpdatesLim, numIter)
            };
            graph.SetValues(numAnts);
            Session["graph"] = graph;

            AlgorithmCreator creator = new AlgorithmCreator(new FileStream(
                ConfigurationManager.AppSettings["Graphs"], FileMode.Open));

            IAlgorithm algorithm = creator.CreateStandartAlgorithm();
            
            Session["algorithm"] = algorithm;
            
            return new HtmlString(JoinIntoString(numAnts, algorithm));            
        }

        [HttpPost]
        public HtmlString ProcessMatrix(string[] array)
        {
            List<int> ints = new List<int>();
            try
            {
                array.ToList().ForEach(s => ints.Add(Convert.ToInt32(s)));
            }
            catch (FormatException e)
            {
                return new HtmlString(e.Message);
            }
            catch(Exception e)
            {
                return new HtmlString(e.Message);
            }

            List<List<int>> lists = SplitList(ints, array.Count() / 2);
            
            StandartAntAlgorithm algorithm = (StandartAntAlgorithm) Session["algorithm"];
            algorithm.Graph = (Graph) Session["graph"];
            int n = ((Matrix) Session["matrix"]).N;
            SetGraphMatrices(algorithm, lists, n, algorithm.Pheromone);
            algorithm.CurrentIteration = 0;
            algorithm.CurrentIterationNoChanges = 0;
            algorithm.BestAnt = new Ant {PathCost = int.MaxValue, VisitedNodes = algorithm.Graph.Nodes};
            algorithm.Run();
            string result = algorithm.ResultBuilder.ToString();
            return new HtmlString(result);
        }
    }
}
