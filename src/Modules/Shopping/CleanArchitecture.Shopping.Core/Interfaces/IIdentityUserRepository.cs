using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArchitecture.Shopping.Core.Interfaces
{
    public interface IIdentityUserRepository
    {
        Task<IdentityUser> GetByNameAsync(string name);

        Task<bool> CheckPasswordAsync(IdentityUser user, string password);

        Task<ClaimsPrincipal> CreatePrincipalAsync(IdentityUser user);

        Task<IList<string>> GetRolesForUserAsync(IdentityUser user);
    }
}
