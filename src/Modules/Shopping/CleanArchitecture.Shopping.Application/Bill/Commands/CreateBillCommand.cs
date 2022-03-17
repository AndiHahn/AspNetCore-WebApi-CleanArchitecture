using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core;
using CleanArchitecture.Shopping.Core.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

#nullable enable

namespace CleanArchitecture.Shopping.Application.Bill.Commands
{
    public class CreateBillCommand : ICommand<Result<BillDto>>
    {
        public CreateBillCommand(
            Guid currentUserId,
            Guid bankAccountId,
            string shopName,
            double price,
            Category category,
            DateTime? date,
            string? notes)
        {
            this.CurrentUserId = currentUserId;
            this.BankAccountId = bankAccountId;
            this.ShopName = shopName;
            this.Price = price;
            this.Category = category;
            this.Date = date;
            this.Notes = notes;
        }

        public Guid CurrentUserId { get; }

        public Guid BankAccountId { get; }

        public string ShopName { get; }

        public double Price { get; }

        public Category Category { get; }

        public DateTime? Date { get; }

        public string? Notes { get; }
    }

    public class CreateBillCommandValidator : AbstractValidator<CreateBillCommand>
    {
        public CreateBillCommandValidator()
        {
            RuleFor(c => c.BankAccountId).NotEmpty();
            RuleFor(c => c.ShopName).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(c => c.Price).GreaterThan(0);
        }
    }

    internal class CreateBillCommandHandler : ICommandHandler<CreateBillCommand, Result<BillDto>>
    {
        private readonly IMapper mapper;
        private readonly IShoppingDbContext dbContext;

        public CreateBillCommandHandler(
            IMapper mapper,
            IShoppingDbContext dbContext)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result<BillDto>> Handle(CreateBillCommand request, CancellationToken cancellationToken)
        {
            var account = await this.dbContext.BankAccount.FindByIdAsync(request.BankAccountId, cancellationToken);
            if (account is null)
            {
                return Result<BillDto>.NotFound($"Account with id {request.BankAccountId} not found.");
            }

            var user = await this.dbContext.User.FindByIdAsync(request.CurrentUserId, cancellationToken);
            if (user is null)
            {
                return Result<BillDto>.NotFound($"User with id {request.CurrentUserId} not found.");
            }

            var bill = await this.dbContext.Bill.AddAsync(
                new Core.Bill(user, account, request.ShopName, request.Price, request.Date, request.Notes, request.Category));

            await this.dbContext.SaveChangesAsync(cancellationToken);

            return this.mapper.Map<BillDto>(bill.Entity);
        }
    }
}
