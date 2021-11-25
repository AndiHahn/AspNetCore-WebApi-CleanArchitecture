using System;

namespace CleanArchitecture.Application.Bill
{
    public class TimeRangeDto
    {
        public TimeRangeDto()
        {
        }

        public TimeRangeDto(DateTime fromDate, DateTime untilDate)
        {
            this.FromDate = fromDate;
            this.UntilDate = untilDate;
        }

        public DateTime FromDate { get; set; }

        public DateTime UntilDate { get; set; }
    }
}
