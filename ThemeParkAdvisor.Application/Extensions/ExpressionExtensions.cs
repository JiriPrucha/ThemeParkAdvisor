using System.Linq.Expressions;

namespace ThemeParkAdvisor.Application
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Combines two boolean expressions into a single expression using logical AND (&&).
        /// </summary>
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            // Create a single parameter to be used by both expressions
            var parameter = Expression.Parameter(typeof(T));

            // Combine the two expressions with a logical AND
            var combined = Expression.AndAlso(
                Expression.Invoke(expr1, parameter),
                Expression.Invoke(expr2, parameter)
            );

            // Return a new lambda expression with the combined logic
            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }

        /// <summary>
        /// Combines two boolean expressions into a single expression using logical OR (||).
        /// </summary>
        public static Expression<Func<T, bool>> OrElse<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            // Create a single parameter to be used by both expressions
            var parameter = Expression.Parameter(typeof(T));

            // Combine the two expressions with a logical OR
            var combined = Expression.OrElse(
                Expression.Invoke(expr1, parameter),
                Expression.Invoke(expr2, parameter)
            );

            // Return a new lambda expression with the combined logic
            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }
    }
}