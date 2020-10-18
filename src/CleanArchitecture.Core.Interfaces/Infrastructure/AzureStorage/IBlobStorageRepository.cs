using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.Infrastructure.AzureStorage
{
    public interface IBlobStorageRepository
    {
        Task<BlobDownloadModel> DownloadBlobAsync(string containerName, string blobUrl);
        Task UploadBlobAsync(string containerName, string blobUrl, BlobUploadModel blob);
        Task RemoveBlobAsync(string containerName, string blobUrl);
        Task<bool> RemoveBlobIfExistsAsync(string containerName, string blobUrl);
    }
}