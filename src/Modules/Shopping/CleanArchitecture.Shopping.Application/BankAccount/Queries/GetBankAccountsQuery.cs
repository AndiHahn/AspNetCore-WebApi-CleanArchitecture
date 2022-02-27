using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;

namespace CleanArchitecture.Shopping.Application.BankAccount.Queries
{
    public class GetBankAccountsQuery : IQuery<Result<IEnumerable<BankAccountDto>>>
    {
        public GetBankAccountsQuery(Guid currentUserId)
        {
            CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }
    }

    internal class GetBankAccountsQueryHandler : IQueryHandler<GetBankAccountsQuery, Result<IEnumerable<BankAccountDto>>>
    {
        private readonly IMapper mapper;
        private readonly IBankAccountRepository bankAccountRepository;

        public GetBankAccountsQueryHandler(
            IMapper mapper,
            IBankAccountRepository bankAccountRepository)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
        }

        public Task<Result<IEnumerable<BankAccountDto>>> Handle(GetBankAccountsQuery request, CancellationToken cancellationToken)
        {
            return this.bankAccountRepository
                .ListOwnAsync(request.CurrentUserId, cancellationToken)
                .ContinueWith(
                    r => Result<IEnumerable<BankAccountDto>>.Success(r.Result.Select(this.mapper.Map<BankAccountDto>)),
                    cancellationToken);
        }
    }
}
