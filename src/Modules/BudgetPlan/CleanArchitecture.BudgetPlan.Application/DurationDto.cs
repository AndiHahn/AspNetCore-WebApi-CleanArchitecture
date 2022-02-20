using CleanArchitecture.BudgetPlan.Core;

namespace CleanArchitecture.BudgetPlan.Application
{
    public enum DurationDto
    {
        Monthly,
        QuarterYear,
        HalfYear,
        Year
    }

    public static class DurationExtensions
    {
        public static Duration FromDto(this DurationDto duration)
        {
            return duration switch
            {
                DurationDto.Monthly => Duration.Monthly,
                DurationDto.QuarterYear => Duration.QuarterYear,
                DurationDto.HalfYear => Duration.HalfYear,
                DurationDto.Year => Duration.Year,
                _ => throw new ArgumentException($"Value {duration} is not valid.", nameof(duration))
            };
        }

        public static DurationDto ToDto(this Duration duration)
        {
            return duration.Name switch
            {
                nameof(Duration.Monthly) => DurationDto.Monthly,
                nameof(Duration.QuarterYear) => DurationDto.QuarterYear,
                nameof(Duration.HalfYear) => DurationDto.HalfYear,
                nameof(Duration.Year) => DurationDto.Year,
                _ => throw new ArgumentException($"Value {duration} is not valid.", nameof(duration))
            };
        }
    }
}
