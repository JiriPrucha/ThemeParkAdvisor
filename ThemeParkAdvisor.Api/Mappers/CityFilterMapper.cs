using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
    public static class CityFilterMapper
    {
        public static CityFilter ToDomain(this CityFilterDto dto)
        {
            return new CityFilter(
                dto.CountryId,
                dto.RegionId
            );
        }
    }
}
