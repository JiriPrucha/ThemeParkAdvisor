namespace ThemeParkAdvisor.Application
{
    public record ThemeParkFilterDto(
        int? CountryId = null,
        int? RegionId = null,
        int? CityId = null,
        int? FavoriteThemeParkId = null,
        IReadOnlyList<int>? ThemeParksNotToVisitIds = null,
        int? MinAttractionsCount = null,
        int? MaxAttractionsCount = null,
        int? MinRollerCoasterCount = null,
        int? MaxRollerCoasterCount = null
    );
}