using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.CrudServices.Interfaces;
using CleanArchitecture.Application.CrudServices.Models.BankAccount;
using CleanArchitecture.Application.Validations;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;

namespace CleanArchitecture.Application.CrudServices
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IMapper mapper;
        private readonly IBankAccountRepository bankAccountRepository;

        public BankAccountService(
            IMapper mapper,
            IBankAccountRepository bankAccountRepository)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
        }

        public async Task<IEnumerable<BankAccountModel>> GetAllAsync(Guid currentUserId)
        {
            var accounts = await bankAccountRepository.ListByUserAsync(currentUserId);

            return accounts.Select(mapper.Map<BankAccountModel>);
        }

        public async Task<BankAccountModel> GetByIdAsync(Guid id, Guid currentUserId)
        {
            var entity = (await bankAccountRepository.GetByIdAsync(id)).AssertEntityFound(id);
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

            var createdEntity = await bankAccountRepository.AddAsync(accountEntity);

            return mapper.Map<BankAccountModel>(createdEntity);
        }
    }
}