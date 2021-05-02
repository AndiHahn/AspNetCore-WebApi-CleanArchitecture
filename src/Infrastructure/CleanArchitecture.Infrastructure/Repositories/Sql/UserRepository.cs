using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Database.Budget;

namespace CleanArchitecture.Infrastructure.Repositories.Sql
{
    public class UserRepository : EfRepository<UserEntity>, IUserRepository
    {
        public UserRepository(BudgetContext context)
        : base(context)
        {
        }
    }
}
