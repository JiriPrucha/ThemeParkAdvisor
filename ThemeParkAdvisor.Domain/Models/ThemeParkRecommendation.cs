namespace ThemeParkAdvisor.Domain
{
    public record ThemeParkRecommendation(
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