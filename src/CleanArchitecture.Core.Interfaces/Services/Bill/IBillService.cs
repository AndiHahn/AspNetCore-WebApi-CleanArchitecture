using System;
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
        Task<BillModel> GetByIdAsync(Guid id);
        Task<BillModel> AddBillAsync(BillCreateModel createModel);
        Task<BillModel> UpdateBillAsync(Guid id, BillUpdateModel updateModel);
        Task DeleteBillAsync(Guid id);
        Task<BlobDownloadModel> GetImageAsync(Guid id);
        Task AddImageToBillAsync(Guid id, IFormFile file);
        Task DeleteImageAsync(Guid id);
        Task<TimeRangeModel> GetAvailableTimeRangeAsync();
    }
}