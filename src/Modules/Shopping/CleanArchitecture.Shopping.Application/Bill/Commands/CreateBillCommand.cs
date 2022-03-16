using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core;
using CleanArchitecture.Shopping.Core.Interfaces;
using FluentValidation;

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
        private readonly IUnitOfWork unitOfWork;

        public CreateBillCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result<BillDto>> Handle(CreateBillCommand request, CancellationToken cancellationToken)
        {
            var account = await this.unitOfWork.BankAccountRepository.GetByIdAsync(request.BankAccountId, cancellationToken);
            if (account == null)
            {
                return Result<BillDto>.NotFound($"Account with id {request.BankAccountId} not found.");
            }

            var user = await this.unitOfWork.UserRepository.GetByIdAsync(request.CurrentUserId);
            if (user == null)
            {
                return Result<BillDto>.NotFound($"User with id {request.CurrentUserId} not found.");
            }

            var bill = this.unitOfWork.BillRepository.Add(
                new Core.Bill(user, account, request.ShopName, request.Price, request.Date, request.Notes, request.Category));

            await this.unitOfWork.CommitAsync(cancellationToken);

            return this.mapper.Map<BillDto>(bill);
        }
    }
}
