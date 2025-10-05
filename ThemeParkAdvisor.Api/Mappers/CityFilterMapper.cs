using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;

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
