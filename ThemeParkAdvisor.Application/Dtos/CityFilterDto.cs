namespace ThemeParkAdvisor.Application
{
    public record CityFilterDto(
        int? CountryId = null,
        int? RegionId = null
    );
}
