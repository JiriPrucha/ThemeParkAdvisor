using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public interface IThemeParkScoringStrategy : IScoringStrategy<ThemePark, ThemeParkPreferences>
    {
    }
}