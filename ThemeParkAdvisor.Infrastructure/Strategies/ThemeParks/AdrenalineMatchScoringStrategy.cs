using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Infrastructure
{
    /// <summary>
    /// Strategy that scores theme parks based on how well their attractions' adrenaline levels
    /// match the user's desired adrenaline level.
    /// </summary>
    public class AdrenalineMatchStrategy : IThemeParkScoringStrategy
    {
        private readonly IAttractionRepository _attractionRepository;

        // Maximum number of attractions needed to reach a full score
        private const int MaxAttractionsForFullScore = 40;

        // Default score when no adrenaline preference is provided
        private const double DefaultScoreNoPreference = 0.5;

        // Score weights based on adrenaline level difference
        private const int ExactMatchDifference = 0;
        private const double ExactMatchScore = 1.0;

        private const int OneOffDifference = 1;
        private const double OneOffScore = 0.7;

        private const int TwoOffDifference = 2;
        private const double TwoOffScore = 0.4;

        private const double NoMatchScore = 0.0;

        // Score boundaries
        private const double MinScore = 0.0;
        private const double MaxScore = 1.0;

        public AdrenalineMatchStrategy(IAttractionRepository attractionRepository)
        {
            _attractionRepository = attractionRepository;
        }

        /// <summary>
        /// Determines if this strategy can be executed based on provided preferences.
        /// </summary>
        public bool CanExecute(ThemeParkPreferences preferences)
            => preferences.AdrenalineLevel.HasValue;

        /// <summary>
        /// Scores each theme park based on how closely its attractions' adrenaline levels
        /// match the user's desired adrenaline level.
        /// </summary>
        public async Task<Dictionary<int, double>> ScoreAsync(
            ThemeParkPreferences preferences,
            IEnumerable<ThemePark> parks)
        {
            var scores = new Dictionary<int, double>();

            // If no adrenaline preference is set, assign a default neutral score
            if (!preferences.AdrenalineLevel.HasValue)
            {
                foreach (var park in parks)
                    scores[park.ThemeParkId] = DefaultScoreNoPreference;
                return scores;
            }

            int desired = preferences.AdrenalineLevel.Value;

            foreach (var park in parks)
            {
                var attractions = await _attractionRepository
                    .GetAttractionsByThemeParkIdAsync(park.ThemeParkId);

                if (attractions == null || !attractions.Any())
                {
                    scores[park.ThemeParkId] = MinScore;
                    continue;
                }

                // Calculate weighted match count based on adrenaline difference
                double weightedCount = attractions.Sum(a =>
                {
                    int diff = Math.Abs(a.AdrenalineRating - desired);
                    return diff switch
                    {
                        ExactMatchDifference => ExactMatchScore,
                        OneOffDifference => OneOffScore,
                        TwoOffDifference => TwoOffScore,
                        _ => NoMatchScore
                    };
                });

                // Normalize score to 0–1 range, capped at MaxScore
                double score = Math.Min(MaxScore, weightedCount / MaxAttractionsForFullScore);
                scores[park.ThemeParkId] = score;
            }

            return scores;
        }
    }
}