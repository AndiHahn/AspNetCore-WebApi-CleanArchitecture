using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.CrudServices.Models.BankAccount;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using MediatR;

namespace CleanArchitecture.Application.UseCases.Collaboration.Queries
{
    public class GetSharedAccountsQuery : IRequest<IEnumerable<BankAccountModel>>
    {
        public Guid CurrentUserId { get; }

        public GetSharedAccountsQuery(Guid currentUserId)
        {
            CurrentUserId = currentUserId;
        }
    }

    public class GetSharedAccountsQueryHandler : IRequestHandler<GetSharedAccountsQuery, IEnumerable<BankAccountModel>>
    {
        private readonly IMapper mapper;
        private readonly IBankAccountRepository bankAccountRepository;

        public GetSharedAccountsQueryHandler(
            IMapper mapper,
            IBankAccountRepository bankAccountRepository)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
        }

        public async Task<IEnumerable<BankAccountModel>> Handle(GetSharedAccountsQuery request, CancellationToken cancellationToken)
        {
            Guid currentUserId = request.CurrentUserId;

            var sharedAccounts = await bankAccountRepository.GetSharedAccountsAsync(currentUserId);

            return sharedAccounts.Select(mapper.Map<BankAccountModel>);
        }
    }
}
