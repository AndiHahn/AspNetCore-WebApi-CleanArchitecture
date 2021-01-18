using System;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Extensions
{
    public static class DoubleExtensions
    {
        public static double ToMonthlyValue(this double value, Duration duration)
        {
            double monthlyValue = 0;
            if (duration == Duration.Monthly)
            {
                monthlyValue = value;
            }
            else if (duration == Duration.QuarterYear)
            {
                monthlyValue = value * 4 / 12;
            }
            else if (duration == Duration.HalfYear)
            {
                monthlyValue = value * 2 / 12;
            }
            else if (duration == Duration.Year)
            {
                monthlyValue = value / 12;
            }

            return monthlyValue.ToTwoDecimals();
        }

        public static double ToTwoDecimals(this double value)
        {
            return Math.Truncate(value * 100) / 100;
        }
    }
}
