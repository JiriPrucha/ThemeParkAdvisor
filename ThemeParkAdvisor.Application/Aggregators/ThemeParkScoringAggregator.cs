using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public class ThemeParkScoringAggregator
        : GenericScoringAggregator<ThemePark, ThemeParkPreferences>
    {
        /// <summary>
        /// Creates a new aggregator for theme parks.
        /// </summary>
        /// <param name="specificStrategies">
        /// Strategies tailored for scoring theme parks based on specific user preferences.
        /// </param>
        /// <param name="genericStrategies">
        /// Fallback strategies applied when no specific strategies are applicable.
        /// </param>
        public ThemeParkScoringAggregator(
            IEnumerable<IThemeParkScoringStrategy> specificStrategies,
            IEnumerable<IFallbackThemeParkScoringStrategy> genericStrategies)
            : base(
                specificStrategies.Cast<IScoringStrategy<ThemePark, ThemeParkPreferences>>(),
                genericStrategies.Cast<IScoringStrategy<ThemePark, ThemeParkPreferences>>(),
                p => p.ThemeParkId, // Function to extract the unique ID from a theme park
                calc => ThemePark.ScoreCalculator = calc) // Assigns the score calculator to the domain model
        {
        }
    }
}