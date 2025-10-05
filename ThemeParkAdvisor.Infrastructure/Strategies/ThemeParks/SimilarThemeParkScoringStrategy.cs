using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Infrastructure
{
    /// <summary>
    /// Strategy for selecting theme parks similar to the user's favorite.
    /// </summary>
    public class SimilarThemeParkScoringStrategy : IThemeParkScoringStrategy
    {
        private readonly IAttractionRepository _attractionRepository;

        // Max differences for normalization
        private const double MaxThemingDifference = 10.0;
        private const double MaxCoasterDifference = 20.0;
        private const double MaxAttractionCountDifference = 50.0;
        private const double MaxAdrenalineDifference = 10.0;

        // Weights for base similarity
        private const double BaseThemingWeight = 0.6;
        private const double BaseCoasterWeight = 0.25;
        private const double BaseAttractionCountWeight = 0.15;

        // Weights for attraction similarity
        private const double TypeSimilarityWeight = 0.6;
        private const double AdrenalineSimilarityWeight = 0.4;

        // Bonus scaling
        private const double MaxAttractionBonus = 0.2;

        // Limit for how many parks get attraction bonus
        private const int TopParksForBonus = 10;

        public SimilarThemeParkScoringStrategy(IAttractionRepository attractionRepository)
        {
            _attractionRepository = attractionRepository;
        }

        public bool CanExecute(ThemeParkPreferences preferences)
        {
            return preferences?.FavoriteThemeParkId is not null;
        }

        public async Task<Dictionary<int, double>> ScoreAsync(
            ThemeParkPreferences preferences,
            IEnumerable<ThemePark> parks)
        {
            var favoriteId = preferences.FavoriteThemeParkId;
            var parkList = parks.ToList();
            var favorite = parkList.FirstOrDefault(p => p.ThemeParkId == favoriteId);

            if (favorite == null)
                return new Dictionary<int, double>();

            // Load attractions for all
            var parkAttractions = await LoadAttractionsForParksAsync(parkList);

            // Calculate base similarity for all except favorite
            var scored = parkList
                .Where(p => p.ThemeParkId != favoriteId)
                .Select(p => (Park: p, Score: CalculateBaseSimilarity(favorite, p)))
                .OrderByDescending(x => x.Score)
                .ToList();

            // Add attraction similarity bonus for top N
            for (int i = 0; i < scored.Count && i < TopParksForBonus; i++)
            {
                var attractionBoost = CalculateAttractionSimilarity(
                    parkAttractions[favorite.ThemeParkId],
                    parkAttractions[scored[i].Park.ThemeParkId]
                );

                scored[i] = (scored[i].Park, scored[i].Score + attractionBoost);
            }

            return scored
                .OrderByDescending(x => x.Score)
                .ToDictionary(x => x.Park.ThemeParkId, x => x.Score);
        }

        // Base similarity
        private double CalculateBaseSimilarity(ThemePark a, ThemePark b)
        {
            double themingSim = NormalizeDifference(a.ThemingRating, b.ThemingRating, MaxThemingDifference);
            double coasterSim = NormalizeDifference(a.RollerCoasterCount ?? 0, b.RollerCoasterCount ?? 0, MaxCoasterDifference);
            double attractionSim = NormalizeDifference(a.AttractionCount ?? 0, b.AttractionCount ?? 0, MaxAttractionCountDifference);

            return themingSim * BaseThemingWeight +
                   coasterSim * BaseCoasterWeight +
                   attractionSim * BaseAttractionCountWeight;
        }

        // Attraction similarity
        private double CalculateAttractionSimilarity(IEnumerable<Attraction> aAttr, IEnumerable<Attraction> bAttr)
        {
            if (!aAttr.Any() || !bAttr.Any()) return 0;

            double typeScore = CalculateTypeSimilarity(aAttr, bAttr);
            double adrenalineScore = CalculateAdrenalineSimilarity(aAttr, bAttr);

            return (typeScore * TypeSimilarityWeight + adrenalineScore * AdrenalineSimilarityWeight) * MaxAttractionBonus;
        }

        private double CalculateTypeSimilarity(IEnumerable<Attraction> aAttr, IEnumerable<Attraction> bAttr)
        {
            var typeMatch = aAttr.Select(x => x.AttractionType).Intersect(bAttr.Select(x => x.AttractionType)).Count();
            return (double)typeMatch / Math.Max(aAttr.Count(), bAttr.Count());
        }

        private double CalculateAdrenalineSimilarity(IEnumerable<Attraction> aAttr, IEnumerable<Attraction> bAttr)
        {
            double adrenalineDiff = Math.Abs(aAttr.Average(x => x.AdrenalineRating) - bAttr.Average(x => x.AdrenalineRating));
            return NormalizeDifference(0, adrenalineDiff, MaxAdrenalineDifference);
        }

        // Helpers
        private double NormalizeDifference(double valueA, double valueB, double maxDifference)
        {
            if (maxDifference <= 0) return 0;
            return 1 - Math.Abs(valueA - valueB) / maxDifference;
        }

        private async Task<Dictionary<int, List<Attraction>>> LoadAttractionsForParksAsync(IEnumerable<ThemePark> parks)
        {
            var result = new Dictionary<int, List<Attraction>>();
            foreach (var park in parks)
            {
                var attractions = await _attractionRepository.GetAttractionsByThemeParkIdAsync(park.ThemeParkId);
                result[park.ThemeParkId] = attractions.ToList();
            }
            return result;
        }
    }
}