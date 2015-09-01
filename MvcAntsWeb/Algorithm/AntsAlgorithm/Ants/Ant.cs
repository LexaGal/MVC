using System.Collections.Generic;
using AntsAlg.AntsAlgorithm.Graph;

namespace AntsAlg.AntsAlgorithm.Ants
{
    public class Ant : IAnt
    {
        public Ant()
        {
            VisitedVetecies = new List<IVertex>();
        }

        public IList<IVertex> VisitedVetecies { get; set; }
    }
}