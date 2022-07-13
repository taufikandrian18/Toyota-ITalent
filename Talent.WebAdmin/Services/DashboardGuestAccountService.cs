using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
    public class DashboardGuestAccountService : Controller
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        IFileStorageService FileService;
        private readonly BlobService BlobService;

        public DashboardGuestAccountService(TalentContext db, ClaimHelper claimHelper, IFileStorageService fileService, BlobService blobService)
        {
            this.DB = db;
            this.ClaimMan = claimHelper;
            this.BlobService = blobService;
            this.FileService = fileService;
        }


        public async Task<ActionResult<BaseResponse>> GetDetail(string employeeId)
        {
            try
            {
                var getEmployeeListGuestAccount = from t1 in DB.Employees
                                                  join t21 in DB.EmployeePositionMappings on t1.EmployeeId equals t21.EmployeeId
                                                  join t22 in DB.Positions on t21.PositionId equals t22.PositionId
                                                  join t3 in DB.Outlets on t1.OutletId equals t3.OutletId
                                                  join t4 in DB.Dealers on t3.DealerId equals t4.DealerId
                                                  join t5 in DB.Blobs on t1.BlobId equals t5.BlobId into blb
                                                  from z in blb.DefaultIfEmpty()
                                                  where t1.IsDeleted == false && t1.EmployeeId == employeeId
                                                  select new EmployeeGuestModel
                                                  {
                                                      EmployeeId = t1.EmployeeId,
                                                      OutletId = t1.OutletId,
                                                      OutletCode = t3.OutletCode,
                                                      OutletName = t3.Name,
                                                      OutletPhone = t3.Phone,
                                                      DealerId = t4.DealerId,
                                                      DealerName = t4.DealerName,
                                                      EmployeeUsername = t1.EmployeeUsername,
                                                      EmployeeName = t1.EmployeeName,
                                                      EmployeeExperience = t1.EmployeeExperience,
                                                      EmployeeEmail = t1.EmployeeEmail,
                                                      EmployeePhone = t1.EmployeePhone,
                                                      Gender = t1.Gender,
                                                      ManpowerStatusType = t1.ManpowerStatusType,
                                                      DealerPeopleCategoryCode = t1.DealerPeopleCategoryCode,
                                                      PositionId = t22.PositionId,
                                                      PositionName = t22.PositionName,
                                                      PositionCode = t22.PositionCode,
                                                      Religion = t1.Religion,
                                                      Ktp = t1.Ktp,
                                                      Address = t1.Addresses,
                                                      AccountValid = t1.AccountValid.Value,
                                                      Picture = (z == null ? String.Empty : z.BlobId.ToString()),
                                                      CreatedBy = t1.CreatedBy,
                                                      UpdatedAt = t1.UpdatedAt,
                                                      UpdatedBy = t1.UpdatedBy,
                                                      IsDeleted = t1.IsDeleted,
                                                      ManpowerCode = t1.ManpowerCode,
                                                      DateOfBirth = t1.DateOfBirth.Value,


                                                  };

                var result = await getEmployeeListGuestAccount.ToListAsync();

                foreach (var datum in result)
                {
                    if (datum.Picture != null)
                    {
                        var imageUrlEmployee = "";
                        var blobData = await this.BlobService.GetBlobById(Guid.Parse(datum.Picture));

                        imageUrlEmployee = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);

                        // Change BlobId with ImageUrl.
                        datum.Picture = imageUrlEmployee;
                    }
                }

                var data = new EmployeeListGuestAccount
                {
                    listEmployeeGuestAccount = result
                };
                return StatusCode(200, BaseResponse.ResponseOk(data));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetListGuestAccount(GetAccountParameter model)
        {
            var count = 0;
            try
            {
                if (model.Limit == 0)
                {
                    model.Limit = 10;
                }

                if (model.Page == 0)
                {
                    model.Page = 1;
                }

                model.Page = (model.Page - 1) * model.Limit;

                var query = DB.Employees.Include(Q => Q.Outlet).Include(Q => Q.EmployeePositionMappings).AsQueryable();

                if (model.Status != null)
                {
                    query = query.Where(Q => model.Status.Contains(Q.Status)).AsQueryable();
                  
                }

                if (model.RegisterdDateStart != null && model.RegisterdDateEnd != null)
                {
                    query = query.Where(Q => Q.CreatedAt.Date >= model.RegisterdDateStart.Value.Date && Q.CreatedAt.Date <= model.RegisterdDateEnd).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(model.DealerID))
                {
                    query = query.Where(Q => Q.Outlet.DealerId == model.DealerID).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(model.OutletID))
                {
                    query = query.Where(Q => Q.OutletId == model.OutletID).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(model.PositionID))
                {
                    var positionID = int.Parse(model.PositionID);
                    query = query.Where(Q => Q.EmployeePositionMappings.FirstOrDefault().PositionId == positionID).AsQueryable();
                }

                if (model.IsSuspended != null) {
                    query = query.Where(Q => Q.IsSuspended == model.IsSuspended.Value);
                }

                if (model.IsRequetUpgrade != null) {
                    query = query.Where(Q => Q.IsRequestUpgrade == model.IsRequetUpgrade);
                }

                if (model.IsDataValidation != null) {
                    query = query.Where(Q => Q.IsDataValidation == model.IsDataValidation);
                }

                

                //if (!String.IsNullOrWhiteSpace(model.Manpower)) {
                //    query = query.Where(Q => Q.ManpowerStatusType == model.Manpower).AsQueryable();

                //}

                query = query.Where(Q => Q.ManpowerStatusType == "GuestAccount" || Q.ManpowerStatusType == "GuestAccountDealer" || Q.ManpowerStatusType == "GuestAccountOther");


                if (!String.IsNullOrWhiteSpace(model.SortBy))
                {
                    switch (model.SortBy)
                    {
                        case "Email":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.EmployeeEmail);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.EmployeeEmail);
                            }
                            break;
                        case "Name":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.EmployeeName);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.EmployeeName);
                            }
                            break;
                        case "RegisteredDate":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.CreatedAt);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.CreatedAt);
                            }
                            break;
                        case "Dealer":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.Outlet.Dealer.DealerName);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.Outlet.Dealer.DealerName);
                            }
                            break;
                        case "Outlet":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.Outlet.Name);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.Outlet.Name);
                            }
                            break;
                        default:
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.CreatedAt);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.CreatedAt);
                            }
                            break;
                    }
                }

                if (!String.IsNullOrWhiteSpace(model.SearchQuery))
                {
                    query = query.Where(Q => Q.EmployeeName.Contains(model.SearchQuery) || Q.EmployeeEmail.Contains(model.SearchQuery) || Q.Outlet.Dealer.DealerName.Contains(model.SearchQuery) || Q.Outlet.Name.Contains(model.SearchQuery));
                }
                count = query.Count();

                var data = await query.Select(Q => new VMGuestList
                {
                    EmployeeID = Q.EmployeeId,
                    Email = Q.EmployeeEmail,
                    DealerName = Q.Outlet.Dealer.DealerName,
                    OutletName = Q.Outlet.Name,
                    Name = Q.EmployeeName,
                    RegisteredDate = Q.CreatedAt,
                    Status = Q.Status,
                    ManPowerStatus = Q.ManpowerStatusType,
                    IsSuspend = Q.IsSuspended,
                    IsRequestUpgrade =Q.IsRequestUpgrade,
                    IsDataValidation = Q.IsDataValidation,
                    NIP = !String.IsNullOrWhiteSpace(Q.NIP) ? Q.NIP : Q.EmployeeId, 
                }).Skip(model.Page).Take(model.Limit).ToListAsync();

                return StatusCode(200, BaseResponse.ResponseOk(data, count));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult> ExportExcel(GetAccountParameter model, string filetype)
        {
            var count = 0;
            try
            {
                if (model.Limit == 0)
                {
                    model.Limit = 100;
                }

                if (model.Page == 0)
                {
                    model.Page = 1;
                }

                model.Page = (model.Page - 1) * model.Limit;

                var query = DB.Employees.Include(Q => Q.Outlet).Include(Q => Q.EmployeePositionMappings).ThenInclude(Q => Q.Position).AsQueryable();

                if (model.Status != null)
                {
                    query = query.Where(Q => model.Status.Contains(Q.Status)).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(model.GUID))
                {
                    query = query.Where(Q => Q.EmployeeId == model.GUID).AsQueryable();
                }

                if (model.RegisterdDateStart != null && model.RegisterdDateEnd != null)
                {
                    query = query.Where(Q => Q.CreatedAt.Date >= model.RegisterdDateStart.Value.Date && Q.CreatedAt.Date <= model.RegisterdDateEnd).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(model.DealerID))
                {
                    query = query.Where(Q => Q.Outlet.DealerId == model.DealerID).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(model.OutletID))
                {
                    query = query.Where(Q => Q.OutletId == model.OutletID).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(model.PositionID))
                {
                    var positionID = int.Parse(model.PositionID);
                    query = query.Where(Q => Q.EmployeePositionMappings.First().PositionId == positionID).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(model.Manpower))
                {
                    query = query.Where(Q => Q.ManpowerStatusType == model.Manpower).AsQueryable();

                }


                if (!String.IsNullOrWhiteSpace(model.SortBy))
                {
                    switch (model.SortBy)
                    {
                        case "Email":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.EmployeeEmail);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.EmployeeEmail);
                            }
                            break;
                        case "Name":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.EmployeeName);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.EmployeeName);
                            }
                            break;
                        case "RegisteredDate":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.CreatedAt);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.CreatedAt);
                            }
                            break;
                        case "Dealer":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.Outlet.Dealer.DealerName);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.Outlet.Dealer.DealerName);
                            }
                            break;
                        case "Outlet":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.Outlet.Name);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.Outlet.Name);
                            }
                            break;
                        default:
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.CreatedAt);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.CreatedAt);
                            }
                            break;
                    }
                }

                query = query.Where(Q => Q.ManpowerStatusType == "GuestAccount" || Q.ManpowerStatusType == "GuestAccountDealer" || Q.ManpowerStatusType == "GuestAccountOther");

                if (!String.IsNullOrWhiteSpace(model.SearchQuery))
                {
                    query = query.Where(Q => Q.EmployeeName.Contains(model.SearchQuery) || Q.EmployeeEmail.Contains(model.SearchQuery) || Q.Outlet.Dealer.DealerName.Contains(model.SearchQuery) || Q.Outlet.Name.Contains(model.SearchQuery));
                }
                count = query.Count();

                var data = await query.Select(Q => new VMGuestExport
                {
                    EmployeeID = Q.EmployeeId,
                    DateOfBirth = Q.DateOfBirth,
                    IdCard = Q.Ktp,
                    Religiion = Q.Religion,
                    Gender = Q.Gender,
                    Phone = Q.EmployeePhone,
                    Email = Q.EmployeeEmail,
                    DealerName = Q.Outlet.Dealer.DealerName,
                    OutletName = Q.Outlet.Name,
                    Name = Q.EmployeeName,
                    PositionName = Q.EmployeePositionMappings.FirstOrDefault(Z => Z.EmployeeId == Q.EmployeeId).Position.PositionName,
                    PositionNotes = Q.PoistionNote,
                    RegisteredDate = Q.CreatedAt,
                    Status = Q.Status,
                    ManPowerStatus = Q.ManpowerStatusType,
                    ManPowerCode = Q.ManpowerCode,
                    NIP = !String.IsNullOrWhiteSpace(Q.NIP) ? Q.NIP : Q.EmployeeId,
                    Address = Q.Addresses
                }).Skip(model.Page).Take(model.Limit).ToListAsync();


                var MS = await CreateExcelReport(data);


                return File(MS, FormatDocumentEnum.ExcelFormat, "Report Account." + filetype);

            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetListAccount(GetAccountParameter model)
        {
            var count = 0;
            try
            {
                if (model.Limit == 0)
                {
                    model.Limit = 10;
                }

                if (model.Page == 0)
                {
                    model.Page = 1;
                }

                model.Page = (model.Page - 1) * model.Limit;

                var query = DB.Employees.Include(Q => Q.EmployeePositionMappings).Include(Q => Q.Outlet).ThenInclude(X => X.Dealer).AsQueryable();

                if (model.Status != null)
                {
                    query = query.Where(Q => model.Status.Contains(Q.Status)).AsQueryable();
                }

                if (model.RegisterdDateStart != null && model.RegisterdDateEnd != null)
                {
                    query = query.Where(Q => Q.CreatedAt.Date >= model.RegisterdDateStart.Value.Date && Q.CreatedAt.Date <= model.RegisterdDateEnd).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(model.DealerID))
                {
                    query = query.Where(Q => Q.Outlet.DealerId == model.DealerID).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(model.OutletID))
                {
                    query = query.Where(Q => Q.OutletId == model.OutletID).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(model.PositionID))
                {
                    var positionID = int.Parse(model.PositionID);
                    query = query.Where(Q => Q.EmployeePositionMappings.First().PositionId == positionID).AsQueryable();
                }

                //query = query.Where(Q => Q.ManpowerStatusType == "Permanent").AsQueryable();

                if (!String.IsNullOrWhiteSpace(model.SortBy))
                {
                    switch (model.SortBy)
                    {
                        case "Email":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.EmployeeEmail);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.EmployeeEmail);
                            }
                            break;
                        case "Name":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.EmployeeName);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.EmployeeName);
                            }
                            break;
                        case "RegisteredDate":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.CreatedAt);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.CreatedAt);
                            }
                            break;
                        case "Dealer":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.Outlet.Dealer.DealerName);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.Outlet.Dealer.DealerName);
                            }
                            break;
                        case "Outlet":
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.Outlet.Name);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.Outlet.Name);
                            }
                            break;
                        default:
                            if (model.Ascending)
                            {
                                query = query.OrderBy(Q => Q.CreatedAt);
                            }
                            else
                            {
                                query = query.OrderByDescending(Q => Q.CreatedAt);
                            }
                            break;
                    }
                }


                count = query.Count();


                var data = await query.Select(Q => new VMGuestList
                {
                    EmployeeID = Q.EmployeeId,
                    Email = Q.EmployeeEmail,
                    DealerName = Q.Outlet.Dealer.DealerName,
                    OutletName = Q.Outlet.Name,
                    Name = Q.EmployeeName,
                    RegisteredDate = Q.CreatedAt,
                    Status = Q.Status,
                    ManPowerStatus = Q.ManpowerStatusType,
                    NIP = !String.IsNullOrWhiteSpace(Q.NIP) ? Q.NIP : Q.EmployeeId,
                }).Skip(model.Page).Take(model.Limit).ToListAsync();

                return StatusCode(200, BaseResponse.ResponseOk(data, count));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetSummaryTotalGuestAccount(List<string> dealerId)
        {
            try
            {
                var query = DB.Employees.Include(Q=> Q.Outlet).Where(Q => Q.ManpowerStatusType == "GuestAccount");
                if (dealerId.Count != 0) {
                    query = query.Where(Q => dealerId.Contains(Q.Outlet.DealerId));
                }
                var data = await query.GroupBy(Q => Q.Status).Select(Q => new SummaryTotalCustomer
                {
                    Status = Q.Key,
                    SumDone = Q.Count(),
                    SumRequestUpgrade = 0,
                }).ToListAsync();

                return StatusCode(200, BaseResponse.ResponseOk(data));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetSummaryGuestDealer(List<string> id)
        {
            try
            {
                var query = DB.Employees.Include(Q => Q.Outlet).ThenInclude(Q=> Q.Dealer).Where(Q=> !Q.Outlet.Dealer.OtherCompany).Where(Q => Q.ManpowerStatusType == "GuestAccount");
                if (id.Count != 0) {
                    query = query.Where( Q=>id.Contains(Q.Outlet.DealerId));
                }
                var data = await query.GroupBy(Q => Q.Status).Select(Q => new SummaryTotalCustomer
                {
                    Status = Q.Key
                }).ToListAsync();

                foreach (var item in data)
                {
                    item.SumDone = query.Where(Q => Q.Status == item.Status).Count();
                    item.SumIsSuspended = query.Where(Q => Q.Status == item.Status && Q.IsSuspended).Count();
                    item.SumRequestUpgrade = item.Status != "Upgraded" ? query.Where(Q => Q.Status == item.Status && Q.IsSuspended == false && Q.IsRequestUpgrade == true).Count() : 0;
                }

                return StatusCode(200, BaseResponse.ResponseOk(data));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetSummaryGuestOutlet(List<string> id, List<string> dealerId)
        {
            try
            {
                var query = DB.Employees.Include(Q => Q.Outlet).ThenInclude(Q => Q.Dealer).Where(Q => Q.ManpowerStatusType == "GuestAccount");
                if (id.Count != 0)
                {
                    query = query.Where(Q => id.Contains(Q.OutletId));
                }

                if (dealerId.Count != 0) {
                    query = query.Where(Q => dealerId.Contains(Q.Outlet.DealerId));
                }

                var data = await query.GroupBy(Q => Q.Status).Select(Q => new SummaryTotalCustomer
                {
                    Status = Q.Key
                }).ToListAsync();

                foreach (var item in data)
                {
                    item.SumDone = query.Where(Q => Q.Status == item.Status).Count();
                    item.SumIsSuspended = query.Where(Q => Q.Status == item.Status && Q.IsSuspended).Count();
                    item.SumRequestUpgrade = item.Status != "Upgraded" ? query.Where(Q => Q.Status == item.Status && Q.IsSuspended == false && Q.IsRequestUpgrade == true).Count() : 0;
                }
                return StatusCode(200, BaseResponse.ResponseOk(data));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetSummaryGuestOther(List<string> id)
        {
            try
            {
                var query = DB.Employees.Include(Q => Q.Outlet).ThenInclude(Z=> Z.Dealer).Where(Z=> Z.Outlet.Dealer.OtherCompany).Where(Q => Q.ManpowerStatusType == "GuestAccount");
                if (id.Count != 0)
                {
                    query = query.Where(Q => id.Contains(Q.OutletId));
                }
                var data = await query.GroupBy(Q => Q.Status).Select(Q => new SummaryTotalCustomer
                {
                    Status = Q.Key
                }).ToListAsync();

                foreach (var item in data)
                {
                    item.SumDone = query.Where(Q => Q.Status == item.Status).Count();
                    item.SumIsSuspended = query.Where(Q => Q.Status == item.Status && Q.IsSuspended).Count();
                    item.SumRequestUpgrade = item.Status != "Upgraded" ? query.Where(Q => Q.Status == item.Status &&  Q.IsSuspended == false && Q.IsRequestUpgrade == true).Count():0;
                }
                return StatusCode(200, BaseResponse.ResponseOk(data));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> UpdateEmployeeGuesstAccount(EmployeeGuestModel model)
        {

            if (model == null)
            {
                var msg = new Message
                {
                    Id = "Data yang anda masukkan tidak valid",
                    En = "Invalid data"
                };
                return StatusCode(400, BaseResponse.BadRequest(null, msg));
            }
            try { 

            var positionMapping = new EmployeePositionMappings();
                var updateModel = await DB.Employees.Include(Z => Z.EmployeePositionMappings).FirstOrDefaultAsync(e => e.EmployeeId == model.EmployeeId);
                if (updateModel == null)
                {
                    var msg = new Message
                    {
                        Id = "Data tidak ditemukan",
                        En = "Data not found"
                    };
                    return StatusCode(400, BaseResponse.BadRequest(null, msg));
                }

                if (!String.IsNullOrEmpty(model.OutletId))
                {
                    updateModel.NIP = model.NIP;
                }

                if (model.DateOfBirth.HasValue) {
                    updateModel.DateOfBirth = model.DateOfBirth.Value;
                }

                if (!String.IsNullOrEmpty(model.OutletId))
                {
                    updateModel.OutletId = model.OutletId;
                }

                if (!String.IsNullOrEmpty(model.EmployeeName))
                {
                    updateModel.EmployeeName = model.EmployeeName;
                }

                if (!String.IsNullOrEmpty(model.EmployeeEmail))
                {
                    updateModel.EmployeeEmail = model.EmployeeEmail;
                }

                if (!String.IsNullOrEmpty(model.Status)) { 
                    updateModel.Status = model.Status;
                }

                if (model.EmployeeExperience != null)
                {
                    updateModel.EmployeeExperience = model.EmployeeExperience.Value;
                }

                if (!String.IsNullOrEmpty(model.EmployeePhone))
                {
                    updateModel.EmployeePhone = model.EmployeePhone;
                }
                if (!String.IsNullOrEmpty(model.ManpowerStatusType))
                {
                    updateModel.ManpowerStatusType = model.ManpowerStatusType;
                }
                if (model.LastSeenAt.HasValue)
                {
                    updateModel.LastSeenAt = model.LastSeenAt;
                }
                if (!String.IsNullOrEmpty(model.Gender))
                {
                    updateModel.Gender = model.Gender;
                }
                if (!String.IsNullOrEmpty(model.DealerPeopleCategoryCode))
                {
                    updateModel.DealerPeopleCategoryCode = model.DealerPeopleCategoryCode;
                }
                if (model.DateOfBirth.HasValue)
                {
                    updateModel.DateOfBirth = model.DateOfBirth;

                }
                if (model.AccountValid.HasValue)
                {
                    updateModel.AccountValid = model.AccountValid;
                }
                updateModel.UpdatedAt = DateTime.Now;
                if (!String.IsNullOrEmpty(model.Religion))
                {
                    updateModel.Religion = model.Religion;
                }
                if (!String.IsNullOrEmpty(model.OutletId))
                {
                    updateModel.OutletId = model.OutletId;
                }
                if (!String.IsNullOrEmpty(model.Ktp))
                {
                    updateModel.Ktp = model.Ktp;
                }
                if (!String.IsNullOrEmpty(model.Address))
                {
                    updateModel.Addresses = model.Address;
                }
                if (!String.IsNullOrEmpty(model.ManpowerCode))
                {
                    updateModel.ManpowerCode = model.ManpowerCode;
                }

                if (model.IsRequestUpgrade != null) {
                    updateModel.IsRequestUpgrade = model.IsRequestUpgrade.Value;
                }

                if (model.IsSuspended != null) {
                    updateModel.IsSuspended = model.IsSuspended.Value;
                }

                if (model.PositionId != null)
                {
                    if (model.PositionId != 0)
                    {
                        positionMapping = updateModel.EmployeePositionMappings.FirstOrDefault(Q => Q.EmployeeId == model.EmployeeId);
                        if (positionMapping != null)
                        {
                            DB.EmployeePositionMappings.Remove(positionMapping);

                            var newPositionMapping = new EmployeePositionMappings
                            {
                                PositionId = model.PositionId,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now,
                                EmployeeId = model.EmployeeId,
                            };
                            DB.EmployeePositionMappings.Add(newPositionMapping);
                        }
                    }
                }

                if (model.IsDataValidation != null)
                {
                    updateModel.IsDataValidation = model.IsDataValidation.Value;
                }
                        

                await DB.SaveChangesAsync();

                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> DeleteDataEmployeeGuesstAccount(string employeeId)
        {
            try
            {
                var deleteModel = await this.DB.Events.FindAsync(employeeId);
                if (deleteModel == null)
                {
                    var msg = new Message
                    {
                        Id = "Data tidak ditemukan",
                        En = "Data not found"
                    };
                    return StatusCode(400, BaseResponse.BadRequest(null, msg));
                }
                deleteModel.IsDeleted = true;

                await DB.SaveChangesAsync();

                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> UpdateEmployeeStatus(EmployeeUpdateStatusVM model)
        {
            try
            {
                var updateModel = DB.Employees.SingleOrDefault(e => e.EmployeeId == model.EmployeeId);
                if (updateModel == null)
                {
                    var msg = new Message
                    {
                        Id = "Data tidak ditemukan",
                        En = "Data not found"
                    };
                    return StatusCode(400, BaseResponse.BadRequest(null, msg));
                }

                if (!string.IsNullOrWhiteSpace(model.Status))
                {
                    updateModel.Status = model.Status;
                }

                if (model.Extend != null)
                {
                    if (model.Extend)
                    {
                        updateModel.AccountValid = DateTime.Now.AddMonths(1);
                        updateModel.IsSuspended = false;
                    }
                }

                if (model.IsSuspend != null)
                {
                    updateModel.IsSuspended = model.IsSuspend.Value;
                }

                if (model.IsRequestUpgrade != null)
                {
                    updateModel.IsRequestUpgrade = model.IsRequestUpgrade.Value;
                }

                if (model.IsDataValidation != null){
                    updateModel.IsDataValidation = model.IsDataValidation.Value;
                }


                await DB.SaveChangesAsync();

                return StatusCode(200, BaseResponse.ResponseOk(updateModel));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }



        public async Task<MemoryStream> CreateExcelReport(List<VMGuestExport> surveyReportList)
        {
            //creating new workbook
            var wb = new XLWorkbook();

            //creating new worksheet
            var ws = wb.AddWorksheet("Sheet1");

            //create header
            ws.Cell("A1").Value = "Register ID";
            ws.Cell("B1").Value = "Registered Date";
            ws.Cell("C1").Value = "Status";
            ws.Cell("D1").Value = "UserType";
            ws.Cell("E1").Value = "Username";
            ws.Cell("F1").Value = "Employee ID";
            ws.Cell("G1").Value = "Name";
            ws.Cell("H1").Value = "ID Card";
            ws.Cell("I1").Value = "Company Name";
            ws.Cell("J1").Value = "Outlet";
            ws.Cell("K1").Value = "Position Group";
            ws.Cell("L1").Value = "Position";
            ws.Cell("M1").Value = "Gender";
            ws.Cell("N1").Value = "Date of Birth";
            ws.Cell("O1").Value = "Email";
            ws.Cell("P1").Value = "Phone";
            ws.Cell("Q1").Value = "Religion";
            ws.Cell("R1").Value = "Address";

            //table data
            int i = 2;
            foreach (var item in surveyReportList)
            {
                ws.Cell("A" + i.ToString()).Value = item.EmployeeID;
                ws.Cell("B" + i.ToString()).Value = item.RegisteredDate;
                ws.Cell("C" + i.ToString()).Value = item.Status;
                ws.Cell("D" + i.ToString()).Value = item.ManPowerStatus;
                ws.Cell("E" + i.ToString()).Value = item.Email;
                ws.Cell("F" + i.ToString()).Value = item.NIP;
                ws.Cell("G" + i.ToString()).Value = item.Name;
                ws.Cell("H" + i.ToString()).Value = item.IdCard;
                ws.Cell("I" + i.ToString()).Value = item.DealerName;
                ws.Cell("J" + i.ToString()).Value = item.OutletName;
                ws.Cell("K" + i.ToString()).Value = "";
                ws.Cell("L" + i.ToString()).Value = item.PositionName;
                ws.Cell("M" + i.ToString()).Value = item.Gender;
                ws.Cell("N" + i.ToString()).Value = item.DateOfBirth;
                ws.Cell("O" + i.ToString()).Value = item.Email;
                ws.Cell("P" + i.ToString()).Value = item.Phone;
                ws.Cell("Q" + i.ToString()).Value = item.Religiion;
                ws.Cell("R" + i.ToString()).Value = item.Address;
                i++;
            }

            //adjust column width
            ws.Columns(1, 10).AdjustToContents();

            //define table header range
            var rangeHeader = ws.Range("A1:R1");
            //set table header border style
            rangeHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            rangeHeader.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
            //rangeHeader.Style.Fill.BackgroundColor = XLColor.Yellow;

            //define table data range
            var dataRange = ws.Range("A2" + ":R" + (i - 1).ToString());
            //set data table border style
            dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //set data table alignment
            dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            MemoryStream MS = new MemoryStream();
            wb.SaveAs(MS);
            MS.Position = 0;

            return MS;
        }
    }


}
