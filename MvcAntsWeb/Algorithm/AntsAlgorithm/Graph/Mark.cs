using AntsAlg.AntsAlgorithm.Algorithm;
using AntsAlg.AntsAlgorithm.Ants;

namespace AntsAlg.AntsAlgorithm.Graph
{
    public class Mark
    {
        public Mark(Path path)
        {
            Path = path;
            Pheromone = new Pheromone();
        }

        public Pheromone Pheromone { get; set; }
        public Path Path { get; private set; }

    }
}