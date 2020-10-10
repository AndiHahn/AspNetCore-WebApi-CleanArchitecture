using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Services.Income.Models
{
    public class IncomeCreateModel
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public Duration Duration { get; set; }

        public override string ToString()
        {
            return $"AccountId: {AccountId}, Name: {Name}, Value: {Value}, Duration: {Duration}";
        }
    }
}