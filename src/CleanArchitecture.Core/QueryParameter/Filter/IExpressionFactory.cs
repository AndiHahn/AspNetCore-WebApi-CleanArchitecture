namespace CleanArchitecture.Core.QueryParameter.Filter
{
    public interface IExpressionFactory<TEntity>
    {
        IExpression<TEntity> GetExpression();
    }
}