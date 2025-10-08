namespace ThemeParkAdvisor.Shared
{
    public record CityFilterDto(
        int? CountryId = null,
        int? RegionId = null
    );
}
