using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Infrastructure
{

    public class RandomThemeParkScoringStrategy : IFallbackThemeParkScoringStrategy
    {
        private readonly Random _random = new();

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
                        return false; // Something is not null or empty
                }
            }
            return true;
        }

        public Task<Dictionary<int, double>> ScoreAsync(
            ThemeParkPreferences preferences,
            IEnumerable<ThemePark> parks)
        {
            var scores = parks.ToDictionary(p => p.ThemeParkId, p => _random.NextDouble());
            return Task.FromResult(scores);
        }
    }
}