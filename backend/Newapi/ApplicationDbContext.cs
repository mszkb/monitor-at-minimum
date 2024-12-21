using Microsoft.EntityFrameworkCore;
using Mam.Models;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options), IDbContext
{
    public virtual required DbSet<Meta> Metas { get; set; }
    public virtual required DbSet<Site> Sites { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultContainer("orders");

        builder.Entity<Site>()
         .HasPartitionKey(c => c.Url);
    }
}