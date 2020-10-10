using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Services.Income.Models
{
    public class IncomeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public Duration Duration { get; set; }
        public string AccountName { get; set; }
    }
}