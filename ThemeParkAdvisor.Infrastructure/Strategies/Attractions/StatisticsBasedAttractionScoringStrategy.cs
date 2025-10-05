using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Infrastructure
{
    /// <summary>
    /// Attraction scoring strategy based on their statistics.
    /// </summary>
    public class StatisticsBasedAttractionSelectionStrategy : IFallbackAttractionSelectionStrategy
    {
        // Weights for individual score components
        private const double AdrenalineWeight = 0.7;
        private const double MinHeightWeight = 0.1;
        private const double TypeBonusWeight = 0.2;

        // Preferred attraction types for bonus points
        private static readonly HashSet<int> PreferredTypes = new()
        {
            1, // Roller Coaster
            4, // Dark Ride
            18 // River rapid
        };

        public bool CanExecute(AttractionPreferences preferences) => true;

        public Task<Dictionary<int, double>> ScoreAsync(
            AttractionPreferences preferences,
            IEnumerable<Attraction> attractions)
        {
            var list = attractions.ToList();
            var scores = new Dictionary<int, double>();

            if (!list.Any())
                return Task.FromResult(scores);

            // Precompute normalization bounds
            int maxAdrenaline = list.Max(a => a.AdrenalineRating);
            int minMinHeight = list.Min(a => a.MinRiderHeight ?? 0);
            int maxMinHeight = list.Max(a => a.MinRiderHeight ?? 0);

            foreach (var attraction in list)
            {
                double finalScore = CalculateFinalScore(
                    attraction,
                    maxAdrenaline,
                    minMinHeight,
                    maxMinHeight
                );

                scores[attraction.AttractionId] = Math.Clamp(finalScore, 0, 1);
            }

            return Task.FromResult(scores);
        }

        private double CalculateFinalScore(
            Attraction attraction,
            int maxAdrenaline,
            int minMinHeight,
            int maxMinHeight)
        {
            double adrenalineScore = NormalizeAdrenaline(attraction.AdrenalineRating, maxAdrenaline);
            double minHeightScore = NormalizeMinHeight(attraction.MinRiderHeight ?? 0, minMinHeight, maxMinHeight);
            double typeBonus = CalculateTypeBonus(attraction.AttractionType);

            return adrenalineScore * AdrenalineWeight +
                   minHeightScore * MinHeightWeight +
                   typeBonus * TypeBonusWeight;
        }

        private double NormalizeAdrenaline(int adrenalineLevel, int maxAdrenaline)
        {
            if (maxAdrenaline <= 0) return 0;
            return (double)adrenalineLevel / maxAdrenaline;
        }

        private double NormalizeMinHeight(int minHeight, int minMinHeight, int maxMinHeight)
        {
            if (maxMinHeight <= minMinHeight) return 0;
            // Lower height = higher score
            return 1 - ((double)(minHeight - minMinHeight) / (maxMinHeight - minMinHeight));
        }

        private double CalculateTypeBonus(AttractionType type)
        {
            return PreferredTypes.Contains(type.AttractionTypeId) ? 1.0 : 0.0;
        }
    }
}
