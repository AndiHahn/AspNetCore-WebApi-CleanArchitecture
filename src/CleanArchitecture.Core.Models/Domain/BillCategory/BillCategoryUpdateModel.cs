using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Core.Models.Domain.BillCategory
{
    public class BillCategoryUpdateModel
    {
#nullable enable
        public string? Name { get; set; }
        public string? Color { get; set; }
#nullable disable

        public void MergeIntoEntity(BillCategoryEntity entity)
        {
            entity.Name = Name ?? entity.Name;
            entity.Color = Color ?? entity.Color;
        }
    }
}
