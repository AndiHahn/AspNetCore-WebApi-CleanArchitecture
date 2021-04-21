using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Validations;
using CleanArchitecture.Core.Interfaces.CrudServices;
using CleanArchitecture.Core.Interfaces.Data;
using CleanArchitecture.Core.Models.Domain.BankAccount;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.CrudServices
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IMapper mapper;
        private readonly IBudgetContext context;

        public BankAccountService(
            IMapper mapper,
            IBudgetContext context)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<BankAccountModel>> GetAllAsync(Guid currentUserId)
        {
            return (await context.BankAccount
                    .Where(ba => ba.OwnerId == currentUserId)
                    .ToListAsync())
                    .Select(mapper.Map<BankAccountModel>);
        }

        public async Task<BankAccountModel> GetByIdAsync(Guid id, Guid currentUserId)
        {
            var entity = (await context.BankAccount.FindAsync(id)).AssertEntityFound(id);
            if (entity.OwnerId != currentUserId)
            {
                throw new ForbiddenException($"Current user does not have access to account {id}.");
            }

            return mapper.Map<BankAccountModel>(entity);
        }

        public async Task<BankAccountModel> CreateAccountAsync(
            BankAccountCreateModel createModel,
            Guid currentUserId)
        {
            var accountEntity = mapper.Map<BankAccountEntity>(createModel);
            accountEntity.OwnerId = currentUserId;
            var createdEntity = context.BankAccount.Add(accountEntity).Entity;
            await context.SaveChangesAsync();
            return mapper.Map<BankAccountModel>(createdEntity);
        }
    }
}