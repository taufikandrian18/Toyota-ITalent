using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
    public class DictionaryService : Controller
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;
        private readonly FileService FileServiceMan;
        private readonly ApprovalService ApprovalMan;
        private readonly IFileStorageService FileMan;
        private readonly ClaimHelper HelperMan;
        private readonly BlobService BlobService;
        private readonly PushNotificationService PNService;

        public DictionaryService(TalentContext db, IContextualService contextual, FileService fs, ApprovalService approvalService, IFileStorageService fm, ClaimHelper hm, BlobService bm, PushNotificationService pn)
        {
            this.DB = db;
            this.ContextMan = contextual;
            this.FileServiceMan = fs;
            this.ApprovalMan = approvalService;
            this.FileMan = fm;
            this.HelperMan = hm;
            this.BlobService = bm;
            this.PNService = pn;
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

        public async Task<DictionaryPaginate> GetAllDictionary(DictionaryGridFilter filter)
        {

            var query = (from sl in this.DB.Dictionaries
                         where sl.IsDeleted == false
                         select new DictionaryModel
                         {
                             DictionaryId = sl.DictionaryId,
                             BlobId = sl.BlobId,
                             DictionaryName = sl.DictionaryName,
                             DictionaryArti = sl.DictionaryArti,
                             DictionaryFakta = sl.DictionaryFakta,
                             DictionaryManfaat = sl.DictionaryManfaat,
                             DictionaryStatus = sl.DictionaryStatus,
                             DictionaryBasicOperation = sl.DictionaryBasicOperation,
                             IsFavorite = sl.IsFavorite.HasValue ? sl.IsFavorite.Value : false,
                             ApprovedAt = sl.ApprovedAt,
                             CreatedAt = sl.CreatedAt,
                             CreatedBy = sl.CreatedBy,
                             UpdatedBy = sl.UpdatedBy,
                             UpdatedAt = sl.UpdatedAt,
                         }).AsNoTracking().AsQueryable();

            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt < endDate) ||
                                         (Q.UpdatedAt >= startDate && Q.UpdatedAt < endDate));
            }

            // search
            if (!string.IsNullOrEmpty(filter.DictionaryName))
            {
                query = query.Where(Q => Q.DictionaryName.Contains(filter.DictionaryName));
            }

            // published
            if (filter.ApprovedAt != null)
            {
                if (filter.ApprovedAt == true)
                {
                    query = query.Where(Q => Q.ApprovedAt != null);
                }
                else
                {
                    query = query.Where(Q => Q.ApprovedAt == null);
                }
            }

            //sort
            switch (filter.SortBy)
            {
                case "DictionaryNameAsc":
                    query = query.OrderBy(Q => Q.DictionaryName);
                    break;
                case "CreatedAtAsc":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "UpdatedAtAsc":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "DictionaryNameDesc":
                    query = query.OrderByDescending(Q => Q.DictionaryName);
                    break;
                case "CreatedAtDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "UpdatedAtDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderBy(Q => Q.DictionaryName);
                    break;
            }
            var dictionaries = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalData = await query.CountAsync();

            var index = 0;
            foreach (var datum in dictionaries)
            {

                var imageURL = "";
                var blobData = await this.BlobService.GetBlobById(datum.BlobId);

                imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

                dictionaries[index].ImageUrl = imageURL;
                dictionaries[index].Blob = blobData;
                index++;
            }

            return new DictionaryPaginate
            {
                DIctionaries = dictionaries,
                TotalDictionaries = totalData
            };
        }

        public async Task<ActionResult<BaseResponse>> InsertDictionaryAsync(DictionaryCreateModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var isExist = await ValidateDictionaryByNameAsync(model.DictionaryName);

                if (isExist)
                {
                    return BadRequest(BaseResponse.BadRequest(model));
                }

                var getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);

                if (model.ProductFileContent != null)
                {
                    if (string.IsNullOrEmpty(model.ProductFileContent.Base64) == false)
                    {
                        getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);
                    }
                }

                var newDictionaries = new Dictionaries
                {
                    DictionaryName = model.DictionaryName,
                    DictionaryStatus = model.DictionaryStatus,
                    BlobId = getUploadModuleBlob,
                    DictionaryArti = model.DictionaryArti,
                    DictionaryFakta = model.DictionaryFakta,
                    DictionaryManfaat = model.DictionaryManfaat,
                    DictionaryBasicOperation = model.DictionaryBasicOperation,
                    ApprovedAt = model.ApprovedAt,
                    CreatedAt = thisDate,
                    UpdatedAt = thisDate,
                    IsDeleted = false,
                    CreatedBy = this.HelperMan.GetLoginUserId(),
                    UpdatedBy = this.HelperMan.GetLoginUserId()
                };

                this.DB.Dictionaries.Add(newDictionaries);

                await this.DB.SaveChangesAsync();
                if (model.ApprovedAt != null)
                {
                    var getIdDictionary = newDictionaries.DictionaryId;
                    var groupPositionList = new List<string>();
                    var manPowerPositionList = new List<string>();
                    var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                    var PushNotificationMyTools = new VMPushNotificationAdd();
                    PushNotificationMyTools.Title = "New Dictionary";
                    PushNotificationMyTools.Body = "New Dictionary " + model.DictionaryName + " has been added.";
                    PushNotificationMyTools.SenderId = this.HelperMan.GetLoginUserId();
                    PushNotificationMyTools.IsPublished = true;
                    PushNotificationMyTools.GroupPositions = groupPositionList;
                    PushNotificationMyTools.ManPowerPosition = manPowerPositionList;

                    PushNotificationDataMyTools.Category = "Dictionary";
                    PushNotificationDataMyTools.DataID = getIdDictionary;
                    PushNotificationDataMyTools.DataSecondId = Guid.Empty;

                    await this.PNService.CreatePushNotificationMyTools(PushNotificationMyTools, PushNotificationDataMyTools);
                }
                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<bool> ValidateDictionaryByNameAsync(string dictionaryName)
        {
            var isExist = await this
                .DB
                .Dictionaries
                .AsNoTracking()
                .Where(Q => Q.IsDeleted == false)
                .AnyAsync(Q => Q.DictionaryName == dictionaryName);

            return isExist;
        }

        public async Task<DictionaryModel> GetDictionaryById(Guid dictionaryId)
        {
            var data = await (from mo in DB.Dictionaries
                              where mo.DictionaryId == dictionaryId && mo.IsDeleted == false
                              select new DictionaryModel
                              {
                                  DictionaryId = mo.DictionaryId,
                                  DictionaryName = mo.DictionaryName,
                                  DictionaryArti = mo.DictionaryArti,
                                  DictionaryFakta = mo.DictionaryFakta,
                                  BlobId = mo.BlobId,
                                  DictionaryManfaat = mo.DictionaryManfaat,
                                  DictionaryStatus = mo.DictionaryStatus,
                                  DictionaryBasicOperation = mo.DictionaryBasicOperation,
                                  IsFavorite = mo.IsFavorite.HasValue ? mo.IsFavorite.Value : false,
                                  ApprovedAt = mo.ApprovedAt,
                                  CreatedAt = mo.CreatedAt,
                                  CreatedBy = mo.CreatedBy,
                                  UpdatedAt = mo.UpdatedAt,
                                  UpdatedBy = mo.UpdatedBy
                              }).FirstOrDefaultAsync();

            var imageURL = "";
            var blobData = await this.BlobService.GetBlobById(data.BlobId);

            imageURL = await this.FileMan.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

            data.ImageUrl = imageURL;
            data.Blob = blobData;

            return data;
        }

        public async Task<ActionResult<BaseResponse>> UpdateDictionaryAsync(DictionaryUpdateModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var isChangedAndExist = await ValidateUpdateDictionaryNameAsync(model.DictionaryName, model.DictionaryId);

                if (isChangedAndExist)
                {
                    return BadRequest(BaseResponse.BadRequest(model));
                }

                var DictionaryToUpdate = await this.DB.Dictionaries.Where(Q => Q.DictionaryId == model.DictionaryId).FirstOrDefaultAsync();

                var getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);

                if (model.ProductFileContent != null)
                {
                    if (!String.IsNullOrEmpty(model.ProductFileContent.Base64))
                    {
                        DictionaryToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.ProductFileContent);
                    }
                }

                DictionaryToUpdate.DictionaryName = model.DictionaryName;
                DictionaryToUpdate.DictionaryStatus = model.DictionaryStatus;
                DictionaryToUpdate.DictionaryArti = model.DictionaryArti;
                DictionaryToUpdate.DictionaryFakta = model.DictionaryFakta;
                DictionaryToUpdate.DictionaryManfaat = model.DictionaryManfaat;
                DictionaryToUpdate.DictionaryBasicOperation = model.DictionaryBasicOperation;
                DictionaryToUpdate.ApprovedAt = model.ApprovedAt;
                DictionaryToUpdate.UpdatedAt = thisDate;
                DictionaryToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

                await this.DB.SaveChangesAsync();
                if (model.ApprovedAt != null)
                {
                    var getIdDictionary = DictionaryToUpdate.DictionaryId;
                    var groupPositionList = new List<string>();
                    var manPowerPositionList = new List<string>();
                    var PushNotificationDataMyTools = new VMPushNotificationDataAddMyTools();
                    var PushNotificationMyTools = new VMPushNotificationAdd();
                    PushNotificationMyTools.Title = "Dictionary Update";
                    PushNotificationMyTools.Body = "Dictionary " + model.DictionaryName + " has been updated.";
                    PushNotificationMyTools.SenderId = this.HelperMan.GetLoginUserId();
                    PushNotificationMyTools.IsPublished = true;
                    PushNotificationMyTools.GroupPositions = groupPositionList;
                    PushNotificationMyTools.ManPowerPosition = manPowerPositionList;

                    PushNotificationDataMyTools.Category = "Dictionary";
                    PushNotificationDataMyTools.DataID = getIdDictionary;
                    PushNotificationDataMyTools.DataSecondId = Guid.Empty;

                    await this.PNService.CreatePushNotificationMyTools(PushNotificationMyTools, PushNotificationDataMyTools);
                }
                return BaseResponse.ResponseOk();
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<bool> ValidateUpdateDictionaryNameAsync(string dictionaryName, Guid dictionaryId)
        {
            var data = await this
                .DB
                .Dictionaries
                .AsNoTracking()
                .Where(Q => Q.DictionaryId == dictionaryId)
                .FirstOrDefaultAsync();

            var isChange = data.DictionaryName != dictionaryName;

            var isExist = await this.ValidateDictionaryByNameAsync(dictionaryName);

            var isTrue = isChange == true && isExist == true;

            return isTrue;
        }

        public async Task<ActionResult<BaseResponse>> DeleteDictionaryAsync(DeleteDictionaryModel model)
        {
            try
            {
                var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

                var module = await this.DB.Dictionaries.Where(Q => Q.DictionaryId == model.DictionaryId).FirstOrDefaultAsync();

                module.IsDeleted = true;
                module.UpdatedAt = thisDate;
                module.UpdatedBy = this.HelperMan.GetLoginUserId();


                await this.DB.SaveChangesAsync();
                return BaseResponse.ResponseOk(null);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
    }
}
