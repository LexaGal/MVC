using AntsAlg.AntsAlgorithm.Ants;
using AntsAlg.AntsAlgorithm.Graph;

namespace AntsAlg.AntsAlgorithm.Rules
{
    internal class IncrimentRule : IIncrimentRule
    {
        private readonly Pheromone _pheromoneIncriment;

        public IncrimentRule(Pheromone pheromoneIncriment)
        {
            _pheromoneIncriment = pheromoneIncriment;
        }

        public Mark Proccess(Mark input)
        {
            var pheramone = new Pheromone {Value = input.Pheromone.Value};
            input.Pheromone.Value += _pheromoneIncriment.Value;
            var mark = new Mark(input.Path) {Pheromone = pheramone};
            return mark;
        }
    }
}