using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CleanArchitecture.Core.Models.QueryParameter;
using CleanArchitecture.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.GenericQuery.Filter.Expressions
{
    public class EnumExpression<TEntity> : IExpression<TEntity>
    {
        private readonly Type propertyType;

        public EnumExpression(Type propertyType)
        {
            this.propertyType = propertyType;
        }

        public Expression<Func<TEntity, bool>> CreateExpression(string propertyName, FilterOperation action, string value)
        {
            if (!Enum.TryParse(propertyType, value, out var enumObject))
            {
                throw new BadRequestException($"Cannot filter by {propertyName} - Value {value} is not available.");
            }

            //Imporant: cast to int is required!!
            return t => ((int)EF.Property<int>(t, propertyName)).Equals((int)enumObject);
        }

        public IList<FilterOperation> ValidOptions()
        {
            return new List<FilterOperation> { FilterOperation.IsEqual };
        }
    }
}
