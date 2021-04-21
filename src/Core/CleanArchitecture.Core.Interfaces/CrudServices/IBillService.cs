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
        Task<PagedResult<BillModel>> QueryAsync(BillQueryParameter queryParameter, Guid currentUserId);
        Task<PagedResult<BillModel>> ListAsync(BillSearchParameter searchParameter, Guid currentUserId);
        Task<BillModel> GetByIdAsync(Guid id, Guid currentUserId);
        Task<BillModel> CreateBillAsync(BillCreateModel createModel, Guid currentUserId);
        Task<BillModel> UpdateBillAsync(Guid id, BillUpdateModel updateModel, Guid currentUserId);
        Task DeleteBillAsync(Guid id, Guid currentUserId);
        Task<BlobEntity> GetImageAsync(Guid id, Guid currentUserId);
        Task AddImageToBillAsync(Guid id, IFormFile file, Guid currentUserId);
        Task DeleteImageAsync(Guid id, Guid currentUserId);
    }
}