using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CleanArchitecture.Shared.Infrastructure.Database
{
    internal static class ModelBuilderExtensions
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

        public static void ApplyRowVersion<TInterface>(this ModelBuilder builder, string propertyName)
        {
            foreach (var clrType in builder.Model.GetEntityTypes().Select(e => e.ClrType))
            {
                if (clrType.GetInterface(typeof(TInterface).Name) != null)
                {
                    builder.Entity(clrType)
                        .Property(propertyName)
                        .IsRowVersion();
                }
            }
        }
    }
}
