namespace ThemeParkAdvisor.Domain
{
    public record CountryFilter(
        int? RegionId,
        int? CityId
    );
}
