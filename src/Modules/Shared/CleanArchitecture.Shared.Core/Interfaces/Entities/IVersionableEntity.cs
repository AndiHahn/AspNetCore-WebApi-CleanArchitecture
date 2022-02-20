namespace CleanArchitecture.Shared.Core.Interfaces.Entities
{
    public interface IVersionableEntity
    {
        byte[] Version { get; set; }
    }
}