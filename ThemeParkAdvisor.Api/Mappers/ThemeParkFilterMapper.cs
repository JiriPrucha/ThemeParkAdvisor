using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Application;

namespace ThemeParkAdvisor.Api
{
    /// <summary>
    /// Provides mapping method for converting Theme park filter DTO to domain models.
    /// </summary>
    public static class ThemeParkFilterMapper
    {
        /// <summary>
        /// Maps a <see cref="ThemeParkFilterDto"/> to the domain <see cref="ThemeParkFilter"/> model.
        /// </summary>
        /// <param name="dto">The data transfer object containing theme park filter data.</param>
        /// <returns>A domain model representing the theme park preferences.</returns>
        public static ThemeParkFilter ToDomain(this ThemeParkFilterDto dto)
        {
            return new ThemeParkFilter(
                dto.CountryId,
                dto.RegionId,
                dto.CityId,
                dto.FavoriteThemeParkId,
                dto.ThemeParksNotToVisitIds,
                dto.MinAttractionsCount,
                dto.MaxAttractionsCount,
                dto.MinRollerCoasterCount,
                dto.MaxRollerCoasterCount
            );
        }
    }
}
