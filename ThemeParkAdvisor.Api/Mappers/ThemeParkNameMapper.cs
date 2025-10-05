using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api;

public static class ThemeParkNameMapper
{
    public static ThemeParkNameDto ToDto(this ThemeParkName domain)
    {
        return new ThemeParkNameDto(
            domain.ThemeParkId,
            domain.Name
        );
    }
}
