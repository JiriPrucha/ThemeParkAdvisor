using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Application;

namespace ThemeParkAdvisor.Api
{
    public static class AttractionMapper
    {
        public static AttractionDto ToDto(this Attraction domain)
        {
            return new AttractionDto(
                domain.AttractionId,
                domain.ThemeParkId,
                domain.Name,
                domain.AttractionType,
                domain.AdrenalineRating,
                domain.MinRiderHeight ?? 0
                );
        }
    }
}
