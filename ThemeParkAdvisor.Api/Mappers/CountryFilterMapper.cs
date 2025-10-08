using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Shared;

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
