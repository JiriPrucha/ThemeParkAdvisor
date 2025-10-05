namespace ThemeParkAdvisor.Application
{
    public interface IScoringStrategy<TEntity, TPreferences>
    {
        bool CanExecute(TPreferences preferences);
        Task<Dictionary<int, double>> ScoreAsync(TPreferences preferences, IEnumerable<TEntity> items);
    }
}
