namespace ThemeParkAdvisor.Domain
{
    public interface IScorable<T>
    {
        int Id { get; }
        Func<T, double> ScoreCalculator { get; set; }
        double GetScore();
    }
}