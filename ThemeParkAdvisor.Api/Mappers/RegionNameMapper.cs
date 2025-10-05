using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
    public static class RegionNameMapper
    {
        public static RegionNameDto ToDto(this RegionName domain)
        {
            return new RegionNameDto(
                domain.RegionId,
                domain.Name
            );
        }
    }
}
