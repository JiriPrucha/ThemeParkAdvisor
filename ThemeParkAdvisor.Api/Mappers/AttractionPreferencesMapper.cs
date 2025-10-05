using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
    /// <summary>
    /// Provides mapping method for converting attraction preference DTO to domain models.
    /// </summary>
    public static class AttractionPreferencesMapper
    {
        /// <summary>
        /// Maps an <see cref="AttractionPreferencesDto"/> to the domain <see cref="AttractionPreferences"/> model.
        /// </summary>
        /// <param name="dto">The data transfer object containing attraction preference data.</param>
        /// <returns>A domain model representing the attraction preferences.</returns>
        public static AttractionPreferences ToDomain(this AttractionPreferencesDto dto)
        {
            return new AttractionPreferences(
                dto.ThemeParkToVisitId,
                dto.WantedAttractionsCount,
                dto.AdrenalineLevel,
                dto.MaxRequiredHeight
            );
        }
    }
}