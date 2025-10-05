using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Application;

namespace ThemeParkAdvisor.Infrastructure
{
    /// <summary>
    /// Theme park scoring strategy based on its statistics.
    /// </summary>
    public class StatisticsBasedThemeParkSelectionStrategy : IFallbackThemeParkScoringStrategy
    {
        private readonly IAttractionRepository _attractionRepository;

        // Weights for each metric
        private const double AdrenalineWeight = 0.25;
        private const double ThemingWeight = 0.20;
        private const double CoasterCountWeight = 0.20;
        private const double AttractionCountWeight = 0.15;
        private const double SizeWeight = 0.10;
        private const double VarietyWeight = 0.10;

        // Normalization constants
        private const double MaxAdrenalineLevel = 10.0;
        private const double MaxThemingScore = 10.0;

        // Size thresholds
        private const int SmallParkThreshold = 30;
        private const int MediumParkThreshold = 80;

        // Size scores
        private const double SmallParkScore = 0.3;
        private const double MediumParkScore = 0.6;
        private const double LargeParkScore = 1.0;

        public StatisticsBasedThemeParkSelectionStrategy(IAttractionRepository attractionRepository)
        {
            _attractionRepository = attractionRepository;
        }

        public bool CanExecute(ThemeParkPreferences preferences)
        {
            foreach (var prop in typeof(ThemeParkPreferences).GetProperties())
            {
                var value = prop.GetValue(preferences);
                if (value == null)
                    continue;

                switch (value)
                {
                    case string s when string.IsNullOrWhiteSpace(s):
                        continue;
                    case IReadOnlyList<int> list when list.Count == 0:
                        continue;
                    default:
                        return true; // Something is not null or empty
                }
            }
            return false;
        }

        public async Task<Dictionary<int, double>> ScoreAsync(
            ThemeParkPreferences preferences,
            IEnumerable<ThemePark> parks)
        {
            var parkList = parks.ToList();
            var scores = new Dictionary<int, double>();

            if (!parkList.Any())
                return scores;

            // Precompute normalization bounds
            int maxCoasters = parkList.Max(p => p.RollerCoasterCount ?? 0);
            int maxAttractions = parkList.Max(p => p.AttractionCount ?? 0);

            // Load all attractions for all parks in one go to avoid repeated DB calls
            var parkAttractions = new Dictionary<int, List<Attraction>>();
            foreach (var park in parkList)
            {
                var attractions = await _attractionRepository.GetAttractionsByThemeParkIdAsync(park.ThemeParkId);
                parkAttractions[park.ThemeParkId] = attractions.ToList();
            }

            // Compute maxTypes across all parks
            int maxTypes = parkAttractions.Values
                .SelectMany(a => a)
                .Select(a => a.AttractionType)
                .Distinct()
                .Count();

            foreach (var park in parkList)
            {
                var attractions = parkAttractions[park.ThemeParkId];

                double adrenalineScore = CalculateAdrenalineScore(attractions);
                double themingScore = NormalizeToUnitRange(park.ThemingRating, MaxThemingScore);
                double coasterScore = NormalizeCount(park.RollerCoasterCount ?? 0, maxCoasters);
                double attractionCountScore = NormalizeCount(park.AttractionCount ?? 0, maxAttractions);
                double sizeScore = SizeToScore(park.Size);
                double varietyScore = CalculateVarietyScore(attractions, maxTypes);

                double finalScore =
                    adrenalineScore * AdrenalineWeight +
                    themingScore * ThemingWeight +
                    coasterScore * CoasterCountWeight +
                    attractionCountScore * AttractionCountWeight +
                    sizeScore * SizeWeight +
                    varietyScore * VarietyWeight;

                scores[park.ThemeParkId] = Math.Clamp(finalScore, 0, 1);
            }

            return scores;
        }

        private double CalculateAdrenalineScore(IEnumerable<Attraction> attractions)
        {
            if (!attractions.Any()) return 0;
            double avgAdrenaline = attractions.Average(a => a.AdrenalineRating);
            return NormalizeToUnitRange(avgAdrenaline, MaxAdrenalineLevel);
        }

        private double NormalizeToUnitRange(double value, double maxValue)
        {
            if (maxValue <= 0) return 0;
            return Math.Clamp(value / maxValue, 0, 1);
        }

        private double NormalizeCount(int count, int maxCount)
        {
            if (maxCount <= 0) return 0;
            return (double)count / maxCount;
        }

        private double SizeToScore(int size)
        {
            if (size <= SmallParkThreshold) return SmallParkScore;
            if (size <= MediumParkThreshold) return MediumParkScore;
            return LargeParkScore;
        }

        private double CalculateVarietyScore(IEnumerable<Attraction> attractions, int maxTypes)
        {
            if (!attractions.Any() || maxTypes <= 0) return 0;
            int uniqueTypes = attractions.Select(a => a.AttractionType).Distinct().Count();
            return (double)uniqueTypes / maxTypes;
        }
    }
}