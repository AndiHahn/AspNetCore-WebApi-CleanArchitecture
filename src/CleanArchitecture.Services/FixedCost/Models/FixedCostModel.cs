using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Services.FixedCost.Models
{
    public class FixedCostModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public Duration Duration { get; set; }
        public CostCategory CostCategory { get; set; }
        public string AccountName { get; set; }
    }
}