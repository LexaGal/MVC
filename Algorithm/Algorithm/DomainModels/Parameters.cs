using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Algorithm.DomainModels
{
    public class Parameters
    {
        [Key]
        public int Id { get; set; }
        public int PheromoneInc { get; set; }
        public int ExtraPheromoneInc { get; set; }
        public int AntsNumber { get; set; }
        public int NoUpdatesLimit { get; set; }
        public int IterationsNumber { get; set; }

        public string StringView
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("Pheromone Increment:\n{0}\nExtra Pheromone Increment:\n{1}\n" +
                                 "Number of Ants:\n{2}\nNo Updates Limit:\n{3}\nNumber of Iterations:" +
                                 "\n{4}\n", PheromoneInc, ExtraPheromoneInc,
                                 AntsNumber, NoUpdatesLimit, IterationsNumber);
                return builder.ToString();
            }
        }
    }
}