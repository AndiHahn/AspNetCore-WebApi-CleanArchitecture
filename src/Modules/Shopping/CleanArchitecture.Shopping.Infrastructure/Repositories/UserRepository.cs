using CleanArchitecture.Shopping.Core.Interfaces;
using CleanArchitecture.Shopping.Core.User;
using CleanArchitecture.Shopping.Infrastructure.Database.Budget;

namespace CleanArchitecture.Shopping.Infrastructure.Repositories
{
    internal class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(ShoppingDbContext context)
        : base(context)
        {
        }
    }
}
