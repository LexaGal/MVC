using System;
using System.Collections.Generic;
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
            catch (FormatException)
            {
                return new HtmlString(null);
            }

            Matrix matrix = new Matrix(numAnts, numAnts);
            Session["matrix"] = matrix;

            AlgorithmCreator creator = new AlgorithmCreator(new FileStream(@"B:\Graph.txt", FileMode.Open));
            IAlgorithm algorithm = creator.CreateStandartAlgorithm();
            
            ((Graph) algorithm.Graph).Info = new Tuple<int, int, int, int, int>(pheromInc,
                extraPheromInc, numAnts, noUpdatesLim, numIter);
            
            Session["algorithm"] = algorithm;
            
            StringBuilder builder = new StringBuilder().AppendFormat("{0} ", numAnts);
            for (int i = 0; i < numAnts; i++)
            {
                for (int j = 0; j < numAnts; j++)
                {
                    var elem = ((Graph) algorithm.Graph).DistanceMatrix[i, j];
                    builder.AppendFormat("{0},", elem);
                }
            }
            builder.Append(" ");
            for (int i = 0; i < numAnts; i++)
            {
                for (int j = 0; j < numAnts; j++)
                {
                    var elem = ((Graph) algorithm.Graph).FlowMatrix[i, j];
                    builder.AppendFormat("{0},", elem);
                }
            }
            return new HtmlString(builder.ToString());            
        }

        [HttpPost]
        public HtmlString ProcessMatrix(string[] array)
        {
            List<int> ints = new List<int>();
            array.ToList().ForEach(s => ints.Add(Convert.ToInt32(s)));
            List<List<int>> lists = ints
                .Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index/(ints.Count/2))
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
            StandartAntAlgorithm algorithm = (StandartAntAlgorithm) Session["algorithm"];
            algorithm.Run();
            string result = algorithm.ResultBuilder.ToString();
            return new HtmlString(result);
        }
    }
}
