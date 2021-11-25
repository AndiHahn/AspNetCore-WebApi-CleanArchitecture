using CleanArchitecture.Core;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Database.Budget;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(BudgetContext context)
        : base(context)
        {
        }
    }
}
