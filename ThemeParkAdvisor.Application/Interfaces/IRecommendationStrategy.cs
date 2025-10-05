namespace ThemeParkAdvisor.Application
{
    public interface IRecommendationStrategy<TItem, TPreferences>
    {
        bool CanExecute(TPreferences preferences);
        Task<Dictionary<int, double>> ScoreAsync(TPreferences preferences, IEnumerable<TItem> items);
    }
}