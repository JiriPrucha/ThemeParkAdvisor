namespace ThemeParkAdvisor.Application
{
    public record AttractionFilterDto(
        int? ThemeParkId = null,
        int? AdrenalineLevel = null,
        int? MaxRequiredHeight = null
    );
}