using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class IdentityUserRepository : IIdentityUserRepository
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public IdentityUserRepository(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager ?? throw new System.ArgumentNullException(nameof(userManager));
            this.signInManager = signInManager ?? throw new System.ArgumentNullException(nameof(signInManager));
        }

        public Task<IdentityUser> GetByNameAsync(string name)
        {
            return this.userManager.FindByNameAsync(name);
        }

        public Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            return this.userManager.CheckPasswordAsync(user, password);
        }

        public Task<ClaimsPrincipal> CreatePrincipalAsync(IdentityUser user)
        {
            return this.signInManager.CreateUserPrincipalAsync(user);
        }

        public Task<IList<string>> GetRolesForUserAsync(IdentityUser user)
        {
            return this.userManager.GetRolesAsync(user);
        }
    }
}
