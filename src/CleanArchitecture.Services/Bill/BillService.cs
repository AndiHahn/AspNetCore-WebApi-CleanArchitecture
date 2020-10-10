using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using CleanArchitecture.Core.Helper;
using CleanArchitecture.Core.QueryParameter;
using CleanArchitecture.Services.Bill.Extensions;
using CleanArchitecture.Core;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Image;
using CleanArchitecture.Core.QueryParameter.Models;
using CleanArchitecture.Services.Bill.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Services.Bill
{
    public class BillService : IBillService
    {
        private readonly IBudgetContext context;
        private readonly IImageStorageService imageStorageService;

        public BillService(IBudgetContext context, IImageStorageService imageStorageService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.imageStorageService = imageStorageService ?? throw new ArgumentNullException(nameof(imageStorageService));
        }

        public async Task<PagedResult<BillModel>> QueryAsync(BillQueryParameter queryParameter)
        {
            var query = context.BillQueries.WithRelationsOrderedByDate();
            query = query.ApplyFilter(queryParameter.Filter)
                         .ApplyOrderBy(queryParameter.Sorting);
            int totalCount = query.Count();
            var queryResult = await query.ApplyPaging(queryParameter)
                            .Select(b => b.ToModel()).ToListAsync();

            return new PagedResult<BillModel>(queryResult, totalCount);
        }

        public async Task<PagedResult<BillModel>> ListAsync(BillSearchParameter searchParameter)
        {
            var query = context.BillQueries.WithRelationsByAccountIdsOrdered(searchParameter.AccountIds);
            if (searchParameter.Search != null)
            {
                query = query.Where(b => b.ShopName.Contains(searchParameter.Search) ||
                                         b.Notes.Contains(searchParameter.Search));
            }
            int totalCount = query.Count();
            var result = await query.ApplyPaging(searchParameter).Select(b => b.ToModel()).ToListAsync();
            return new PagedResult<BillModel>(result, totalCount);
        }

        public async Task<BillModel> GetByIdAsync(int id)
        {
            return (await context.BillQueries.WithRelations().FirstOrDefaultAsync(b => b.Id == id))
                                             .AssertEntityFound(id).ToModel();
        }

        public async Task<BillModel> AddBillAsync(BillCreateModel createModel)
        {
            var billEntity = createModel.ToEntity();
            context.Bill.Add(billEntity);
            await context.SaveChangesAsync();
            return await GetByIdAsync(billEntity.Id);
        }

        public async Task DeleteBillAsync(int id)
        {
            var billEntity = (await context.Bill.FindAsync(id)).AssertEntityFound(id);
            context.Bill.Remove(billEntity);
            await imageStorageService.RemoveImageIfExistsAsync(GetImagePath(id));
            await context.SaveChangesAsync();
        }

        public async Task<BlobDownloadInfo> GetImageAsync(int id)
        {
            await EnsureBillAvailableAsync(id);
            string imageUrl = GetImagePath(id);
            return await imageStorageService.GetImageDownloadInfoAsync(imageUrl);
        }

        public async Task AddImageToBillAsync(int id, IFormFile image)
        {
            await EnsureBillAvailableAsync(id);
            string fileUrl = GetImagePath(id);
            await imageStorageService.UploadImageAsync(fileUrl, image);
        }

        public async Task<BillModel> UpdateBillAsync(int id, BillUpdateModel updateModel)
        {
            var billEntity = (await context.Bill.FindAsync(id)).AssertEntityFound(id);
            updateModel.MergeIntoEntity(billEntity);
            context.UpdateVersion(billEntity, updateModel.Version);
            await context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task DeleteImageAsync(int id)
        {
            await EnsureBillAvailableAsync(id);
            await imageStorageService.RemoveImageAsync(GetImagePath(id));
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

        private string GetImagePath(int billId)
        {
            return Constants.BlobStorage.IMAGES_FOLDER_NAME +
                   Constants.BlobStorage.FOLDER_DELIMITER +
                   billId;
        }

        private async Task EnsureBillAvailableAsync(int id)
        {
            (await context.Bill.FindAsync(id)).AssertEntityFound(id);
        }
    }
}
