using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Infrastructure
{
    public interface IThemeParkNameRepository
    {
        Task<List<ThemeParkName>> GetAllThemeParkNamesAsync();
    }
}
