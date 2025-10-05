using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Infrastructure
{
    /// <summary>
    /// Repository implementation for accessing and querying <see cref="Attraction"/> entities
    /// </summary>
    public class AttractionRepository : IAttractionRepository
    {
        private readonly ThemeParkDbContext _db;

        public AttractionRepository(ThemeParkDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Retrieves all attractions belonging to a specific theme park.
        /// </summary>
        public async Task<List<Attraction>> GetAttractionsByThemeParkIdAsync(int themeParkId)
        {
            return await _db.Attractions
                .Include(a => a.AttractionType)
                .AsNoTracking()
                .Where(a => a.ThemeParkId == themeParkId)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all attractions from the database.
        /// </summary>
        public async Task<List<Attraction>> GetAllAttractionsAsync()
        {
            return await _db.Attractions
                .Include(a => a.AttractionType)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves attractions that match the given filter criteria.
        /// Dynamically builds a predicate expression based on which filter values are provided.
        /// </summary>
        /// <param name="filter">The filter object containing optional search criteria.</param>
        /// <returns>A list of attractions matching the filter.</returns>
        public async Task<List<Attraction>> GetAttractionsAsync(AttractionFilter filter)
        {
            IQueryable<Attraction> attractions = _db.Attractions.AsNoTracking();

            // Build a list of individual filter conditions
            var conditions = new List<Expression<Func<Attraction, bool>>>();

            if (filter.ThemeParkId.HasValue)
                conditions.Add(a => a.ThemeParkId == filter.ThemeParkId.Value);

            if (filter.AdrenalineRating.HasValue)
                conditions.Add(a => a.AdrenalineRating >= filter.AdrenalineRating.Value);

            if (filter.MaxRequiredHeight.HasValue)
                conditions.Add(a => a.MinRiderHeight <= filter.MaxRequiredHeight.Value);

            // Combine all conditions into a single expression using AndAlso extension
            Expression<Func<Attraction, bool>> baseCondition = a => true; // Start with a condition that always passes
            foreach (var condition in conditions)
            {
                baseCondition = baseCondition.AndAlso(condition);
            }

            // Execute the query with the combined filter
            return await attractions
                .Include(a => a.AttractionType)
                .Where(baseCondition)
                .ToListAsync();
        }

        public async Task<List<AttractionType>> GetAttractionTypes()
        {
            return await _db.AttractionTypes
                .AsNoTracking()
                .ToListAsync();
        }
    }
}