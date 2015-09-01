using System.Collections.Generic;

namespace AntsAlg.AntsAlgorithm.Ants
{
    public class AntFactory : IAntFactory
    {
        public IList<IAnt> GeneratePapulation(int number)
        {
            var result = new List<IAnt>();
            for (var i = 0; i < number; i++)
            {
                result.Add(new Ant());
            }
            return result;
        }
    }
}