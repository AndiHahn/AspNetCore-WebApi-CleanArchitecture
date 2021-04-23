using System;

namespace CleanArchitecture.Common.Models.Query
{
    public class SortItem<EnumSortingField> : ISortingParameter<EnumSortingField>
        where EnumSortingField : Enum
    {
        public EnumSortingField SortingField { get; set; }
        public SortingDirection SortingDirection { get; set; }
    }
}
