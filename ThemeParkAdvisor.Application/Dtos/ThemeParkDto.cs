using System.ComponentModel.DataAnnotations.Schema;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public record ThemeParkDto(
        int ThemeParkId,
        string Name,
        int Size,
        int CityId,
        double Latitude,
        double Longitude,
        int ThemingRating,
        string WebsiteUrl,
        int AttractionCount,
        int RollerCoasterCount,
        City City
    );
}
