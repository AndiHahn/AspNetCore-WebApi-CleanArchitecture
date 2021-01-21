namespace CleanArchitecture.Application.UseCases.Expenses.Models
{
    public class ExpenseModel
    {
        public string Category { get; set; }
        public double Costs { get; set; }

        public ExpenseModel()
        {
        }

        public ExpenseModel(string category, double costs)
        {
            Category = category;
            Costs = costs;
        }
    }
}