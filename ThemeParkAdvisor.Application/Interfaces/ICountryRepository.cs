using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public interface ICountryRepository
    {
        Task<List<CountryName>> GetCountriesAsync(CountryFilter filter);
    }
}
