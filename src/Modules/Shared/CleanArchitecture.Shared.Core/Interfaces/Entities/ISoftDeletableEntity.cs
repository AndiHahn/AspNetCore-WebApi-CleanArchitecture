namespace CleanArchitecture.Shared.Core.Interfaces.Entities
{
    public interface ISoftDeletableEntity
    {
        bool Deleted { get; set; }
    }
}
