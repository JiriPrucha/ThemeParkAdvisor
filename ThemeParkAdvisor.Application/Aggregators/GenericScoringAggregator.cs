using ThemeParkAdvisor.Domain;

/// A generic scoring aggregator that combines results from multiple selection strategies
/// to produce a ranked list of entities.
/// 
/// 1. Filters applicable strategies based on the given preferences.
/// 2. Executes each strategy to produce score maps.
/// 3. Averages scores for each entity across all applicable strategies.
/// 4. Assigns a score calculator back to the domain model for later retrieval.
/// 5. Returns the entities ordered by their computed score.

namespace ThemeParkAdvisor.Application
{
    /// <typeparam name="TEntity">The entity type being scored (e.g., Attraction, ThemePark).</typeparam>
    /// <typeparam name="TPreferences">The type representing user preferences for scoring.</typeparam>
    public class GenericScoringAggregator<TEntity, TPreferences>
    {
        private readonly IEnumerable<IScoringStrategy<TEntity, TPreferences>> _specificStrategies;
        private readonly IEnumerable<IScoringStrategy<TEntity, TPreferences>> _genericStrategies;
        private readonly Func<TEntity, int> _idSelector;
        private readonly Action<Func<TEntity, double>> _setScoreCalculator;

        /// <summary>
        /// Creates a new generic scoring aggregator.
        /// </summary>
        public GenericScoringAggregator(
            IEnumerable<IScoringStrategy<TEntity, TPreferences>> specificStrategies,
            IEnumerable<IScoringStrategy<TEntity, TPreferences>> genericStrategies,
            Func<TEntity, int> idSelector,
            Action<Func<TEntity, double>> setScoreCalculator)
        {
            _specificStrategies = specificStrategies;
            _genericStrategies = genericStrategies;
            _idSelector = idSelector;
            _setScoreCalculator = setScoreCalculator;
        }

        /// <summary>
        /// Produces a ranked list of entities based on the provided preferences and scoring strategies.
        /// </summary>
        /// <returns>Entities ordered from highest to lowest score.</returns>
        public async Task<IEnumerable<TEntity>> GetRankedAsync(
            TPreferences preferences,
            IEnumerable<TEntity> items)
        {
            var list = items.ToList();
            if (!list.Any())
                return list; // No items to score

            // Filter strategies that can execute given the current preferences
            var applicableSpecific = _specificStrategies.Where(s => s.CanExecute(preferences)).ToList();
            var applicableGeneric = _genericStrategies.Where(s => s.CanExecute(preferences)).ToList();

            // If no strategies are applicable, fall back to all generic strategies
            if (!applicableSpecific.Any() && !applicableGeneric.Any())
                applicableGeneric = _genericStrategies.ToList();

            var scoreMaps = new List<Dictionary<int, double>>();

            // Execute specific strategies
            foreach (var s in applicableSpecific)
                scoreMaps.Add(await s.ScoreAsync(preferences, list));

            // Execute generic strategies
            foreach (var g in applicableGeneric)
                scoreMaps.Add(await g.ScoreAsync(preferences, list));

            // Combine scores by averaging across all strategies
            var combined = new Dictionary<int, double>();
            foreach (var entity in list)
            {
                double sum = 0;
                int count = 0;
                foreach (var map in scoreMaps)
                {
                    if (map.TryGetValue(_idSelector(entity), out var val))
                    {
                        sum += val;
                        count++;
                    }
                }
                combined[_idSelector(entity)] = count == 0 ? 0 : sum / count;
            }

            // Assign the score calculator back to the domain model
            _setScoreCalculator(e =>
                combined.TryGetValue(_idSelector(e), out var val) ? val : 0);

            // Return the list ordered by score (highest first)
            return list.OrderByDescending(e => e switch
            {
                Attraction a => a.GetScore(),
                ThemePark p => p.GetScore(),
                _ => 0
            });
        }
    }
}