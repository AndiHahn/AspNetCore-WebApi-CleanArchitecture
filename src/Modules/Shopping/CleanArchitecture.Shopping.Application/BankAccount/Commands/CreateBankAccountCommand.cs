using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Shared.Core.Models.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Shopping.Application.BankAccount.Commands
{
    public class CreateBankAccountCommand : IRequest<Result<BankAccountDto>>
    {
        public CreateBankAccountCommand(Guid currentUserId, string name)
        {
            this.CurrentUserId = currentUserId;
            this.Name = name;
        }

        public Guid CurrentUserId { get; }

        public string Name { get; }
    }

    internal class CreateBankAccountCommandHandler : IRequestHandler<CreateBankAccountCommand, Result<BankAccountDto>>
    {
        private readonly IBankAccountRepository accountRepository;
        private readonly IMapper mapper;

        public CreateBankAccountCommandHandler(
            IBankAccountRepository repository,
            IMapper mapper)
        {
            this.accountRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<Result<BankAccountDto>> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
        {
            return this.accountRepository
                .AddAsync(
                    new Core.BankAccount.BankAccount(
                        request.Name,
                        request.CurrentUserId),
                    cancellationToken)
                .ContinueWith(a => Result<BankAccountDto>.Success(this.mapper.Map<BankAccountDto>(a.Result)));
        }
    }
}
