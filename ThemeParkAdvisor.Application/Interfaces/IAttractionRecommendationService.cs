using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public interface IAttractionRecommendationService
    {
        Task<IEnumerable<AttractionRecommendation>> RecommendAsync(AttractionPreferences preferences);
    }
}