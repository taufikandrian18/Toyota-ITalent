using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
    public class ModuleService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper HelperMan;
        private readonly IFileStorageService FileMan;
        private readonly ApprovalService ApprovalMan;

        public ModuleService(TalentContext talentContext, ClaimHelper claimHelper, IFileStorageService fileService, ApprovalService approvalService)
        {
            this.DB = talentContext;
            this.HelperMan = claimHelper;
            this.FileMan = fileService;
            this.ApprovalMan = approvalService;
        }

        /// <summary>
        /// get all module before/after filter and sorting
        /// </summary>
        /// <param name="filter">query from the html to filter and sorting the data at the same time</param>
        /// <returns>return module data after the filter and sorting, total data and total data after filter</returns>
        public async Task<ModuleGridModel> GetAllModulesAsync(ModuleGridFilter filter)
        {
            var query = (from mo in DB.Modules
                         join atm in DB.ApprovalToModules on mo.ModuleId equals atm.ModuleId
                         join a in DB.Approvals on atm.ApprovalId equals a.ApprovalId
                         join aps in DB.ApprovalStatus on a.ApprovalStatusId equals aps.ApprovalStatusId
                         join mob in DB.Blobs on mo.BlobId equals mob.BlobId
                         join mab in DB.Blobs on mo.MaterialBlobId equals mab.BlobId into mabNull
                         from mab1 in mabNull.DefaultIfEmpty()
                         join mat in DB.MaterialTypes on mo.MaterialTypeId equals mat.MaterialTypeId
                         join mtm in DB.ModuleTopicMappings on mo.ModuleId equals mtm.ModuleId
                         join t in DB.Topics on mtm.TopicId equals t.TopicId
                         where mo.IsDeleted == false
                         select new
                         {
                             modules = mo,
                             approvalStatus = aps,
                             materialTypes = mat,
                             topics = t
                         });

            if (!string.IsNullOrEmpty(filter.ModuleName))
            {
                query = query.Where(Q => Q.modules.ModuleName.Contains(filter.ModuleName));
            }

            if (filter.MaterialTypeId != 0)
            {
                query = query.Where(Q => Q.materialTypes.MaterialTypeId == filter.MaterialTypeId);
            }

            if (!string.IsNullOrEmpty(filter.TopicName))
            {
                query = query.Where(Q => Q.topics.TopicName.Contains(filter.TopicName));
            }

            if (filter.ApprovalStatus != 0)
            {
                query = query.Where(Q => Q.approvalStatus.ApprovalStatusId == filter.ApprovalStatus);
            }

            if (filter.DateStart != null && filter.DateEnd != null)
            {
                var newStartDate = filter.DateStart.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.DateEnd.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.modules.CreatedAt >= startDate && Q.modules.CreatedAt <= endDate) || (Q.modules.UpdatedAt >= startDate && Q.modules.UpdatedAt <= endDate));
            }


            var totalDataFilter = await query.CountAsync();

            switch (filter.SortBy)
            {
                case "ModuleName":
                    query = query.OrderBy(Q => Q.modules.ModuleName);
                    break;
                case "ModuleNameDesc":
                    query = query.OrderByDescending(Q => Q.modules.ModuleName);
                    break;
                case "MaterialType":
                    query = query.OrderBy(Q => Q.materialTypes.MaterialTypeName);
                    break;
                case "MaterialTypeDesc":
                    query = query.OrderByDescending(Q => Q.materialTypes.MaterialTypeName);
                    break;
                case "TopicName":
                    query = query.OrderBy(Q => Q.topics.TopicName);
                    break;
                case "TopicNameDesc":
                    query = query.OrderByDescending(Q => Q.topics.TopicName);
                    break;
                case "ApprovalStatus":
                    query = query.OrderBy(Q => Q.approvalStatus.ApprovalName);
                    break;
                case "ApprovalStatusDesc":
                    query = query.OrderByDescending(Q => Q.approvalStatus.ApprovalName);
                    break;
                case "CreatedAt":
                    query = query.OrderBy(Q => Q.modules.CreatedAt);
                    break;
                case "CreatedAtDesc":
                    query = query.OrderByDescending(Q => Q.modules.CreatedAt);
                    break;
                case "UpdatedAt":
                    query = query.OrderBy(Q => Q.modules.UpdatedAt);
                    break;
                case "UpdatedAtDesc":
                    query = query.OrderByDescending(Q => Q.modules.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.modules.UpdatedAt);
                    break;
            }

            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            query = query.Skip((int)skipCount).Take(filter.PageSize);

            var modules = await query.Select(Q => new ModuleGridViewModel
            {
                ModuleId = Q.modules.ModuleId,
                ModuleName = Q.modules.ModuleName,
                TopicName = Q.topics.TopicName,
                TypeMaterialName = Q.materialTypes.MaterialTypeName,
                ApprovalStatus = Q.approvalStatus.ApprovalName,
                CreatedAt = Q.modules.CreatedAt,
                UpdatedAt = Q.modules.UpdatedAt
            }).ToListAsync();

            var grid = new ModuleGridModel
            {
                Modules = modules,
                TotalFilterData = totalDataFilter
            };

            return grid;
        }

        /// <summary>
        /// validate if name exist
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public async Task<bool> ValidateModuleByNameAsync(string moduleName)
        {
            var isExist = await this
                .DB
                .Modules
                .AsNoTracking()
                .Where(Q => Q.IsDeleted == false)
                .AnyAsync(Q => Q.ModuleName == moduleName);

            return isExist;
        }

        /// <summary>
        /// insert new module into database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> InsertModuleAsync(ModuleCreateModel model)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isExist = await ValidateModuleByNameAsync(model.ModuleName);

            if (isExist)
            {
                return false;
            }

            var getUploadModuleBlob = await this.FileMan.UploadFileFromBase64(model.ModuleFileContent);
            Guid? getUploadMaterialBlob = null;

            if (String.IsNullOrEmpty(model.MaterialFileContent.Base64) == false)
            {
                getUploadMaterialBlob = await this.FileMan.UploadFileFromBase64(model.MaterialFileContent);
            }

            var newModule = new Modules
            {
                ModuleName = model.ModuleName,
                ModuleDescription = model.ModuleDesc,
                BlobId = getUploadModuleBlob,
                MaterialTypeId = model.MaterialTypeId,
                MaterialBlobId = getUploadMaterialBlob,
                MaterialLink = model.Link,
                MaterialDownloadable = model.Downloadable,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                IsDeleted = false,
                CreatedBy = this.HelperMan.GetLoginUserId(),
                UpdatedBy = this.HelperMan.GetLoginUserId(),
                ApprovedAt = null
            };

            this.DB.Modules.Add(newModule);

            List<ModuleTopicMappings> topicsMapping = new List<ModuleTopicMappings>();

            foreach (var topic in model.TopicId)
            {
                topicsMapping.Add(new ModuleTopicMappings
                {
                    ModuleId = newModule.ModuleId,
                    TopicId = topic
                });
            }

            this.DB.ModuleTopicMappings.AddRange(topicsMapping);
            var newApproval = new ApprovalCreateFormModel
            {
                ContentName = newModule.ModuleName,
                ContentCategory = ContentCategoryEnums.Module,
                ApprovalStatusId = model.ApprovalStatusId,
                ContentId = newModule.ModuleId,
                PageEnum = PageEnums.Module,
            };

            var approval = await ApprovalMan.CreateNewApproval(newApproval);

            if (approval.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
            {
                newModule.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            await this.DB.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// get module detail by module id
        /// </summary>
        /// <returns></returns>
        public async Task<ModuleViewDetailModel> GetModuleById(int moduleId)
        {
            var data = await (from mo in DB.Modules
                              join apm in DB.ApprovalToModules on mo.ModuleId equals apm.ModuleId
                              join a in DB.Approvals on apm.ApprovalId equals a.ApprovalId
                              join mob in DB.Blobs on mo.BlobId equals mob.BlobId
                              join mab in DB.Blobs on mo.MaterialBlobId equals mab.BlobId into mabNull
                              from mab1 in mabNull.DefaultIfEmpty()
                              join mat in DB.MaterialTypes on mo.MaterialTypeId equals mat.MaterialTypeId
                              where mo.ModuleId == moduleId
                              select new ModuleViewDetailModel
                              {
                                  ModuleId = mo.ModuleId,
                                  ModuleName = mo.ModuleName,
                                  ModuleDescription = mo.ModuleDescription,
                                  ModuleBlobId = mob.BlobId,
                                  ModuleBlobName = mob.Name,
                                  ModuleBlobMIME = mob.Mime,
                                  MaterialType = new MaterialTypeDropdownModel
                                  {
                                      MaterialTypeId = mat.MaterialTypeId,
                                      MaterialTypeName = mat.MaterialTypeName
                                  },
                                  MaterialBlobId = mab1.BlobId,
                                  MaterialBlobName = mab1.Name,
                                  MaterialBlobMIME = mab1.Mime,
                                  isDownloadable = mo.MaterialDownloadable,
                                  MaterialLink = mo.MaterialLink,
                                  ApprovalStatusId = a.ApprovalStatusId,
                                  Topics = new List<TopicDropdownModel>()
                              }).FirstOrDefaultAsync();

            data.Topics = await (from mtm in DB.ModuleTopicMappings
                                 join t in DB.Topics on mtm.TopicId equals t.TopicId
                                 where mtm.ModuleId == moduleId
                                 select new TopicDropdownModel
                                 {
                                     TopicId = t.TopicId,
                                     TopicName = t.TopicName
                                 }).ToListAsync();
            var courseId = await DB.SetupModules.Where(x=> x.ModuleId == data.ModuleId).OrderByDescending(x=> x.SetupModuleId).Select(x=> x.CourseId).FirstOrDefaultAsync();
            var trainingId  = await DB.Trainings.FirstOrDefaultAsync(x=> x.CourseId == courseId);
            //var relatedTraining = await DB.TrainingCertifications.FirstOrDefaultAsync(x=> x.TrainingId == trainingId.TrainingId);
            //data.RelatedTrainingId = relatedTraining == null? "" : relatedTraining.TrainingId.ToString();

            return data;
        }

        public async Task<bool> ValidateUpdateModuleNameAsync(string moduleName, int moduleId)
        {
            var data = await this
                .DB
                .Modules
                .AsNoTracking()
                .Where(Q => Q.ModuleId == moduleId)
                .FirstOrDefaultAsync();

            var isChange = data.ModuleName != moduleName;

            var isExist = await this.ValidateModuleByNameAsync(moduleName);

            var isTrue = isChange == true && isExist == true;

            return isTrue;
        }

        public async Task<bool> UpdateModuleAsync(ModuleUpdateModel model)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isChangedAndExist = await ValidateUpdateModuleNameAsync(model.ModuleName, model.ModuleId);

            if (isChangedAndExist)
            {
                return false;
            }

            var moduleToUpdate = await this.DB.Modules.Where(Q => Q.ModuleId == model.ModuleId).FirstOrDefaultAsync();

            if (!String.IsNullOrEmpty(model.ModuleFileContent.Base64))
            {
                moduleToUpdate.BlobId = await this.FileMan.UploadFileFromBase64(model.ModuleFileContent);
            }

            if (!String.IsNullOrEmpty(model.MaterialFileContent.Base64))
            {
                moduleToUpdate.MaterialBlobId = await this.FileMan.UploadFileFromBase64(model.MaterialFileContent);
            }

            moduleToUpdate.ModuleName = model.ModuleName;
            moduleToUpdate.ModuleDescription = model.ModuleDesc;
            moduleToUpdate.MaterialTypeId = model.MaterialTypeId;
            moduleToUpdate.MaterialLink = model.Link;
            moduleToUpdate.MaterialDownloadable = model.Downloadable;
            moduleToUpdate.UpdatedAt = thisDate;
            moduleToUpdate.UpdatedBy = this.HelperMan.GetLoginUserId();

            var oldData = await this.DB.ModuleTopicMappings.Where(Q => Q.ModuleId == model.ModuleId).ToListAsync();
            this.DB.ModuleTopicMappings.RemoveRange(oldData);

            List<ModuleTopicMappings> newData = new List<ModuleTopicMappings>();
            foreach (var topic in model.TopicId)
            {
                newData.Add(new ModuleTopicMappings
                {
                    ModuleId = model.ModuleId,
                    TopicId = topic
                });
            }
            this.DB.ModuleTopicMappings.AddRange(newData);
            var currentApproval = await DB.ApprovalToModules.Where(Q => Q.ModuleId == moduleToUpdate.ModuleId).FirstOrDefaultAsync();

            var newApproval = new ApprovalUpdateFormModel
            {
                ApprovalId = currentApproval.ApprovalId,
                PageEnum = PageEnums.Module,
                ApprovalStatusId = model.ApprovalStatusId,
                ContentName = moduleToUpdate.ModuleName,
                ContentCategory = ContentCategoryEnums.Module
            };

            var approval = await ApprovalMan.UpdateNewApproval(newApproval);

            if (approval.ApprovalStatusId == ApprovalStatusesEnum.ApproveId) // submit
            {
                moduleToUpdate.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else
            {
                moduleToUpdate.ApprovedAt = null;
            }
            await this.DB.SaveChangesAsync();
            return true;
        }

        public async Task DeleteModuleAsync(DeleteModuleModel model)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            if (!model.isDeleteModule)
            {
                var toBeDeletedMapping = await this.DB.ModuleTopicMappings.Where(Q => Q.ModuleId == model.ModuleId && model.TopicIds.Contains(Q.TopicId)).ToListAsync();

                this.DB.ModuleTopicMappings.RemoveRange(toBeDeletedMapping);
                await this.DB.SaveChangesAsync();
                return;
            }

            var module = await this.DB.Modules.Where(Q => Q.ModuleId == model.ModuleId).FirstOrDefaultAsync();

            var approvalToModule = await this.DB.ApprovalToModules.Where(Q => Q.ModuleId == model.ModuleId).FirstOrDefaultAsync();

            if (approvalToModule == null)
            {
                return;
            }

            this.DB.ApprovalToModules.Remove(approvalToModule);
            var isDeleted = await this.ApprovalMan.DeleteApproval(approvalToModule.ApprovalId);

            if (isDeleted == false)
            {
                return;
            }

            module.IsDeleted = true;
            module.UpdatedAt = thisDate;
            module.UpdatedBy = this.HelperMan.GetLoginUserId();

            var setupLearning = await (from sl in DB.SetupLearning.AsNoTracking()
                                       join sm in DB.SetupModules.AsNoTracking() on sl.SetupModuleId equals sm.SetupModuleId
                                       join m in DB.Modules.AsNoTracking() on sm.ModuleId equals m.ModuleId
                                       where m.ModuleId == model.ModuleId
                                       select sl).FirstOrDefaultAsync();

            if (setupLearning != null)
            {
                DB.SetupLearning.RemoveRange(setupLearning);
            }
            await this.DB.SaveChangesAsync();
            return;
        }

    }
}
