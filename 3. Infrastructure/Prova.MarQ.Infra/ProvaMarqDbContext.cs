using Microsoft.EntityFrameworkCore;
using Prova.MarQ.Domain.Entities;

namespace Prova.MarQ.Infra
{
    public class ProvaMarqDbContext : DbContext
    {
        public ProvaMarqDbContext(DbContextOptions<ProvaMarqDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
                var connectionString = "Data Source=ProvaMarqDatabase.db";

                optionsBuilder.UseSqlite(connectionString, b =>
                    b.MigrationsAssembly("Prova.MarQ.Infra"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Documento).IsUnique();
                entity.Property(e => e.PIN).HasMaxLength(4).IsRequired();
                entity.HasIndex(e => e.PIN).IsUnique();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            });

            modelBuilder.Entity<TimeEntry>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Timestamp).IsRequired();
            });
        }
    }
}
