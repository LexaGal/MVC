using AntsAlg.AntsAlgorithm.Ants;
using AntsAlg.AntsAlgorithm.Graph;

namespace AntsAlg.AntsAlgorithm.Rules
{
    internal class DecrimentRule : IDecrimentRule
    {
        private readonly Pheromone _decrimentPheromone;

        public DecrimentRule(Pheromone decrimentPheromone)
        {
            _decrimentPheromone = decrimentPheromone;
        }

        public Mark Proccess(Mark input)
        {
            var result = new Mark(input.Path) {Pheromone = {Value = input.Pheromone.Value - _decrimentPheromone.Value}};
            if (result.Pheromone.Value < 0)
            {
                result.Pheromone.Value = 0;
            }
            return result;
        }
    }
}