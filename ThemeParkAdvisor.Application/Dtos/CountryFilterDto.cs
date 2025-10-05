namespace ThemeParkAdvisor.Application
{
    public record CountryFilterDto(
        int? RegionId,
        int? CityId
    );
}
