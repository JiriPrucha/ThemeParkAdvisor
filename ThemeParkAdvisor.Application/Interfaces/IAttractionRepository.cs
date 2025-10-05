using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public interface IAttractionRepository
    {
        Task<List<Attraction>> GetAttractionsByThemeParkIdAsync(int themeParkId);
        Task<List<Attraction>> GetAllAttractionsAsync();
        Task<List<Attraction>> GetAttractionsAsync(AttractionFilter filter);
        Task<List<AttractionType>> GetAttractionTypes();
    }
}
