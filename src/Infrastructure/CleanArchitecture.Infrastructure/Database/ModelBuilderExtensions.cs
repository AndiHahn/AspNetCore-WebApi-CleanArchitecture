using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CleanArchitecture.Infrastructure.Database
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyGlobalFilters<TInterface>(
            this ModelBuilder builder,
            Expression<Func<TInterface, bool>> expression)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetInterface(typeof(TInterface).Name) != null)
                {
                    var newParam = Expression.Parameter(entityType.ClrType);
                    var newbody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam,
                        expression.Body);
                    builder.Entity(entityType.ClrType)
                        .HasQueryFilter(Expression.Lambda(newbody, newParam));
                }
            }
        }
    }
}
