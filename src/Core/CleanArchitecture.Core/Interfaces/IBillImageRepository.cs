using CleanArchitecture.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IBillImageRepository
    {
        Task UploadImageAsync(Guid billId, IFormFile image, CancellationToken cancellationToken = default);

        Task DeleteImageAsync(Guid billId, CancellationToken cancellationToken = default);

        Task<Blob> DownloadImageAsync(Guid billId, CancellationToken cancellationToken = default);

        Task<bool> ImageExistsAsync(Guid billId, CancellationToken cancellationToken = default);
    }
}
