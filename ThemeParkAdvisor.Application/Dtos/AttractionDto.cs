using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public record AttractionDto(
        int AttractionId,
        int ThemeParkId,
        string Name,
        AttractionType Type,
        int AdrenalineLevel,
        int MinHeight
    );
}