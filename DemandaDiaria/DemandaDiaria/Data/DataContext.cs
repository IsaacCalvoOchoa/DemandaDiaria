using DemandaDiaria.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace DemandaDiaria.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Plaza> Plazas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Plaza>().HasIndex(p => p.Name).IsUnique();
        }
    }
}
