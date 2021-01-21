using System;

namespace CleanArchitecture.Core.Models.QueryParameter
{
    public class SortItem<EnumSortingField> : ISortingParameter<EnumSortingField>
        where EnumSortingField : Enum
    {
        public EnumSortingField SortingField { get; set; }
        public SortingDirection SortingDirection { get; set; }
    }
}
