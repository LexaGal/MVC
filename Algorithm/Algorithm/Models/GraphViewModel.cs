using System;
using System.Linq;
using System.Text;

namespace Algorithm.Models
{
    public class GraphViewModel
    {
        public InputParametersViewModel InputParameters { get; set; }
        public int[] DistGraph { get; set; }
        public int[] FlowGraph { get; set; }
        public int N { get; set; }

        public GraphViewModel()
        {}

        public GraphViewModel(int n)
        {
            N = n;
            DistGraph = new int[n];
            FlowGraph = new int[n];
        }

        public string StringView
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                if (InputParameters != null)
                {
                    builder.AppendFormat("Pheromone Increment:\n{0}\nExtra Pheromone Increment:\n{1}\n" +
                                         "Number of Ants:\n{2}\nNo Updates Limit:\n{3}\nNumber of Iterations:" +
                                         "\n{4}\n", InputParameters.PheromoneIncrement,
                        InputParameters.ExtraPheromoneIncrement,
                        InputParameters.AntsNumber, InputParameters.NoUpdatesLimit, InputParameters.IterationsNumber);

                    int i = 0;
                    builder.Append("Distances Matrix:\n");
                    foreach (var e in DistGraph.ToList())
                    {
                        i++;
                        if (i%N == 0)
                        {
                            builder.Append(e).Append("\r\n");
                            continue;
                        }
                        builder.Append(e).Append('\t');
                    }

                    i = 0;
                    builder.Append("Flows Matrix:\n");
                    foreach (var e in FlowGraph.ToList())
                    {
                        i++;
                        if (i%N == 0)
                        {
                            builder.Append(e).Append("\r\n");
                            continue;
                        }
                        builder.Append(e).Append('\t');
                    }
                    return builder.ToString();
                }
                return String.Empty;
            }
        }
    }
}