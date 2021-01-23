using System.Threading.Tasks;
using CleanArchitecture.Domain.BlobEntities;

namespace CleanArchitecture.Core.Interfaces.Data.Repositories
{
    public interface IBlobStorageRepository
    {
        Task<BlobEntity> DownloadBlobAsync(string containerName, string blobUrl);
        Task UploadBlobAsync(string containerName, string blobUrl, BlobEntity blob);
        Task RemoveBlobAsync(string containerName, string blobUrl);
        Task<bool> RemoveBlobIfExistsAsync(string containerName, string blobUrl);
    }
}