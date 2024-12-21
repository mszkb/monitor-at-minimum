using Mam.Models;

namespace Newapi.Repositories
{
    public interface ISiteRepository
    {
        // Define the methods for the repository
        Task<IEnumerable<Site>> GetAllSitesAsync();
        Task<Site> GetSiteByIdAsync(int id);
        Task<Site> AddSiteAsync(Site site);
        Task<Site> UpdateSiteAsync(Site site);
        Task<bool> DeleteSiteAsync(int id);
    }
}