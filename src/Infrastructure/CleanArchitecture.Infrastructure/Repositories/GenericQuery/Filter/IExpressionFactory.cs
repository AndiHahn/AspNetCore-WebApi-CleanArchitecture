namespace CleanArchitecture.Infrastructure.Repositories.GenericQuery.Filter
{
    public interface IExpressionFactory<TEntity>
    {
        IExpression<TEntity> GetExpression();
    }
}