using CleanArchitecture.Shopping.Core;

namespace CleanArchitecture.Shopping.Application.Bill
{
    public class ExpensesDto
    {
        public Category Category { get; set; }

        public double TotalAmount { get; set; }
    }
}
