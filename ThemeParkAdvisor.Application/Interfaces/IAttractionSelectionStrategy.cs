using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public interface IAttractionSelectionStrategy : IScoringStrategy<Attraction, AttractionPreferences>
    {
    }
}