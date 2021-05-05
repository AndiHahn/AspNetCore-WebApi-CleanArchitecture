using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;

namespace CleanArchitecture.Infrastructure.Repositories.Sql
{
    public class UserRepository : EfRepository<UserEntity>, IUserRepository
    {
        public UserRepository(IBudgetContext context)
        : base(context)
        {
        }
    }
}
