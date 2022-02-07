using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Shared.Core.Interfaces.Entities
{
    public interface IVersionableEntity
    {
        [Timestamp]
        byte[] Version { get; set; }
    }
}