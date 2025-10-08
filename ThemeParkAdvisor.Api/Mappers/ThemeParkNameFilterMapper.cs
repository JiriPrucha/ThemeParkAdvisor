using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
    public static class ThemeParkNameFilterMapper
    {
        public static ThemeParkNameFilter ToDomain(this ThemeParkNameFilterDto dto)
        {
            return new ThemeParkNameFilter(
                dto.CountryId,
                dto.RegionId,
                dto.CityId
            );
        }
    }
}
