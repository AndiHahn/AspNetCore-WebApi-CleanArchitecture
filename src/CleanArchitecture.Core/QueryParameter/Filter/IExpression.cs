using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CleanArchitecture.Core.QueryParameter.Models;

namespace CleanArchitecture.Core.QueryParameter.Filter
{
    public interface IExpression<TEntity>
    {
        Expression<Func<TEntity, bool>> CreateExpression(string propertyName, FilterOperation operation, string value);
        IList<FilterOperation> ValidOptions();
    }
}