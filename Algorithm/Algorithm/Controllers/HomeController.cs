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
        public JsonResult InputMatrix(string rows, string columns)
        {
            int n = 0;
            int m = 0;
            try
            {
                n = Convert.ToInt32(rows);
                m = Convert.ToInt32(columns);
            }
            catch (FormatException)
            {
                Response.Write("Ups!");
            }
            if (n != m)
            {
                return Json(null);
            }
            Matrix matrix = new Matrix(n, m);
            Session["matrix"] = matrix;
            return Json(n);            
        }

        [HttpPost]
        public JsonResult ProcessMatrix(string[] array)
        {
            List<int> ints = new List<int>();
            array.ToList().ForEach(s => ints.Add(Convert.ToInt32(s)));
            ((Matrix) Session["matrix"]).Graph = ints;
            int sum = ints.Sum();

            AlgorithmCreator standartCreator = new AlgorithmCreator(new FileStream(@"B:\Graph.txt", FileMode.Open));

            IAlgorithm standartAlgorithm = standartCreator.CreateStandartAlgorithm();

            standartAlgorithm.Run();

            StreamWriter writer = new StreamWriter(@"B:\Result.txt");
            ((StandartAntAlgorithm)standartAlgorithm).Result.CopyTo(writer.BaseStream);

            writer.Write(true);
            ((StandartAntAlgorithm)standartAlgorithm).Result.Close();
            writer.Close();

            return Json(new StreamReader(((StandartAntAlgorithm)standartAlgorithm).Result).ReadToEnd());
        }
    }
}
