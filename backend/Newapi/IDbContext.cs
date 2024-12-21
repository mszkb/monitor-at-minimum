using Mam.Models;
using Microsoft.EntityFrameworkCore;

public interface IDbContext
{
    DbSet<Meta> Metas { get; set; }
    DbSet<Site> Sites { get; set; }
}