namespace ThemeParkAdvisor.Shared
{
    public record AttractionPreferencesDto(
        int? ThemeParkToVisitId,
        int? WantedAttractionsCount,
        int? AdrenalineLevel,
        int? MaxRequiredHeight
    );
}