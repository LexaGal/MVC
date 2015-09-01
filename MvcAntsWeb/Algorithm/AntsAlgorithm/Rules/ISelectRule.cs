using System.Collections.Generic;
using AntsAlg.AntsAlgorithm.Graph;

namespace AntsAlg.AntsAlgorithm.Rules
{
    public interface ISelectRule : IRule<Dictionary<IVertex, double>, IVertex>
    {
    }
}
