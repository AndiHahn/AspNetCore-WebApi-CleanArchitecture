using System.Threading.Tasks;
using CleanArchitecture.Domain.BlobEntities;

namespace CleanArchitecture.Domain.Interfaces
{
    public interface IBlobStorageRepository
    {
        Task<BlobDownloadEntity> DownloadBlobAsync(string containerName, string blobUrl);
        Task UploadBlobAsync(string containerName, string blobUrl, BlobUploadModel blob);
        Task RemoveBlobAsync(string containerName, string blobUrl);
        Task<bool> RemoveBlobIfExistsAsync(string containerName, string blobUrl);
    }
}