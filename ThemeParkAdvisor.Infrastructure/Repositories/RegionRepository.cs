using Microsoft.EntityFrameworkCore;
using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Infrastructure
{
    public class RegionRepository : IRegionRepository
    {
        private readonly ThemeParkDbContext _db;

        /// <summary>
        /// Initializes a new instance of the RegionRepository with the given database context.
        /// </summary>
        public RegionRepository(ThemeParkDbContext db)
        {
            _db = db;
        }

        public async Task<List<RegionName>> GetRegionsAsync(RegionFilter filter)
        {
            var query = _db.Regions.AsQueryable();

            // filtr podle země
            if (filter.CountryId.HasValue)
            {
                query = query.Where(r => r.CountryId == filter.CountryId.Value);
            }

            // filtr podle města
            if (filter.CityId.HasValue)
            {
                query = query.Where(r =>
                    _db.Cities.Any(c => c.CityId == filter.CityId.Value && c.RegionId == r.RegionId));
            }

            return await query
                .AsNoTracking()
                .Select(r => new RegionName(
                    r.RegionId,
                    r.Name
                ))
                .ToListAsync();
        }
    }
}
