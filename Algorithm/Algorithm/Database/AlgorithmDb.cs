using System.Data.Entity;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using Algorithm.Authentication;
using Algorithm.DomainModels;

namespace Algorithm.Database
{
    public class AlgorithmDb : DbContext
    {
        public DbSet<Parameters> Parameters { get; set; }
        public DbSet<DistMatrix> DistMatrices { get; set; }
        public DbSet<FlowMatrix> FlowMatrices { get; set; }
        public DbSet<ResultInfo> ResultsInfo { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parameters>().ToTable("Parameters");
            modelBuilder.Entity<DistMatrix>().ToTable("DistMatrices");
            modelBuilder.Entity<FlowMatrix>().ToTable("FlowMatrices");
            modelBuilder.Entity<ResultInfo>().ToTable("ResultsInfo");
            modelBuilder.Entity<Client>().ToTable("Clients");
        }
    }
}