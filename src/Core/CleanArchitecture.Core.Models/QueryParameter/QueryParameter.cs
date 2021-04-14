using System;

namespace CleanArchitecture.Core.Models.QueryParameter
{
    public abstract class QueryParameter<EnumSortField, EnumFilterField> :
                IPagingParameter
        where EnumFilterField : Enum
        where EnumSortField : Enum
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public SortItem<EnumSortField> Sorting { get; set; }
        public FilterItem<EnumFilterField> Filter { get; set; }
    }
}
