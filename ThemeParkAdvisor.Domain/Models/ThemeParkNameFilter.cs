namespace ThemeParkAdvisor.Domain
{
    public record ThemeParkNameFilter(
        int? CountryId,
        int? RegionId,
        int? CityId
    );
}
