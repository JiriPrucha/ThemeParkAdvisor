namespace ThemeParkAdvisor.Domain
{
    public interface IScoredEntity<T>
    {
        static Func<T, double> ScoreCalculator { get; set; }
        double GetScore();
    }
}
