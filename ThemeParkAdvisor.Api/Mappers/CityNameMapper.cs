using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
    public static class CityNameMapper
    {
        public static CityNameDto ToDto(this CityName domain)
        {
            return new CityNameDto(
                domain.CityId,
                domain.Name
            );
        }
    }
}
