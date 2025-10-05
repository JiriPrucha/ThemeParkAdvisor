using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Api
{
    public static class CountryFilterMapper
    {
        public static CountryFilter ToDomain(this CountryFilterDto dto)
        {
            return new CountryFilter(
                dto.RegionId,
                dto.CityId
            );
        }
    }
}
