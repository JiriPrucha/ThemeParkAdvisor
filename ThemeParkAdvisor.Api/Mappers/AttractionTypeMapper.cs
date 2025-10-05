using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
    public static class AttractionTypeMapper
    {
        public static AttractionTypeDto ToDto(this AttractionType domain)
        {
            return new AttractionTypeDto(
                domain.AttractionTypeId,
                domain.Name
            );
        }
    }
}
