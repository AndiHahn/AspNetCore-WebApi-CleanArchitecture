namespace CleanArchitecture.Core.GenericQuery.Filter
{
    public interface IExpressionFactory<TEntity>
    {
        IExpression<TEntity> GetExpression();
    }
}