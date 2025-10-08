namespace ThemeParkAdvisor.Shared
{
    public record ThemeParkNameFilterDto(
        int? CountryId,
        int? RegionId,
        int? CityId
    );
}
