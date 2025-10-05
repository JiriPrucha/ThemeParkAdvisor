using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Api
{
    public static class AttractionFilterMapper
    {
        public static AttractionFilter ToDomain(this AttractionFilterDto dto)
        {
            return new AttractionFilter(
                dto.ThemeParkId,
                dto.AdrenalineLevel,
                dto.MaxRequiredHeight
            );
        }
    }
}
