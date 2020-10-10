using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Base
{
    public abstract class VersionableEntity : BaseEntity
    {
        [Timestamp]
        public byte[] Version { get; set; }
    }
}