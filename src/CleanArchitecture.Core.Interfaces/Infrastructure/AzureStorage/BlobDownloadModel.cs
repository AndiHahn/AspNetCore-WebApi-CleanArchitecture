using System;
using System.IO;

namespace CleanArchitecture.Core.Interfaces.Infrastructure.AzureStorage
{
    public class BlobDownloadModel : IDisposable
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
