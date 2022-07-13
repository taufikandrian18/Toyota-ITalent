using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
    public class DigitalSignatureService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileServiceMan;
        private readonly IContextualService ContextMan;


        public DigitalSignatureService(TalentContext talentContext, IFileStorageService fileService, IContextualService contextualService)
        {
            this.DB = talentContext;
            this.FileServiceMan = fileService;
            this.ContextMan = contextualService;
        }

        public string GetLoginUser()
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

        /// <summary>
        /// Get All Digital Signature using filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<DigitalSignatureViewModel> GetDigitalSignature(DigitalSignatureFilter filter)
        {

            var query = from ds in DB.DigitalSignatures
                        join emp in DB.Employees on ds.EmployeeId equals emp.EmployeeId
                        join b in DB.Blobs on ds.BlobId equals b.BlobId
                        select new DigitalSignatureJoinedModel
                        {
                            BlobId = ds.BlobId,
                            BlobName = b.Name,
                            CreatedAt = ds.CreatedAt,
                            DigitalSignatureId = ds.DigitalSignatureId,
                            EmployeeId = ds.EmployeeId,
                            EmployeeName = emp.EmployeeName,
                            IsDeleted = ds.IsDeleted,
                            UpdatedAt = ds.UpdatedAt,
                            Mime = b.Mime
                        };


            if (string.IsNullOrEmpty(filter.EmployeeName) == false)
            {
                query = query.Where(Q => Q.EmployeeName.Contains(filter.EmployeeName));
            }

            switch (filter.SortBy)
            {
                case "employeeName":
                    query = query.OrderBy(Q => Q.EmployeeName);
                    break;
                case "employeeNameDesc":
                    query = query.OrderByDescending(Q => Q.EmployeeName);
                    break;
                case "uploadFile":
                    query = query.OrderBy(Q => Q.BlobName);
                    break;
                case "uploadFileDesc":
                    query = query.OrderByDescending(Q => Q.BlobName);
                    break;
                case "createdDate":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "createdDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "updateDate":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "updateDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var totalData = await query.Where(Q => Q.IsDeleted == false).CountAsync();
            var resultQuery = await query.Where(Q => Q.IsDeleted == false).Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();

            var digitalSignatureResult = new DigitalSignatureViewModel
            {
                ListDigitalSignature = resultQuery,
                TotalItem = totalData
            };

            return digitalSignatureResult;
        }

        /// <summary>
        /// Insert Digital Singnature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> InsertDigitalSignature(DigitalSignatureFormModel model)
        {

            try
            {
                //var searchBlob = await this.DB.Blobs.Where(Q => Q.BlobId == model.BlobId).FirstOrDefaultAsync();

                //if (searchBlob == null)
                //{
                //    return -1;
                //}

                var searchEmployee = await this.DB.DigitalSignatures.Where(Q => Q.EmployeeId == model.EmployeeId && Q.IsDeleted == false).FirstOrDefaultAsync();

                if (searchEmployee != null)
                {
                    return -2;
                }

                var getUploadBlob = await this.FileServiceMan.UploadFileFromBase64(model.FileContent);

                var insertDigital = new DigitalSignatures
                {
                    //BlobId = model.BlobId,
                    BlobId = getUploadBlob,
                    EmployeeId = model.EmployeeId,
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    IsDeleted = false,
                    CreatedBy = GetLoginUser(),
                    UpdatedBy = GetLoginUser()
                };

                this.DB.DigitalSignatures.Add(insertDigital);
                await this.DB.SaveChangesAsync();

                return insertDigital.DigitalSignatureId;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                var errorMessage = e.Message;
                if (model.BlobId != null)
                {
                    var blob = await this.DB.Blobs.Where(Q => Q.BlobId == model.BlobId).FirstOrDefaultAsync();
                    if (blob != null)
                    {
                        await this.FileServiceMan.DeleteFileAsync(blob.BlobId, blob.Mime);
                        this.DB.Blobs.Remove(blob);
                        await this.DB.SaveChangesAsync();
                    }
                }
                return -1;
            }
        }


        /// <summary>
        /// Delete Digital Signature
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteDigitalSignature(int id)
        {
            var query = await this.DB.DigitalSignatures.Where(Q => Q.DigitalSignatureId == id && Q.IsDeleted == false).FirstOrDefaultAsync();
            if (query == null)
            {
                return false;
            }

            query.IsDeleted = true;
            query.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            query.UpdatedBy = GetLoginUser();
            await this.DB.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Validate Update for Digital Signature
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public async Task<bool> ValidateUpdate(int id, string employeeID)
        {
            var findDigitalSignatureById = await this.DB.DigitalSignatures.Where(Q => Q.DigitalSignatureId == id).FirstOrDefaultAsync();

            if (findDigitalSignatureById == null)
            {
                return false;
            }

            var duplicateQuery = this.DB.DigitalSignatures.Where(Q => Q.EmployeeId == employeeID && Q.IsDeleted == false);
            var duplicate = await duplicateQuery.FirstOrDefaultAsync();

            if (findDigitalSignatureById != duplicate)
            {
                var counterEmployee = await duplicateQuery.CountAsync();

                if (counterEmployee != 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Update Digital Signature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateDigitalSignature(DigitalSignatureFormModel model)
        {
            var findDigitalSignatureById = await this.DB.DigitalSignatures.Where(Q => Q.DigitalSignatureId == model.DigitalSignatureId).FirstOrDefaultAsync();

            if (findDigitalSignatureById == null)
            {
                return false;
            }

            var duplicateQuery = this.DB.DigitalSignatures.Where(Q => Q.EmployeeId == model.EmployeeId && Q.IsDeleted == false);
            var duplicate = await duplicateQuery.FirstOrDefaultAsync();

            if (findDigitalSignatureById != duplicate)
            {
                var counterEmployee = await duplicateQuery.CountAsync();

                if (counterEmployee != 0)
                {
                    return false;
                }
            }

            //findDigitalSignatureById.BlobId = model.BlobId;
            if (!String.IsNullOrEmpty(model.FileContent.Base64))
            {
                findDigitalSignatureById.BlobId = await this.FileServiceMan.UploadFileFromBase64(model.FileContent);
            }

            findDigitalSignatureById.EmployeeId = model.EmployeeId;
            findDigitalSignatureById.UpdatedAt = DateTime.UtcNow.AddHours(7);
            findDigitalSignatureById.UpdatedBy = GetLoginUser();

            await this.DB.SaveChangesAsync();

            return true;
        }

    }
}
