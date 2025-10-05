using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Application
{
    public class RecommendationResult
    {
        public ThemePark? ThemePark { get; set; }
        public double AverageScore { get; set; }
    }
}