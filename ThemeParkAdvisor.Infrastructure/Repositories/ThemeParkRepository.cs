using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Infrastructure
{
    /// <summary>
    /// Repository implementation for accessing and querying ThemePark entities.
    /// Provides methods to retrieve theme parks based on various filter criteria.
    /// </summary>
    public class ThemeParkRepository : IThemeParkRepository
    {
        private readonly ThemeParkDbContext _db;

        /// <summary>
        /// Initializes a new instance of the ThemeParkRepository with the given database context.
        /// </summary>
        /// <param name="db">The database context for ThemePark entities.</param>
        public ThemeParkRepository(ThemeParkDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Retrieves all theme park names with their IDs from the database.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of <see cref="ThemeParkName"/> objects.
        /// </returns>
        public async Task<List<ThemeParkName>> GetThemeParkNamesAsync()
        {
            return await _db.ThemeParks
                .AsNoTracking()
                .Select(p => new ThemeParkName(
                    p.ThemeParkId,
                    p.Name
                ))
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a list of theme parks that match the specified filter criteria.
        /// </summary>
        /// <param name="filter">An object containing filtering options such as location, attraction counts, and exclusions.</param>
        /// <returns>A list of ThemePark entities matching the filter.</returns>
        public async Task<List<ThemePark>> GetThemeParksAsync(ThemeParkFilter filter)
        {
            var query =
                from park in _db.ThemeParks
                    .Include(p => p.City)
                        .ThenInclude(c => c.Region)
                            .ThenInclude(r => r.Country)
                where (!filter.CountryId.HasValue || park.City.Region.CountryId == filter.CountryId)
                   && (!filter.RegionId.HasValue || park.City.RegionId == filter.RegionId)
                   && (!filter.CityId.HasValue || park.CityId == filter.CityId)
                select new ThemePark
                {
                    ThemeParkId = park.ThemeParkId,
                    Name = park.Name,
                    Size = park.Size,
                    CityId = park.CityId,
                    Latitude = park.Latitude,
                    Longitude = park.Longitude,
                    ThemingRating = park.ThemingRating,
                    WebsiteUrl = park.WebsiteUrl,
                    City = park.City,
                    AttractionCount = _db.Attractions.Count(a => a.ThemeParkId == park.ThemeParkId),
                    RollerCoasterCount = _db.Attractions.Count(a => a.ThemeParkId == park.ThemeParkId && a.AttractionTypeId == 1)
                };

            // Filtry na počty atrakcí
            if (filter.MinAttractionsCount.HasValue)
                query = query.Where(x => x.AttractionCount >= filter.MinAttractionsCount.Value);

            if (filter.MaxAttractionsCount.HasValue)
                query = query.Where(x => x.AttractionCount <= filter.MaxAttractionsCount.Value);

            if (filter.MinRollerCoasterCount.HasValue)
                query = query.Where(x => x.RollerCoasterCount >= filter.MinRollerCoasterCount.Value);

            if (filter.MaxRollerCoasterCount.HasValue)
                query = query.Where(x => x.RollerCoasterCount <= filter.MaxRollerCoasterCount.Value);

            // Exclude blocked parks
            if (filter.ThemeParksNotToVisitIds?.Any() == true)
            {
                var blockedIds = filter.ThemeParksNotToVisitIds;
                query = query.Where(p =>
                    !blockedIds.Contains(p.ThemeParkId)
                    || (filter.FavoriteThemeParkId.HasValue && p.ThemeParkId == filter.FavoriteThemeParkId.Value));
            }

            // Pokud je oblíbený park, přidej ho i když neprojde filtry
            if (filter.FavoriteThemeParkId.HasValue)
            {
                var favoriteId = filter.FavoriteThemeParkId.Value;
                var result = await query.AsNoTracking().ToListAsync();

                if (!result.Any(p => p.ThemeParkId == favoriteId))
                {
                    var favorite = await _db.ThemeParks
                        .Include(p => p.City)
                            .ThenInclude(c => c.Region)
                                .ThenInclude(r => r.Country)
                        .Where(p => p.ThemeParkId == favoriteId)
                        .Select(p => new ThemePark
                        {
                            ThemeParkId = p.ThemeParkId,
                            Name = p.Name,
                            Size = p.Size,
                            CityId = p.CityId,
                            Latitude = p.Latitude,
                            Longitude = p.Longitude,
                            ThemingRating = p.ThemingRating,
                            WebsiteUrl = p.WebsiteUrl,
                            City = p.City,
                            AttractionCount = _db.Attractions.Count(a => a.ThemeParkId == p.ThemeParkId),
                            RollerCoasterCount = _db.Attractions.Count(a => a.ThemeParkId == p.ThemeParkId && a.AttractionTypeId == 1)
                        })
                        .FirstOrDefaultAsync();

                    if (favorite != null)
                        result.Add(favorite);
                }

                return result;
            }

            return await query.AsNoTracking().ToListAsync();
        }
    }
}