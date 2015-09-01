using System;

namespace AntsAlg.AntsAlgorithm.Graph
{
    [Serializable]
    class Vertex : IVertex
    {
        public Vertex(int number)
        {
            Number = number;
        }

        public int Number { get; set; }
    }
}