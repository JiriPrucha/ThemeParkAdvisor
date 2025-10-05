namespace ThemeParkAdvisor.Shared
{
    public record ThemeParkRecommendationDto(
        string Name,
        int Size,
        int AttractionCount,
        int RollerCoasterCount,
        string CountryName,
        string RegionName,
        string CityName,
        double Latitude,
        double Longitude,
        string WebsiteUrl,
        double Score
    );
}