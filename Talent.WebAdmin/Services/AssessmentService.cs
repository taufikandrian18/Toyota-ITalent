using Microsoft.EntityFrameworkCore;
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
    public class AssessmentService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        private readonly IFileStorageService FileMan;

        public AssessmentService(TalentContext context, ClaimHelper claimHelper, IFileStorageService fileService)
        {
            this.DB = context;
            this.ClaimMan = claimHelper;
            this.FileMan = fileService;
        }

        public async Task<AssessmentGridModel> GetAllAssessmentAsync(AssessmentFilterModel filter)
        {
            var query = from a in DB.Assessments
                        join e in DB.Employees on a.EmployeeId equals e.EmployeeId
                        join o in DB.Outlets on e.OutletId equals o.OutletId
                        join epm in DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                        join p in DB.Positions on epm.PositionId equals p.PositionId
                        where a.IsDeleted == false
                        select new
                        {
                            assessment = a,
                            employees = e,
                            outlets = o,
                            position = p
                        };

            if (!String.IsNullOrEmpty(filter.OutletName))
            {
                query = query.Where(Q => Q.outlets.Name.Contains(filter.OutletName));
            }

            if (!String.IsNullOrEmpty(filter.AssessmentName))
            {
                query = query.Where(Q => Q.assessment.Name.Contains(filter.AssessmentName));
            }

            if (!String.IsNullOrEmpty(filter.PositionName))
            {
                query = query.Where(Q => Q.position.PositionName.Contains(filter.PositionName));
            }

            if (!String.IsNullOrEmpty(filter.EmployeeName))
            {
                query = query.Where(Q => Q.employees.EmployeeName.Contains(filter.EmployeeName));
            }

            if (!String.IsNullOrEmpty(filter.PublisherName))
            {
                query = query.Where(Q => Q.assessment.Publisher.Contains(filter.PublisherName));
            }

            if (filter.DateStart != null && filter.DateEnd != null)
            {
                var newStartDate = filter.DateStart.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.DateEnd.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.assessment.CreatedAt >= startDate && Q.assessment.CreatedAt <= endDate) || (Q.assessment.UpdatedAt >= startDate && Q.assessment.UpdatedAt <= endDate));
            }

            var totalFilterData = await query.CountAsync();

            switch (filter.SortBy)
            {
                case "name":
                    query = query.OrderBy(Q => Q.assessment.Name);
                    break;
                case "nameDesc":
                    query = query.OrderByDescending(Q => Q.assessment.Name);
                    break;
                case "employee":
                    query = query.OrderBy(Q => Q.employees.EmployeeName);
                    break;
                case "employeeDesc":
                    query = query.OrderByDescending(Q => Q.employees.EmployeeName);
                    break;
                case "position":
                    query = query.OrderBy(Q => Q.position.PositionName);
                    break;
                case "positionDesc":
                    query = query.OrderByDescending(Q => Q.position.PositionName);
                    break;
                case "outlet":
                    query = query.OrderBy(Q => Q.outlets.Name);
                    break;
                case "outletDesc":
                    query = query.OrderByDescending(Q => Q.outlets.Name);
                    break;
                case "publisher":
                    query = query.OrderBy(Q => Q.assessment.Publisher);
                    break;
                case "publisherDesc":
                    query = query.OrderByDescending(Q => Q.assessment.Publisher);
                    break;
                case "CreatedAt":
                    query = query.OrderBy(Q => Q.assessment.CreatedAt);
                    break;
                case "CreatedAtDesc":
                    query = query.OrderByDescending(Q => Q.assessment.CreatedAt);
                    break;
                case "UpdatedAt":
                    query = query.OrderBy(Q => Q.assessment.UpdatedAt);
                    break;
                case "UpdatedAtDesc":
                    query = query.OrderByDescending(Q => Q.assessment.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.assessment.UpdatedAt);
                    break;
            }

            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            query = query.Skip((int)skipCount).Take(filter.PageSize);

            var assessments = await query.Select(Q => new AssessmentGridViewModel
            {
                AssessmentId = Q.assessment.AssessmentId,
                AssessmentName = Q.assessment.Name,
                EmployeeName = Q.employees.EmployeeName,
                OutletName = Q.outlets.Name,
                PositionName = Q.position.PositionName,
                PublisherName = Q.assessment.Publisher,
                CreatedAt = Q.assessment.CreatedAt,
                UpdatedAt = Q.assessment.UpdatedAt
            }).ToListAsync();

            var grid = new AssessmentGridModel
            {
                Assessments = assessments,
                TotalFilterData = totalFilterData
            };

            return grid;
        }

        public async Task InsertAssessmentAsync(AssessmentCreateModel model)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var getUploadBlob = await this.FileMan.UploadFileFromBase64(model.FileContent);

            var newAssessment = new Assessments
            {
                EmployeeId = model.EmployeeId,
                Name = model.AssessmentName,
                Description = model.Description,
                Publisher = model.Publisher,
                CreatedAt = thisDate,
                CreatedBy = ClaimMan.GetLoginUserId(),
                UpdatedAt = thisDate,
                UpdatedBy = ClaimMan.GetLoginUserId(),
                IsDeleted = false,
                BlobId = getUploadBlob
            };

            this.DB.Assessments.Add(newAssessment);
            await this.DB.SaveChangesAsync();
        }

        public async Task<AssessmentViewDetailModel> GetAssessmentByIdAsync(int assessmentId)
        {
            var data = await (from a in DB.Assessments
                              join b in DB.Blobs on a.BlobId equals b.BlobId
                              join e in DB.Employees on a.EmployeeId equals e.EmployeeId
                              join o in DB.Outlets on e.OutletId equals o.OutletId
                              join d in DB.Dealers on o.DealerId equals d.DealerId
                              join epm in DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                              join p in DB.Positions on epm.PositionId equals p.PositionId
                              where a.AssessmentId == assessmentId
                              select new AssessmentViewDetailModel
                              {
                                  AssessmentId = a.AssessmentId,
                                  AssessmentName = a.Name,
                                  PublisherName = a.Publisher,
                                  Description = a.Description,
                                  Dealer = new DealerDropdownModel
                                  {
                                      DealerId = d.DealerId,
                                      DealerName = d.DealerName
                                  },
                                  Employee = new EmployeeDropdownModel
                                  {
                                      EmployeeId = e.EmployeeId,
                                      EmployeeName = e.EmployeeName
                                  },
                                  Outlet = new OutletDropdownModel
                                  {
                                      OutletId = o.OutletId,
                                      OutletName = o.Name
                                  },
                                  Position = new PositionDropdownModel
                                  {
                                      PositionId = p.PositionId,
                                      PositionName = p.PositionName
                                  },
                                  ImageBlobId = b.BlobId.ToString(),
                                  ImageBlobName = b.Name,
                                  ImageBlobMIME = b.Mime
                              }).FirstOrDefaultAsync();

            return data;
        }

        public async Task UpdateAssessmentAsync(AssessmentUpdateModel model)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var data = await this.DB.Assessments.Where(Q => Q.AssessmentId == model.AssessmentId).FirstOrDefaultAsync();
            data.EmployeeId = model.EmployeeId;
            data.Name = model.AssessmentName;
            data.Publisher = model.Publisher;
            data.Description = model.Description;
            //data.BlobId = model.BlobId;
            data.UpdatedAt = thisDate;
            data.UpdatedBy = ClaimMan.GetLoginUserId();

            if (!String.IsNullOrEmpty(model.FileContent.Base64))
            {
                data.BlobId = await this.FileMan.UploadFileFromBase64(model.FileContent);

                await FileMan.DeleteFileAsync(model.BlobId, model.FileContent.ContentType);
                var BlobId = await this.DB.Blobs.Where(Q => Q.BlobId == model.BlobId).FirstOrDefaultAsync();
                this.DB.Blobs.Remove(BlobId);
            }

            await this.DB.SaveChangesAsync();
        }

        public async Task DeleteAssessmentAsync(int assessmentId)
        {
            var assessmentData = await this.DB.Assessments.Where(Q => Q.AssessmentId == assessmentId).FirstOrDefaultAsync();

            assessmentData.IsDeleted = true;
            assessmentData.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            assessmentData.UpdatedBy = ClaimMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
        }
    }
}
