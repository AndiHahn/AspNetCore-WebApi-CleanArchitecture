using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace CleanArchitecture.Infrastructure.Database
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<TTarget, bool>> Convert<TSource, TTarget>(
            this Expression<Func<TSource, bool>> root)
        {
            var visitor = new ParameterTypeVisitor<TSource, TTarget>();
            return (Expression<Func<TTarget, bool>>)visitor.Visit(root);
        }

        class ParameterTypeVisitor<TSource, TTarget> : ExpressionVisitor
        {
            private ReadOnlyCollection<ParameterExpression> parameters;

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return parameters?.FirstOrDefault(p => p.Name == node.Name)
                       ?? (node.Type == typeof(TSource) ? Expression.Parameter(typeof(TTarget), node.Name) : node);
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                parameters = VisitAndConvert<ParameterExpression>(node.Parameters, "VisitLambda");
                return Expression.Lambda(Visit(node.Body), parameters);
            }
        }
    }
}
