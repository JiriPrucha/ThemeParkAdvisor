namespace ThemeParkAdvisor.Application
{
    public interface IRecommendationService<TEntity, TPreferences, TRecommendation>
    {
        Task<IEnumerable<TRecommendation>> RecommendAsync(TPreferences preferences);
    }
}
