using System;
using System.IO;

namespace CleanArchitecture.Domain.BlobEntities
{
    public class BlobUploadModel : IDisposable
    {
        public Stream Content { get; set; }
        public string ContentType { get; set; }

        public void Dispose()
        {
            Content?.Dispose();
        }
    }
}
