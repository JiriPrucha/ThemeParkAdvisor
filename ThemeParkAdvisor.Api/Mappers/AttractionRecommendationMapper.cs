using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
    public static class AttractionRecommendationMapper
    {
        public static AttractionRecommendationDto ToDto(this AttractionRecommendation domain)
        {
            return new AttractionRecommendationDto(
                domain.Name,
                domain.TypeName,
                domain.AdrenalineLevel,
                domain.MinHeight,
                domain.Score
            );
        }
    }
}
