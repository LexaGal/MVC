using System.Data.Entity;
using Entities.DatabaseModels;

namespace DatabaseAccess.DatabaseContext
{
    public class AlgorithmDb : DbContext
    {
        public DbSet<Parameters> Parameters { get; set; }
        public DbSet<DistMatrix> DistMatrices { get; set; }
        public DbSet<FlowMatrix> FlowMatrices { get; set; }
        public DbSet<ResultInfo> ResultsInfo { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parameters>().ToTable("Parameters");
            modelBuilder.Entity<DistMatrix>().ToTable("DistMatrices");
            modelBuilder.Entity<FlowMatrix>().ToTable("FlowMatrices");
            modelBuilder.Entity<ResultInfo>().ToTable("ResultsInfo");
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}