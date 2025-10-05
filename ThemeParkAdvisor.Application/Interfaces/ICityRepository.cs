using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public interface ICityRepository
    {
        Task<List<CityName>> GetCitiesAsync(CityFilter filter);
    }
}
