namespace CleanArchitecture.Shared.Core.Interfaces
{
    public interface IDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
