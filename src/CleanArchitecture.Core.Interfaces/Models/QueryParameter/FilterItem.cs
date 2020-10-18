using System;

namespace CleanArchitecture.Core.Interfaces.Models.QueryParameter
{
    public class FilterItem<EnumFilterField> : IFilterParameter<EnumFilterField>
        where EnumFilterField : Enum
    {
        public EnumFilterField FilterField { get; set; }
        public string FilterValue { get; set; }
        public FilterOperation FilterOperation { get; set; }
    }
}
