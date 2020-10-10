using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.QueryParameter.Models;

namespace CleanArchitecture.Core.QueryParameter.Filter.Expressions
{
    public class DateTimeExpression<TEntity> : IExpression<TEntity>
    {
        public Expression<Func<TEntity, bool>> CreateExpression(string propertyName, FilterOperation operation, string value)
        {
            if (ValidOptions().Contains(operation))
            {
                switch (operation)
                {
                    case FilterOperation.IsEqual:
                        return t => EF.Property<DateTime>(t, propertyName).Equals(value);
                    case FilterOperation.LessThan:
                        return t => EF.Property<DateTime>(t, propertyName).CompareTo(value) < 0;
                    case FilterOperation.GreaterThan:
                        return t => EF.Property<DateTime>(t, propertyName).CompareTo(value) > 0;
                    case FilterOperation.LessOrEqual:
                        return t => EF.Property<DateTime>(t, propertyName).CompareTo(value) <= 0;
                    case FilterOperation.GreaterOrEqual:
                        return t => EF.Property<DateTime>(t, propertyName).CompareTo(value) >= 0;
                }
            }
            throw new BadRequestException($"Filter action {operation} is not allowed on Property {propertyName}.");
        }

        public IList<FilterOperation> ValidOptions()
        {
            return new List<FilterOperation>
            {
                FilterOperation.IsEqual,
                FilterOperation.LessThan,
                FilterOperation.GreaterThan,
                FilterOperation.LessOrEqual,
                FilterOperation.GreaterOrEqual
            };
        }
    }
}