using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideDictionaryService : Controller
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileMan;
        private readonly ClaimHelper HelperMan;
        private readonly IContextualService ContextMan;
        private readonly BlobService BlobService;

        public UserSideDictionaryService(TalentContext context, IContextualService contextService, IFileStorageService fm, ClaimHelper hm, BlobService blobService)
        {
            this.DB = context;
            this.ContextMan = contextService;
            this.FileMan = fm;
            this.HelperMan = hm;
            this.BlobService = blobService;
        }

        public string GetUserLogin()
        {
            try
            {
                return ContextMan.CookieClaims.EmployeeId;
            }
            catch
            {
                return "SYSTEM";
            }
        }

        public async Task<UserSideDictionaryPaginateListView> GetDictionaryNameListViewAsync(string employeeId)
        {

            var dictionaryNameList = await (from ct in DB.Dictionaries
                                            where ct.IsDeleted == false && ct.ApprovedAt != null
                                            select new UserSideDictionaryNameListView
                                            {
                                                DictionaryId = ct.DictionaryId,
                                                DictionaryName = ct.DictionaryName,
                                                IsFavorite = ct.IsFavorite
                                            }).ToListAsync();

            var dictionaryIsFavoriteList = await (from dc in DB.Dictionaries
                                                  join dcm in DB.DictionaryMappings on dc.DictionaryId equals dcm.DictionaryId
                                                  where dc.IsDeleted == false && dcm.EmployeeId == employeeId && dc.ApprovedAt != null
                                                  select new UserSideDictionaryIsFavoriteListView
                                                  {
                                                      DictionaryId = dcm.DictionaryId,
                                                      DictionaryName = dc.DictionaryName,
                                                      IsFavorite = dc.IsFavorite,
                                                      EmployeeId = dcm.EmployeeId
                                                  }).ToListAsync();

            return new UserSideDictionaryPaginateListView
            {
                DictionaryList = dictionaryNameList,
                DictionaryIsFavoriteList = dictionaryIsFavoriteList
            };
        }

        public async Task<List<UserSideDictionaryModel>> GetDictionaryDetailByIdListViewAsync(Guid DictionaryId,string employeeId)
        {
            var dictionaryDetail = await (from ct in DB.Dictionaries
                                            where ct.IsDeleted == false && ct.DictionaryId == DictionaryId && ct.ApprovedAt != null
                                            select new UserSideDictionaryModel
                                            {
                                                DictionaryId = ct.DictionaryId,
                                                BlobId = ct.BlobId,
                                                DictionaryName = ct.DictionaryName,
                                                DictionaryStatus = ct.DictionaryStatus,
                                                IsFavorite = ct.IsFavorite,
                                                DictionaryArti = ct.DictionaryArti,
                                                DictionaryFakta = ct.DictionaryFakta,
                                                DictionaryManfaat = ct.DictionaryManfaat,
                                                DictionaryBasicOperation = ct.DictionaryBasicOperation,
                                                CreatedAt = ct.CreatedAt,
                                                UpdatedAt = ct.UpdatedAt
                                            }).AsNoTracking().ToListAsync();

            var index = 0;
            foreach (var datum in dictionaryDetail)
            {

                var imageURL = "";
                var ImageFileType = "";

                var blobData = await this.BlobService.GetBlobById(datum.BlobId);

                imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
                ImageFileType = blobData.Mime;

                dictionaryDetail[index].ImageUrl = imageURL;
                dictionaryDetail[index].FileType = ImageFileType;
                index++;
            }

            return dictionaryDetail;
        }

        public async Task<ActionResult<BaseResponse>> AddIsFavoriteDictionaryAsync(Guid dictionaryId, string employeeId)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var dictionaryToUpdate = await this.DB.Dictionaries.Where(item => item.DictionaryId == dictionaryId).FirstOrDefaultAsync();

                dictionaryToUpdate.IsFavorite = true;
                dictionaryToUpdate.UpdatedAt = thisDate;
                dictionaryToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

                var newDictionaryMappings = new DictionaryMappings
                {
                    DictionaryId = dictionaryId,
                    EmployeeId = employeeId,
                };

                this.DB.DictionaryMappings.Add(newDictionaryMappings);

                await this.DB.SaveChangesAsync();
                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
            
        }

        public async Task<ActionResult<BaseResponse>> RemoveIsFavoriteDictionaryAsync(Guid dictionaryId, string employeeId)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var dictionaryToUpdate = await this.DB.Dictionaries.Where(item => item.DictionaryId == dictionaryId).FirstOrDefaultAsync();

                dictionaryToUpdate.IsFavorite = false;
                dictionaryToUpdate.UpdatedAt = thisDate;
                dictionaryToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

                var dataDictionaryMapping = await this
                    .DB
                    .DictionaryMappings
                    .Where(Q => Q.DictionaryId == dictionaryId && Q.EmployeeId == employeeId)
                    .FirstOrDefaultAsync();

                this.DB.DictionaryMappings.Remove(dataDictionaryMapping);
                await this.DB.SaveChangesAsync();

                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
            
        }
    }
}
