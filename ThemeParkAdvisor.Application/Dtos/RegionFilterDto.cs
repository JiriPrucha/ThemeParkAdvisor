namespace ThemeParkAdvisor.Application
{
    public record RegionFilterDto(
        int? CountryId,
        int? CityId
    );
}
