using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Infrastructure;
using CleanArchitecture.Core.Interfaces.Infrastructure.AzureStorage;
using CleanArchitecture.Core.Interfaces.Models;
using CleanArchitecture.Core.Interfaces.Services.Bill;
using CleanArchitecture.Core.Interfaces.Services.Bill.Models;
using CleanArchitecture.Core.Validations;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Core.CrudServices
{
    public class BillService : IBillService
    {
        private readonly IBudgetContext context;
        private readonly IMapper mapper;
        private readonly IBlobStorageRepository blobStorageRepository;

        public BillService(
                    IBudgetContext context,
                    IMapper mapper,
                    IBlobStorageRepository blobStorageRepository)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.blobStorageRepository = blobStorageRepository ?? throw new ArgumentNullException(nameof(blobStorageRepository));
        }

        public async Task<PagedResult<BillModel>> QueryAsync(BillQueryParameter queryParameter)
        {
            var pagedResult = await context.BillQueries
                .QueryAsync(queryParameter);

            return new PagedResult<BillModel>(
                pagedResult.Result.Select(mapper.Map<BillModel>),
                pagedResult.TotalCount);
        }

        public async Task<PagedResult<BillModel>> ListAsync(BillSearchParameter searchParameter)
        {
            var pagedResult = await context.BillQueries
                .SearchAsync(searchParameter);

            return new PagedResult<BillModel>(
                pagedResult.Result.Select(mapper.Map<BillModel>),
                pagedResult.TotalCount);
        }

        public async Task<BillModel> GetByIdAsync(Guid id)
        {
            var entity = (await context.Bill.FindAsync(id)).AssertEntityFound(id);
            return mapper.Map<BillModel>(entity);
        }

        public async Task<BillModel> AddBillAsync(BillCreateModel createModel)
        {
            var billEntity = mapper.Map<BillEntity>(createModel);
            context.Bill.Add(billEntity);
            await context.SaveChangesAsync();
            return await GetByIdAsync(billEntity.Id);
        }

        public async Task DeleteBillAsync(Guid id)
        {
            var billEntity = (await context.Bill.FindAsync(id)).AssertEntityFound(id);
            context.Bill.Remove(billEntity);
            await blobStorageRepository.RemoveBlobIfExistsAsync(
                        Constants.ImageStorage.CONTAINER_NAME, GetImagePath(id));
            await context.SaveChangesAsync();
        }

        public async Task<BlobDownloadModel> GetImageAsync(Guid id)
        {
            await EnsureBillAvailableAsync(id);
            string imageUrl = GetImagePath(id);

            try
            {
                return await blobStorageRepository.DownloadBlobAsync(
                    Constants.ImageStorage.CONTAINER_NAME, imageUrl);
            }
            catch (NotFoundException)
            {
                return null;
            }
        }

        public async Task AddImageToBillAsync(Guid id, IFormFile file)
        {
            await EnsureBillAvailableAsync(id);
            string fileUrl = GetImagePath(id);
            var uploadModel = new BlobUploadModel();
            await file.CopyToAsync(uploadModel.Content);
            uploadModel.ContentType = file.ContentType;
            await blobStorageRepository.UploadBlobAsync(
                        Constants.ImageStorage.CONTAINER_NAME, fileUrl, uploadModel);
        }

        public async Task<BillModel> UpdateBillAsync(Guid id, BillUpdateModel updateModel)
        {
            var billEntity = (await context.Bill.FindAsync(id)).AssertEntityFound(id);
            updateModel.MergeIntoEntity(billEntity);
            await context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task DeleteImageAsync(Guid id)
        {
            await EnsureBillAvailableAsync(id);
            await blobStorageRepository.RemoveBlobAsync(
                        Constants.ImageStorage.CONTAINER_NAME, GetImagePath(id));
        }

        public async Task<TimeRangeModel> GetAvailableTimeRangeAsync()
        {
            var firstBill = await context.Bill.OrderBy(b => b.Date).FirstOrDefaultAsync();
            var lastBill = await context.Bill.OrderByDescending(b => b.Date).FirstOrDefaultAsync();
            if (firstBill == null || lastBill == null)
            {
                throw new NotFoundException("No bill is available.");
            }

            return new TimeRangeModel(firstBill.Date, lastBill.Date);
        }

        private string GetImagePath(Guid billId)
        {
            return Constants.ImageStorage.IMAGES_FOLDER_NAME +
                   Constants.ImageStorage.FOLDER_DELIMITER +
                   billId;
        }

        private async Task EnsureBillAvailableAsync(Guid id)
        {
            (await context.Bill.FindAsync(id)).AssertEntityFound(id);
        }
    }
}
