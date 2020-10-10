using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.Image
{
    public interface IImageStorageService
    {
        Task<BlobDownloadInfo> GetImageDownloadInfoAsync(string fileUrl);
        Task UploadImageAsync(string fileUrl, IFormFile image);
        Task RemoveImageAsync(string fileUrl);
        Task RemoveImageIfExistsAsync(string fileUrl);
    }
}