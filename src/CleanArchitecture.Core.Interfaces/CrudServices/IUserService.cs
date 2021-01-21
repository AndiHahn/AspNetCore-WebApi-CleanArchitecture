﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Models.Domain.User;

namespace CleanArchitecture.Core.Interfaces.CrudServices
{
    public interface IUserService
    {
        Task<AuthenticatedUserModel> AuthenticateAsync(SignInModel model);
        Task<IEnumerable<UserModel>> ListAsync(UserQueryParameter queryParameter);
    }
}