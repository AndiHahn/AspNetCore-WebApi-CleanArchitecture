using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Infrastructure.AzureStorage;
using CleanArchitecture.Core.Interfaces.Models;
using CleanArchitecture.Core.Interfaces.Services.Bill.Models;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Core.Interfaces.Services.Bill
{
    public interface IBillService
    {
        Task<PagedResult<BillModel>> QueryAsync(BillQueryParameter queryParameter);
        Task<PagedResult<BillModel>> ListAsync(BillSearchParameter searchParameter);
        Task<BillModel> GetByIdAsync(int id);
        Task<BillModel> AddBillAsync(BillCreateModel createModel);
        Task<BillModel> UpdateBillAsync(int id, BillUpdateModel updateModel);
        Task DeleteBillAsync(int id);
        Task<BlobDownloadModel> GetImageAsync(int id);
        Task AddImageToBillAsync(int id, IFormFile file);
        Task DeleteImageAsync(int id);
        Task<TimeRangeModel> GetAvailableTimeRangeAsync();
    }
}