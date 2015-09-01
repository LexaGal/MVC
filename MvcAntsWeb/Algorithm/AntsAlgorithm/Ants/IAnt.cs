using System.Collections.Generic;
using AntsAlg.AntsAlgorithm.Graph;

namespace AntsAlg.AntsAlgorithm.Ants
{
    public interface IAnt
    {
        IList<IVertex> VisitedVetecies { get; set; }
    }
}