using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Algorithm.Models
{
    public class InputParametersViewModel
    {
        public string PheromoneIncrement { get; set; }
        public string ExtraPheromoneIncrement { get; set; }
        public string AntsNumber { get; set; }
        public string NoUpdatesLimit { get; set; }
        public string IterationsNumber { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Pheromone Increment: {0}, Extra Pheromone Increment: {1}, " +
                                 "Number of Ants: {2}, No Updates Limit: {3}, Number of Iterations:" +
                                 " {4} ", PheromoneIncrement, ExtraPheromoneIncrement,
                                 AntsNumber, NoUpdatesLimit, IterationsNumber);
            return builder.ToString(); 
        }
    }
}