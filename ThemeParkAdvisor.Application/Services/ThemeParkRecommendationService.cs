using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    // Service responsible for generating theme park recommendations
    public class ThemeParkRecommendationService
        : IThemeParkRecommendationService
    {
        // Inner generic recommendation service that handles the core logic
        private readonly IRecommendationService<ThemePark, ThemeParkPreferences, ThemeParkRecommendation> _inner;

        public ThemeParkRecommendationService(
            GenericScoringAggregator<ThemePark, ThemeParkPreferences> aggregator,
            IThemeParkRepository themeParkRepository)
        {
            // Configure the generic recommendation service for ThemePark
            _inner = new GenericRecommendationService<ThemePark, ThemeParkPreferences, ThemeParkFilter, ThemeParkRecommendation>(
                aggregator,
                // Factory to create a ThemeParkFilter from user preferences
                prefs => new ThemeParkFilter
                {
                    CountryId = prefs.CountryId,
                    CityId = prefs.CityId,
                    RegionId = prefs.RegionId,
                    MinAttractionsCount = prefs.MinAttractionsCount,
                    ThemeParksNotToVisitIds = prefs.ThemeParksNotToVisitIds
                },
                // Retrieves theme parks from repository based on filter
                async filter => await themeParkRepository.GetThemeParksAsync(filter),

                p => new ThemeParkRecommendation(
                    p.Name,
                    p.Size,
                    p.AttractionCount != null ? p.AttractionCount.Value : 0,
                    p.RollerCoasterCount != null ? p.RollerCoasterCount.Value : 0,
                    p.City.Region.Country.Name,
                    p.City.Region.Name,
                    p.City.Name,
                    p.Latitude,
                    p.Longitude,
                    p.WebsiteUrl,
                    p.GetScore()
                ),
                // Selector for how many recommendations to take
                prefs => prefs.WantedThemeParksCount
            );
        }

        // Public method to get recommendations based on preferences
        public Task<IEnumerable<ThemeParkRecommendation>> RecommendAsync(ThemeParkPreferences preferences)
            => _inner.RecommendAsync(preferences);
    }
}