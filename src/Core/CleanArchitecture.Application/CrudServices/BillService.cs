using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.CrudServices.Models.Bill;
using CleanArchitecture.Application.Validations;
using CleanArchitecture.Common.Models.Resource.Bill;
using CleanArchitecture.Domain.BlobEntities;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.CrudServices
{
    public class BillService : IBillService
    {
        private readonly IMapper mapper;
        private readonly IBillRepository billRepository;
        private readonly IBlobStorageRepository blobStorageRepository;

        public BillService(
                    IMapper mapper,
                    IBillRepository billRepository,
                    IBlobStorageRepository blobStorageRepository)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
            this.blobStorageRepository = blobStorageRepository ?? throw new ArgumentNullException(nameof(blobStorageRepository));
        }

        public async Task<PagedResult<BillModel>> QueryAsync(BillQueryParameter queryParameter, Guid currentUserId)
        {
            var pagedResult = await billRepository.ListByUserAsync(queryParameter, currentUserId);

            return new PagedResult<BillModel>(
                pagedResult.Result.Select(mapper.Map<BillModel>),
                pagedResult.TotalCount);
        }

        public async Task<PagedResult<BillModel>> ListAsync(BillSearchParameter searchParameter, Guid currentUserId)
        {
            var pagedResult = await billRepository.SearchByUserAsync(searchParameter, currentUserId);

            return new PagedResult<BillModel>(
                pagedResult.Result.Select(mapper.Map<BillModel>),
                pagedResult.TotalCount);
        }

        public async Task<BillModel> GetByIdAsync(Guid id, Guid currentUserId)
        {
            var entity = await LoadBillAndEnsureAccessRightAsync(id, currentUserId);
            return mapper.Map<BillModel>(entity);
        }

        public async Task<BillModel> CreateBillAsync(BillCreateModel createModel, Guid currentUserId)
        {
            var billEntity = mapper.Map<BillEntity>(createModel);
            billEntity.CreatedByUserId = currentUserId;

            var createdEntity = await billRepository.AddAsync(billEntity);

            return mapper.Map<BillModel>(createdEntity);
        }

        public async Task DeleteBillAsync(Guid id, Guid currentUserId)
        {
            var billEntity = await LoadBillAndEnsureAccessRightAsync(id, currentUserId);
            if (billEntity.CreatedByUserId != currentUserId)
            {
                throw new ForbiddenException("Cannot delete bill from another user.");
            }

            await billRepository.DeleteAsync(billEntity);

            await blobStorageRepository.RemoveBlobIfExistsAsync(
                        Constants.ImageStorage.ContainerName, GetImagePath(id));
        }

        public async Task<BlobEntity> GetImageAsync(Guid id, Guid currentUserId)
        {
            await LoadBillAndEnsureAccessRightAsync(id, currentUserId);
            string imageUrl = GetImagePath(id);
            return await blobStorageRepository.DownloadBlobAsync(
                    Constants.ImageStorage.ContainerName, imageUrl);
        }

        public async Task AddImageToBillAsync(Guid id, IFormFile file, Guid currentUserId)
        {
            await LoadBillAndEnsureAccessRightAsync(id, currentUserId);
            string fileUrl = GetImagePath(id);

            var entity = new BlobEntity();
            await file.CopyToAsync(entity.Content);
            entity.ContentType = file.ContentType;

            await blobStorageRepository.UploadBlobAsync(
                        Constants.ImageStorage.ContainerName, fileUrl, entity);
        }

        public async Task<BillModel> UpdateBillAsync(Guid id, BillUpdateModel updateModel, Guid currentUserId)
        {
            var billEntity = await LoadBillAndEnsureAccessRightAsync(id, currentUserId);

            updateModel.MergeIntoEntity(billEntity);

            await billRepository.UpdateAsync(billEntity);

            return await GetByIdAsync(id, currentUserId);
        }

        public async Task DeleteImageAsync(Guid id, Guid currentUserId)
        {
            await LoadBillAndEnsureAccessRightAsync(id, currentUserId);
            await blobStorageRepository.RemoveBlobAsync(
                        Constants.ImageStorage.ContainerName, GetImagePath(id));
        }

        private string GetImagePath(Guid billId)
        {
            return Constants.ImageStorage.ImagesFolderName +
                   Constants.ImageStorage.FolderDelimiter +
                   billId;
        }

        private async Task<BillEntity> LoadBillAndEnsureAccessRightAsync(Guid billId, Guid currentUserId)
        {
            var bill = (await billRepository.GetByIdAsync(billId)).AssertEntityFound(billId);

            if (bill.CreatedByUserId != currentUserId)
            {
                throw new ForbiddenException($"Current user does not have access to bill {billId}");
            }

            return bill;
        }
    }
}
