using System;
using System.Collections.Generic;
using System.Linq;
using AntsAlg.AntsAlgorithm.Graph;

namespace AntsAlg.AntsAlgorithm.Rules
{
    internal class SelectRule : ISelectRule
    {
        private readonly Random _random;

        public SelectRule(Random random)
        {
            _random = random;
        }

        public IVertex Proccess(Dictionary<IVertex, double> input)
        {
            var totalValue = input.Values.Sum();
            var likelihoodRatio = double.MaxValue/totalValue;
            var ratios = new List<KeyValuePair<double, IVertex>>();
            double lastRatio = 0;

            foreach (var key in input.Keys)
            {
                var ratio = lastRatio + input[key]*likelihoodRatio;
                ratios.Add(new KeyValuePair<double, IVertex>(ratio, key));
                lastRatio = ratio;
            }

            var value = _random.NextDouble()*lastRatio;
            foreach (var t in ratios.Where(t => value < t.Key))
            {
                return t.Value;
            }
            return ratios.Last().Value;
        }
    }
}