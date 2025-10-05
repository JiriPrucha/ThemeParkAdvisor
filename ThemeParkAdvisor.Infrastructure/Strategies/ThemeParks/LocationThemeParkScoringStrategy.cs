using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Infrastructure
{
    /// <summary>
    /// Strategy that scores theme parks based on their geographic distance from the user's location.
    /// Parks within MaxScoringDistanceKm get a score between 1 and 0, decreasing with distance.
    /// Parks farther than MaxScoringDistanceKm get a score of 0.
    /// </summary>
    public class LocationThemeParkScoringStrategy : IThemeParkScoringStrategy
    {
        private const double EarthRadiusKm = 6378.0;
        private const double DegreesToRadians = Math.PI / 180.0;

        // Maximum distance at which a park still gets a non-zero score
        private const double MaxScoringDistanceKm = 300.0;

        public bool CanExecute(ThemeParkPreferences preferences)
            => preferences.Latitude.HasValue && preferences.Longitude.HasValue;

        public Task<Dictionary<int, double>> ScoreAsync(
            ThemeParkPreferences preferences,
            IEnumerable<ThemePark> parks)
        {
            if (preferences.Latitude == null || preferences.Longitude == null)
                return Task.FromResult(new Dictionary<int, double>());

            var scores = parks.ToDictionary(
                p => p.ThemeParkId,
                p =>
                {
                    double dist = GetDistance(
                        preferences.Latitude.Value,
                        preferences.Longitude.Value,
                        p.Latitude,
                        p.Longitude);

                    return NormalizeDistance(dist, MaxScoringDistanceKm);
                });

            return Task.FromResult(scores);
        }

        /// <summary>
        /// Calculates the great-circle distance between two points on Earth using the Haversine formula.
        /// </summary>
        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double dLat = (lat2 - lat1) * DegreesToRadians;
            double dLon = (lon2 - lon1) * DegreesToRadians;

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * DegreesToRadians) * Math.Cos(lat2 * DegreesToRadians) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadiusKm * c;
        }

        /// <summary>
        /// Converts a raw distance into a normalized score between 0 and 1 based on a fixed maximum scoring distance.
        /// </summary>
        private double NormalizeDistance(double distance, double maxDistance)
        {
            if (distance >= maxDistance) return 0.0;
            return 1 - (distance / maxDistance);
        }
    }
}