using System;
using System.IO;

namespace CleanArchitecture.Domain.BlobEntities
{
    public class BlobEntity : IDisposable
    {
        public Stream Content { get; set; }
        public string ContentType { get; set; }
        public DateTime LastModified { get; set; }

        public void Dispose()
        {
            Content?.Dispose();
        }
    }
}
