using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class MasterNewsService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        private readonly FileService FileServiceMan;
        private readonly ApprovalService ApprovalServiceMan;


        public MasterNewsService(TalentContext db, ClaimHelper claimHelper, FileService fs, ApprovalService approvalService)
        {
            this.ApprovalServiceMan = approvalService;
            this.DB = db;
            this.ClaimMan = claimHelper;
            this.FileServiceMan = fs;
        }

        /// <summary>
        /// Get news view dengan paginate dan filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<ResponseMasterNewsModel> GetNewsPaginateAsync(MasterNewsFilterModel filter)
        {
            if (filter.Page < 1)
            {
                filter.Page = 1;
            }

            if (filter.ItemPage < 1)
            {
                filter.ItemPage = 10;
            }

            //get all data
            var getData =
            from news in DB.News
            join map in DB.ApprovalToNews
            on news.NewsId equals map.NewsId into maps
            from allMaps in maps.DefaultIfEmpty()
                //join approval in DB.Approvals
                //on allMaps.ApprovalId equals approval.ApprovalId into ap
                //from approvals in ap.DefaultIfEmpty()
            where news.IsDeleted == false
            select new MasterNewsViewModel
            {
                Id = news.NewsId,
                NewsTitle = news.Title,
                NewsCategory = news.NewsCategory.Name,
                NewsStatus = allMaps.Approval.ApprovalStatus.ApprovalName,
                CreatedDate = news.CreatedAt,
                UpdatedDate = news.UpdatedAt
            };

            var totalData2 = await getData.CountAsync();

            //filtering section
            //content name
            if ((string.IsNullOrEmpty(filter.NewsTitle) || string.IsNullOrWhiteSpace(filter.NewsTitle)) == false)
            {
                getData = getData.Where(Q => Q.NewsTitle.ToLower().Contains(filter.NewsTitle.ToLower()));
            }
            // category
            if ((string.IsNullOrEmpty(filter.NewsCategory)) == false)
            {
                getData = getData.Where(Q => Q.NewsCategory.ToLower().Contains(filter.NewsCategory.ToLower()));
            }
            // date
            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                getData = getData.Where(Q => (Q.CreatedDate >= startDate && Q.CreatedDate <= endDate) || (Q.UpdatedDate >= startDate && Q.UpdatedDate <= endDate));
            }

            //status
            if ((string.IsNullOrEmpty(filter.NewsStatus) || string.IsNullOrWhiteSpace(filter.NewsStatus)) == false)
            {
                getData = getData.Where(Q => Q.NewsStatus.ToLower().Contains(filter.NewsStatus.ToLower()));
            }

            // --------------- Order by ------------------

            switch (filter.OrderBy)
            {
                case "NewsTitleUp":
                    {
                        getData = getData.OrderBy(Q => Q.NewsTitle);
                        break;
                    }

                case "NewsTitleDown":
                    {
                        getData = getData.OrderByDescending(Q => Q.NewsTitle);
                        break;
                    }

                case "NewsCategoryUp":
                    {
                        getData = getData.OrderBy(Q => Q.NewsCategory);
                        break;
                    }

                case "NewsCategoryDown":
                    {
                        getData = getData.OrderByDescending(Q => Q.NewsCategory);
                        break;
                    }

                case "NewsStatusUp":
                    {
                        getData = getData.OrderBy(Q => Q.NewsStatus);
                        break;
                    }

                case "NewsStatusDown":
                    {
                        getData = getData.OrderByDescending(Q => Q.NewsStatus);
                        break;
                    }

                case "CreatedDateUp":
                    {
                        getData = getData.OrderBy(Q => Q.CreatedDate);
                        break;
                    }

                case "CreatedDateDown":
                    {
                        getData = getData.OrderByDescending(Q => Q.CreatedDate);
                        break;
                    }

                case "UpdatedDateUp":
                    {
                        getData = getData.OrderBy(Q => Q.UpdatedDate);
                        break;
                    }

                case "UpdatedDateDown":
                    {
                        getData = getData.OrderByDescending(Q => Q.UpdatedDate);
                        break;
                    }

                case "":
                    {
                        getData = getData.OrderByDescending(Q => Q.UpdatedDate.Value).ThenByDescending(Q => Q.UpdatedDate);
                        break;
                    }
                case null:
                    {
                        getData = getData.OrderByDescending(Q => Q.UpdatedDate).ThenByDescending(Q => Q.UpdatedDate);
                        break;
                    }

                default:
                    {
                        getData = getData.OrderByDescending(Q => Q.UpdatedDate).ThenByDescending(Q => Q.UpdatedDate);
                        break;
                    }
            }

            // ------------ final --------------------
            var totalData = await getData.CountAsync();

            var listData = await getData
                .Skip((filter.Page - 1) * filter.ItemPage)
                .Take(filter.ItemPage)
                .AsNoTracking()
                .ToListAsync();

            var responseModel = new ResponseMasterNewsModel
            {
                ContentList = listData,
                TotalData = totalData
            };

            return responseModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<string> ChangeNewsStatusAsync(int id, string type)
        {
            var getData = await DB.ApprovalToNews.Where(Q => Q.ApprovalId == id).Select(Q => Q.Approval).FirstOrDefaultAsync();
            var getStatusId = await DB.ApprovalStatus.Where(Q => Q.ApprovalName == type).Select(Q => Q.ApprovalStatusId).FirstOrDefaultAsync();

            if (getData == null || getStatusId == 0)
            {
                return ResponseMessageEnum.FailedSave;
            }

            getData.ApprovalStatusId = getStatusId;
            // getData.ActionAt = DateTime.Now;

            DB.Approvals.Update(getData);

            try
            {
                await DB.SaveChangesAsync();
            }
            catch (Exception)
            {
                return ResponseMessageEnum.FailedSave;
            }

            return ResponseMessageEnum.SuccessSave;
        }

        /// <summary>
        /// insert news
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> InsertMasterNewsAsync(string saveType, MasterNewsFormModel model)
        {
            var type = 0;

            //create news model
            var NewsModel = new News
            {
                Title = model.NewsTitle,
                Description = model.Description,
                Link = model.NewsLink,
                Author = model.Author,
                NewsCategoryId = model.NewsCategoryId,
                IsDownloadable = model.IsDownloadable,
                IsDeleted = false,

                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = ClaimMan.GetLoginUserId(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedBy = ClaimMan.GetLoginUserId()
            };

            if (model.FileUpload != null)
            {
                if (string.IsNullOrEmpty(model.FileUpload.Base64) == false)
                {
                    var blobId = await FileServiceMan.UploadFileFromBase64(model.FileUpload);
                    NewsModel.FileBlobId = blobId;
                }
            }

            if (model.ThumbnailUpload != null)
            {
                if (string.IsNullOrEmpty(model.ThumbnailUpload.Base64) == false)
                {
                    var blobId = await FileServiceMan.UploadFileFromBase64(model.ThumbnailUpload);
                    NewsModel.ThumbnailBlobId = blobId;
                }
            }

            // getId approval mapping id
            var getApprovalMapId = await DB.ApprovalMappings.Where(Q => Q.Page.PageId == PageEnums.News).Select(Q => Q.ApprovalMappingid).FirstOrDefaultAsync();

            // if submit dan approval map id exist
            if (saveType.ToLower() == "submit")
            {
                type = ApprovalStatusesEnum.WaitingId;
            }
            else
            {
                type = ApprovalStatusesEnum.DraftId;
            }

            //add model to table
            DB.News.Add(NewsModel);

            var newApproval = new ApprovalCreateFormModel
            {
                PageEnum = PageEnums.News,
                ContentName = model.NewsTitle,
                ApprovalStatusId = type,
                ContentCategory = ContentCategoryEnums.News,
                ContentId = NewsModel.NewsId
            };


            var approvals = await this.ApprovalServiceMan.CreateNewApproval(newApproval);

            if (approvals.ApprovalStatusId == 1)
            {
                NewsModel.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else
            {
                NewsModel.ApprovedAt = null;
            }

            //save change DB
            try
            {
                await DB.SaveChangesAsync();
            }
            catch (Exception)
            {
                return ResponseMessageEnum.FailedSave;
            }

            return ResponseMessageEnum.SuccessAddSave;
        }

        public async Task<MasterNewsFormModel> GetDetailNewsByIdAsync(int id)
        {

            var getNewsData = await DB.News
            .Where(Q => Q.NewsId == id)
            .Select(Q => new MasterNewsFormModel
            {
                NewsTitle = Q.Title,
                Description = Q.Description,
                NewsLink = Q.Link,
                Author = Q.Author,
                NewsCategoryId = Q.NewsCategoryId,
                IsDownloadable = Q.IsDownloadable,
                FileBlobId = Q.FileBlobId,
                ThumbnailBlobId = Q.ThumbnailBlobId,
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

            return getNewsData;
        }

        /// <summary>
        /// update data news
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> UpdateMasterNewsAsync(string saveType, MasterNewsFormModel model)
        {
            var type = 0;

            //get data
            var getNewsData = await DB.News.Where(Q => Q.NewsId == model.NewsId).FirstOrDefaultAsync();

            if (getNewsData == null)
            {
                return ResponseMessageEnum.FailedSave;
            }

            Blobs fileBlobs = null;
            Blobs thumbnailBlobs = null;

            if (getNewsData.FileBlobId != null)
            {
                fileBlobs = await DB.Blobs.Where(Q => Q.BlobId == getNewsData.FileBlobId).FirstOrDefaultAsync();

            }
            if (getNewsData.FileBlobId != null)
            {
                thumbnailBlobs = await DB.Blobs.Where(Q => Q.BlobId == getNewsData.FileBlobId).FirstOrDefaultAsync();
            }

            var getApprovalData = await DB.ApprovalToNews.Where(Q => Q.NewsId == model.NewsId).FirstOrDefaultAsync();

            //if (getApprovalData == null)
            //{
            //    return ResponseMessageEnum.FailedSave;
            //}

            //update news data
            getNewsData.Title = model.NewsTitle;
            getNewsData.Description = model.Description;
            getNewsData.Link = model.NewsLink;
            getNewsData.Author = model.Author;
            getNewsData.NewsCategoryId = model.NewsCategoryId;
            getNewsData.IsDownloadable = model.IsDownloadable;
            getNewsData.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            getNewsData.UpdatedBy = ClaimMan.GetLoginUserId();

            if (model.FileUpload != null)
            {
                if (string.IsNullOrEmpty(model.FileUpload.Base64) == false)
                {
                    var blobId = await FileServiceMan.UploadFileFromBase64(model.FileUpload);

                    if (fileBlobs != null)
                    {
                        await FileServiceMan.DeleteFileAsync(fileBlobs.BlobId, fileBlobs.Mime);
                    }

                    getNewsData.FileBlobId = blobId;
                }
            }

            if (model.ThumbnailUpload != null)
            {
                if (string.IsNullOrEmpty(model.ThumbnailUpload.Base64) == false)
                {
                    var blobId = await FileServiceMan.UploadFileFromBase64(model.ThumbnailUpload);

                    if (thumbnailBlobs != null)
                    {
                        await FileServiceMan.DeleteFileAsync(thumbnailBlobs.BlobId, thumbnailBlobs.Mime);
                    }

                    getNewsData.ThumbnailBlobId = blobId;
                }
            }

            // if submit
            if (saveType.ToLower() == "submit")
            {
                type = ApprovalStatusesEnum.WaitingId;
            }
            else
            {
                type = ApprovalStatusesEnum.DraftId;
            }

            var newApprovals = new ApprovalUpdateFormModel
            {
                ApprovalId = getApprovalData.ApprovalId,
                ApprovalStatusId = type,
                ContentName = model.NewsTitle,
                PageEnum = PageEnums.News,
                ContentId = getNewsData.NewsId,
                ContentCategory = ContentCategoryEnums.News
            };

            var approvals = await this.ApprovalServiceMan.UpdateNewApproval(newApprovals);

            if(approvals.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
            {
                getNewsData.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else
            {
                getNewsData.ApprovedAt = null;
            }

            //update model to table
            DB.News.Update(getNewsData);

            //save change DB
            try
            {
                await DB.SaveChangesAsync();
            }
            catch (Exception)
            {
                return ResponseMessageEnum.FailedSave;
            }

            return ResponseMessageEnum.SuccessEditSave;
            //if(type == ApprovalStatusesEnum.Approve || type == ApprovalStatusesEnum.Waiting){
            //}
            //else{
            //    return "";
            //}

        }

        /// <summary>
        /// soft delete data news 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> DeleteNewsAsync(int id)
        {
            var getNewsData = await DB.News.Where(Q => Q.NewsId == id).FirstOrDefaultAsync();

            if (getNewsData == null)
            {
                return ResponseMessageEnum.FailedDelete;
            }

            var approvalToNews = await this.DB.ApprovalToNews.Where(Q => Q.NewsId == id).FirstOrDefaultAsync();

            if (approvalToNews == null)
            {
                return ResponseMessageEnum.FailedDelete;
            }

            this.DB.ApprovalToNews.Remove(approvalToNews);

            var isDeleted = await this.ApprovalServiceMan.DeleteApproval(approvalToNews.ApprovalId);

            if (isDeleted == false)
            {
                return ResponseMessageEnum.FailedDelete;
            }

            getNewsData.IsDeleted = true;
            getNewsData.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            getNewsData.UpdatedBy = ClaimMan.GetLoginUserId();


            //update dan remove
            DB.News.Update(getNewsData);

            //save change DB
            try
            {
                await DB.SaveChangesAsync();
            }
            catch (Exception)
            {
                return ResponseMessageEnum.FailedDelete;
            }

            return ResponseMessageEnum.SuccessDelete;
        }
    }
}