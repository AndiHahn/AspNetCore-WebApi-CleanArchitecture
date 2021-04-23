using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Infrastructure.Database.Budget;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class UserRepository : EfRepository<UserEntity>, IUserRepository
    {
        public UserRepository(BudgetContext context)
        : base(context)
        {
        }
    }
}
