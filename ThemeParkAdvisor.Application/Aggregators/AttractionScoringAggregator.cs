using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public class AttractionScoringAggregator
        : GenericScoringAggregator<Attraction, AttractionPreferences>
    {
        /// <summary>
        /// Creates a new aggregator for attractions.
        /// </summary>
        /// <param name="specificStrategies">
        /// Strategies tailored for scoring attractions based on specific user preferences.
        /// </param>
        /// <param name="genericStrategies">
        /// Fallback strategies applied when no specific strategies are applicable.
        /// </param>
        public AttractionScoringAggregator(
            IEnumerable<IAttractionSelectionStrategy> specificStrategies,
            IEnumerable<IFallbackAttractionSelectionStrategy> genericStrategies)
            : base(
                // Cast to the generic selection strategy interface so the base class can work with them
                specificStrategies.Cast<IScoringStrategy<Attraction, AttractionPreferences>>(),
                genericStrategies.Cast<IScoringStrategy<Attraction, AttractionPreferences>>(),
                a => a.AttractionId, // Function to extract the unique ID from an attraction
                calc => Attraction.ScoreCalculator = calc) // Assigns the score calculator to the domain model
        {
        }
    }
}