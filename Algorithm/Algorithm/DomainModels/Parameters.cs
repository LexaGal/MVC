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
                builder.AppendFormat("Pheromone Increment: {0}, Extra Pheromone Increment: {1}, " +
                                     "Number of Ants: {2}, No Updates Limit: {3}, Number of Iterations:" +
                                     " {4} ", PheromoneInc, ExtraPheromoneInc,
                    AntsNumber, NoUpdatesLimit, IterationsNumber);
                return builder.ToString();
            }
        }
    }
}