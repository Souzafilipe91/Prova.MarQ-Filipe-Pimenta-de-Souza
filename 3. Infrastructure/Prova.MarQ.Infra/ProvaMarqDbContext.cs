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
    }
}
