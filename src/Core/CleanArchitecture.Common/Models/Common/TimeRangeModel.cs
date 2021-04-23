using System;

namespace CleanArchitecture.Common.Models.Common
{
    public class TimeRangeModel
    {
        public DateTime FromDate { get; set; }
        public DateTime UntilDate { get; set; }

        public TimeRangeModel()
        {
        }

        public TimeRangeModel(DateTime fromDate, DateTime untilDate)
        {
            FromDate = fromDate;
            UntilDate = untilDate;
        }
    }
}
