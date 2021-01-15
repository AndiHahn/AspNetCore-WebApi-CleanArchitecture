using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CleanArchitecture.Core.Interfaces.Models.QueryParameter;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Core.GenericQuery.Filter.Expressions
{
    public class BoolExpression<TEntity> : IExpression<TEntity>
    {
        Expression<Func<TEntity, bool>> IExpression<TEntity>.CreateExpression(string propertyName, FilterOperation operation, string value)
        {
            return t => EF.Property<bool>(t, propertyName).Equals(bool.Parse(value));
        }

        public IList<FilterOperation> ValidOptions()
        {
            return new List<FilterOperation> { FilterOperation.IsEqual };
        }
    }
}