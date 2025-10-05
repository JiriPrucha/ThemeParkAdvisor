using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Api
{
    public static class ThemeParkMapper
    {
        public static ThemeParkDto ToDto(this ThemePark domain)
        {
            return new ThemeParkDto(
                domain.ThemeParkId,
                domain.Name,
                domain.Size,
                domain.CityId,
                domain.Latitude,
                domain.Longitude,
                domain.ThemingRating,
                domain.WebsiteUrl,
                domain.AttractionCount ?? 0,
                domain.RollerCoasterCount ?? 0,
                domain.City
            );
        }

        public static ThemePark ToDomain(this ThemeParkDto dto)
        {
            return new ThemePark(
                dto.ThemeParkId,
                dto.Name,
                dto.Size,
                dto.CityId,
                dto.Latitude,
                dto.Longitude,
                dto.ThemingRating,
                dto.WebsiteUrl,
                dto.AttractionCount,
                dto.RollerCoasterCount,
                dto.City
            );
        }
    }
}
