using System.Collections.Generic;
using System.Threading.Tasks;
using Mam.Models;
using Newapi.Repositories;

namespace Newapi.Service
{
    public class SiteService
    {
        private readonly ISiteRepository _siteRepository;

        public SiteService(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public async Task<IEnumerable<Site>> GetAllSitesAsync()
        {
            return await _siteRepository.GetAllSitesAsync();
        }

        public async Task<Site> GetSiteByIdAsync(int id)
        {
            return await _siteRepository.GetSiteByIdAsync(id);
        }

        public async Task<Site> CreateSiteAsync(Site site)
        {
            return await _siteRepository.AddSiteAsync(site);
        }

        public async Task<Site> UpdateSiteAsync(Site site)
        {
            return await _siteRepository.UpdateSiteAsync(site);
        }

        public async Task<bool> DeleteSiteAsync(int id)
        {
            return await _siteRepository.DeleteSiteAsync(id);
        }
    }
}