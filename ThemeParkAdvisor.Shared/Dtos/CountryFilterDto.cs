namespace ThemeParkAdvisor.Shared
{
    public record CountryFilterDto(
        int? RegionId = null,
        int? CityId = null
    );
}
