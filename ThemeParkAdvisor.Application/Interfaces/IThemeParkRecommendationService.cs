using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public interface IThemeParkRecommendationService
    {
        Task<IEnumerable<ThemeParkRecommendation>> RecommendAsync(ThemeParkPreferences preferences);
    }
}
