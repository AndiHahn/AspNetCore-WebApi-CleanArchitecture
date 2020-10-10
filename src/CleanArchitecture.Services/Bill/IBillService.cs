using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using CleanArchitecture.Core.QueryParameter.Models;
using CleanArchitecture.Services.Bill.Models;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Services.Bill
{
    public interface IBillService
    {
        Task<PagedResult<BillModel>> QueryAsync(BillQueryParameter queryParameter);
        Task<PagedResult<BillModel>> ListAsync(BillSearchParameter searchParameter);
        Task<BillModel> GetByIdAsync(int id);
        Task<BillModel> AddBillAsync(BillCreateModel createModel);
        Task<BillModel> UpdateBillAsync(int id, BillUpdateModel updateModel);
        Task DeleteBillAsync(int id);
        Task<BlobDownloadInfo> GetImageAsync(int id);
        Task AddImageToBillAsync(int id, IFormFile file);
        Task DeleteImageAsync(int id);
        Task<TimeRangeModel> GetAvailableTimeRangeAsync();
    }
}