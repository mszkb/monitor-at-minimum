using System.Linq;
using System.Threading.Tasks;
using Mam.Models;
using Microsoft.EntityFrameworkCore;

namespace Newapi.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly ApplicationDbContext _context;

        public SiteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Site>> GetAllSitesAsync()
        {
            return await _context.Sites.ToListAsync();
        }

        public async Task<Site> GetSiteByIdAsync(int id)
        {
            return await _context.Sites.FindAsync(id) ?? throw new InvalidOperationException("Site not found");
        }

        public async Task<Site> AddSiteAsync(Site site)
        {
            _context.Sites.Add(site);
            await _context.SaveChangesAsync();
            return site;
        }

        public async Task<Site> UpdateSiteAsync(Site site)
        {
            _context.Entry(site).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return site;
        }

        public async Task<bool> DeleteSiteAsync(int id)
        {
            var site = await _context.Sites.FindAsync(id);
            if (site != null)
            {
                _context.Sites.Remove(site);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}