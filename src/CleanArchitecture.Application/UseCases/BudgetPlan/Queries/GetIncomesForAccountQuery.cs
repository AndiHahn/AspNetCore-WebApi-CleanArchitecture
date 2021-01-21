using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.UseCases.BudgetPlan.Models;
using CleanArchitecture.Application.Validations;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.UseCases.BudgetPlan.Queries
{
    public class GetIncomesForAccountQuery : IRequest<IEnumerable<BudgetPlanModel>>
    {
        public IEnumerable<Guid> AccountIds { get; }

        public GetIncomesForAccountQuery(IEnumerable<Guid> accountIds)
        {
            AccountIds = accountIds;
        }
    }

    public class GetIncomesForAccountQueryHandler : IRequestHandler<GetIncomesForAccountQuery, IEnumerable<BudgetPlanModel>>
    {
        private readonly IBudgetContext context;

        public GetIncomesForAccountQueryHandler(IBudgetContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<BudgetPlanModel>> Handle(
            GetIncomesForAccountQuery request,
            CancellationToken cancellationToken)
        {
            var incomes = new List<IncomeEntity>();
            foreach (var accountId in request.AccountIds)
            {
                incomes.AddRange(await GetIncomesByAccountId(accountId));
            }

            var incomesByAccount = incomes.GroupBy(i => i.AccountId, i => i.Account.Name);

            return incomesByAccount.Select(a => new BudgetPlanModel
            {
                //Name = a.Key.,
                //Value = a.Sum(b => b.Value.ToMonthlyValue(b.Duration))
            });
        }

        private async Task<IEnumerable<IncomeEntity>> GetIncomesByAccountId(Guid accountId)
        {
            (await context.Account.FindAsync(accountId)).AssertEntityFound(accountId);
            return (await context.Income
                    .Include(i => i.Account)
                    .Where(i => i.AccountId == accountId)
                    .OrderByDescending(i => i.Value)
                    .ToListAsync());
        }
    }
}
