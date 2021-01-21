using System;
using System.IO;

namespace CleanArchitecture.Domain.BlobEntities
{
    public class BlobDownloadEntity : IDisposable
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
