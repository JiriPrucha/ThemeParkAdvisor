namespace ThemeParkAdvisor.Domain
{
    public record AttractionPreferences(
        int? ThemeParkToVisitId,
        int? WantedAttractionsCount,
        int? AdrenalineRating,
        int? MaxRequiredHeight
    );
}