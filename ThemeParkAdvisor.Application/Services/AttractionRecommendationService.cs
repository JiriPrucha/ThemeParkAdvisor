using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    // Service responsible for generating attraction recommendations
    public class AttractionRecommendationService : IAttractionRecommendationService
    {
        private readonly GenericScoringAggregator<Attraction, AttractionPreferences> _aggregator;
        private readonly IAttractionRepository _attractionRepository;
        private readonly IThemeParkRepository _themeParkRepository;

        public AttractionRecommendationService(
            GenericScoringAggregator<Attraction, AttractionPreferences> aggregator,
            IAttractionRepository attractionRepository,
            IThemeParkRepository themeParkRepository)
        {
            _aggregator = aggregator;
            _attractionRepository = attractionRepository;
            _themeParkRepository = themeParkRepository;
        }

        public async Task<IEnumerable<AttractionRecommendation>> RecommendAsync(AttractionPreferences preferences)
        {
            // Preload theme park names into a dictionary for quick lookup
            var parkNames = (await _themeParkRepository.GetThemeParkNamesAsync())
                .ToDictionary(p => p.ThemeParkId, p => p.Name);

            // Configure the generic recommendation service for Attraction
            var service = new GenericRecommendationService<Attraction, AttractionPreferences, AttractionFilter, AttractionRecommendation>(
                _aggregator,
                // Factory to create an AttractionFilter from user preferences
                preferencess => new AttractionFilter(
                    preferencess.ThemeParkToVisitId,
                    preferencess.AdrenalineRating,
                    preferencess.MaxRequiredHeight
                ),
                // Retrieves attractions from repository based on filter
                async filter => await _attractionRepository.GetAttractionsAsync(filter),
                // Converts Attraction domain model to AttractionRecommendation
                a => new AttractionRecommendation(
                    parkNames.TryGetValue(a.ThemeParkId, out var parkName) ? parkName : string.Empty,
                    a.Name,
                    a.AttractionType.Name,
                    a.AdrenalineRating,
                    a.MinRiderHeight ?? 0,
                    a.GetScore()
                ),
                // Selector for how many recommendations to take
                prefs => prefs.WantedAttractionsCount
            );

            // Delegate the recommendation logic to the generic service
            return await service.RecommendAsync(preferences);
        }
    }
}