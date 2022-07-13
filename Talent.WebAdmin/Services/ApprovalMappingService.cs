using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class ApprovalMappingService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;

        public ApprovalMappingService(TalentContext talentContext, ClaimHelper claimHelper)
        {
            this.DB = talentContext;
            this.ClaimMan = claimHelper;
        }

        public async Task<ApprovalMappingViewModel> GetApprovalMappingFiltered(ApprovalMappingFilterModel filter)
        {
            var query = (from am in this.DB.ApprovalMappings
                         join al in this.DB.ApprovalLevels on am.ApprovalLevelId equals al.ApprovalLevelId
                         join pa in this.DB.Pages on am.PageId equals pa.PageId
                         select new ApprovalMappingModel
                         {
                             PageId = pa.PageId,
                             ApprovalMappingid = am.ApprovalMappingid,
                             CreatedAt = am.CreatedAt,
                             UpdatedAt = am.UpdatedAt,
                             ApprovalLevelId = al.ApprovalLevelId,
                             PageName = pa.Name,
                             PositionName = al.Name,
                             Description = al.Description
                         }).AsQueryable();

            if (string.IsNullOrEmpty(filter.PositionName) == false)
            {
                query = query.Where(Q => Q.PositionName.ToLower() == filter.PositionName.ToLower());
            }

            if (string.IsNullOrEmpty(filter.PageName) == false)
            {
                query = query.Where(Q => Q.PageName == filter.PageName);
            }

            if (filter.Level != null)
            {
                query = query.Where(Q => Q.ApprovalLevelId == filter.Level);
            }

            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt <= endDate) || (Q.UpdatedAt >= startDate && Q.UpdatedAt <= endDate));
            }

            switch (filter.SortBy)
            {
                case "menuPage":
                    query = query.OrderBy(Q => Q.PageName);
                    break;
                case "menuPageDesc":
                    query = query.OrderByDescending(Q => Q.PageName);
                    break;

                case "position":
                    query = query.OrderBy(Q => Q.PositionName);
                    break;
                case "positionDesc":
                    query = query.OrderByDescending(Q => Q.PositionName);
                    break;

                case "type":
                    query = query.OrderBy(Q => Q.ApprovalLevelId);
                    break;
                case "typeDesc":
                    query = query.OrderByDescending(Q => Q.ApprovalLevelId);
                    break;
                case "createdDate":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "createdDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;

                case "updatedDate":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "updatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var count = await query.CountAsync();

            var resultQuery = await query.Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();

            var approvalMapping = new ApprovalMappingViewModel
            {
                ListApprovalMapping = resultQuery,
                TotalData = count
            };

            return approvalMapping;

        }

        public async Task<bool> InsertApprovalMapping(ApprovalMappingFormModel model)
        {
            var findApprovalMapping = await this.DB.ApprovalMappings.Where(Q => Q.PageId == model.PageId).FirstOrDefaultAsync();
            var userLogin = this.ClaimMan.GetLoginUserId();

            if (findApprovalMapping != null)
            {
                return false;
            }

            var newApprovalMapping = new ApprovalMappings
            {
                PageId = model.PageId,
                ApprovalLevelId = model.ApprovalLevelId,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = userLogin,
                UpdatedBy = userLogin
            };


            this.DB.ApprovalMappings.Add(newApprovalMapping);
            await this.DB.SaveChangesAsync();

            await this.InsertApprovals(model.PageId, newApprovalMapping.ApprovalMappingid);
            await this.DB.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteApprovalMapping(int id)
        {

            try
            {
                var userLogin = this.ClaimMan.GetLoginUserId();

                var findApprovalMappings = await this.DB.ApprovalMappings.Where(Q => Q.ApprovalMappingid == id).FirstOrDefaultAsync();
                if (findApprovalMappings == null)
                {
                    return false;
                }
                this.DB.ApprovalMappings.Remove(findApprovalMappings);

                var findApprovals = await this.DB.Approvals.Where(Q => Q.ApprovalMappingId == findApprovalMappings.ApprovalMappingid).ToListAsync();
                var updateApprovalIds = new List<int>();

                foreach (var approval in findApprovals)
                {
                    approval.ApprovalMappingId = null;
                    approval.ActionAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    approval.ActionBy = userLogin;
                    if (approval.ApprovalStatusId == ApprovalStatusesEnum.WaitingId)
                    {
                        approval.ApprovalStatusId = ApprovalStatusesEnum.ApproveId;
                        updateApprovalIds.Add(approval.ApprovalId);
                    }
                }

                await UpdatingApprovalsContent(updateApprovalIds, findApprovalMappings.PageId);

                await this.DB.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                var message = e.Message;
                return false;
            }

        }

        public async Task<bool> UpdateApprovalMapping(ApprovalMappingFormModel model)
        {
            var userLogin = this.ClaimMan.GetLoginUserId();

            var findApprovalMapping = await this.DB.ApprovalMappings.Where(Q => Q.ApprovalMappingid == model.ApprovalMappingid).FirstOrDefaultAsync();

            if (findApprovalMapping == null)
            {
                return false;
            }

            var otherApproval = await this.DB.ApprovalMappings.Where(Q => Q.PageId == model.PageId).FirstOrDefaultAsync();

            //Validation if update model already exist
            if (otherApproval != null)
            {
                if (otherApproval != findApprovalMapping)
                {
                    return false;
                }
            }

            //Take waiting for approval that should be approved if approval mapping changed
            var approvals = await this.DB.Approvals.Where(Q => Q.ApprovalMappingId == findApprovalMapping.ApprovalMappingid && Q.ApprovalStatusId == ApprovalStatusesEnum.WaitingId && Q.ApprovalLevel > findApprovalMapping.ApprovalLevelId).ToListAsync();

            foreach (var approval in approvals)
            {
                approval.ApprovalStatusId = ApprovalStatusesEnum.ApproveId;
                approval.ActionAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                approval.ActionBy = userLogin;
                approval.ApprovalLevel = model.ApprovalLevelId;
            }

            var approvalIds = approvals.Select(Q => Q.ApprovalId).ToList();

            //Update
            findApprovalMapping.ApprovalLevelId = model.ApprovalLevelId;
            findApprovalMapping.PageId = model.PageId;
            findApprovalMapping.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            findApprovalMapping.UpdatedBy = this.ClaimMan.GetLoginUserId();

            await this.UpdatingApprovalsContent(approvalIds, findApprovalMapping.PageId);

            await this.DB.SaveChangesAsync();
            return true;
        }

        public async Task InsertApprovals(string PageId, int approvalMappingId)
        {

            if (PageId == PageEnums.Banner)
            {
                var approvalList = await this.DB.Approvals.Where(Q => Q.ContentCategory == ContentCategoryEnums.Banner).ToListAsync();

                foreach (var approv in approvalList)
                {
                    approv.ApprovalMappingId = approvalMappingId;
                }

            }

            if (PageId == PageEnums.Course)
            {
                var approvalList = await this.DB.Approvals.Where(Q => Q.ContentCategory == ContentCategoryEnums.Course).ToListAsync();

                foreach (var approv in approvalList)
                {
                    approv.ApprovalMappingId = approvalMappingId;
                }
            }

            if (PageId == PageEnums.Event)
            {
                var approvalList = await this.DB.Approvals.Where(Q => Q.ContentCategory == ContentCategoryEnums.Event).ToListAsync();

                foreach (var approv in approvalList)
                {
                    approv.ApprovalMappingId = approvalMappingId;
                }
            }

            if (PageId == PageEnums.Guide)
            {
                var approvalList = await this.DB.Approvals.Where(Q => Q.ContentCategory == ContentCategoryEnums.Guide).ToListAsync();

                foreach (var approv in approvalList)
                {
                    approv.ApprovalMappingId = approvalMappingId;
                }
            }

            if (PageId == PageEnums.Module)
            {
                var approvalList = await this.DB.Approvals.Where(Q => Q.ContentCategory == ContentCategoryEnums.Module).ToListAsync();

                foreach (var approv in approvalList)
                {
                    approv.ApprovalMappingId = approvalMappingId;
                }
            }

            if (PageId == PageEnums.News)
            {
                var approvalList = await this.DB.Approvals.Where(Q => Q.ContentCategory == ContentCategoryEnums.News).ToListAsync();

                foreach (var approv in approvalList)
                {
                    approv.ApprovalMappingId = approvalMappingId;
                }
            }

            if (PageId == PageEnums.ReleaseTraining)
            {
                var approvalList = await this.DB.Approvals.Where(Q => Q.ContentCategory == ContentCategoryEnums.ReleaseTraining).ToListAsync();

                foreach (var approv in approvalList)
                {
                    approv.ApprovalMappingId = approvalMappingId;
                }

            }

            if (PageId == PageEnums.SetupCourse)
            {
                var approvalList = await this.DB.Approvals.Where(Q => Q.ContentCategory == ContentCategoryEnums.SetupCourse).ToListAsync();

                foreach (var approv in approvalList)
                {
                    approv.ApprovalMappingId = approvalMappingId;
                }
            }

            if (PageId == PageEnums.SetupModule)
            {
                var approvalList = await this.DB.Approvals.Where(Q => Q.ContentCategory == ContentCategoryEnums.SetupModule).ToListAsync();

                foreach (var approv in approvalList)
                {
                    approv.ApprovalMappingId = approvalMappingId;
                }
            }

            if (PageId == PageEnums.SetupPopQuiz)
            {
                var approvalList = await this.DB.Approvals.Where(Q => Q.ContentCategory == ContentCategoryEnums.SetupPopQuiz).ToListAsync();

                foreach (var approv in approvalList)
                {
                    approv.ApprovalMappingId = approvalMappingId;
                }
            }

            if (PageId == PageEnums.Survey)
            {
                var approvalList = await this.DB.Approvals.Where(Q => Q.ContentCategory == ContentCategoryEnums.Survey).ToListAsync();

                foreach (var approv in approvalList)
                {
                    approv.ApprovalMappingId = approvalMappingId;
                }
            }

            if (PageId == PageEnums.Task)
            {
                var approvalList = await this.DB.Approvals.Where(Q => Q.ContentCategory == ContentCategoryEnums.Task).ToListAsync();

                foreach (var approv in approvalList)
                {
                    approv.ApprovalMappingId = approvalMappingId;
                }
            }
        }

        public async Task UpdatingApprovalsContent(List<int> approvalIds, string PageId)
        {
            if (PageId == PageEnums.Banner)
            {
                var bannerIds = await this.DB.ApprovalToBanners.Where(Q => approvalIds.Contains(Q.ApprovalId)).Select(Q => Q.BannerId).ToListAsync();
                var banners = await this.DB.Banners.Where(Q => bannerIds.Contains(Q.BannerId)).ToListAsync();

                foreach (var banner in banners)
                {
                    if (banner != null)
                    {
                        banner.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }
            }

            if (PageId == PageEnums.Course)
            {
                var coursesIds = await this.DB.ApprovalToCourses.Where(Q => approvalIds.Contains(Q.ApprovalId)).Select(Q => Q.CourseId).ToListAsync();
                var courses = await this.DB.Courses.Where(Q => coursesIds.Contains(Q.CourseId)).ToListAsync();

                foreach (var course in courses)
                {
                    if (course != null)
                    {
                        course.CourseApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }
            }

            if (PageId == PageEnums.Event)
            {
                var eventIds = await this.DB.ApprovalToEvents.Where(Q => approvalIds.Contains(Q.ApprovalId)).Select(Q => Q.EventId).ToListAsync();
                var events = await this.DB.Events.Where(Q => eventIds.Contains(Q.EventId) && Q.IsDeleted == false).ToListAsync();

                foreach (var anEvent in events)
                {
                    if (anEvent != null)
                    {
                        anEvent.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }
            }

            if (PageId == PageEnums.Guide)
            {
                var guideIds = await this.DB.ApprovalToGuides.Where(Q => approvalIds.Contains(Q.ApprovalId)).Select(Q => Q.GuideId).ToListAsync();
                var guides = await this.DB.Guides.Where(Q => guideIds.Contains(Q.GuideId)).ToListAsync();

                foreach (var guide in guides)
                {
                    if (guide != null)
                    {
                        guide.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }
            }

            if (PageId == PageEnums.Module)
            {
                var moduleIds = await this.DB.ApprovalToModules.Where(Q => approvalIds.Contains(Q.ApprovalId)).Select(Q => Q.ModuleId).ToListAsync();
                var modules = await this.DB.Modules.Where(Q => moduleIds.Contains(Q.ModuleId) && Q.IsDeleted == false).ToListAsync();

                foreach (var module in modules)
                {
                    if (module != null)
                    {
                        module.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }
            }

            if (PageId == PageEnums.News)
            {
                var newsIds = await this.DB.ApprovalToNews.Where(Q => approvalIds.Contains(Q.ApprovalId)).Select(Q => Q.NewsId).ToListAsync();
                var listOfNews = await this.DB.News.Where(Q => newsIds.Contains(Q.NewsId) && Q.IsDeleted == false).ToListAsync();

                foreach (var news in listOfNews)
                {
                    if (news != null)
                    {
                        news.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }
            }

            if (PageId == PageEnums.ReleaseTraining)
            {
                var trainingIds = await this.DB.ApprovalToTrainings.Where(Q => approvalIds.Contains(Q.ApprovalId)).Select(Q => Q.TrainingId).ToListAsync();
                var trainings = await this.DB.Trainings.Where(Q => trainingIds.Contains(Q.TrainingId) && Q.IsDeleted == false).ToListAsync();
                foreach (var training in trainings)
                {
                    if (training != null)
                    {
                        training.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }
            }

            if (PageId == PageEnums.SetupCourse)
            {
                var courseIds = await this.DB.ApprovalToSetupCourses.Where(Q => approvalIds.Contains(Q.ApprovalId)).Select(Q => Q.CourseId).ToListAsync();
                var courses = await this.DB.Courses.Where(Q => courseIds.Contains(Q.CourseId) && Q.IsDeleted == false).ToListAsync();

                foreach (var course in courses)
                {
                    if (course != null)
                    {
                        course.SetupCourseApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }

                var setupLearnings = await this.DB.SetupLearning.Where(Q => courseIds.Contains(Q.CourseId.GetValueOrDefault())).ToListAsync();

                foreach (var learning in setupLearnings)
                {
                    if (learning != null)
                    {
                        learning.ApprovalStatus = ApprovalStatusesEnum.Approve;
                        learning.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }
            }

            if (PageId == PageEnums.SetupModule)
            {
                var setupModuleIds = await this.DB.ApprovalToSetupModules.Where(Q => approvalIds.Contains(Q.ApprovalId)).Select(Q => Q.SetupModuleId).ToListAsync();
                var setupModules = await this.DB.SetupModules.Where(Q => setupModuleIds.Contains(Q.SetupModuleId) && Q.IsDeleted == false).ToListAsync();

                foreach (var setupModule in setupModules)
                {
                    if (setupModule != null)
                    {
                        setupModule.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }

                var setupLearnings = await this.DB.SetupLearning.Where(Q => setupModuleIds.Contains(Q.SetupModuleId.GetValueOrDefault())).ToListAsync();

                foreach (var learning in setupLearnings)
                {
                    if (learning != null)
                    {
                        learning.ApprovalStatus = ApprovalStatusesEnum.Approve;
                        learning.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }
            }

            if (PageId == PageEnums.SetupPopQuiz)
            {
                var popQuizIds = await this.DB.ApprovalToPopQuizzes.Where(Q => approvalIds.Contains(Q.ApprovalId)).Select(Q => Q.PopQuizId).ToListAsync();
                var popQuizes = await this.DB.PopQuizzes.Where(Q => popQuizIds.Contains(Q.PopQuizId) && Q.IsDeleted == false).ToListAsync();

                foreach (var popQuiz in popQuizes)
                {
                    if (popQuiz != null)
                    {
                        popQuiz.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }

                var setupLearnings = await this.DB.SetupLearning.Where(Q => popQuizIds.Contains(Q.PopQuizId.GetValueOrDefault())).ToListAsync();

                foreach (var learning in setupLearnings)
                {
                    if (learning != null)
                    {
                        learning.ApprovalStatus = ApprovalStatusesEnum.Approve;
                        learning.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }
            }

            if (PageId == PageEnums.Survey)
            {
                var surveyIds = await this.DB.ApprovalToSurveys.Where(Q => approvalIds.Contains(Q.ApprovalId)).Select(Q => Q.SurveyId).ToListAsync();
                var surveys = await this.DB.Surveys.Where(Q => surveyIds.Contains(Q.SurveyId) && Q.IsDeleted == false).ToListAsync();

                foreach (var survey in surveys)
                {
                    if (survey != null)
                    {
                        survey.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }
            }

            if (PageId == PageEnums.Task)
            {
                var taskIds = await this.DB.ApprovalToTasks.Where(Q => approvalIds.Contains(Q.ApprovalId)).Select(Q => Q.TaskId).ToListAsync();
                var tasks = await this.DB.Tasks.Where(Q => taskIds.Contains(Q.TaskId) && Q.IsDeleted == false).ToListAsync();

                foreach (var task in tasks)
                {
                    if (task != null)
                    {
                        task.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                }
            }
        }

        public async Task<List<PagesModel>> GetAllPage()
        {
            var getPage = await this.DB.Pages.Where(Q => Q.NeedApproval == true).Select(Q => new PagesModel
            {
                Name = Q.Name,
                NeedApproval = Q.NeedApproval,
                PageId = Q.PageId
            }).ToListAsync();

            return getPage;
        }

        public async Task<List<PagesModel>> GetPageForInsert()
        {
            var getPage = await this.DB.Pages.Where(page => !this.DB.ApprovalMappings.Select(Q => Q.PageId).Contains(page.PageId) && page.NeedApproval == true).Select(Q => new PagesModel
            {
                Name = Q.Name,
                NeedApproval = Q.NeedApproval,
                PageId = Q.PageId
            }).ToListAsync();

            return getPage;
        }

        public async Task<List<ApprovalLevelModel>> GetAllApprovalLevel()
        {
            var getAllApprovalLevels = await this.DB.ApprovalLevels.Select(Q => new ApprovalLevelModel
            {
                ApprovalLevelId = Q.ApprovalLevelId,
                Description = Q.Description,
                Name = Q.Name
            }).AsNoTracking().ToListAsync();

            return getAllApprovalLevels;
        }

    }
}
