namespace CleanArchitecture.BudgetPlan.Application.BudgetPlan
{
    public class BudgetPlanDto
    {
        public IEnumerable<BudgetPlanIncomeDto> Incomes { get; set; }

        public IEnumerable<BudgetPlanCostDto> Costs { get; set; }
    }
}
