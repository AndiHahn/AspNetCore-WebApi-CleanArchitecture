using System;

namespace CleanArchitecture.Common.Models.Query
{
    public interface IFilterParameter<EnumFilterField> 
        where EnumFilterField : Enum
    {
        public EnumFilterField FilterField { get; set; }
        public string FilterValue { get; set; }
        public FilterOperation FilterOperation { get; set; }
    }
}