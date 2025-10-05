namespace ThemeParkAdvisor.Shared
{
    public record AttractionRecommendationDto(
        string Name,
        string TypeName,
        int AdrenalineLevel,
        int MinHeight,
        double Score
    );
}