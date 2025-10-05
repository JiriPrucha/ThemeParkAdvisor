using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public interface IThemeParkScorer
    {
        double Score(ThemePark park, ThemeParkPreferences prefs);
    }
}