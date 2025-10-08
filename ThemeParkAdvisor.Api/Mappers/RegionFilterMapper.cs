using ThemeParkAdvisor.Shared;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Api
{
    public static class RegionFilterMapper
    {
        public static RegionFilter ToDomain(this RegionFilterDto dto)
        {
            return new RegionFilter(
                dto.CountryId,
                dto.CityId
            );
        }
    }
}
