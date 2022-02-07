using CleanArchitecture.Shared.Infrastructure.Database.Budget;
using CleanArchitecture.Shopping.Core.Interfaces;
using CleanArchitecture.Shopping.Core.User;

namespace CleanArchitecture.Shopping.Infrastructure.Repositories
{
    internal class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(BudgetContext context)
        : base(context)
        {
        }
    }
}
