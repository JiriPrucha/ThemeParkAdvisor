using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
    public static class ThemeParkRecommendationMapper
    {
        /// <summary>
        /// Maps a <see cref="ThemeParkRecommendation"/> to the domain <see cref="ThemeParkRecommendationDto"/> model.
        /// </summary>
        /// <param name="domain">The data transfer object containing theme park recommendation data.</param>
        /// <returns>A domain model representing the theme park recommendation.</returns>
        public static ThemeParkRecommendationDto ToDto(this ThemeParkRecommendation domain)
        {
            return new ThemeParkRecommendationDto(
                domain.Name,
                domain.Size,
                domain.AttractionCount,
                domain.RollerCoasterCount,
                domain.CountryName,
                domain.RegionName,
                domain.CityName,
                domain.Latitude,
                domain.Longitude,
                domain.WebsiteUrl,
                domain.Score
            );
        }
    }
}


/// <summary>
/// Provides mapping method for converting theme park preference DTO to domain models.
/// </summary>
public static class ThemeParkPreferencesMapper
{
    /// <summary>
    /// Maps a <see cref="ThemeParkPreferencesDto"/> to the domain <see cref="ThemeParkPreferences"/> model.
    /// </summary>
    /// <param name="dto">The data transfer object containing theme park preference data.</param>
    /// <returns>A domain model representing the theme park preferences.</returns>
    public static ThemeParkPreferences ToDomain(this ThemeParkPreferencesDto dto)
    {
        return new ThemeParkPreferences(
            dto.FavoriteThemeParkId,
            dto.ThemeParksNotToVisitIds,
            dto.UserAges,
            dto.CountryId,
            dto.RegionId,
            dto.CityId,
            dto.AdrenalineLevel,
            dto.Latitude,
            dto.Longitude,
            dto.WantedThemeParksCount,
            dto.MinAttractionsCount
        );
    }
}