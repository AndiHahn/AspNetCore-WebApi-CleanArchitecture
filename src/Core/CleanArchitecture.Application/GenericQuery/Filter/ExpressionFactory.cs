using System;
using System.Reflection;
using CleanArchitecture.Application.GenericQuery.Filter.Expressions;
using CleanArchitecture.Domain.Exceptions;

namespace CleanArchitecture.Application.GenericQuery.Filter
{
    public class ExpressionFactory<TEntity> : IExpressionFactory<TEntity>
    {
        private readonly string propertyName;

        public ExpressionFactory(string propertyName)
        {
            this.propertyName = propertyName;
        }

        public IExpression<TEntity> GetExpression()
        {
            PropertyInfo propertyInfo = typeof(TEntity).GetProperty(propertyName);

            if (propertyInfo == null)
            {
                throw new BadRequestException($"Cannot filter Property {propertyName} - Property does not exist.");
            }

            Type propertyType = propertyInfo.PropertyType;

            if (propertyType.Equals(typeof(string)))
            {
                return new StringExpression<TEntity>();
            }
            else if (propertyType.IsEnum)
            {
                return new EnumExpression<TEntity>(propertyType);
            }
            else if (propertyType.Equals(typeof(bool)))
            {
                return new BoolExpression<TEntity>();
            }
            else if (propertyType.Equals(typeof(DateTime)))
            {
                return new DateTimeExpression<TEntity>();
            }

            throw new BadRequestException($"Filter property of type {propertyType.Name} is not supported.");
        }
    }
}
