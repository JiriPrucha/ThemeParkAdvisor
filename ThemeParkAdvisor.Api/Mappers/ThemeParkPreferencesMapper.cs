using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
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
}