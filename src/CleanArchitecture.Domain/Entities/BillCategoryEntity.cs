using CleanArchitecture.Domain.Base;

namespace CleanArchitecture.Domain.Entities
{
    public class BillCategoryEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Color { get; set; }

        public BillCategoryEntity()
        {
        }

        public BillCategoryEntity(string name, string color = null)
        {
            Name = name;
            Color = color;
        }
    }
}
