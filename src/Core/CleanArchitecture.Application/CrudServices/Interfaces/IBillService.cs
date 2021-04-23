using System;
using System.Threading.Tasks;
using CleanArchitecture.Application.CrudServices.Models.Bill;
using CleanArchitecture.Common.Models.Resource.Bill;
using CleanArchitecture.Domain.BlobEntities;
using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.CrudServices
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