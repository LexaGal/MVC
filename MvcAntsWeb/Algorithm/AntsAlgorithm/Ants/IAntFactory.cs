using System.Collections.Generic;

namespace AntsAlg.AntsAlgorithm.Ants
{
    public interface IAntFactory
    {
        IList<IAnt> GeneratePapulation(int number);
    }
}
