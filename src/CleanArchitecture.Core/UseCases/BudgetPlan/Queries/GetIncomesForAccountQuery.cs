using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Services.BudgetPlan.Models;
using CleanArchitecture.Core.Interfaces.Services.Income;
using CleanArchitecture.Domain.Extensions;
using MediatR;

namespace CleanArchitecture.Core.UseCases.BudgetPlan.Queries
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
        private readonly IIncomeService incomeService;

        public GetIncomesForAccountQueryHandler(IIncomeService incomeService)
        {
            this.incomeService = incomeService ?? throw new ArgumentNullException(nameof(incomeService));
        }

        public async Task<IEnumerable<BudgetPlanModel>> Handle(
            GetIncomesForAccountQuery request,
            CancellationToken cancellationToken)
        {
            var incomes = await incomeService.GetByAccountIdsAsync(request.AccountIds);
            var incomesByAccount = incomes.GroupBy(i => i.AccountId);

            return incomesByAccount.Select(a => new BudgetPlanModel
            {
                Name = a.Key,
                Value = a.Sum(b => b.Value.ToMonthlyValue(b.Duration))
            });
        }
    }
}
