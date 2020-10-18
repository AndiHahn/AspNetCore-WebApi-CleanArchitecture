using System;

namespace CleanArchitecture.Core.Interfaces.Services.Bill.Models
{
    public class TimeRangeModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public TimeRangeModel()
        {
        }

        public TimeRangeModel(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }
    }
}
