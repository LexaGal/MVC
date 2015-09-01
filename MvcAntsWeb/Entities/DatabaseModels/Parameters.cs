using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace Entities.DatabaseModels
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
                                     "\n{4}", PheromoneInc, ExtraPheromoneInc,
                    AntsNumber, NoUpdatesLimit, IterationsNumber);
                return builder.ToString();
            }
        }

        public Parameters(){}

        public Parameters(string data)
        {
            int pheromone = 0;
            int extraPheromone = 0;
            int noUpdatesLimit = 0;
            int nIterations = 0;
            int nAnts = 0;
            
            using (TextReader textReader = new StringReader(data))
            {
                textReader.ReadLine();
                string s = textReader.ReadLine();
                if (!String.IsNullOrEmpty(s))
                {
                    pheromone = int.Parse(s);
                }
                textReader.ReadLine();
                s = textReader.ReadLine();
                if (!String.IsNullOrEmpty(s))
                {
                    extraPheromone = int.Parse(s);
                }
                textReader.ReadLine();
                s = textReader.ReadLine();
                if (!String.IsNullOrEmpty(s))
                {
                    nAnts = int.Parse(s);
                }
                textReader.ReadLine();
                s = textReader.ReadLine();
                if (!String.IsNullOrEmpty(s))
                {
                    noUpdatesLimit = int.Parse(s);
                }
                textReader.ReadLine();
                s = textReader.ReadLine();
                if (!String.IsNullOrEmpty(s))
                {
                    nIterations = int.Parse(s);
                }
                PheromoneInc = pheromone;
                ExtraPheromoneInc = extraPheromone;
                AntsNumber = nAnts;
                NoUpdatesLimit = noUpdatesLimit;
                IterationsNumber = nIterations;
            }
        }
    }
}