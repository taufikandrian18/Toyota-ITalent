using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.DbQueryModels;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.Services
{
    public class ApprovalService
    {
        private readonly TalentContext DB;
        private readonly IContextualService ContextMan;
        private readonly InboxService InboxMan;
        private readonly UserSideInboxService mobileInboxesMan;
        private readonly ClaimHelper ClaimMan;

        public ApprovalService(TalentContext db, IContextualService contextual, ClaimHelper claim, InboxService inboxService, UserSideInboxService userSideInboxService)
        {
            this.DB = db;
            this.ContextMan = contextual;
            this.ClaimMan = claim;
            this.InboxMan = inboxService;
            this.mobileInboxesMan = userSideInboxService;
        }

        public async Task<Approvals> CreateNewApproval(ApprovalCreateFormModel model)
        {

            var approvalTo = new SuperiorEmployee();
            var currUserEmployeeId = ClaimMan.GetLoginUserId();
            var currUserTalentLevel = await this.DB.TamemployeeStructure
                            .Where(Q => Q.NoReg == currUserEmployeeId)
                            .Select(Q => Q.TalentLevel)
                            .FirstOrDefaultAsync();

            var approvalModel = new Approvals
            {
                ContentName = model.ContentName,
                ContentCategory = model.ContentCategory,
                ApprovalMappingId = null,
                ApprovalLevel = currUserTalentLevel,
                ApprovalStatusId = model.ApprovalStatusId,
                CreatedBy = null,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                ActionBy = null,
                ActionAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                ApprovalTo = null,
            };

            if (currUserEmployeeId.ToLower() == "system") {
                approvalModel.CreatedBy = null;
                approvalModel.ActionBy = null;
            }

            var approvalMapping = await this.DB.ApprovalMappings
            .Where(Q => Q.PageId == model.PageEnum)
            .FirstOrDefaultAsync();

            if (approvalModel.ApprovalStatusId == ApprovalStatusesEnum.DraftId) // save
            {
                if (approvalMapping != null)
                {
                    approvalModel.ApprovalMappingId = approvalMapping.ApprovalMappingid;
                }

                this.DB.Approvals.Add(approvalModel);
            }
            else // submit
            {
                if (approvalMapping == null)
                {
                    approvalModel.ActionBy = null;
                    approvalModel.ApprovalStatusId = ApprovalStatusesEnum.ApproveId;
                }
                else
                {
                    approvalModel.ApprovalMappingId = approvalMapping.ApprovalMappingid;
                    approvalTo = await GetNextEmployeeToApprove(currUserEmployeeId, currUserTalentLevel, approvalMapping.ApprovalLevelId);
                    if (approvalTo == null)
                    {
                        approvalModel.ActionBy = null;
                        approvalModel.ApprovalStatusId = ApprovalStatusesEnum.ApproveId;
                    }
                    else
                    {
                        approvalModel.ApprovalTo = approvalTo.NoReg;
                        approvalModel.ApprovalStatusId = ApprovalStatusesEnum.WaitingId;
                    }
                }
                this.DB.Approvals.Add(approvalModel);
            }

            var approvalToOctopus = new ApprovalOctopusModel
            {
                ApprovalId = approvalModel.ApprovalId,
                PageEnums = model.PageEnum,
                ContentId = model.ContentId
            };

            InsertApprovalOctopus(approvalToOctopus);

            if (approvalModel.ApprovalStatusId == ApprovalStatusesEnum.WaitingId && approvalModel.ApprovalMappingId != null)
            {
                //Add eagar service here
                var inbox = new InboxFormModel
                {
                    ApprovalId = approvalModel.ApprovalId,
                    LinkToPage = null,
                    ApprovalTo = approvalTo.NoReg,
                    ContentCategory = model.ContentCategory
                };

                var insertInbox = this.InboxMan.InsertInbox(inbox);

                //Save for get the ApprovalId
                await this.DB.SaveChangesAsync();

                if (approvalModel.ApprovalStatusId == ApprovalStatusesEnum.WaitingId)
                {
                    await this.InboxMan.UpdateLinkToPage(approvalModel.ApprovalId, model.PageEnum);
                }
            }

            return approvalModel;
        }

        private void InsertApprovalOctopus(ApprovalOctopusModel model)
        {
            switch (model.PageEnums)
            {
                case PageEnums.Banner:
                    var banner = new ApprovalToBanners
                    {
                        ApprovalId = model.ApprovalId,
                        BannerId = model.ContentId,
                    };
                    this.DB.ApprovalToBanners.Add(banner);
                    break;
                case PageEnums.Course:
                    var course = new ApprovalToCourses
                    {
                        ApprovalId = model.ApprovalId,
                        CourseId = model.ContentId
                    };
                    this.DB.ApprovalToCourses.Add(course);
                    break;
                case PageEnums.Event:
                    var events = new ApprovalToEvents
                    {
                        ApprovalId = model.ApprovalId,
                        EventId = model.ContentId
                    };
                    this.DB.ApprovalToEvents.Add(events);
                    break;
                case PageEnums.Guide:
                    var guide = new ApprovalToGuides
                    {
                        ApprovalId = model.ApprovalId,
                        GuideId = model.ContentId
                    };
                    this.DB.ApprovalToGuides.Add(guide);
                    break;
                case PageEnums.Module:
                    var module = new ApprovalToModules
                    {
                        ApprovalId = model.ApprovalId,
                        ModuleId = model.ContentId
                    };
                    this.DB.ApprovalToModules.Add(module);
                    break;
                case PageEnums.News:
                    var news = new ApprovalToNews
                    {
                        ApprovalId = model.ApprovalId,
                        NewsId = model.ContentId
                    };
                    this.DB.ApprovalToNews.Add(news);
                    break;
                case PageEnums.ReleaseTraining:
                    var releaseTraining = new ApprovalToTrainings
                    {
                        ApprovalId = model.ApprovalId,
                        TrainingId = model.ContentId
                    };
                    this.DB.ApprovalToTrainings.Add(releaseTraining);
                    break;
                case PageEnums.SetupCourse:
                    var setupCourse = new ApprovalToSetupCourses
                    {
                        ApprovalId = model.ApprovalId,
                        CourseId = model.ContentId
                    };
                    this.DB.ApprovalToSetupCourses.Add(setupCourse);
                    break;
                case PageEnums.SetupModule:
                    var setupModule = new ApprovalToSetupModules
                    {
                        ApprovalId = model.ApprovalId,
                        SetupModuleId = model.ContentId
                    };
                    this.DB.ApprovalToSetupModules.Add(setupModule);
                    break;
                case PageEnums.SetupPopQuiz:
                    var setupPopQuiz = new ApprovalToPopQuizzes
                    {
                        ApprovalId = model.ApprovalId,
                        PopQuizId = model.ContentId
                    };
                    this.DB.ApprovalToPopQuizzes.Add(setupPopQuiz);
                    break;
                case PageEnums.Survey:
                    var survey = new ApprovalToSurveys
                    {
                        ApprovalId = model.ApprovalId,
                        SurveyId = model.ContentId
                    };
                    this.DB.ApprovalToSurveys.Add(survey);
                    break;
            }
        }

        public async Task<Approvals> UpdateNewApproval(ApprovalUpdateFormModel model)
        {
            var approvalTo = new SuperiorEmployee();
            var currUserEmployeeId = ClaimMan.GetLoginUserId();
            var currUserTalentLevel = await this.DB.TamemployeeStructure
                            .Where(Q => Q.NoReg == currUserEmployeeId)
                            .Select(Q => Q.TalentLevel)
                            .FirstOrDefaultAsync();
            var currentApproval = await this.DB.Approvals
                                .Where(Q => Q.ApprovalId == model.ApprovalId)
                                .FirstOrDefaultAsync();
            if (currentApproval == null) return null;
            var approvalMapping = await this.DB.ApprovalMappings
                                    .Where(Q => Q.PageId == model.PageEnum)
                                    .FirstOrDefaultAsync();

            currentApproval.ActionBy = currUserEmployeeId;
            currentApproval.ActionAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            currentApproval.ContentName = model.ContentName;
            currentApproval.ApprovalLevel = currUserTalentLevel;
            currentApproval.CreatedBy = currUserEmployeeId;
            if (model.ApprovalStatusId == ApprovalStatusesEnum.DraftId) // save
            {
                currentApproval.ApprovalStatusId = ApprovalStatusesEnum.DraftId;
                if (approvalMapping != null)
                {
                    currentApproval.ApprovalMappingId = approvalMapping.ApprovalMappingid;
                }
            }
            else // submit
            {
                if (approvalMapping == null)
                {
                    currentApproval.ApprovalStatusId = ApprovalStatusesEnum.ApproveId;
                }
                else
                {
                    currentApproval.ApprovalMappingId = approvalMapping.ApprovalMappingid;
                    approvalTo = await GetNextEmployeeToApprove(currUserEmployeeId, currUserTalentLevel, approvalMapping.ApprovalLevelId);
                    if (approvalTo == null)
                    {
                        currentApproval.ActionBy = currUserEmployeeId;
                        currentApproval.ApprovalStatusId = ApprovalStatusesEnum.ApproveId;
                    }
                    else
                    {
                        currentApproval.ApprovalTo = approvalTo.NoReg;
                        currentApproval.ApprovalStatusId = ApprovalStatusesEnum.WaitingId;
                    }

                   

                }
            }
            if (currUserEmployeeId.ToLower() == "system")
            {
                currentApproval.CreatedBy = null;
                currentApproval.ActionBy = null;
            }
            if (currentApproval.ApprovalStatusId == ApprovalStatusesEnum.WaitingId && currentApproval.ApprovalMappingId != null)
            {
                //Add eagar service here
                var inbox = new InboxFormModel
                {
                    ApprovalId = currentApproval.ApprovalId,
                    LinkToPage = null,
                    ApprovalTo = approvalTo.NoReg,
                    ContentCategory = model.ContentCategory
                };

                var insertInbox = this.InboxMan.InsertInbox(inbox);

                //Save for get the ApprovalId
                await this.DB.SaveChangesAsync();

                if (currentApproval.ApprovalStatusId == ApprovalStatusesEnum.WaitingId)
                {
                    await this.InboxMan.UpdateLinkToPage(currentApproval.ApprovalId, model.PageEnum);
                }
            }
            return currentApproval;
        }
        private async Task<SuperiorEmployee> GetNextEmployeeToApprove(string employeeId, int currentApprovalLevel, int maximumLevel)
        {
            if (currentApprovalLevel >= maximumLevel) return null;
            var superiors = await this.DB
                            .GetAllSuperiorEmployees(employeeId)
                            .ToListAsync();
            if (superiors == null) return null;
            var superior = superiors.OrderBy(Q => Q.TalentLevel)
                          .Where(Q => Q.TalentLevel > currentApprovalLevel).
                          FirstOrDefault();

            if (superior == null) return null;
            if (superior.TalentLevel > maximumLevel) return null;
            return superior;
        }

        public async Task<Approvals> ProcessApproval(int approvalId, int approvalStatusId, string rejectMessage = null)
        {

            var currentApproval = await DB.Approvals
                                        .Where(Q => Q.ApprovalId == approvalId)
                                        .SingleOrDefaultAsync();
            var superiors = await this.DB
                            .GetAllSuperiorEmployees(currentApproval.CreatedBy)
                            .ToListAsync();

            var currEmployeeAsSuperior = superiors
                                        .Where(Q => Q.NoReg == ClaimMan.GetLoginUserId() &&
                                                Q.TalentLevel > currentApproval.ApprovalLevel)
                                        .FirstOrDefault();
            if (currentApproval == null || currEmployeeAsSuperior == null) return null;
            if (!(approvalStatusId == ApprovalStatusesEnum.ApproveId
            || approvalStatusId == ApprovalStatusesEnum.RejectedId))
            {
                return null;
            }
            if (approvalStatusId == ApprovalStatusesEnum.ApproveId)
            {
                await AcceptApproval(currentApproval, currEmployeeAsSuperior);
            }
            else
            {
                await RejectApproval(currentApproval);
            }
            currentApproval.ActionBy = currEmployeeAsSuperior.NoReg;
            currentApproval.ActionAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            AddApprovalToHistory(currentApproval);

            if (currentApproval.ApprovalStatusId == ApprovalStatusesEnum.WaitingId)
            {
                await this.InboxMan.CreateNextInboxApproval(currentApproval);
            }
            else if (currentApproval.ApprovalStatusId == ApprovalStatusesEnum.RejectedId)
            {
                await this.InboxMan.CreateRejectInbox(currentApproval, rejectMessage);
            }
            else if (currentApproval.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
            {
                await this.InboxMan.MarkDoneApprovedInbox(currentApproval.ApprovalId);
                await this.UpdateApprovalContent(currentApproval);
            }

            await this.SetupLearningUpdate(currentApproval);

            await DB.SaveChangesAsync();
            return currentApproval;
        }

        private void AddApprovalToHistory(Approvals approval)
        {
            int approvalStatusId = approval.ApprovalStatusId;
            string approvalName = ApprovalStatusesEnum.Rejected;
            if (approvalStatusId != ApprovalStatusesEnum.RejectedId)
            {
                approvalStatusId = ApprovalStatusesEnum.ApproveId;
                approvalName = ApprovalStatusesEnum.Approve;
            }

            var history = new ApprovalHistories
            {
                ApprovalId = approval.ApprovalId,
                ApprovalStatusId = approvalStatusId,
                ApprovalStatus = approvalName,
                ActionAt = approval.ActionAt.Value,
                ActionBy = approval.ActionBy,
            };
            DB.ApprovalHistories.Add(history);
            return;
        }
        private async Task AcceptApproval(Approvals approval, SuperiorEmployee currEmployee)
        {
            var maxApprovalLevel = await DB.ApprovalMappings
                                    .Where(Q => Q.ApprovalMappingid == approval.ApprovalMappingId)
                                    .Select(Q => Q.ApprovalLevel.ApprovalLevelId)
                                    .FirstOrDefaultAsync();
            approval.ApprovalLevel = currEmployee.TalentLevel;
            var approvalTo = await GetNextEmployeeToApprove(approval.CreatedBy, approval.ApprovalLevel, maxApprovalLevel);
            if (approvalTo == null)
            {
                approval.ApprovalStatusId = ApprovalStatusesEnum.ApproveId;
            }
            else
            {
                approval.ApprovalTo = approvalTo.NoReg;
                approval.ApprovalStatusId = ApprovalStatusesEnum.WaitingId;
            }
            return;
        }

        private async Task RejectApproval(Approvals approval)
        {
            approval.ApprovalStatusId = ApprovalStatusesEnum.RejectedId;
            return;
        }

        public async Task<bool> DeleteApproval(int approvalId)
        {
            var approvals = await this.DB.Approvals.Where(Q => Q.ApprovalId == approvalId).FirstOrDefaultAsync();

            if (approvals.ApprovalStatusId == ApprovalStatusesEnum.WaitingId)
            {
                return false;
            }

            //Delete Inbox
            var deleteInbox = await this.DB.Inboxes.Where(Q => Q.ApprovalId == approvals.ApprovalId).ToListAsync();
            var deleteApprovalHistories = await this.DB.ApprovalHistories.Where(Q => Q.ApprovalId == approvals.ApprovalId).ToListAsync();

            this.DB.ApprovalHistories.RemoveRange(deleteApprovalHistories);
            this.DB.Inboxes.RemoveRange(deleteInbox);
            this.DB.Approvals.Remove(approvals);

            return true;
        }

        public async Task SetupLearningUpdate(Approvals approval)
        {
            var setupLearning = new SetupLearning();

            if (approval.ContentCategory == ContentCategoryEnums.SetupCourse)
            {
                setupLearning = await (from atsc in this.DB.ApprovalToSetupCourses
                                       join sl in this.DB.SetupLearning on atsc.CourseId equals sl.CourseId into lsl
                                       from sl in lsl.DefaultIfEmpty()
                                       where atsc.ApprovalId == approval.ApprovalId
                                       select sl).FirstOrDefaultAsync();
            }
            else if (approval.ContentCategory == ContentCategoryEnums.SetupModule)
            {
                setupLearning = await (from atsm in this.DB.ApprovalToSetupModules
                                       join sl in this.DB.SetupLearning on atsm.SetupModuleId equals sl.SetupModuleId into lsl
                                       from sl in lsl.DefaultIfEmpty()
                                       where atsm.ApprovalId == approval.ApprovalId
                                       select sl).FirstOrDefaultAsync();
            }
            else if (approval.ContentCategory == ContentCategoryEnums.SetupPopQuiz)
            {
                setupLearning = await (from atpq in this.DB.ApprovalToPopQuizzes
                                       join sl in this.DB.SetupLearning on atpq.PopQuizId equals sl.PopQuizId into lsl
                                       from sl in lsl.DefaultIfEmpty()
                                       where atpq.ApprovalId == approval.ApprovalId
                                       select sl).FirstOrDefaultAsync();
            }
            else
            {
                return;
            }

            if (setupLearning.SetupLearningId == 0)
            {
                return;
            }

            if (approval.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
            {
                setupLearning.ApprovalStatus = ApprovalStatusesEnum.Approve;
                setupLearning.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else if (approval.ApprovalStatusId == ApprovalStatusesEnum.RejectedId)
            {
                setupLearning.ApprovalStatus = ApprovalStatusesEnum.Rejected;
                setupLearning.ApprovedAt = null;
            }
            else if (approval.ApprovalStatusId == ApprovalStatusesEnum.DraftId)
            {
                setupLearning.ApprovalStatus = ApprovalStatusesEnum.Draft;
                setupLearning.ApprovedAt = null;
            }
            else if (approval.ApprovalStatusId == ApprovalStatusesEnum.WaitingId)
            {
                setupLearning.ApprovalStatus = ApprovalStatusesEnum.Waiting;
                setupLearning.ApprovedAt = null;
            }
        }

        public async Task UpdateApprovalContent(Approvals approval)
        {
            if (approval.ContentCategory == ContentCategoryEnums.Banner)
            {
                var bannerId = await this.DB.ApprovalToBanners.Where(Q => Q.ApprovalId == approval.ApprovalId).Select(Q => Q.BannerId).FirstOrDefaultAsync();
                var banner = await this.DB.Banners.Where(Q => Q.BannerId == bannerId).FirstOrDefaultAsync();
                banner.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else if (approval.ContentCategory == ContentCategoryEnums.Course)
            {
                var courseId = await this.DB.ApprovalToCourses.Where(Q => Q.ApprovalId == approval.ApprovalId).Select(Q => Q.CourseId).FirstOrDefaultAsync();
                var course = await this.DB.Courses.Where(Q => Q.CourseId == courseId).FirstOrDefaultAsync();
                course.CourseApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else if (approval.ContentCategory == ContentCategoryEnums.Event)
            {
                var eventId = await this.DB.ApprovalToEvents.Where(Q => Q.ApprovalId == approval.ApprovalId).Select(Q => Q.EventId).FirstOrDefaultAsync();
                var contentEvent = await this.DB.Events.Where(Q => Q.EventId == eventId).FirstOrDefaultAsync();
                contentEvent.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else if (approval.ContentCategory == ContentCategoryEnums.Guide)
            {
                var guideId = await this.DB.ApprovalToGuides.Where(Q => Q.ApprovalId == approval.ApprovalId).Select(Q => Q.GuideId).FirstOrDefaultAsync();
                var guide = await this.DB.Guides.Where(Q => Q.GuideId == guideId).FirstOrDefaultAsync();
                guide.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else if (approval.ContentCategory == ContentCategoryEnums.Module)
            {
                var moduleId = await this.DB.ApprovalToModules.Where(Q => Q.ApprovalId == approval.ApprovalId).Select(Q => Q.ModuleId).FirstOrDefaultAsync();
                var module = await this.DB.Modules.Where(Q => Q.ModuleId == moduleId).FirstOrDefaultAsync();
                module.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else if (approval.ContentCategory == ContentCategoryEnums.News)
            {
                var newsId = await this.DB.ApprovalToNews.Where(Q => Q.ApprovalId == approval.ApprovalId).Select(Q => Q.NewsId).FirstOrDefaultAsync();
                var news = await this.DB.News.Where(Q => Q.NewsId == newsId).FirstOrDefaultAsync();
                news.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else if (approval.ContentCategory == ContentCategoryEnums.Survey)
            {
                var surveyId = await this.DB.ApprovalToSurveys.Where(Q => Q.ApprovalId == approval.ApprovalId).Select(Q => Q.SurveyId).FirstOrDefaultAsync();
                var survey = await this.DB.Surveys.Where(Q => Q.SurveyId == surveyId).FirstOrDefaultAsync();
                survey.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else if (approval.ContentCategory == ContentCategoryEnums.ReleaseTraining)
            {
                var trainingId = await this.DB.ApprovalToTrainings.Where(Q => Q.ApprovalId == approval.ApprovalId).Select(Q => Q.TrainingId).FirstOrDefaultAsync();
                var training = await this.DB.Trainings.Where(Q => Q.TrainingId == trainingId).FirstOrDefaultAsync();
                training.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();

                //send notification inbox to each user in a training
                if (training.RescheduledAt != null)
                {
                    await mobileInboxesMan.InsertInboxForTrainingEdited(trainingId);
                }
            }
            else if (approval.ContentCategory == ContentCategoryEnums.SetupCourse)
            {
                var setupCourseId = await this.DB.ApprovalToSetupCourses.Where(Q => Q.ApprovalId == approval.ApprovalId).Select(Q => Q.CourseId).FirstOrDefaultAsync();
                var setupCourse = await this.DB.Courses.Where(Q => Q.CourseId == setupCourseId).FirstOrDefaultAsync();
                setupCourse.SetupCourseApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else if (approval.ContentCategory == ContentCategoryEnums.SetupModule)
            {
                var setupModuleId = await this.DB.ApprovalToSetupModules.Where(Q => Q.ApprovalId == approval.ApprovalId).Select(Q => Q.SetupModuleId).FirstOrDefaultAsync();
                var setupModule = await this.DB.SetupModules.Where(Q => Q.SetupModuleId == setupModuleId).FirstOrDefaultAsync();
                setupModule.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
            else if (approval.ContentCategory == ContentCategoryEnums.SetupPopQuiz)
            {
                var popQuizId = await this.DB.ApprovalToPopQuizzes.Where(Q => Q.ApprovalId == approval.ApprovalId).Select(Q => Q.PopQuizId).FirstOrDefaultAsync();
                var popQuiz = await this.DB.PopQuizzes.Where(Q => Q.PopQuizId == popQuizId).FirstOrDefaultAsync();
                popQuiz.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            }
        }
    }
}

