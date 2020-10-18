using System;
using System.IO;

namespace CleanArchitecture.Core.Interfaces.Infrastructure.AzureStorage
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
