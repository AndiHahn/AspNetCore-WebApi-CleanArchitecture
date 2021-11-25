namespace CleanArchitecture.Core.Shared
{
    public interface ISoftDeletableEntity
    {
        bool Deleted { get; set; }
    }
}
