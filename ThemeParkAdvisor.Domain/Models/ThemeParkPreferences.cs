namespace ThemeParkAdvisor.Domain
{
    public record ThemeParkPreferences(
        int? FavoriteThemeParkId,
        IReadOnlyList<int>? ThemeParksNotToVisitIds,
        IReadOnlyList<int>? UserAges,
        int? CountryId,
        int? RegionId,
        int? CityId,
        int? AdrenalineLevel,
        double? Latitude,
        double? Longitude,
        int? WantedThemeParksCount,
        int? MinAttractionsCount
    );
}