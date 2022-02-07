namespace CleanArchitecture.Shared.Core.Models
{
    public class TimeRangeModel
    {
        public TimeRangeModel()
        {
        }

        public TimeRangeModel(DateTime fromDate, DateTime untilDate)
        {
            FromDate = fromDate;
            UntilDate = untilDate;
        }

        public DateTime FromDate { get; set; }
        public DateTime UntilDate { get; set; }
    }
}
