namespace CleanArchitecture.Application.GenericQuery.Filter
{
    public interface IExpressionFactory<TEntity>
    {
        IExpression<TEntity> GetExpression();
    }
}