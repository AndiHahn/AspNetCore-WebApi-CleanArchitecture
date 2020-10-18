using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Services.User.Models;

namespace CleanArchitecture.Core.Interfaces.Services.User
{
    public interface IUserService
    {
        Task<AuthenticatedUserModel> AuthenticateAsync(SignInModel model);
        Task<IEnumerable<UserModel>> ListAsync(UserQueryParameter queryParameter);
    }
}