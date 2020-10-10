using CleanArchitecture.Services.Account;
using CleanArchitecture.Services.Bill;
using CleanArchitecture.Services.BillCategory;
using CleanArchitecture.Services.BudgetPlan;
using CleanArchitecture.Services.Expense;
using CleanArchitecture.Services.FixedCost;
using CleanArchitecture.Services.Income;
using CleanArchitecture.Services.User;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Services
{
    public static class ServiceRegistrationExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<IFixedCostService, FixedCostService>();
            services.AddScoped<IBudgetPlanService, BudgetPlanService>();
            services.AddScoped<IBillCategoryService, BillCategoryService>();
        }
    }
}