using System.ComponentModel.DataAnnotations;

namespace ThemeParkAdvisor.Shared
{
    public record ThemeParkPreferencesDto(
    int? FavoriteThemeParkId,
    IReadOnlyList<int>? ThemeParksNotToVisitIds,
    IReadOnlyList<int>? UserAges,
    int? CountryId,
    int? RegionId,
    int? CityId,
    [Range(0, 10, ErrorMessage = "Úroveň adrenalinu musí být mezi {1} a {2}.")]
    int? AdrenalineLevel,
    double? Latitude,
    double? Longitude,
    int? WantedThemeParksCount,
    int? MinAttractionsCount
);
}
