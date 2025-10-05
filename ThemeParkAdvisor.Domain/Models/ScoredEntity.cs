namespace ThemeParkAdvisor.Domain
{
    /// <summary>
    /// Abstract base class for entities that can be scored and compared based on their score.
    /// Implements IComparable and provides comparison operators.
    /// </summary>
    /// <typeparam name="T">The concrete type inheriting from this class.</typeparam>
    public abstract class ScoredEntity<T> : IComparable<T>, IScoredEntity<T>
        where T : ScoredEntity<T>
    {
        /// <summary>
        /// Delegate used to calculate the score for a given entity instance.
        /// </summary>
        public static Func<T, double> ScoreCalculator { get; set; }

        /// <summary>
        /// Returns the score of the current entity using the configured ScoreCalculator.
        /// Throws an exception if ScoreCalculator is not set.
        /// </summary>
        public double GetScore()
        {
            if (ScoreCalculator == null)
                throw new InvalidOperationException("ScoreCalculator is not set.");
            return ScoreCalculator((T)this);
        }

        /// <summary>
        /// Compares this entity to another entity of the same type based on their scores.
        /// Returns a positive number if this entity has a higher score, negative if lower, and zero if equal.
        /// </summary>
        public int CompareTo(T other) =>
            other == null ? 1 : GetScore().CompareTo(other.GetScore());

        // Operators based on entity scores.
        public static bool operator >(ScoredEntity<T> a, ScoredEntity<T> b) => a.GetScore() > b.GetScore();
        public static bool operator <(ScoredEntity<T> a, ScoredEntity<T> b) => a.GetScore() < b.GetScore();
        public static bool operator >=(ScoredEntity<T> a, ScoredEntity<T> b) => a.GetScore() >= b.GetScore();
        public static bool operator <=(ScoredEntity<T> a, ScoredEntity<T> b) => a.GetScore() <= b.GetScore();
        public override bool Equals(object obj) =>
            obj is T other && GetScore() == other.GetScore();

        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Combines multiple scoring strategies into a single scoring function by averaging their results.
        /// If no scorers are provided, returns 0.
        /// </summary>
        public static Func<T, double> CombineStrategies(params Func<T, double>[] scorers) =>
            entity => scorers.Length == 0 ? 0 : scorers.Average(s => s(entity));
    }
}