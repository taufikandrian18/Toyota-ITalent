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
    public class ApprovalContentService
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;
        private readonly InboxService inboxServiceMan;
        private readonly ClaimHelper ClaimMan;
        private readonly ApprovalService ApprovalMan;
        public ApprovalContentService(TalentContext db, IContextualService contextual, InboxService inboxService, ClaimHelper claim, ApprovalService approvalService)
        {
            this.DB = db;
            this.ContextMan = contextual;
            this.inboxServiceMan = inboxService;
            this.ClaimMan = claim;
            this.ApprovalMan = approvalService;
        }

        /// <summary>
        /// get approval list for view
        /// </summary>
        /// <param name="filter">filter model</param>
        /// <returns></returns>
        public async Task<ResponseApprovalContentModel> GetApprovalContentPaginateAsync(ApprovalContentFilterModel filter)
        {
            //set default

            if (filter.Page < 1)
            {
                filter.Page = 1;
            }

            if (filter.ItemPage < 1)
            {
                filter.ItemPage = 10;
            }

            var anything = DB.ApprovalToTasks;
            var currUserEmployeeId = ClaimMan.GetLoginUserId();
            //get all data
            var getData =
                (from ap in DB.Approvals
                 join att in DB.ApprovalToTasks on ap.ApprovalId equals att.ApprovalId into apt
                 from aapt in apt.DefaultIfEmpty()

                 join e in DB.Employees on ap.ActionBy equals e.EmployeeId
                 join am in DB.ApprovalMappings on ap.ApprovalMappingId equals am.ApprovalMappingid into ampt
                 from amt in ampt.DefaultIfEmpty()

                 join ah in DB.ApprovalHistories on ap.ApprovalId equals ah.ApprovalId into ahnull
                 from ah in ahnull.DefaultIfEmpty()

                 join al in DB.ApprovalLevels on amt.ApprovalLevelId equals al.ApprovalLevelId

                 join e2 in DB.Employees on ap.CreatedBy equals e2.EmployeeId
                 where (ap.ApprovalTo == currUserEmployeeId &&
                     ap.ApprovalStatusId == ApprovalStatusesEnum.WaitingId)
                     || ap.CreatedBy == currUserEmployeeId || ah.ActionBy == currUserEmployeeId

                 select new
                 {
                     ApprovalId = ap.ApprovalId,
                     ContentName = ap.ContentName,
                     ContentCategory = ap.ContentCategory == "Task" ?
                     ap.ContentCategory + " - " + aapt.Task.QuestionType.QuestionTypeName :
                     ap.ContentCategory,
                     CurrentLevel = ap.ApprovalLevel,
                     MaxLevel = ((int?)ap.ApprovalMapping.ApprovalLevelId).HasValue ? ap.ApprovalMapping.ApprovalLevelId : 0,
                     ApprovalDate = ap.ActionAt,
                     CreatedBy = e2.EmployeeName,
                     CreatedAt = ap.CreatedAt,
                     ApprovalBy = e.EmployeeName,
                     ApprovalStatus = ap.ApprovalStatus.ApprovalName,
                     Position = al.Name,
                     ApprovalTo = ap.ApprovalTo
                 });



            //filtering section
            //content name
            if ((string.IsNullOrEmpty(filter.ContentName) || string.IsNullOrWhiteSpace(filter.ContentName)) == false)
            {
                getData = getData.Where(Q => Q.ContentName.ToLower().Contains(filter.ContentName.ToLower()));
            }
            // category
            if ((string.IsNullOrEmpty(filter.ContentCategory)) == false)
            {
                getData = getData.Where(Q => Q.ContentCategory.ToLower().Equals(filter.ContentCategory.ToLower()));
            }
            // date
            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                getData = getData.Where(Q => Q.ApprovalDate >= startDate && Q.ApprovalDate <= endDate);
            }
            //position
            if ((string.IsNullOrEmpty(filter.ContentPosition) || string.IsNullOrWhiteSpace(filter.ContentPosition)) == false)
            {
                getData = getData.Where(Q => Q.Position.ToLower().Contains(filter.ContentPosition.ToLower()));
            }
            //ctrated by
            if ((string.IsNullOrEmpty(filter.CreatedBy) || string.IsNullOrWhiteSpace(filter.CreatedBy)) == false)
            {
                getData = getData.Where(Q => Q.CreatedBy.ToLower().Contains(filter.CreatedBy.ToLower()));
            }
            //status
            if ((string.IsNullOrEmpty(filter.ApprovalStatus)) == false)
            {
                getData = getData.Where(Q => Q.ApprovalStatus.ToLower().Contains(filter.ApprovalStatus.ToLower()));
            }

            // --------------- Order by ------------------

            switch (filter.OrderBy)
            {
                case "ContentNameUp":
                    {
                        getData = getData.OrderByDescending(Q => Q.ContentName);
                        break;
                    }

                case "ContentNameDown":
                    {
                        getData = getData.OrderBy(Q => Q.ContentName);
                        break;
                    }

                case "ContentCategoryUp":
                    {
                        getData = getData.OrderByDescending(Q => Q.ContentCategory);
                        break;
                    }

                case "ContentCategoryDown":
                    {
                        getData = getData.OrderBy(Q => Q.ContentCategory);
                        break;
                    }

                case "CreatedByUp":
                    {
                        getData = getData.OrderByDescending(Q => Q.CreatedBy);
                        break;
                    }

                case "CreatedByDown":
                    {
                        getData = getData.OrderBy(Q => Q.CreatedBy);
                        break;
                    }

                case "PositionUp":
                    {
                        getData = getData.OrderByDescending(Q => Q.Position);
                        break;
                    }

                case "PositionDown":
                    {
                        getData = getData.OrderBy(Q => Q.Position);
                        break;
                    }

                case "ApprovalStatusUp":
                    {
                        getData = getData.OrderByDescending(Q => Q.ApprovalStatus);
                        break;
                    }

                case "ApprovalStatusDown":
                    {
                        getData = getData.OrderBy(Q => Q.ApprovalStatus);
                        break;
                    }

                case "LearningDateUp":
                    {
                        getData = getData.OrderByDescending(Q => Q.ApprovalDate);
                        break;
                    }

                case "LearningDateDown":
                    {
                        getData = getData.OrderBy(Q => Q.ApprovalDate);
                        break;
                    }

                case "ActionByUp":
                    {
                        getData = getData.OrderByDescending(Q => Q.ApprovalBy);
                        break;
                    }

                case "ContenLevelUp":
                    {
                        getData = getData.OrderBy(Q => Q.CurrentLevel);
                        break;
                    }

                case "ContenLevelDown":
                    {
                        getData = getData.OrderByDescending(Q => Q.CurrentLevel);
                        break;
                    }

                case "ActionByDown":
                    {
                        getData = getData.OrderBy(Q => Q.ApprovalBy);
                        break;
                    }
                case "":
                    {
                        getData = getData.OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.ApprovalDate);
                        break;
                    }
                case null:
                    {
                        getData = getData.OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.ApprovalDate);
                        break;
                    }
                default:
                    {
                        getData = getData.OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.ApprovalDate);
                        break;
                    }
            }

            // ------------ final --------------------

            var totalData = await getData.Distinct().CountAsync();

            var listData = await getData
                .Select(Q => new ApprovalContentViewModel
                {
                    ApprovalId = Q.ApprovalId,
                    ContentName = Q.ContentName,
                    ContentCategory = Q.ContentCategory,
                    CurrentLevel = Q.CurrentLevel,
                    ApprovalDate = Q.ApprovalDate == null ? null : Q.ApprovalDate,
                    CreatedBy = Q.CreatedBy,
                    CreatedAt = Q.CreatedAt,
                    ApprovalBy = Q.ApprovalBy,
                    ApprovalStatus = Q.ApprovalStatus,
                    Position = Q.Position,
                    IsApprovableByUser = Q.ApprovalTo == currUserEmployeeId &&
                    Q.ApprovalStatus == ApprovalStatusesEnum.Waiting
                })
                .Distinct()
                .Skip((filter.Page - 1) * filter.ItemPage)
                .Take(filter.ItemPage)
                .AsNoTracking()
                .ToListAsync();


            //loop to get detail id

            foreach (var data in listData)
            {
                data.DetailId = await GetDetailIdAsync(data.ContentCategory, data.ApprovalId);
            }

            //get user login level
            var loginLevel = 1;

            try
            {
                //var employeeId = ContextMan.CookieClaims.EmployeeId;
                var employeeId = ClaimMan.GetLoginUserId();
                loginLevel = await DB.EmployeePositionMappings
                .Where(Q => Q.EmployeeId == employeeId)
                .Select(Q => Q.Position.ApprovalPositionMappings.ApprovalLevel)
                .FirstAsync();
            }
            catch
            {
                loginLevel = 1;
            }

            var responseModel = new ResponseApprovalContentModel
            {
                ContentList = listData,
                TotalData = totalData,
            };

            return responseModel;
        }

        /// <summary>
        /// to get detail id from type
        /// </summary>
        /// <param name="type"> type of mapping </param>
        /// <param name="approvalId"></param>
        /// <returns></returns>
        private async Task<int> GetDetailIdAsync(string type, int approvalId)
        {
            var getId = 0;

            if (type == ContentCategoryEnums.Banner)
            {
                var id = await DB.ApprovalToBanners.Where(Q => Q.ApprovalId == approvalId).Select(Q => Q.BannerId).FirstOrDefaultAsync();
                getId = id;
            }
            else if (type == ContentCategoryEnums.Event)
            {
                var id = await DB.ApprovalToEvents.Where(Q => Q.ApprovalId == approvalId).Select(Q => Q.EventId).FirstOrDefaultAsync();
                getId = id;
            }
            else if (type == ContentCategoryEnums.Guide)
            {
                var id = await DB.ApprovalToGuides.Where(Q => Q.ApprovalId == approvalId).Select(Q => Q.GuideId).FirstOrDefaultAsync();
                getId = id;
            }
            else if (type == ContentCategoryEnums.Module)
            {
                var id = await DB.ApprovalToModules.Where(Q => Q.ApprovalId == approvalId).Select(Q => Q.ModuleId).FirstOrDefaultAsync();
                getId = id;
            }
            else if (type == ContentCategoryEnums.News)
            {
                var id = await DB.ApprovalToNews.Where(Q => Q.ApprovalId == approvalId).Select(Q => Q.NewsId).FirstOrDefaultAsync();
                getId = id;
            }
            else if (type == ContentCategoryEnums.SetupCourse)
            {
                var id = await DB.ApprovalToSetupCourses.Where(Q => Q.ApprovalId == approvalId).Select(Q => Q.CourseId).FirstOrDefaultAsync();
                getId = id;
            }
            else if (type == ContentCategoryEnums.SetupModule)
            {
                var id = await DB.ApprovalToSetupModules.Where(Q => Q.ApprovalId == approvalId).Select(Q => Q.SetupModuleId).FirstOrDefaultAsync();
                getId = id;
            }
            else if (type == ContentCategoryEnums.SetupPopQuiz)
            {
                var id = await DB.ApprovalToPopQuizzes.Where(Q => Q.ApprovalId == approvalId).Select(Q => Q.PopQuizId).FirstOrDefaultAsync();
                getId = id;
            }
            else if (type == ContentCategoryEnums.Course)
            {
                var id = await DB.ApprovalToCourses.Where(Q => Q.ApprovalId == approvalId).Select(Q => Q.CourseId).FirstOrDefaultAsync();
                getId = id;
            }
            else if (type.Contains(ContentCategoryEnums.Task))
            {
                var id = await DB.ApprovalToTasks.Where(Q => Q.ApprovalId == approvalId).Select(Q => Q.TaskId).FirstOrDefaultAsync();
                getId = id;
            }
            else if (type == ContentCategoryEnums.ReleaseTraining)
            {
                var id = await DB.ApprovalToTrainings.Where(Q => Q.ApprovalId == approvalId).Select(Q => Q.TrainingId).FirstOrDefaultAsync();
                getId = id;
            }
            else if (type == ContentCategoryEnums.Survey)
            {
                var id = await DB.ApprovalToSurveys.Where(Q => Q.ApprovalId == approvalId).Select(Q => Q.SurveyId).FirstOrDefaultAsync();
                getId = id;
            }

            return getId;
        }

        /// <summary>
        /// to change approval's status into the approval status type
        /// </summary>
        /// <param name="id"> approval id </param>
        /// <param name="type"> type of approval status </param>
        /// <returns></returns>
        public async Task<bool> ChangeApprovalContentStatusAsync(int id, string type)
        {
            var approvalStatusList = await DB.ApprovalStatus.ToListAsync();
            var getStatus = approvalStatusList.Where(Q => Q.ApprovalName == type).FirstOrDefault();
            var getWaitingStatusId = approvalStatusList.Where(Q => Q.ApprovalName == ApprovalStatusesEnum.Waiting).Select(Q => Q.ApprovalStatusId).FirstOrDefault();
            var getApproveStatusId = approvalStatusList.Where(Q => Q.ApprovalName == ApprovalStatusesEnum.Approve).Select(Q => Q.ApprovalStatusId).FirstOrDefault();

            if (getStatus.ApprovalStatusId == 0)
            {
                return false;
            }
            var approval = await ApprovalMan.ProcessApproval(id, getStatus.ApprovalStatusId);

            await DB.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectedFromInbox(InboxRejectedModel model)
        {
            var approval = await this.ApprovalMan.ProcessApproval(model.ApprovalId, ApprovalStatusesEnum.RejectedId, model.Message);
            return true;
        }

        public async Task<bool> AcceptRequestInbox(int approvalId, int inboxId)
        {
            var approval = await this.ApprovalMan.ProcessApproval(approvalId, ApprovalStatusesEnum.ApproveId);
            return true;
        }
    }
}
