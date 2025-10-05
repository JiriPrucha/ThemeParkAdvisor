namespace ThemeParkAdvisor.Domain
{
    public class AttractionType
    {
        public int AttractionTypeId { get; set; }
        public string Name { get; set; } = string.Empty;

        public AttractionType() { }

        public AttractionType(
            int attractionTypeId,
            string name
            )
        {
            AttractionTypeId = attractionTypeId;
            Name = name;
        }
    }
}
