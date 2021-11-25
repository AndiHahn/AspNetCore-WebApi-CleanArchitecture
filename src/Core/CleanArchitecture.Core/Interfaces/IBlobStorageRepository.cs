using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Models;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IBlobStorageRepository
    {
        Task<Blob> DownloadBlobAsync(string containerName, string blobUrl, CancellationToken cancellationToken = default);

        Task UploadBlobAsync(string containerName, string blobUrl, Blob blob, CancellationToken cancellationToken = default);

        Task RemoveBlobAsync(string containerName, string blobUrl, CancellationToken cancellationToken = default);

        Task<bool> RemoveBlobIfExistsAsync(string containerName, string blobUrl, CancellationToken cancellationToken = default);

        Task<bool> BlobExistsAsync(string containerName, string blobUrl, CancellationToken cancellationToken = default);
    }
}
