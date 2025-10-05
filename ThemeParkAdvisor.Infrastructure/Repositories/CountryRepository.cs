using Microsoft.EntityFrameworkCore;
using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Infrastructure
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ThemeParkDbContext _db;

        /// <summary>
        /// Initializes a new instance of the CountryRepository with the given database context.
        /// </summary>
        public CountryRepository(ThemeParkDbContext db)
        {
            _db = db;
        }

        public async Task<List<CountryName>> GetCountriesAsync(CountryFilter filter)
        {
            var query = _db.Countries.AsQueryable();

            if (filter.RegionId.HasValue)
            {
                query = query.Where(c =>
                    _db.Regions.Any(r => r.RegionId == filter.RegionId.Value && r.CountryId == c.CountryId));
            }

            if (filter.CityId.HasValue)
            {
                query = query.Where(c =>
                    _db.Regions.Any(r =>
                        r.CountryId == c.CountryId &&
                        _db.Cities.Any(city => city.CityId == filter.CityId.Value && city.RegionId == r.RegionId)
                    ));
            }

            return await query
                .AsNoTracking()
                .Select(c => new CountryName(
                    c.CountryId,
                    c.Name
                ))
                .ToListAsync();
        }
    }
}
