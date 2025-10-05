using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public interface IRegionRepository
    {
        Task<List<RegionName>> GetRegionsAsync(RegionFilter filter);
    }
}
