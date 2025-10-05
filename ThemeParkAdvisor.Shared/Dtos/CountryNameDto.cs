using System;
namespace ThemeParkAdvisor.Shared
{
    public record CountryNameDto(
        int? CountryId,
        string? Name
    );
}
