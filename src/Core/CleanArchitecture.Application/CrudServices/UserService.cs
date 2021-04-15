using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.GenericQuery;
using CleanArchitecture.Core.Interfaces.CrudServices;
using CleanArchitecture.Core.Interfaces.Data;
using CleanArchitecture.Core.Models.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.CrudServices
{
    public class UserService : IUserService
    {
        private readonly IBudgetContext context;
        private readonly IMapper mapper;

        public UserService(
                IBudgetContext context,
                IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<UserModel>> ListAsync(UserQueryParameter queryParameter)
        {
            return (await context.User.ApplyPaging(queryParameter).ToListAsync())
                            .Select(mapper.Map<UserModel>);
        }
    }
}