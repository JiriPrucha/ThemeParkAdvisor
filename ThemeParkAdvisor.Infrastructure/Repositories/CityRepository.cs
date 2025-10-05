using Microsoft.EntityFrameworkCore;
using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Infrastructure
{
    public class CityRepository : ICityRepository
    {
        private readonly ThemeParkDbContext _db;

        /// <summary>
        /// Initializes a new instance of the CityRepository with the given database context.
        /// </summary>
        public CityRepository(ThemeParkDbContext db)
        {
            _db = db;
        }


        public async Task<List<CityName>> GetCitiesAsync(CityFilter filter)
        {
            var query = _db.Cities.AsQueryable();

            // pokud je vyplněná země
            if (filter.CountryId.HasValue)
            {
                query = query.Where(c => c.Region.CountryId == filter.CountryId.Value);
            }

            // pokud je vyplněný region
            if (filter.RegionId.HasValue)
            {
                query = query.Where(c => c.RegionId == filter.RegionId.Value);
            }

            return await query
                .AsNoTracking()
                .Select(c => new CityName(
                    c.CityId,
                    c.Name
                ))
                .ToListAsync();
        }
    }
}
