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
        private readonly IShoppingDbContext dbContext;
        private readonly IMapper mapper;

        public CreateBankAccountCommandHandler(
            IShoppingDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<BankAccountDto>> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.dbContext.BankAccount.AddAsync(
                new Core.BankAccount(request.Name, request.CurrentUserId),
                cancellationToken);

            await this.dbContext.SaveChangesAsync(cancellationToken);

            return Result<BankAccountDto>.Success(this.mapper.Map<BankAccountDto>(entity.Entity));
        }
    }
}
