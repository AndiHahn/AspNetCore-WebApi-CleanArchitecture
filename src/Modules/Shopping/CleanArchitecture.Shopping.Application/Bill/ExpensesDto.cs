using CleanArchitecture.Shopping.Core.Bill;

namespace CleanArchitecture.Shopping.Application.Bill
{
    public class ExpensesDto
    {
        public Category Category { get; set; }

        public double TotalAmount { get; set; }
    }
}
