using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Validations;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.CrudServices;
using CleanArchitecture.Core.Models.Domain.Account;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.CrudServices
{
    public class AccountService : IAccountService
    {
        private readonly IBudgetContext context;
        private readonly IMapper mapper;

        public AccountService(IBudgetContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<AccountModel>> GetAllAsync()
        {
            return (await context.Account.ToListAsync())
                            .Select(e => mapper.Map<AccountModel>(e));
        }

        public async Task<AccountModel> GetByIdAsync(Guid id)
        {
            var entity = (await context.Account.FindAsync(id)).AssertEntityFound(id);
            return mapper.Map<AccountModel>(entity);
        }
    }
}