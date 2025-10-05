using ThemeParkAdvisor.Application;

namespace ThemeParkAdvisor.Infrastructure
{
    public class RandomScoringStrategy<TItem, TPreferences> : IGenericSelectionStrategy<TItem, TPreferences>
    {
        private readonly Random _random = new();

        public bool CanExecute(TPreferences preferences) => false;

        public Task<Dictionary<int, double>> ScoreAsync(TPreferences preferences, IEnumerable<TItem> items)
        {
            var scores = new Dictionary<int, double>();

            foreach (var item in items)
            {
                var idProp = item!.GetType().GetProperty("AttractionId")
                             ?? item.GetType().GetProperty("ThemeParkId")
                             ?? item.GetType().GetProperty("Id");

                if (idProp == null)
                    continue;

                var idVal = idProp.GetValue(item);
                if (idVal is int id)
                    scores[id] = _random.NextDouble();
            }

            return Task.FromResult(scores);
        }
    }
}