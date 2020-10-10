using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Services.User.Models;

namespace CleanArchitecture.Services.User
{
    public interface IUserService
    {
        Task<AuthenticatedUserModel> AuthenticateAsync(SignInModel model);
        Task<IEnumerable<UserModel>> ListAsync(UserQueryParameter queryParameter);
    }
}