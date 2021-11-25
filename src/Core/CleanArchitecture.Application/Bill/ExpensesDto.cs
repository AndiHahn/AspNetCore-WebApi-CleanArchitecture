using CleanArchitecture.Core;

namespace CleanArchitecture.Application.Bill
{
    public class ExpensesDto
    {
        public Category Category { get; set; }

        public double TotalAmount { get; set; }
    }
}
