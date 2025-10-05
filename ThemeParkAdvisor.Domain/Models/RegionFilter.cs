namespace ThemeParkAdvisor.Domain
{
    public record RegionFilter(
        int? CountryId,
        int? CityId
    );
}
