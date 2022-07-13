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
    public class ApprovalTrainingService : Controller
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;

        public ApprovalTrainingService(TalentContext talentContext, IContextualService contextualService)
        {
            this.DB = talentContext;
            this.ContextMan = contextualService;
        }
        public async Task<ApprovalTrainingViewModel> GetApprovalTraining(ApprovalTrainingFilter filter)
        {
            //Get Query
            var query = DB.GetApprovalTraining().AsQueryable();

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
            //By Enrolment
            if (filter.Enrolment != null)
            {
                query = query.Where(Q => Q.Enrolment == filter.Enrolment.Value);
            }
            //By Quota
            if (filter.Quota != null)
            {
                query = query.Where(Q => Q.Quota == filter.Quota.Value);
            }
            //By Date
            if (filter.StartDate != null && filter.EndDate != null)
            {
                query = query.Where(Q => (Q.TrainingStartDate >= filter.StartDate && Q.TrainingStartDate < filter.EndDate.Value.AddDays(1)) ||
                                         (Q.TrainingEndDate >= filter.StartDate && Q.TrainingEndDate < filter.EndDate.Value.AddDays(1)));
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
                case "enrolment":
                    query = query.OrderBy(Q => Q.Enrolment);
                    break;
                case "enrolmentDesc":
                    query = query.OrderByDescending(Q => Q.Enrolment);
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

            var listApprovalTrainings = await query
                .Skip((filter.PageNumber - 1) * 10).Take(10)
                .Select(Q => new ApprovalTrainingModel
                {
                    TrainingId = Q.TrainingId,
                    CourseName = Q.CourseName,
                    ProgramType = Q.ProgramType,
                    Batch = Q.Batch,
                    Enrolment = Q.Enrolment,
                    Quota = Q.Quota,
                    TrainingEndDate = Q.TrainingEndDate,
                    TrainingStartDate = Q.TrainingStartDate
                })
                .ToListAsync();

            var totalItem = await query.CountAsync();

            var trainings = new ApprovalTrainingViewModel
            {
                ListApprovalTraining = listApprovalTrainings,
                TotalItem = totalItem
            };
            return trainings;
        }

        public async Task<ApprovalTrainingDetailViewModel> GetApprovalTrainingById(int trainingId)
        {
            var course = await (
                from t in DB.Trainings
                join c in DB.Courses on t.CourseId equals c.CourseId
                join pt in DB.ProgramTypes on c.ProgramTypeId equals pt.ProgramTypeId
                where t.TrainingId == trainingId
                select new
                {
                    CourseName = c.CourseName,
                    ProgramTypeName = pt.ProgramTypeName,
                    Batch = t.Batch,
                    Quota = t.Quota,
                    startDate = t.StartDate,
                    endDate = t.EndDate,
                    c.LearningTypeId
                }).FirstOrDefaultAsync();

            if (course == null)
            {
                return new ApprovalTrainingDetailViewModel();
            }

            var employees = await (
                from el in DB.EnrollLearnings
                join e in DB.Employees on el.EmployeeId equals e.EmployeeId
                join em in DB.EmployeePositionMappings on e.EmployeeId equals em.EmployeeId
                join pos in DB.Positions on em.PositionId equals pos.PositionId
                join o in DB.Outlets on e.OutletId equals o.OutletId into o2
                from o in o2.DefaultIfEmpty()
                join d in DB.Dealers on o.DealerId equals d.DealerId into d2
                from d in d2.DefaultIfEmpty()
                join ti in DB.TrainingInvitations on new { TrainingId = el.TrainingId.Value, EmployeeId = el.EmployeeId.ToLower() } equals new { TrainingId = ti.TrainingId, EmployeeId = ti.EmployeeId.ToLower() } into lti
                from ti in lti.DefaultIfEmpty()
                where el.TrainingId == trainingId
                && el.IsEnrolled == true
                select new ApprovalTrainingEmployee 
                {
                    EnrollLearningId = el.EnrollLearningId,
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.EmployeeName,
                    PositionId = em.PositionId.ToString(),
                    PositionName = pos.PositionName, 
                    DealerName = d.DealerName,
                    OutletName = o.Name,
                    IsJoined = el.IsJoined,
                    IsDrafted = el.IsDrafted,
                    IsLocked = ti == null ? false : true,
                    TrainingInvitationApprovalStatus = ti.ApprovalStatusId
                }).ToListAsync();

            employees = ArrangingApprovalTrainigEmployees(employees, course.Quota);
            var data = new ApprovalTrainingDetailViewModel
            {
                TrainingId = trainingId,
                CourseName = course.CourseName,
                ProgramTypeName = course.ProgramTypeName,
                Batch = course.Batch,
                Quota = course.Quota,
                StartDate = course.startDate,
                EndDate = course.endDate,
                Employees = employees,
                LearningTypeId = course.LearningTypeId
            };
            return data;
        }

        //Function for Auto Joined Training
        public List<ApprovalTrainingEmployee> ArrangingApprovalTrainigEmployees(List<ApprovalTrainingEmployee> employees, int? quota)
        {
            if (quota == null)
            {
                return employees;
            }

            //List for Quota
            var quotaEmployees = new List<ApprovalTrainingEmployee>();

            //Employee that alraedy joined
            var joinedEmployees = employees.Where(Q => Q.IsJoined == true).ToList();
            employees = employees.Except(joinedEmployees).ToList();
            quotaEmployees = joinedEmployees;

            //Employee that save as drafted, not joined yet, has been invited, not rejecting
            var invitedEmployees = employees.Where(Q => Q.IsJoined == false && Q.IsDrafted == true && (Q.TrainingInvitationApprovalStatus != null && Q.TrainingInvitationApprovalStatus != ApprovalStatusesEnum.RejectedId)).ToList();
            employees = employees.Except(invitedEmployees).ToList();
            quotaEmployees.AddRange(invitedEmployees);

            //Employee that save as drafted, not joined yet, and has not been invited
            var draftedEmployees = employees.Where(Q => Q.IsJoined == false && Q.IsDrafted == true && Q.TrainingInvitationApprovalStatus == null).ToList();
            employees = employees.Except(draftedEmployees).ToList();
            quotaEmployees.AddRange(draftedEmployees);

            //Waiting employee
            // var waitingEmployees = employees.Where(Q => Q.IsJoined == false && Q.IsDrafted == false && Q.TrainingInvitationApprovalStatus == null).ToList();
            var waitingEmployees = employees;

            // var availableQuota = quota - quotaEmployees.Count;
            // var toBeJoinedCount = availableQuota > waitingEmployees.Count ? waitingEmployees.Count : availableQuota;

            // if (availableQuota > 0)
            // {
            //     if (waitingEmployees.Count > 0)
            //     {
            //         for (int i = 0; i < toBeJoinedCount; i++)
            //         {
            //             waitingEmployees[i].IsDrafted = true;
            //         }
            //     }
            // }

            quotaEmployees.AddRange(waitingEmployees);

            return quotaEmployees.Distinct().ToList();
        }

        public async Task SaveApprovalTraining(List<ApprovalTrainingEmployee> model)
        {
            var enrollLearningIds = model.Select(Q => Q.EnrollLearningId).ToList();

            var enrollLearnings = await DB.EnrollLearnings
                .Where(Q => enrollLearningIds.Contains(Q.EnrollLearningId))
                .ToListAsync();

            foreach (var item in enrollLearnings)
            {
                var enroll = model.Where(Q => Q.EnrollLearningId == item.EnrollLearningId).FirstOrDefault();
                item.IsDrafted = enroll.IsDrafted;
            }

            DB.EnrollLearnings.UpdateRange(enrollLearnings);
            await DB.SaveChangesAsync();
        }

        public async Task SendInvitation(TrainingInvitationModel model)
        {
            using (var transaction = await DB.Database.BeginTransactionAsync())
            {
                var alreadyInviteEmployeeIds = await DB.TrainingInvitations.Where(Q => Q.TrainingId == model.TrainingId).Select(Q => Q.EmployeeId).ToListAsync();

                // var toBeDeletedEmployeeIds = new List<string>();

                var newInviteEmployeeIds = new List<string>();
                //if (model.EmployeeIds == null)
                //{
                //    // toBeDeletedEmployeeIds = alreadyInviteEmployeeIds;
                //}
                //else
                //{
                //    // toBeDeletedEmployeeIds = alreadyInviteEmployeeIds.Where(Q => model.EmployeeIds.Contains(Q) == false).ToList();
                //    newInviteEmployeeIds = model.EmployeeIds.Where(Q => alreadyInviteEmployeeIds.Contains(Q) == false).ToList();
                //}

                if (model.EmployeeIds != null)
                {
                    newInviteEmployeeIds = model.EmployeeIds.Where(Q => alreadyInviteEmployeeIds.Contains(Q) == false).ToList();
                }

                // var toBeDeletedTrainingInvitations = await DB.TrainingInvitations
                //     .Where(Q => Q.TrainingId == model.TrainingId && toBeDeletedEmployeeIds.Contains(Q.EmployeeId))
                //     .ToListAsync();

                // var toBeDeletedInvitationTrainingId = toBeDeletedTrainingInvitations.Select(Q => Q.TrainingInvitationId).ToList();

                // var toBeDeletedTrainingProcess = await DB.TrainingProcesses
                //     .Where(Q => toBeDeletedInvitationTrainingId.Contains(Q.TrainingInvitationId))
                //     .ToListAsync();

                var currentTime = DateTime.UtcNow.ToIndonesiaTimeZone();

                var invitations = newInviteEmployeeIds.Select(Q => new TrainingInvitations
                {
                    TrainingId = model.TrainingId,
                    EmployeeId = Q,
                    ApprovalStatusId = 2,
                    CreatedAt = currentTime,
                    CreatedBy = GetUserLogin(),
                    UpdatedAt = currentTime,
                    UpdatedBy = GetUserLogin()
                }).ToList();

                var courseName = await this.DB.Trainings.Where(Q => Q.TrainingId == model.TrainingId).Select(Q => Q.Course.CourseName).FirstOrDefaultAsync();

                var newInboxes = newInviteEmployeeIds.Select(Q => new MobileInboxes
                {
                    EmployeeId = Q,
                    MobileInboxTypeId = (int)MobileInboxType.TrainingInvitation,
                    Title = "Training Invitation",
                    Message = "Anda telah diundang untuk mengikuti Training "+ courseName + ". Silahkan cek detail di halaman Training Process. Terima kasih.",
                    CreatedBy = "SYSTEM",
                    CreatedAt = currentTime
                }).ToList();

                // DB.TrainingProcesses.RemoveRange(toBeDeletedTrainingProcess);
                // await DB.SaveChangesAsync();

                // DB.TrainingInvitations.RemoveRange(toBeDeletedTrainingInvitations);
                // await DB.SaveChangesAsync();

                DB.TrainingInvitations.AddRange(invitations);
                await DB.SaveChangesAsync();

                DB.MobileInboxes.AddRange(newInboxes);
                await this.DB.SaveChangesAsync();
                transaction.Commit();
            }
        }

         public async Task SendInvitationNew(TrainingInvitationModel model)
        {
            using (var transaction = await DB.Database.BeginTransactionAsync())
            {
                var alreadyInviteEmployeeIds = await DB.TrainingInvitations.Where(Q => Q.TrainingId == model.TrainingId).Select(Q => Q.EmployeeId).ToListAsync();


                var newInviteEmployeeIds = new List<string>();
               

                if (model.EmployeeIds != null)
                {
                    newInviteEmployeeIds = model.EmployeeIds.Where(Q => alreadyInviteEmployeeIds.Contains(Q) == false).ToList();
                }


                var currentTime = DateTime.UtcNow.ToIndonesiaTimeZone();

                var invitations = newInviteEmployeeIds.Select(Q => new TrainingInvitations
                {
                    TrainingId = model.TrainingId,
                    EmployeeId = Q,
                    ApprovalStatusId = 1,
                    CreatedAt = currentTime,
                    CreatedBy = GetUserLogin(),
                    UpdatedAt = currentTime,
                    UpdatedBy = GetUserLogin()
                }).ToList();

                var courseName = await this.DB.Trainings.Where(Q => Q.TrainingId == model.TrainingId).Select(Q => Q.Course.CourseName).FirstOrDefaultAsync();

                var newInboxes = newInviteEmployeeIds.Select(Q => new MobileInboxes
                {
                    EmployeeId = Q,
                    MobileInboxTypeId = (int)MobileInboxType.TrainingInvitation,
                    Title = "Training Invitation",
                    Message = "Anda telah diundang untuk mengikuti Training "+ courseName + ". Silahkan cek detail di halaman Training Process. Terima kasih.",
                    CreatedBy = "SYSTEM",
                    CreatedAt = currentTime
                }).ToList();

                DB.TrainingInvitations.AddRange(invitations);
                await DB.SaveChangesAsync();

                DB.MobileInboxes.AddRange(newInboxes);
                await this.DB.SaveChangesAsync();

                foreach (var invitation in invitations)
                {
                    var trainingProcessData = new TrainingProcesses
                    {
                        TrainingInvitationId = invitation.TrainingInvitationId,
                        CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                        UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                        CreatedBy = "SYSTEM",
                        UpdatedBy = "SYSTEM",
                        IsConfirmed = true,
                        AccomodationId = null, // fill this,
                        DateofStayEnd = null,  // fill this
                        DateofStayStart = null,// fill this
                        
                        
                    };

                    var enrollLearningData = DB.EnrollLearnings.FirstOrDefault(x=> x.TrainingId == invitation.TrainingId && x.EmployeeId == invitation.EmployeeId );
                    if (enrollLearningData != null){
                        enrollLearningData.IsJoined = true;
                        enrollLearningData.IsDrafted = true;
                        
                    }
                    
                }
                await this.DB.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task<ActionResult> ExportExcel(List<VMExportlEmployeeTrainingApprovalInput> model)
        {
            var listOfId = model.Select(Q => Q.EmployeeId).ToList();
            var dataExport = await DB.Employees.Include(Q => Q.EmployeePositionMappings).ThenInclude(Q => Q.Position).Include(Q => Q.Outlet).ThenInclude(Q => Q.Dealer).Include(Q=> Q.EnrollLearnings)
                            .Where(Q => listOfId.Contains(Q.EmployeeId)).Select(Z => new VMExportlEmployeeTrainingApproval
                            {
                                Email = Z.EmployeeEmail,
                                Username = Z.EmployeeUsername,
                                Address = Z.Addresses,
                                Religion = Z.Religion,
                                DateOfBirth = Z.DateOfBirth,
                                CompanyName = Z.Outlet.Dealer.DealerName,
                                OutletName = Z.Outlet.Name,
                                Gender = Z.Gender,
                                Name = Z.EmployeeName,
                                Ktp = Z.Ktp,
                                Phone = Z.EmployeePhone,
                                Position = Z.EmployeePositionMappings.FirstOrDefault().Position.PositionName,
                                OCEC = Z.OutletId + "" + Z.EmployeeId,
                                ManpowerCode = Z.ManpowerCode,
                                UserType = Z.Status,
                                AccountType = Z.ManpowerStatusType,
                                PositionGroup = "",
                                EnrollmentTime = Z.EnrollLearnings.FirstOrDefault().CreatedAt.ToString(),
                            }).ToListAsync();

            var MS = await CreateExcelReport(dataExport);
            return File(MS, FormatDocumentEnum.ExcelFormat, "Export Training Approval.xlsx");

        }

        public async Task<MemoryStream> CreateExcelReport(List<VMExportlEmployeeTrainingApproval> surveyReportList)
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
    }
}

