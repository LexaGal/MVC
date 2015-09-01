namespace AntsAlg.AntsAlgorithm.Graph
{
    internal class Edge : IEdge
    {
        public Edge(Mark mark)
        {
            Mark = mark;
        }

        public Mark Mark { get; set; }
    }
}