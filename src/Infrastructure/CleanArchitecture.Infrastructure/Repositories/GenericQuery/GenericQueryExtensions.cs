using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using CleanArchitecture.Common.Models.Query;
using CleanArchitecture.Domain.Base;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Infrastructure.Repositories.GenericQuery.Filter;

namespace CleanArchitecture.Infrastructure.Repositories.GenericQuery
{
    public static class GenericQueryExtensions
    {
        public static IQueryable<TEntity> ApplyQueryParameter<TEntity, TSortingFields, TFilterField>(
                this IQueryable<TEntity> query,
                QueryParameter<TSortingFields, TFilterField> queryParameter)
            where TEntity : BaseEntity
            where TSortingFields : Enum
            where TFilterField : Enum
        {
            if (queryParameter != null)
            {
                return query.ApplyFilter(queryParameter.Filter)
                            .ApplyOrderBy(queryParameter.Sorting)
                            .ApplyPaging(queryParameter);
            }

            return query;
        }

        public static IQueryable<TEntity> ApplyFilter<TEntity, TFilterField>(
                this IQueryable<TEntity> query,
                IFilterParameter<TFilterField> filterParameter)
            where TEntity : BaseEntity where TFilterField : Enum
        {
            if (filterParameter != null)
            {
                IExpressionFactory<TEntity> expressionFactory = new ExpressionFactory<TEntity>(filterParameter.FilterField.ToString());
                IExpression<TEntity> expression = expressionFactory.GetExpression();
                return query.Where(expression.CreateExpression(filterParameter.FilterField.ToString(),
                                                               filterParameter.FilterOperation,
                                                               filterParameter.FilterValue));
            }

            return query;
        }

        public static IQueryable<TEntity> ApplyOrderBy<TEntity, TSortingField>(
                this IQueryable<TEntity> query,
                ISortingParameter<TSortingField> sortingParameter)
            where TEntity : BaseEntity where TSortingField : Enum
        {
            if (sortingParameter != null)
            {
                string sortingDirection = sortingParameter.SortingDirection == SortingDirection.Asc ? "asc" : "desc";
                return query.OrderBy($"{sortingParameter.SortingField} {sortingDirection}");
            }
            return query;
        }

        public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> query,
                                                               IPagingParameter pagingParameter)
            where TEntity : BaseEntity
        {
            if (pagingParameter == null || pagingParameter.PageSize <= 0 || pagingParameter.PageIndex < 0)
            {
                throw new BadRequestException($"Paging parameters must contain valid values ({nameof(pagingParameter.PageSize)} > 0) and ({nameof(pagingParameter.PageIndex)} >= 0)");
            }

            if (pagingParameter.PageSize > 0)
            {
                int skip = pagingParameter.PageIndex * pagingParameter.PageSize;
                return query.Skip(skip).Take(pagingParameter.PageSize);
            }

            return query;
        }
    }
}