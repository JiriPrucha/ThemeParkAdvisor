using System.ComponentModel.DataAnnotations.Schema;

namespace ThemeParkAdvisor.Domain;

public class Attraction : ScoredEntity<Attraction>
{
    public int AttractionId { get; set; }
    public int ThemeParkId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AttractionTypeId { get; set; }
    public int AdrenalineRating { get; set; }
    public int ThemingRating { get; set; }
    public int? MinRiderHeight { get; set; }
    public int? MaxRiderHeight { get; set; }
    public bool IsForKids { get; set; }
    public bool IsForAdults { get; set; }

    public AttractionType AttractionType { get; set; } = null!;

    public Attraction() { }

    public Attraction(
        int attractionId,
        int themeParkId,
        string name,
        int attractionTypeId,
        int adrenalineRating,
        int themingRating,
        int minRiderHeight,
        int maxRiderHeight,
        bool isForKids,
        bool isForAdults,
        AttractionType type)
    {
        AttractionId = attractionId;
        ThemeParkId = themeParkId;
        Name = name;
        AttractionTypeId = attractionTypeId;
        AdrenalineRating = adrenalineRating;
        ThemingRating = themingRating;
        MinRiderHeight = minRiderHeight;
        MaxRiderHeight = maxRiderHeight;
        IsForKids = isForKids;
        IsForAdults = isForAdults;
        AttractionType = type;
    }
}