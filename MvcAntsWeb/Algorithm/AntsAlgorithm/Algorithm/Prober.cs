using System;
using AntsAlg.AntsAlgorithm.Graph;

namespace AntsAlg.AntsAlgorithm.Algorithm
{
    internal class Prober : IProber
    {
        private readonly double _alpha;
        private readonly double _beta;

        public Prober(double alpha, double beta)
        {
            _alpha = alpha;
            _beta = beta;
        }

        public double GetProb(IEdge item)
        {
            var d1 = Math.Pow(item.Mark.Path.Value, _alpha);
            var d2 = Math.Pow(item.Mark.Pheromone.Value, _beta);
            return d2*d1;
        }
    }
}