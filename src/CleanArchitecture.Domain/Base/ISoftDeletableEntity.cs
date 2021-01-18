namespace CleanArchitecture.Domain.Base
{
    public interface ISoftDeletableEntity
    {
        bool Deleted { get; set; }
    }
}
