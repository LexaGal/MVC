namespace AntsAlg.AntsAlgorithm.Algorithm
{
    public class Path
    {
        public Path(double value)
        {
            Value = value;
        }

        public double Value { get; private set; }

        public static bool operator <(Path path1, Path path2)
        {
            return path1.Value < path2.Value;
        }

        public static bool operator >(Path path1, Path path2)
        {
            return path1.Value > path2.Value;
        }
    }
}