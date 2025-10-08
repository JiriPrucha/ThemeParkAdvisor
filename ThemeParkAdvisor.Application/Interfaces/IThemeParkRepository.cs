using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public interface IThemeParkRepository
    {
        Task<List<ThemePark>> GetThemeParksAsync(ThemeParkFilter filter);
        Task<List<ThemeParkName>> GetThemeParkNamesAsync(ThemeParkNameFilter filter);
    }
}