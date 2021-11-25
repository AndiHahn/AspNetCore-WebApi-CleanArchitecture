using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.Shared
{
    public interface IVersionableEntity
    {
        [Timestamp]
        byte[] Version { get; set; }
    }
}