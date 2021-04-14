using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Base
{
    public interface IVersionableEntity
    {
        [Timestamp]
        byte[] Version { get; set; }
    }
}