using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
    public static class CountryNameMapper
    {
        public static CountryNameDto ToDto(this CountryName domain)
        {
            return new CountryNameDto(
                domain.CountryId,
                domain.Name
            );
        }
    }
}
