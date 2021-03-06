﻿using System.Threading.Tasks;
using CleanArchitecture.Application.CrudServices.Models.User;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.CrudServices.Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<UserModel>> ListAsync(UserQueryParameter queryParameter);
    }
}