using System;
using System.Threading.Tasks;
using CleanArchitecture.Core.Models.Common;
using CleanArchitecture.Core.Models.Domain.Bill;
using CleanArchitecture.Domain.BlobEntities;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Core.Interfaces.CrudServices
{
    public interface IBillService
    {
        Task<PagedResult<BillModel>> QueryAsync(BillQueryParameter queryParameter);
        Task<PagedResult<BillModel>> ListAsync(BillSearchParameter searchParameter);
        Task<BillModel> GetByIdAsync(Guid id);
        Task<BillModel> AddBillAsync(BillCreateModel createModel);
        Task<BillModel> UpdateBillAsync(Guid id, BillUpdateModel updateModel);
        Task DeleteBillAsync(Guid id);
        Task<BlobDownloadEntity> GetImageAsync(Guid id);
        Task AddImageToBillAsync(Guid id, IFormFile file);
        Task DeleteImageAsync(Guid id);
        Task<TimeRangeModel> GetAvailableTimeRangeAsync();
    }
}