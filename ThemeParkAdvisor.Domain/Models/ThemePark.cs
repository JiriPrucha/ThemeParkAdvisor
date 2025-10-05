using System.ComponentModel.DataAnnotations.Schema;

namespace ThemeParkAdvisor.Domain;

public class ThemePark : ScoredEntity<ThemePark>
{
    public int ThemeParkId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Size { get; set; }
    public int CityId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int ThemingRating { get; set; }
    public string WebsiteUrl { get; set; } = string.Empty;

    [NotMapped]
    public int? AttractionCount { get; set; }

    [NotMapped]
    public int? RollerCoasterCount { get; set; }

    public City City { get; set; } = null!;

    public ThemePark() { }

    public ThemePark(
        int themeParkId,
        string name,
        int size,
        int cityId,
        double latitude,
        double longitude,
        int themingRating,
        string websiteUrl,
        int? attractionCount,
        int? rollerCoasterCount,
        City City)
    {
        ThemeParkId = themeParkId;
        Name = name;
        Size = size;
        CityId = cityId;
        Latitude = latitude;
        Longitude = longitude;
        ThemingRating = themingRating;
        WebsiteUrl = websiteUrl;
        AttractionCount = attractionCount;
        RollerCoasterCount = rollerCoasterCount;
    }
}
