using ThemeParkAdvisor.Domain;

public class Region
{
    public int RegionId { get; set; }
    public int CountryId { get; set; }
    public string Name { get; set; } = string.Empty;

    public Country Country { get; set; } = null!;
}