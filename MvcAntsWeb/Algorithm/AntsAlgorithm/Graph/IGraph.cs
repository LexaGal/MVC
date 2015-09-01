using System.Collections.Generic;
using AntsAlg.AntsAlgorithm.Algorithm;

namespace AntsAlg.AntsAlgorithm.Graph
{
    public interface IGraph
    {
        IList<IVertex> Vertecies { get; }
        IList<IEdge> EdgesList { get; }

        Dictionary<IVertex, IEdge> GetSibilings(IVertex vertex);
        Path CalculatePath(IList<IVertex> items);
        IList<IEdge> GetEdgePath(IList<IVertex> items);
    }
}
