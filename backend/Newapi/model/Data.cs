using Microsoft.EntityFrameworkCore;
using Mam.Models;

namespace Mam.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Site> Sites { get; set; }
        public DbSet<Meta> Meta { get; set; }
    }
}