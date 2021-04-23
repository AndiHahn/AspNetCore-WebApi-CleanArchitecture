using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CleanArchitecture.Common.Models.Query;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories.GenericQuery.Filter.Expressions
{
    public class StringExpression<TEntity> : IExpression<TEntity>
    {
        public Expression<Func<TEntity, bool>> CreateExpression(string propertyName, FilterOperation operation, string value)
        {
            if (ValidOptions().Contains(operation))
            {
                switch (operation)
                {
                    case FilterOperation.Contains:
                        return t => EF.Property<string>(t, propertyName).Contains(value);
                    case FilterOperation.StartsWith:
                        return t => EF.Property<string>(t, propertyName).StartsWith(value);
                    case FilterOperation.EndsWith:
                        return t => EF.Property<string>(t, propertyName).EndsWith(value);
                    case FilterOperation.IsEqual:
                        return t => EF.Property<string>(t, propertyName).Equals(value);
                }
            }
            throw new InvalidOperationException($"Filter action {operation} is not allowed on Property {propertyName}.");
        }

        public IList<FilterOperation> ValidOptions()
        {
            return new List<FilterOperation>
            {
                FilterOperation.Contains,
                FilterOperation.StartsWith,
                FilterOperation.EndsWith,
                FilterOperation.IsEqual
            };
        }
    }
}