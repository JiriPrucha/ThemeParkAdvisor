namespace ThemeParkAdvisor.Application
{
    // Generic recommendation service that can work with any entity, preferences, filter, and recommendation type
    public class GenericRecommendationService<TEntity, TPreferences, TFilter, TRecommendation>
        : IRecommendationService<TEntity, TPreferences, TRecommendation>
    {
        private readonly GenericScoringAggregator<TEntity, TPreferences> _aggregator;
        private readonly Func<TPreferences, TFilter> _filterFactory;
        private readonly Func<TFilter, Task<IEnumerable<TEntity>>> _dataFetcher;
        private readonly Func<TEntity, TRecommendation> _mapper;
        private readonly Func<TPreferences, int?> _takeCountSelector;

        private const int DefaultRecommendationCount = 5;

        public GenericRecommendationService(
            GenericScoringAggregator<TEntity, TPreferences> aggregator,
            Func<TPreferences, TFilter> filterFactory,
            Func<TFilter, Task<IEnumerable<TEntity>>> dataFetcher,
            Func<TEntity, TRecommendation> mapper,
            Func<TPreferences, int?> takeCountSelector)
        {
            _aggregator = aggregator;
            _filterFactory = filterFactory;
            _dataFetcher = dataFetcher;
            _mapper = mapper;
            _takeCountSelector = takeCountSelector;
        }

        public async Task<IEnumerable<TRecommendation>> RecommendAsync(TPreferences preferences)
        {
            // Create filter from preferences
            var filter = _filterFactory(preferences);

            // Fetch data using the provided data fetcher
            var items = await _dataFetcher(filter);

            // If no items found, return empty result
            if (!items.Any())
                return Enumerable.Empty<TRecommendation>();

            // Rank items using the scoring aggregator
            var ranked = await _aggregator.GetRankedAsync(preferences, items);

            // Take the desired number of recommendations and map them to DTOs
            return ranked
                .Take(_takeCountSelector(preferences) ?? DefaultRecommendationCount)
                .Select(_mapper);
        }
    }
}