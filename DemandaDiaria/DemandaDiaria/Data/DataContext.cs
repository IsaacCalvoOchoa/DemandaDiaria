using DemandaDiaria.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace DemandaDiaria.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Demanda> Demandas { get; set; }

        public DbSet<Plaza> Plazas { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Sucursal> Sucursales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Plaza>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Sucursal>().HasIndex("Name", "PlazaId").IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => p.Codigo).IsUnique();
            //modelBuilder.Entity<Demanda>().HasIndex("ProductId", "DemandaId").IsUnique();

        }
    }
}
