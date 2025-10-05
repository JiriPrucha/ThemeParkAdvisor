namespace ThemeParkAdvisor.Domain
{
    public record AttractionRecommendation(
        string ThemeParkName,
        string Name,
        string TypeName,
        int AdrenalineLevel,
        int MinHeight,
        double Score
    );
}
