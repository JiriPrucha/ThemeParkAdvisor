namespace ThemeParkAdvisor.Domain
{
    public record CityFilter(
        int? CountryId,
        int? RegionId
    );
}
