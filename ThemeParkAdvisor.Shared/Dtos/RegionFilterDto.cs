namespace ThemeParkAdvisor.Shared
{
    public record RegionFilterDto(
        int? CountryId = null,
        int? CityId = null
    );
}
