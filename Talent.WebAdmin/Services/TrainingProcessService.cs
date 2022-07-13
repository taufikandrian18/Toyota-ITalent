using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.UserSide.Enums;

namespace Talent.WebAdmin.Services
{
    public class TrainingProcessService :Controller
    {
        private readonly TalentContext DB;

        public TrainingProcessService(TalentContext talentContext)
        {
            this.DB = talentContext;
        }
        public async Task<TrainingProcessViewModel> GetTrainingProcess(TrainingProcessFilter filter)
        {
            //Get Query
            var query = DB.GetTrainingProcess().AsQueryable();

            //Filter
            //By Course Name
            if (!string.IsNullOrEmpty(filter.CourseName))
            {
                query = query.Where(Q => Q.CourseName.Contains(filter.CourseName));
            }
            //By Program Type
            if (!string.IsNullOrEmpty(filter.ProgramTypeName))
            {
                query = query.Where(Q => Q.ProgramType.Equals(filter.ProgramTypeName));
            }
            //By Batch
            if (filter.Batch != null)
            {
                query = query.Where(Q => Q.Batch == filter.Batch.Value);
            }
            //By confirmed
            if (filter.Confirmed != null)
            {
                query = query.Where(Q => Q.Confirmed == filter.Confirmed.Value);
            }
            //By Quota
            if (filter.Quota != null)
            {
                query = query.Where(Q => Q.Quota == filter.Quota.Value);
            }
            //By Date
            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.TrainingStartDate >= startDate && Q.TrainingStartDate <= endDate) || (Q.TrainingEndDate >= startDate && Q.TrainingEndDate <= endDate));
            }

            switch (filter.SortBy)
            {
                case "courseName":
                    query = query.OrderBy(Q => Q.CourseName);
                    break;
                case "courseNameDesc":
                    query = query.OrderByDescending(Q => Q.CourseName);
                    break;
                case "programType":
                    query = query.OrderBy(Q => Q.ProgramType);
                    break;
                case "programTypeDesc":
                    query = query.OrderByDescending(Q => Q.ProgramType);
                    break;
                case "batch":
                    query = query.OrderBy(Q => Q.Batch);
                    break;
                case "batchDesc":
                    query = query.OrderByDescending(Q => Q.Batch);
                    break;
                case "confirmed":
                    query = query.OrderBy(Q => Q.Confirmed);
                    break;
                case "confirmedDesc":
                    query = query.OrderByDescending(Q => Q.Confirmed);
                    break;
                case "quota":
                    query = query.OrderBy(Q => Q.Quota);
                    break;
                case "quotaDesc":
                    query = query.OrderByDescending(Q => Q.Quota);
                    break;
                case "startDate":
                    query = query.OrderBy(Q => Q.TrainingStartDate);
                    break;
                case "startDateDesc":
                    query = query.OrderByDescending(Q => Q.TrainingStartDate);
                    break;
                case "endDate":
                    query = query.OrderBy(Q => Q.TrainingEndDate);
                    break;
                case "endDateDesc":
                    query = query.OrderByDescending(Q => Q.TrainingEndDate);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.TrainingUpdatedAt);
                    break;
            }

            var listTrainingProcess = await query
                .Skip((filter.PageNumber - 1) * 10).Take(10)
                .Select(Q => new TrainingProcessModel
                {
                    TrainingId = Q.TrainingId,
                    CourseName = Q.CourseName,
                    ProgramType = Q.ProgramType,
                    Batch = Q.Batch,
                    Confirmed = Q.Confirmed,
                    Quota = Q.Quota,
                    TrainingEndDate = Q.TrainingEndDate,
                    TrainingStartDate = Q.TrainingStartDate
                })
                .ToListAsync();

            var totalItem = await query.CountAsync();

            var trainings = new TrainingProcessViewModel
            {
                ListTrainingProcess = listTrainingProcess,
                TotalItem = totalItem
            };
            return trainings;
        }

        public XLWorkbook CreateExcelReport(TrainingProcessDetailViewModel model)
        {
            //creating new workbook
            var wb = new XLWorkbook();

            //creating new worksheet
            var ws = wb.AddWorksheet("Sheet1");


            //create header
            ws.Cell("A1").Value = "No";
            ws.Cell("B1").Value = "Employee Name";
            ws.Cell("C1").Value = "Gender";
            ws.Cell("D1").Value = "Dealer";
            ws.Cell("E1").Value = "Outlet";
            ws.Cell("F1").Value = "Accommodation";
            ws.Cell("G1").Value = "Price";
            ws.Cell("H1").Value = "Date of Stay";

            //table data
            int i = 2;
            foreach (var item in model.TrainingProcessDetailList)
            {
                ws.Cell("A" + i.ToString()).Value = i-1;
                ws.Cell("B" + i.ToString()).Value = item.EmployeeName;
                ws.Cell("C" + i.ToString()).Value = item.Gender;
                ws.Cell("D" + i.ToString()).Value = item.Dealer;
                ws.Cell("E" + i.ToString()).Value = item.Outlet;
                ws.Cell("F" + i.ToString()).Value = item.Accommodation;
                ws.Cell("G" + i.ToString()).Value = item.Price;
                if (item.DateofStayStart != null && item.DateofStayEnd != null)
                {
                    ws.Cell("H" + i.ToString()).Value = $"{item.DateofStayStart.Value:dd/MM/yyyy} - {item.DateofStayEnd.Value:dd/MM/yyyy}";
                }
                else
                {
                    ws.Cell("H" + i.ToString()).Value = "";
                }

                i++;
            }

            //adjust column width
            ws.Columns(1, 13).AdjustToContents();

            //define table header range
            var rangeHeader = ws.Range("A1:H1");
            //set table header border style
            rangeHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            rangeHeader.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
            //rangeHeader.Style.Fill.BackgroundColor = XLColor.Yellow;

            if (model.TrainingProcessDetailList.Count > 0)
            {
                //define table data range
                var dataRange = ws.Range("A2" + ":H" + (i - 1).ToString());
                //set data table border style
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                //set data table alignment
                dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }


            return wb;
        }

        public async Task<MemoryStream> DownloadExcel(int trainingId)
        {
            var model = await GetTrainingProcessById(trainingId);

            model.TrainingProcessDetailList = model.TrainingProcessDetailList.Where(Q => Q.IsConfirmed == true).ToList();

            var excelWb = CreateExcelReport(model);

            MemoryStream MS = new MemoryStream();

            excelWb.SaveAs(MS);
            MS.Position = 0;

            return MS;
        }

        public async Task<TrainingProcessDetailViewModel> GetTrainingProcessById(int trainingId)
        {
            var trainingProcessEmployee = await (from ti in this.DB.TrainingInvitations
                                                 join tp in this.DB.TrainingProcesses on ti.TrainingInvitationId equals tp.TrainingInvitationId
                                                 join e in this.DB.Employees on ti.EmployeeId equals e.EmployeeId
                                                 join em in this.DB.EmployeePositionMappings on e.EmployeeId equals em.EmployeeId
                                                 join pos in this.DB.Positions on em.PositionId equals pos.PositionId
                                                 join el in this.DB.EnrollLearnings on new { TrainingId = ti.TrainingId, EmployeeId = ti.EmployeeId.ToLower() } equals new { TrainingId = el.TrainingId.Value, EmployeeId = el.EmployeeId.ToLower() }
                                                 join o in this.DB.Outlets on e.OutletId equals o.OutletId into lo
                                                 from o in lo.DefaultIfEmpty()
                                                 join d in this.DB.Dealers on o.DealerId equals d.DealerId into ld
                                                 from d in ld.DefaultIfEmpty()
                                                 join a in this.DB.Accommodations on tp.AccomodationId equals a.AccommodationId into la
                                                 from a in la.DefaultIfEmpty()
                                                 where ti.TrainingId == trainingId && ti.ApprovalStatusId == ApprovalStatusesEnum.ApproveId
                                                 select new TrainingProcessDetailModel
                                                 {
                                                     EmployeeId = e.EmployeeId,
                                                     CreatedAt = tp.CreatedAt,
                                                     Accommodation = a.Name,
                                                     DateofStayEnd = tp.DateofStayEnd,
                                                     DateofStayStart = tp.DateofStayStart,
                                                     Dealer = d.DealerName,
                                                     EmployeeName = e.EmployeeName,
                                                     Gender = e.Gender,
                                                     IsJoined = el.IsJoined,
                                                     IsConfirmed = tp.IsConfirmed,
                                                     PositionId = em.PositionId.ToString(),
                                                     PositionName = pos.PositionName,
                                                     Outlet = o.Name,
                                                     Price = a.Price,
                                                     TrainingInvitationId = ti.TrainingInvitationId
                                                 }).ToListAsync();

            var trainingProcessDetail = new TrainingProcessDetailViewModel
            {
                TrainingProcessDetailList = trainingProcessEmployee,
            };

            return trainingProcessDetail;
        }

        //Save as draft Training Process
        public async Task<bool> SaveAsDraftTrainingProcess(TrainingProcessDetailViewModel model, int trainingId)
        {
            var trainingInvitationList = await this.DB.TrainingInvitations.Where(Q => Q.TrainingId == trainingId).ToListAsync();
            var trainingInvitiationListIds = trainingInvitationList.Select(Q => Q.TrainingInvitationId).ToList();
            var trainingProcessList = await this.DB.TrainingProcesses.Where(Q => trainingInvitiationListIds.Contains(Q.TrainingInvitationId)).ToListAsync();

            var currentListTrainingProcess = model.TrainingProcessDetailList;

            foreach (var trainingProcess in trainingProcessList)
            {
                var currentTrainingProcess = currentListTrainingProcess.Where(Q => Q.TrainingInvitationId == trainingProcess.TrainingInvitationId).FirstOrDefault();
                if (currentTrainingProcess == null)
                {
                    continue;
                }

                trainingProcess.IsConfirmed = currentTrainingProcess.IsConfirmed;
            }

            await this.DB.SaveChangesAsync();

            return true;

        }

        //Send invitation
        public async Task<bool> UpdateTrainingProcess(TrainingProcessDetailViewModel model, int trainingId)
        {
            var trainingInvitationList = await DB.TrainingInvitations.Where(Q => Q.TrainingId == trainingId).ToListAsync();
            var trainingInvitationListIds = trainingInvitationList.Select(Q => Q.TrainingInvitationId).ToList();
            var trainingProcessList = await DB.TrainingProcesses.Where(Q => trainingInvitationListIds.Contains(Q.TrainingInvitationId)).ToListAsync();
            var enrollLearningList = await DB.EnrollLearnings.Where(Q => Q.TrainingId == trainingId).ToListAsync();

            var mobileInboxes = new List<MobileInboxes>();

            var trainingInformation = await this.DB.Trainings.Where(Q => Q.TrainingId == trainingId).Select(Q => new
            {
                Q.Course.CourseName,
                StartDate = Q.StartDate == null ? "" : Q.StartDate.Value.ToString("dd/MM/yyyy"),
                EndDate = Q.EndDate == null ? "" : Q.EndDate.Value.ToString("dd/MM/yyyy")
            }).FirstOrDefaultAsync();

            var approvedMessage = "Selamat, Anda telah terkonfirmasi untuk mengikuti Training " + trainingInformation.CourseName + " pada tanggal " + trainingInformation.StartDate + " - " + trainingInformation.EndDate + ". Silahkan cek detail di halaman Training Process. Terima kasih.";
            var cancellationMessage = "Mohon maaf, Anda masih dalam antrian untuk bisa mengikuti Training " + trainingInformation.CourseName + " pada tanggal " + trainingInformation.StartDate + " - " + trainingInformation.EndDate + ". Silahkan cek detail di halaman Training Process. Terima kasih.";
            var currentTime = DateTime.UtcNow.ToIndonesiaTimeZone();

            foreach (var trainingProcess in model.TrainingProcessDetailList)
            {
                var trainingInvitation = trainingInvitationList.Where(Q => Q.TrainingInvitationId == trainingProcess.TrainingInvitationId).FirstOrDefault();
                var trainingProcessEntities = trainingProcessList.Where(Q => Q.TrainingInvitationId == trainingProcess.TrainingInvitationId).FirstOrDefault();

                var enrollLearning = enrollLearningList.Where(Q => Q.TrainingId == trainingInvitation.TrainingId && Q.EmployeeId.ToLower() == trainingInvitation.EmployeeId.ToLower()).FirstOrDefault();

                if (trainingProcess.IsConfirmed == true)
                {
                    trainingProcess.IsJoined = true;
                }
                else{
                    trainingProcess.IsJoined = false;
                }

                trainingProcessEntities.IsConfirmed = trainingProcess.IsConfirmed;

                //Checking if date updated
                if (trainingProcess.IsJoined != enrollLearning.IsJoined)
                {
                    //Send confirmation Inbox
                    if (trainingProcess.IsJoined == true)
                    {
                        var newConfirmedInbox = new MobileInboxes
                        {
                            EmployeeId = enrollLearning.EmployeeId,
                            MobileInboxTypeId = (int)MobileInboxType.TrainingInvitation,
                            Title = "Training Confirmed",
                            Message = approvedMessage,
                            CreatedBy = "SYSTEM",
                            CreatedAt = currentTime
                        };

                        mobileInboxes.Add(newConfirmedInbox);
                    }
                    //Send cancellation inbox
                    else
                    {
                        var cancellationInbox = new MobileInboxes
                        {
                            EmployeeId = enrollLearning.EmployeeId,
                            MobileInboxTypeId = (int)MobileInboxType.TrainingInvitation,
                            Title = "Training Notification",
                            Message = cancellationMessage,
                            CreatedBy = "SYSTEM",
                            CreatedAt = currentTime
                        };

                        mobileInboxes.Add(cancellationInbox);
                    }

                    enrollLearning.IsJoined = trainingProcess.IsJoined;
                }
            }

            this.DB.MobileInboxes.AddRange(mobileInboxes);

            await DB.SaveChangesAsync();

            return true;
        }

        public async Task<ActionResult> ExportExcel(List<VMExportTrainingProcessInput> model)
        {
            var listOfId = model.Select(Q => Q.EmployeeId).ToList();
            var dataExport = await DB.Employees.Include(Q => Q.EmployeePositionMappings).ThenInclude(Q => Q.Position).Include(Q => Q.Outlet).ThenInclude(Q => Q.Dealer).Include(Q=> Q.EnrollLearnings)
                            .Where(Q => listOfId.Contains(Q.EmployeeId)).Select(Z => new VMExportTrainingProcess
                            {
                                Email = Z.EmployeeEmail,
                                Username = Z.EmployeeUsername,
                                Address = Z.Addresses,
                                Religion = Z.Religion,
                                DateOfBirth = Z.DateOfBirth,
                                CompanyName = Z.Outlet.Dealer.DealerName,
                                OutletName = Z.Outlet.Name,
                                Gender = Z.Gender,
                                Name=Z.EmployeeName,
                                Ktp = Z.Ktp,
                                Phone = Z.EmployeePhone,
                                Position = Z.EmployeePositionMappings.FirstOrDefault().Position.PositionName,
                                OCEC = Z.OutletId + "" + Z.EmployeeId,
                                ManpowerCode = Z.ManpowerCode,
                                UserType = Z.Status,
                                AccountType = Z.ManpowerStatusType,
                                PositionGroup = "",
                                EnrollmentTime =Z.EnrollLearnings.FirstOrDefault().CreatedAt.ToString(),
                            }).ToListAsync();

            var MS = await CreateExcelReport(dataExport);
            return File(MS, FormatDocumentEnum.ExcelFormat, "Export Training Process.xlsx");
        }

        public async Task<MemoryStream> CreateExcelReport(List<VMExportTrainingProcess> surveyReportList)
        {
            //creating new workbook
            var wb = new XLWorkbook();

            //creating new worksheet
            var ws = wb.AddWorksheet("Sheet1");

            //create header
            ws.Cell("A1").Value = "Timestamp";
            ws.Cell("B1").Value = "Account Type";
            ws.Cell("C1").Value = "User Type";
            ws.Cell("D1").Value = "Username";
            ws.Cell("E1").Value = "OCEC";
            ws.Cell("F1").Value = "Manpower Code";
            ws.Cell("G1").Value = "Name";
            ws.Cell("H1").Value = "KTP";
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
                ws.Cell("A" + i.ToString()).Value = item.EnrollmentTime;
                ws.Cell("B" + i.ToString()).Value = item.AccountType;
                ws.Cell("C" + i.ToString()).Value = item.UserType;
                ws.Cell("D" + i.ToString()).Value = item.Username;
                ws.Cell("E" + i.ToString()).Value = item.OCEC;
                ws.Cell("F" + i.ToString()).Value = item.ManpowerCode;
                ws.Cell("G" + i.ToString()).Value = item.Name;
                ws.Cell("H" + i.ToString()).Value = item.Ktp;
                ws.Cell("I" + i.ToString()).Value = item.CompanyName;
                ws.Cell("J" + i.ToString()).Value = item.OutletName;
                ws.Cell("K" + i.ToString()).Value = item.PositionGroup;
                ws.Cell("L" + i.ToString()).Value = item.Position;
                ws.Cell("M" + i.ToString()).Value = item.Gender;
                ws.Cell("N" + i.ToString()).Value = item.DateOfBirth;
                ws.Cell("O" + i.ToString()).Value = item.Email;
                ws.Cell("P" + i.ToString()).Value = item.Phone;
                ws.Cell("Q" + i.ToString()).Value = item.Religion;
                ws.Cell("R" + i.ToString()).Value = item.Address;
                i++;
            }

            //adjust column width
            ws.Columns(1, 17).AdjustToContents();

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
