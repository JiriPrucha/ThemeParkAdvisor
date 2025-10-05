namespace ThemeParkAdvisor.Domain
{
    public record AttractionFilter(
        int? ThemeParkId,
        int? AdrenalineRating,
        int? MaxRequiredHeight
    );
}