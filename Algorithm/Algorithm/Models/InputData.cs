using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Algorithm.Models
{
    public class InputData
    {
        public string PheromoneIncrement { get; set; }
        public string ExtraPheromoneIncrement { get; set; }
        public string AntsNumber { get; set; }
        public string NoUpdatesLimit { get; set; }
        public string IterationsNumber { get; set; }
    }
}