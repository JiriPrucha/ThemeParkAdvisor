using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Application;

namespace ThemeParkAdvisor.Infrastructure
{
    /// <summary>
    /// Strategy for selecting theme parks based on the ages of the users.
    /// Each age group has defined adrenaline level ranges, minimum theming score,
    /// and minimum number of attractions required.
    /// </summary>
    public class AgeBasedThemeParkScoringStrategy : IThemeParkScoringStrategy
    {
        private readonly IAttractionRepository _attractionRepository;

        // General configuration constants
        private const int MinAttractionsRequired = 5;
        private const double ThemingScaleMax = 10.0;

        // Age group boundaries
        private const int MaxChildrenAge = 9;
        private const int MaxTeenAge = 17;
        private const int MaxAdultAge = 65;

        // Adrenaline level ranges for each age group
        private const int ChildrenMinAdrenaline = 0;
        private const int ChildrenMaxAdrenaline = 3;
        private const double ChildrenMinTheming = 0.5;

        private const int TeenMinAdrenaline = 6;
        private const int TeenMaxAdrenaline = 10;
        private const double TeenMinTheming = 0.0;

        private const int AdultMinAdrenaline = 4;
        private const int AdultMaxAdrenaline = 9;
        private const double AdultMinTheming = 0.25;

        private const int SeniorMinAdrenaline = 0;
        private const int SeniorMaxAdrenaline = 4;
        private const double SeniorMinTheming = 0.5;

        public AgeBasedThemeParkScoringStrategy(IAttractionRepository attractionRepository)
        {
            _attractionRepository = attractionRepository;
        }

        /// <summary>
        /// Determines if this strategy can be executed based on provided preferences.
        /// </summary>
        public bool CanExecute(ThemeParkPreferences preferences)
        {
            return preferences.UserAges != null && preferences.UserAges.Any();
        }

        /// <summary>
        /// Scores each theme park based on whether it meets the criteria for all user ages.
        /// </summary>
        public async Task<Dictionary<int, double>> ScoreAsync(
            ThemeParkPreferences preferences,
            IEnumerable<ThemePark> parks)
        {
            var scores = new Dictionary<int, double>();

            foreach (var park in parks)
            {
                var attractions = await _attractionRepository
                    .GetAttractionsByThemeParkIdAsync(park.ThemeParkId);

                bool suitableForAll = preferences.UserAges
                    .All(age => MeetsCriteriaForAge(age, park, attractions));

                scores[park.ThemeParkId] = suitableForAll ? 1.0 : 0.0;
            }

            return scores;
        }

        /// <summary>
        /// Checks if a park meets the criteria for a specific age group.
        /// </summary>
        private bool MeetsCriteriaForAge(int age, ThemePark park, IEnumerable<Attraction> attractions)
        {
            var (minAdr, maxAdr, minTheming, minCount) = GetCriteriaForAge(age);

            // Count attractions within the adrenaline range
            int relevantCount = attractions.Count(a =>
                a.AdrenalineRating >= minAdr &&
                a.AdrenalineRating <= maxAdr);

            // Normalize theming score to 0–1 range
            double normalizedTheming = Math.Clamp(park.ThemingRating / ThemingScaleMax, 0.0, 1.0);

            bool themingOk = normalizedTheming >= minTheming;

            return relevantCount >= minCount && themingOk;
        }

        /// <summary>
        /// Returns criteria for adrenaline level, theming, and minimum attraction count based on age.
        /// </summary>
        private (int minAdr, int maxAdr, double minTheming, int minCount) GetCriteriaForAge(int age)
        {
            if (age <= MaxChildrenAge) // Young children
                return (ChildrenMinAdrenaline, ChildrenMaxAdrenaline, ChildrenMinTheming, MinAttractionsRequired);
            if (age <= MaxTeenAge) // Teenagers
                return (TeenMinAdrenaline, TeenMaxAdrenaline, TeenMinTheming, MinAttractionsRequired);
            if (age < MaxAdultAge) // Adults
                return (AdultMinAdrenaline, AdultMaxAdrenaline, AdultMinTheming, MinAttractionsRequired);
            // Seniors
            return (SeniorMinAdrenaline, SeniorMaxAdrenaline, SeniorMinTheming, MinAttractionsRequired);
        }
    }
}