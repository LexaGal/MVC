using System.Data.Entity;
using Algorithm.DomainModels;

namespace Algorithm.Database
{
    public class AlgorithmDb : DbContext
    {
        public DbSet<Parameters> Parameters { get; set; }
        public DbSet<DistMatrix> DistMatrices { get; set; }
        public DbSet<FlowMatrix> FlowMatrices { get; set; }
        public DbSet<ResultInfo> ResultsInfo { get; set; }
    }
}