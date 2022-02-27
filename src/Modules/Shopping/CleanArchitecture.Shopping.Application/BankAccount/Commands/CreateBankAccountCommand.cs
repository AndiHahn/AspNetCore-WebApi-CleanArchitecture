using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using FluentValidation;

namespace CleanArchitecture.Shopping.Application.BankAccount.Commands
{
    public class CreateBankAccountCommand : ICommand<Result<BankAccountDto>>
    {
        public CreateBankAccountCommand(Guid currentUserId, string name)
        {
            this.CurrentUserId = currentUserId;
            this.Name = name;
        }

        public Guid CurrentUserId { get; }

        public string Name { get; }
    }

    public class CreateBankAccountCommandValidator : AbstractValidator<CreateBankAccountCommand>
    {
        public CreateBankAccountCommandValidator()
        {
            RuleFor(c => c.Name).NotNull().NotEmpty();
        }
    }

    internal class CreateBankAccountCommandHandler : ICommandHandler<CreateBankAccountCommand, Result<BankAccountDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CreateBankAccountCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<BankAccountDto>> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = this.unitOfWork.BankAccountRepository
                .Add(new Core.BankAccount.BankAccount(request.Name, request.CurrentUserId));

            await this.unitOfWork.CommitAsync(cancellationToken);

            return Result<BankAccountDto>.Success(this.mapper.Map<BankAccountDto>(entity));
        }
    }
}
