using Microsoft.AspNetCore.Http;
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
    public class InboxService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;

        public InboxService(TalentContext talentContext, ClaimHelper claimHelper)
        {
            this.DB = talentContext;
            this.ClaimMan = claimHelper;
        }

        public async Task<InboxViewModel> GetInbox(InboxFilterModel filter)
        {
            var employeeId = this.ClaimMan.GetLoginUserId();

            var query = (from i in this.DB.Inboxes
                         join e in this.DB.Employees on i.CreatedBy equals e.EmployeeId into le
                         from e in le.DefaultIfEmpty()

                         where i.EmployeeId == employeeId

                         select new InboxModel
                         {
                             ApprovalId = i.ApprovalId,
                             CreatedAt = i.CreatedAt,
                             EmployeeName = e.EmployeeName,
                             CreatedBy = e.EmployeeId,
                             EmployeeId = i.EmployeeId,
                             InboxId = i.InboxId,
                             IsDone = i.IsDone,
                             IsRead = i.IsRead,
                             LinkToPage = i.LinkToPage,
                             Message = i.Message,
                             Title = i.Title
                         });

            if (string.IsNullOrEmpty(filter.Search) == false)
            {
                query = query.Where(Q => Q.Title.ToLower().Contains(filter.Search.ToLower())).AsQueryable();
            }

            var totalData = await query.CountAsync();

            var result = await query.OrderByDescending(Q => Q.CreatedAt).Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();

            var inboxViewModel = new InboxViewModel
            {
                ListInbox = result,
                TotalData = totalData
            };

            return inboxViewModel;

        }


        public async Task<InboxViewModel> GetInbox(InboxFilterModel filter, string empId)
        {
            var employeeId = empId;

            var query = (from i in this.DB.Inboxes
                         join e in this.DB.Employees on i.CreatedBy equals e.EmployeeId into le
                         from e in le.DefaultIfEmpty()

                         where i.EmployeeId == employeeId

                         select new InboxModel
                         {
                             ApprovalId = i.ApprovalId,
                             CreatedAt = i.CreatedAt,
                             EmployeeName = e.EmployeeName,
                             CreatedBy = e.EmployeeId,
                             EmployeeId = i.EmployeeId,
                             InboxId = i.InboxId,
                             IsDone = i.IsDone,
                             IsRead = i.IsRead,
                             LinkToPage = i.LinkToPage,
                             Message = i.Message,
                             Title = i.Title
                         });

            if (string.IsNullOrEmpty(filter.Search) == false)
            {
                query = query.Where(Q => Q.Title.ToLower().Contains(filter.Search.ToLower())).AsQueryable();
            }

            var totalData = await query.CountAsync();

            var result = await query.OrderByDescending(Q => Q.CreatedAt).Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();

            var inboxViewModel = new InboxViewModel
            {
                ListInbox = result,
                TotalData = totalData
            };

            return inboxViewModel;

        }

        public async Task<int> GetUnreadInboxCount()
        {
            var employeeId = this.ClaimMan.GetLoginUserId();

            var inbox = this.DB.Inboxes.Where(Q => Q.EmployeeId == employeeId && Q.IsRead == false).AsNoTracking();

            var totalData = await inbox.CountAsync();

            return totalData;
        }

        public Inboxes InsertInbox(InboxFormModel model)
        {
            var userLogin = this.ClaimMan.GetLoginUserId();

            var linkToPage = (string)null;

            if (!string.IsNullOrEmpty(model.LinkToPage))
            {
                linkToPage = model.LinkToPage;
            }

            var newInbox = new Inboxes
            {
                ApprovalId = model.ApprovalId,
                EmployeeId = model.ApprovalTo,
                Title = model.ContentCategory + " Approval",
                Message = "Please Approve this Content",
                LinkToPage = linkToPage,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = userLogin,
                UpdatedBy = userLogin,
                IsRead = false,
                IsDone = false,
            };

            this.DB.Inboxes.Add(newInbox);
            // await this.DB.SaveChangesAsync();

            return newInbox;
        }

        public async Task UpdateLinkToPage(int approvalId, string pageEnums)
        {
            var updateInbox = new UpdateInboxModel();

            switch (pageEnums)
            {
                case PageEnums.Banner:
                    updateInbox = await (from i in this.DB.Inboxes
                                         join atb in this.DB.ApprovalToBanners on i.ApprovalId equals atb.ApprovalId
                                         where atb.ApprovalId == approvalId && i.LinkToPage == null
                                         select new UpdateInboxModel
                                         {
                                             ContentId = atb.BannerId,
                                             Inbox = i
                                         }).FirstOrDefaultAsync();
                    updateInbox.Inbox.LinkToPage = "MasterData/Banner?BannerId=" + updateInbox.ContentId + "&fromoutside=true";
                    break;
                case PageEnums.Course:
                    updateInbox = await (from i in this.DB.Inboxes
                                         join atc in this.DB.ApprovalToCourses on i.ApprovalId equals atc.ApprovalId
                                         where atc.ApprovalId == approvalId && i.LinkToPage == null
                                         select new UpdateInboxModel
                                         {
                                             ContentId = atc.CourseId,
                                             Inbox = i
                                         }).FirstOrDefaultAsync();
                    updateInbox.Inbox.LinkToPage = "MasterData/Course?CourseId=" + updateInbox.ContentId + "&fromoutside=true";
                    break;
                case PageEnums.Event:
                    updateInbox = await (from i in this.DB.Inboxes
                                         join ate in this.DB.ApprovalToEvents on i.ApprovalId equals ate.ApprovalId
                                         where ate.ApprovalId == approvalId && i.LinkToPage == null
                                         select new UpdateInboxModel
                                         {
                                             ContentId = ate.EventId,
                                             Inbox = i
                                         }).FirstOrDefaultAsync();
                    updateInbox.Inbox.LinkToPage = "MasterData/Event?EventId=" + updateInbox.ContentId + "&fromoutside=true";
                    break;
                case PageEnums.Guide:
                    updateInbox = await (from i in this.DB.Inboxes
                                         join atg in this.DB.ApprovalToGuides on i.ApprovalId equals atg.ApprovalId
                                         where atg.ApprovalId == approvalId && i.LinkToPage == null
                                         select new UpdateInboxModel
                                         {
                                             ContentId = atg.GuideId,
                                             Inbox = i
                                         }).FirstOrDefaultAsync();
                    updateInbox.Inbox.LinkToPage = "MasterData/Guide?GuideId=" + updateInbox.ContentId + "&fromOutside=true";
                    break;
                case PageEnums.Module:
                    updateInbox = await (from i in this.DB.Inboxes
                                         join atm in this.DB.ApprovalToModules on i.ApprovalId equals atm.ApprovalId
                                         where atm.ApprovalId == approvalId && i.LinkToPage == null
                                         select new UpdateInboxModel
                                         {
                                             ContentId = atm.ModuleId,
                                             Inbox = i
                                         }).FirstOrDefaultAsync();
                    updateInbox.Inbox.LinkToPage = "MasterData/Module?ModuleId=" + updateInbox.ContentId + "&fromoutside=true";
                    break;
                case PageEnums.News:
                    updateInbox = await (from i in this.DB.Inboxes
                                         join atn in this.DB.ApprovalToNews on i.ApprovalId equals atn.ApprovalId
                                         where atn.ApprovalId == approvalId && i.LinkToPage == null
                                         select new UpdateInboxModel
                                         {
                                             ContentId = atn.NewsId,
                                             Inbox = i
                                         }).FirstOrDefaultAsync();
                    updateInbox.Inbox.LinkToPage = "MasterData/News?type=fromOutside&newsId=" + updateInbox.ContentId;
                    break;
                case PageEnums.ReleaseTraining:
                    updateInbox = await (from i in this.DB.Inboxes
                                         join att in this.DB.ApprovalToTrainings on i.ApprovalId equals att.ApprovalId
                                         where att.ApprovalId == approvalId && i.LinkToPage == null
                                         select new UpdateInboxModel
                                         {
                                             ContentId = att.TrainingId,
                                             Inbox = i
                                         }).FirstOrDefaultAsync();
                    updateInbox.Inbox.LinkToPage = "Setup/ReleaseTraining?trainingid=" + updateInbox.ContentId + "&fromoutside=true";
                    break;
                case PageEnums.SetupCourse:
                    updateInbox = await (from i in this.DB.Inboxes
                                         join atsc in this.DB.ApprovalToSetupCourses on i.ApprovalId equals atsc.ApprovalId
                                         where atsc.ApprovalId == approvalId && i.LinkToPage == null
                                         select new UpdateInboxModel
                                         {
                                             ContentId = atsc.CourseId,
                                             Inbox = i
                                         }).FirstOrDefaultAsync();
                    updateInbox.Inbox.LinkToPage = "Setup/SetupCourse/View?SetupCourseId=" + updateInbox.ContentId + "&fromoutside=true";
                    break;
                case PageEnums.SetupModule:
                    updateInbox = await (from i in this.DB.Inboxes
                                         join atsm in this.DB.ApprovalToSetupModules on i.ApprovalId equals atsm.ApprovalId
                                         where atsm.ApprovalId == approvalId && i.LinkToPage == null
                                         select new UpdateInboxModel
                                         {
                                             ContentId = atsm.SetupModuleId,
                                             Inbox = i
                                         }).FirstOrDefaultAsync();
                    updateInbox.Inbox.LinkToPage = "Setup/setupmodule/View?setupModuleId=" + updateInbox.ContentId + "&fromoutside=true";
                    break;
                case PageEnums.SetupPopQuiz:
                    updateInbox = await (from i in this.DB.Inboxes
                                         join atpq in this.DB.ApprovalToPopQuizzes on i.ApprovalId equals atpq.ApprovalId
                                         where atpq.ApprovalId == approvalId && i.LinkToPage == null
                                         select new UpdateInboxModel
                                         {
                                             ContentId = atpq.PopQuizId,
                                             Inbox = i
                                         }).FirstOrDefaultAsync();
                    updateInbox.Inbox.LinkToPage = "Setup/setuppopupquiz/View?popQuizId=" + updateInbox.ContentId + "&fromoutside=true";
                    break;
                case PageEnums.Survey:
                    updateInbox = await (from i in this.DB.Inboxes
                                         join atts in this.DB.ApprovalToSurveys on i.ApprovalId equals atts.ApprovalId
                                         where atts.ApprovalId == approvalId && i.LinkToPage == null
                                         select new UpdateInboxModel
                                         {
                                             ContentId = atts.SurveyId,
                                             Inbox = i
                                         }).FirstOrDefaultAsync();
                    updateInbox.Inbox.LinkToPage = "MasterData/Survey/?type=fromOutside&surveyId=" + updateInbox.ContentId;
                    break;
                case PageEnums.Task:
                    break;
            }
        }

        public async Task<ApprovalInboxModel> GetApprovalDetailId(int approvalId, int inboxId)
        {
            var getInbox = await this.DB.Inboxes.Where(Q => Q.InboxId == inboxId).FirstOrDefaultAsync();

            var getApproval = await this.DB.Approvals.Where(Q => Q.ApprovalId == approvalId).Select(Q => new ApprovalInboxModel
            {
                ApprovalId = Q.ApprovalId,
                ContentCategory = Q.ContentCategory,
                ContentName = Q.ContentName,
                CreatedAt = Q.CreatedAt,
                CreatedBy = Q.CreatedBy,
                LinkToPage = getInbox.LinkToPage,
                InboxId = getInbox.InboxId,
                IsDone = getInbox.IsDone,
                Message = getInbox.Message,
                Title = getInbox.Title,
                ApprovalLevel = Q.ApprovalLevel
            }).FirstOrDefaultAsync();

            var positionName = await (from epm in this.DB.EmployeePositionMappings
                                      join p in this.DB.Positions on epm.PositionId equals p.PositionId
                                      where epm.EmployeeId == getInbox.EmployeeId
                                      select new
                                      {
                                          PositionName = p.PositionName
                                      }).FirstOrDefaultAsync();

            getApproval.UserPosition = positionName.PositionName;
            getApproval.UserName = await this.DB.Employees.Where(Q => Q.EmployeeId == getInbox.CreatedBy).Select(Q => Q.EmployeeName).FirstOrDefaultAsync();
            getApproval.CreatedBy = getApproval.UserName;

            getInbox.IsRead = true;

            await this.DB.SaveChangesAsync();

            return getApproval;
        }

        public async Task CreateRejectInbox(Approvals approval, string message)
        {
            var userLogin = this.ClaimMan.GetLoginUserId();
            var listInbox = await this.DB.Inboxes.Where(Q => Q.ApprovalId == approval.ApprovalId).ToListAsync();
            var firstInbox = listInbox.First();

            foreach (var inbox in listInbox)
            {
                inbox.IsDone = true;
                inbox.IsRead = true;
            }

            var newInbox = new Inboxes
            {
                ApprovalId = approval.ApprovalId,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = userLogin,
                UpdatedBy = userLogin,
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                EmployeeId = firstInbox.CreatedBy,
                IsDone = true,
                IsRead = false,
                LinkToPage = firstInbox.LinkToPage,
                Message = string.IsNullOrEmpty(message) == true ? "Your approval request has been rejected" : message,
                Title = "Approval Rejected"
            };

            this.DB.Inboxes.Add(newInbox);
        }

        public async Task CreateNextInboxApproval(Approvals approvals)
        {
            var prevInbox = await this.DB.Inboxes.Where(Q => Q.ApprovalId == approvals.ApprovalId).ToListAsync();
            var linkToPage = prevInbox.First().LinkToPage;

            foreach (var inbox in prevInbox)
            {
                inbox.IsDone = true;
                inbox.IsRead = true;
            }

            var newInbox = new InboxFormModel
            {
                ApprovalId = approvals.ApprovalId,
                ApprovalTo = approvals.ApprovalTo,
                ContentCategory = approvals.ContentCategory,
                LinkToPage = linkToPage
            };

            var createInbox = this.InsertInbox(newInbox);
        }

        public async Task MarkDoneApprovedInbox(int approvalId)
        {
            var prevInbox = await this.DB.Inboxes.Where(Q => Q.ApprovalId == approvalId && Q.IsDone == false).ToListAsync();

            foreach (var inbox in prevInbox)
            {
                inbox.IsDone = true;
                inbox.IsRead = true;
            }
        }
    }
}
