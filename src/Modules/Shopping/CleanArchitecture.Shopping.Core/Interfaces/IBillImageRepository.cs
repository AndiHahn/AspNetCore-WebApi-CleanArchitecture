using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Core.Models;

namespace CleanArchitecture.Shopping.Core.Interfaces
{
    public interface IBillImageRepository
    {
        Task UploadImageAsync(Guid billId, Blob image, CancellationToken cancellationToken = default);

        Task DeleteImageAsync(Guid billId, CancellationToken cancellationToken = default);

        Task<Blob> DownloadImageAsync(Guid billId, CancellationToken cancellationToken = default);

        Task<bool> ImageExistsAsync(Guid billId, CancellationToken cancellationToken = default);
    }
}
