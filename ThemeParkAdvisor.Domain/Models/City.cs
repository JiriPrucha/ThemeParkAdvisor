namespace ThemeParkAdvisor.Domain
{
    public class City
    {
        public int CityId { get; set; }
        public int RegionId { get; set; }
        public string Name { get; set; } = string.Empty;

        public Region Region { get; set; } = null!;
    }
}