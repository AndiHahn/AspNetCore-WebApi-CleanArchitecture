using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.BankAccount
{
    public class GetBankAccountByIdQuery : IRequest<Result<BankAccountDto>>
    {
        public GetBankAccountByIdQuery(Guid currentUserId, Guid id)
        {
            this.CurrentUserId = currentUserId;
            this.Id = id;
        }

        public Guid CurrentUserId { get; }

        public Guid Id { get; set; }
    }

    internal class GetBankAccountByIdQueryHandler : IRequestHandler<GetBankAccountByIdQuery, Result<BankAccountDto>>
    {
        private readonly IBankAccountRepository bankAccountRepository;
        private readonly IMapper mapper;

        public GetBankAccountByIdQueryHandler(
            IBankAccountRepository bankAccountRepository,
            IMapper mapper)
        {
            this.bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<BankAccountDto>> Handle(GetBankAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await this.bankAccountRepository.GetByIdWithUsersAsync(request.Id, cancellationToken);

            if (!account.HasAccess(request.CurrentUserId))
            {
                return Result<BankAccountDto>.Forbidden($"Current user does not have access to account {request.Id}.");
            }

            return this.mapper.Map<BankAccountDto>(account);
        }
    }
}
