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
    public class SetupCourseService
    {
        private readonly TalentContext DB;
        private readonly InboxService InboxMan;
        private readonly IContextualService ContextMan;
        private readonly ApprovalService ApprovalMan;

        public SetupCourseService(TalentContext db, InboxService inboxService, IContextualService contextual, ApprovalService approvalService)
        {
            this.DB = db;
            this.InboxMan = inboxService;
            this.ContextMan = contextual;
            this.ApprovalMan = approvalService;
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

        public async Task<CourseViewModel> GetAllCourseNoSetup(string search, bool? descending)
        {
            var setupCourses = await this.DB.SetupLearning.AsNoTracking().Where(Q => Q.CourseId != null).Select(Q => Q.CourseId).ToListAsync();
            var approvedCourses = await (
                from atc in DB.ApprovalToCourses
                join a in DB.Approvals on atc.ApprovalId equals a.ApprovalId
                where a.ApprovalStatusId == ApprovalStatusesEnum.ApproveId
                select atc.CourseId
                ).ToListAsync();

            var allCourses = await this.DB.Courses.Select(Q => new CourseModel
            {
                CourseId = Q.CourseId,
                ProgramTypeId = Q.ProgramTypeId,
                LevelId = Q.LevelId,
                CourseCategoryId = Q.CourseCategoryId,
                LearningTypeId = Q.LearningTypeId,
                BlobId = Q.BlobId,
                CourseName = Q.CourseName,
                CoursePrice = Q.CoursePrice,
                CourseDescription = Q.CourseDescription,
                Others = Q.Others,
                IsRecommendedForYou = Q.IsRecommendedForYou,
                IsPopular = Q.IsPopular,
                //IsPublished = Q.IsPublished,
                CreatedBy = Q.CreatedBy,
                CreatedAt = Q.CreatedAt,
                UpdatedAt = Q.UpdatedAt,
                IsDeleted = Q.IsDeleted,
            }).Where(Q => Q.IsDeleted == false && !setupCourses.Contains(Q.CourseId) && approvedCourses.Contains(Q.CourseId)).ToListAsync(); // && Q.CourseName.Contains(search)

            if (descending.Value)
            {
                allCourses.OrderByDescending(Q => Q.CourseName);
            }
            else
            {
                allCourses.OrderBy(Q => Q.CourseName);
            }
            var totalItem = await this.DB.Courses.Where(Q => Q.IsDeleted == false && !setupCourses.Contains(Q.CourseId) && approvedCourses.Contains(Q.CourseId)).CountAsync();

            var result = new CourseViewModel
            {
                ListCourseModel = allCourses,
                TotalItem = totalItem
            };

            return result;
        }

        public async Task<List<CoursePrerequisiteViewModel>> GetAllPrerequisites(string search)
        {
            var query = from sl in DB.SetupLearning
                        where sl.LearningCategoryName == LearningCategoryNameEnum.Module || sl.LearningCategoryName == LearningCategoryNameEnum.Course
                        select new CoursePrerequisiteViewModel
                        {
                            NextCourseId = sl.CourseId,
                            NextSetupModuleId = sl.SetupModuleId,
                            Name = sl.LearningName
                        };

            var data = await query.Where(Q => Q.Name.Contains(search)).ToListAsync();

            return data;
        }

        public async Task<List<ModuleForSetupModel>> GetAllModules(string search)
        {
            var getModules = await this.DB.Modules.Where(Q => Q.IsDeleted == false).Select(Q => new ModuleForSetupModel
            {
                ModuleName = Q.ModuleName,
                ModuleId = Q.ModuleId
            }).AsNoTracking().ToListAsync();

            if (!String.IsNullOrWhiteSpace(search))
            {
                getModules = getModules.Where(Q => Q.ModuleName.ToLower().Contains(search.ToLower())).ToList();
            }

            return getModules;
        }

        public async Task<List<CompetencyMappingJoinModel>> GetAllTaskCompetencies(string search)
        {
            var query = from t in DB.Tasks
                        join c in DB.Competencies on t.CompetencyId equals c.CompetencyId
                        join ct in DB.CompetencyTypes on c.CompetencyTypeId equals ct.CompetencyTypeId
                        where t.IsDeleted == false
                        select new CompetencyMappingJoinModel
                        {
                            CompetencyId = c.CompetencyId,
                            CompetencyTypeId = c.CompetencyTypeId,
                            PrefixCode = c.PrefixCode,
                            CompetencyTypeName = ct.CompetencyTypeName,
                            CompetencyNameMapping = ct.CompetencyTypeName.Substring(0, 1) + "-" + c.PrefixCode,
                        };
            var result = await query.AsNoTracking().AsQueryable().Distinct().Where(Q => Q.CompetencyNameMapping.Contains(search)).ToListAsync();
            return result;
        }

        public async Task<List<ModuleForSetupModel>> GetAllTaskModules(string search)
        {
            var query = from t in DB.Tasks
                        join m in DB.Modules on t.ModuleId equals m.ModuleId
                        where t.IsDeleted == false
                        select new ModuleForSetupModel
                        {
                            ModuleName = m.ModuleName,
                            ModuleId = m.ModuleId
                        };
            var result = await query.AsNoTracking().AsQueryable().Distinct().Where(Q => Q.ModuleName.Contains(search)).ToListAsync();
            return result;
        }

        public async Task<List<TaskForSetupModuleModel>> GetAllTaskFiltered(TaskCodeFilteredFormModel model)
        {
            var query = (from T in this.DB.Tasks
                         join M in this.DB.Modules on T.ModuleId equals M.ModuleId
                         join C in this.DB.Competencies on T.CompetencyId equals C.CompetencyId
                         join CT in this.DB.CompetencyTypes on C.CompetencyTypeId equals CT.CompetencyTypeId
                         join ET in this.DB.EvaluationTypes on T.EvaluationTypeId equals ET.EvaluationTypeId
                         where T.IsDeleted == false
                         select new
                         {
                             TaskCode = CT.CompetencyTypeName.Substring(0, 1) + "-" + C.PrefixCode + "-" + ET.EvaluationTypeName + "-" + T.TaskNumber + "-" + M.ModuleName,
                             T.TaskId,
                             C.CompetencyId,
                             M.ModuleId,
                             T.QuestionTypeId
                         }).AsNoTracking().AsQueryable();

            if (model.CompetencyId != null)
            {
                query = query.Where(Q => Q.CompetencyId == model.CompetencyId);
            }

            if (model.ModuleId != null)
            {
                query = query.Where(Q => Q.ModuleId == model.ModuleId);
            }

            if (model.TaskCode != null)
            {
                query = query.Where(Q => Q.TaskCode.Contains(model.TaskCode));
            }

            var result = await query.Select(Q => new TaskForSetupModuleModel
            {
                TaskCode = Q.TaskCode,
                TaskId = Q.TaskId,
                TaskTypeId = Q.QuestionTypeId
            }).ToListAsync();

            for (var i = 0; i < result.Count(); i++)
            {
                // var scorePoints = await GetScorePoints(result[i].TaskTypeId ?? default(int), result[i].TaskId);
                // result[i].Score = scorePoints.Score;
                // result[i].Points = scorePoints.Points;
                result[i].Score = 0;
                result[i].Points = 0;
            }

            return result;
        }

        public async Task<ScorePointsModel> GetScorePoints(int taskTypeId, int taskId)
        {
            var model = new ScorePointsModel();
            //1   True / False
            if (taskTypeId == 1)
            {
                var timePointId = await DB.TaskTrueFalseTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).FirstOrDefaultAsync();
                model.Score = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Score).FirstOrDefaultAsync() ?? default(int);
                model.Points = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            //2   Matching
            if (taskTypeId == 2)
            {
                var timePointId = await DB.TaskMatchingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).FirstOrDefaultAsync();
                model.Score = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Score).FirstOrDefaultAsync() ?? default(int);
                model.Points = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            //3   Sequence
            if (taskTypeId == 3)
            {
                var timePointIds = await DB.TaskSequenceChoices.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).ToListAsync();
                //var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Score).ToListAsync();
                var scores = await (from tp in DB.TimePoints
                                    join tsc in DB.TaskSequenceChoices on tp.TimePointId equals tsc.Score
                                    where tsc.TaskId == taskId
                                    select tp.Score).ToListAsync();
                model.Score = scores.Sum() ?? default(int);
                var points = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Points).ToListAsync();
                model.Points = points.Sum();
            }
            //4   Tebak Gambar
            if (taskTypeId == 4)
            {
                var timePointId = await DB.TaskTebakGambarTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).FirstOrDefaultAsync();
                model.Score = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Score).FirstOrDefaultAsync() ?? default(int);
                model.Points = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            //5   Hot Spot
            if (taskTypeId == 5)
            {
                var timePointIds = await DB.TaskHotSpotAnswers.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).ToListAsync();
                //var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Score).ToListAsync();
                //model.Score = scores.Sum() ?? default(int);
                //var points = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Points).ToListAsync();
                //model.Points = points.Sum();

                int? scores = 0;
                int? points = 0;
                //foreach karena ada timepointsIds yang sama (dipakai berulang) saat setup task
                foreach (var item in timePointIds)
                {
                    var data = await DB.TimePoints.Where(q => q.TimePointId == item).Select(Q => new { Q.Score, Q.Points }).FirstOrDefaultAsync();
                    scores = scores + data.Score;
                    points = points + data.Points;
                }
                model.Score = scores ?? default(int);
                model.Points = points ?? default(int);

            }
            //6   Short Answer
            //7   Open Question/ Essay
            //11  File Upload
            if (taskTypeId == 6 || taskTypeId == 7 || taskTypeId == 11)
            {
                model.Score = 100;
                model.Points = 0;
            }
            //8   Checklist
            if (taskTypeId == 8)
            {
                var timePointIds = await DB.TaskChecklistChoices.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).ToListAsync();
                var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Score).ToListAsync();
                model.Score = scores.Sum() ?? default(int);
                //model.Score = timePointIds.Sum();
                var points = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Points).ToListAsync();
                model.Points = points.Sum();
            }
            //9   Rating
            if (taskTypeId == 9)
            {
                var timePointIds = new List<int>
                    {
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score0To20).FirstOrDefaultAsync(),
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score21To40).FirstOrDefaultAsync(),
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score41To60).FirstOrDefaultAsync(),
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score61To80).FirstOrDefaultAsync(),
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score81To100).FirstOrDefaultAsync()
                    };
                var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => new { Score = Q.Score, Point = Q.Points }).ToListAsync();
                var choises = await DB.TaskRatingChoices.Where(Q => Q.TaskId == taskId).CountAsync();
                model.Score = scores.Max(q => q.Score) * choises ?? default(int);
                model.Points = scores.Max(q => q.Point) * choises;
                //model.Points = await DB.TimePoints.Where(Q => Q.Score == scores.Max()).Select(Q => Q.Points * choises).FirstOrDefaultAsync();
            }
            //10  Multiple Choice
            if (taskTypeId == 10)
            {
                var timePointId = await DB.TaskMultipleChoiceTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).FirstOrDefaultAsync();
                model.Score = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Score).FirstOrDefaultAsync() ?? default(int);
                model.Points = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            //12  Matrix
            if (taskTypeId == 12)
            {
                var timePointIds = new List<int>
                    {
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn1).FirstOrDefaultAsync(),
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn2).FirstOrDefaultAsync(),
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn3).FirstOrDefaultAsync(),
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn4).FirstOrDefaultAsync(),
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn5).FirstOrDefaultAsync()
                    };
                //var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Score).ToListAsync();
                var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => new { Score = Q.Score, Point = Q.Points }).ToListAsync();
                var choises = await DB.TaskMatrixQuestions.Where(Q => Q.TaskId == taskId).CountAsync();
                model.Score = scores.Max(q => q.Score) * choises ?? default(int);
                model.Points = scores.Max(q => q.Point) * choises;
                //model.Points = await DB.TimePoints.Where(Q => Q.Score == model.Score).Select(Q => Q.Points * choises).FirstOrDefaultAsync();
            }
            return model;
        }

        public async Task<SetupCourseFormModel> GetSetupCourseById(int id)
        {
            var result = await (
                from c in this.DB.Courses
                join p in this.DB.ProgramTypes on c.ProgramTypeId equals p.ProgramTypeId
                join cp in this.DB.CoursePrerequisiteMappings on c.CourseId equals cp.PrevCourseId into lcp
                from cp in lcp.DefaultIfEmpty()
                join cl in this.DB.CourseLearningObjectives on c.CourseId equals cl.CourseId into lcl
                from cl in lcl.DefaultIfEmpty()
                where c.CourseId == id
                select new SetupCourseFormModel
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    ProgramTypeName = p.ProgramTypeName,
                    IsPopular = c.IsPopular,
                    IsRecommendedForYou = c.IsRecommendedForYou,
                    ListCourseTrainingTypeMappings = new List<CourseTrainingTypeMappingModel>(),
                    ListCourseLearningObjectives = new List<CourseLearningObjectiveModel>(),
                    ListCoursePrerequisiteMappings = new List<CoursePrerequisiteViewModel>(),
                    ListSetupModules = new List<SetupModuleFormModel>()
                }).AsNoTracking().FirstOrDefaultAsync();

            if (result == null)
            {
                return new SetupCourseFormModel();
            }

            result.ListCourseTrainingTypeMappings = await DB.CourseTrainingTypeMappings.AsNoTracking().Select(Q => new CourseTrainingTypeMappingModel
            {
                CourseId = Q.CourseId,
                MinimumScore = Q.MinimumScore,
                Percentage = Q.Percentage,
                WorkloadRequirement = Q.WorkloadRequirement,
                TrainingTypeId = Q.TrainingTypeId
            }).Where(Q => Q.CourseId == id).ToListAsync();

            result.ListCoursePrerequisiteMappings = await (
                from cp in DB.CoursePrerequisiteMappings.AsNoTracking()
                join slm in DB.SetupLearning.AsNoTracking() on cp.NextSetupModuleId equals slm.SetupModuleId into jslm
                from slm in jslm.DefaultIfEmpty()
                join slc in DB.SetupLearning.AsNoTracking() on cp.NextCourseId equals slc.CourseId into jslc
                from slc in jslc.DefaultIfEmpty()
                where cp.PrevCourseId == id
                select new CoursePrerequisiteViewModel
                {
                    NextSetupModuleId = cp.NextSetupModuleId,
                    NextCourseId = cp.NextCourseId,
                    Name = slm.LearningName ?? slc.LearningName
                }).ToListAsync();

            result.ListCourseLearningObjectives = await DB.CourseLearningObjectives.AsNoTracking().Select(Q => new CourseLearningObjectiveModel
            {
                CourseId = Q.CourseId,
                LearningObjectiveId = Q.LearningObjectiveId,
                LearningObjectiveName = Q.LearningObjectiveName
            }).Where(Q => Q.CourseId == id).ToListAsync();

            result.ListSetupModules = await (
                from SM in this.DB.SetupModules
                join TP in this.DB.TimePoints on SM.TimePointId equals TP.TimePointId
                join M in this.DB.Modules on SM.ModuleId equals M.ModuleId
                where SM.CourseId == id && SM.IsDeleted == false
                select new SetupModuleFormModel
                {
                    SetupModuleId = SM.SetupModuleId,
                    TrainingTypeId = SM.TrainingTypeId ?? default(int),
                    MinimumScore = SM.MinimumScore ?? default(int),
                    TimePointId = SM.TimePointId,
                    Score = TP.Time,
                    Points = TP.Points,
                    ModuleId = M.ModuleId,
                    ModuleName = M.ModuleName,
                    IsForRemedial = SM.IsForRemedial ?? default(bool),
                    EnumRemedialOption = SM.EnumRemedialOption,
                    EnumScoringMethod = SM.EnumScoringMethod,
                    IsByOption = SM.IsByOption,
                    RemedialLimit = SM.RemedialLimit,
                    Order = (int)SM.Order,
                    SetupTaskForm = new SetupTaskFormModel()
                }).OrderBy(Q => Q.SetupModuleId).AsNoTracking().ToListAsync();

            var listAssesment = await (
                                from SM in this.DB.SetupModules
                                join A in this.DB.Assesments on SM.AssesmentId equals A.GUID
                                // join skillCheckNav in this.DB.AssesmentSkillChecks on A.GUID equals skillCheckNav.AssesmentGUID
                                where SM.CourseId == id && SM.IsDeleted == false
                                select new SetupModuleFormModel
                                {
                                    SetupModuleId = SM.SetupModuleId,
                                    TrainingTypeId = SM.TrainingTypeId ?? default(int),
                                    MinimumScore = SM.MinimumScore ?? default(int),
                                    TimePointId = SM.TimePointId,
                                    Score = (int?)A.MinimumScore,
                                    AssesmentId = A.GUID,
                                    ModuleName = A.Name,
                                    IsForRemedial = SM.IsForRemedial ?? default(bool),
                                    EnumRemedialOption = SM.EnumRemedialOption,
                                    EnumScoringMethod = SM.EnumScoringMethod,
                                    IsByOption = SM.IsByOption,
                                    RemedialLimit = SM.RemedialLimit,
                                    Order = (int)SM.Order,
                                    SetupTaskForm = new SetupTaskFormModel()
                                }).OrderBy(Q => Q.SetupModuleId).AsNoTracking().ToListAsync();

            result.ListSetupModules.AddRange(listAssesment);

            foreach (var setupModule in result.ListSetupModules)
            {
                if (setupModule.ModuleId != null)
                {
                    setupModule.SetupTaskForm = await (from ST in this.DB.SetupTasks

                                                       join M in this.DB.Modules on ST.ModuleId equals M.ModuleId into LM
                                                       from M in LM.DefaultIfEmpty()

                                                       join C in this.DB.Competencies on ST.CompetencyId equals C.CompetencyId into LC
                                                       from C in LC.DefaultIfEmpty()

                                                       join CT in this.DB.CompetencyTypes on C.CompetencyTypeId equals CT.CompetencyTypeId into LCT
                                                       from CT in LCT.DefaultIfEmpty()

                                                       where ST.SetupModuleId == setupModule.SetupModuleId

                                                       select new SetupTaskFormModel
                                                       {
                                                           SetupTaskId = ST.SetupTaskId,
                                                           TestTime = ST.TestTime,
                                                           IsGrouping = ST.IsGrouping,
                                                           CompetencyId = C.CompetencyId,
                                                           CompetencyTypeId = C.CompetencyTypeId,
                                                           CompetencyTypeName = CT.CompetencyTypeName,
                                                           PrefixCode = C.PrefixCode,
                                                           CompetencyNameMapping = ST.CompetencyId != null ? CT.CompetencyTypeName.Substring(0, 1) + "-" + C.PrefixCode : "",
                                                           ModuleId = M.ModuleId,
                                                           ModuleName = ST.ModuleId != null ? M.ModuleName : "",
                                                           ModuleDescription = M.ModuleDescription,
                                                           TotalParticipant = ST.TotalParticipant,
                                                           TotalQuestion = ST.TotalQuestion,
                                                           QuestionPerParticipant = ST.QuestionPerParticipant,
                                                           TaskList = new List<SetupTaskCodesFormModel>(),
                                                       }).AsNoTracking().FirstOrDefaultAsync();

                    if (setupModule.SetupTaskForm == null)
                    {
                        setupModule.SetupTaskForm = new SetupTaskFormModel();
                    }

                    setupModule.SetupTaskForm.TaskList = await (from STC in this.DB.SetupTaskCodes

                                                                join T in this.DB.Tasks on STC.TaskId equals T.TaskId into lt
                                                                from T in lt.DefaultIfEmpty()
                                                                join Q in this.DB.QuestionTypes on T.QuestionTypeId equals Q.QuestionTypeId into lq
                                                                from Q in lq.DefaultIfEmpty()
                                                                join M in this.DB.Modules on T.ModuleId equals M.ModuleId
                                                                join C in this.DB.Competencies on T.CompetencyId equals C.CompetencyId into lc
                                                                from C in lc.DefaultIfEmpty()
                                                                join CT in this.DB.CompetencyTypes on C.CompetencyTypeId equals CT.CompetencyTypeId into lct
                                                                from CT in lct.DefaultIfEmpty()
                                                                join ET in this.DB.EvaluationTypes on T.EvaluationTypeId equals ET.EvaluationTypeId into ljet
                                                                from ET in ljet.DefaultIfEmpty()

                                                                where STC.SetupTaskId == setupModule.SetupTaskForm.SetupTaskId

                                                                select new SetupTaskCodesFormModel
                                                                {
                                                                    SetupTaskId = STC.SetupTaskId,
                                                                    TaskId = STC.TaskId,
                                                                    QuestionNumber = STC.QuestionNumber,
                                                                    TaskName = CT.CompetencyTypeName.Substring(0, 1) + "-" + C.PrefixCode + "-" + ET.EvaluationTypeName + "-" + T.TaskNumber + "-" + M.ModuleName,
                                                                    TaskCode = CT.CompetencyTypeName.Substring(0, 1) + "-" + C.PrefixCode + "-" + ET.EvaluationTypeName + "-" + T.TaskNumber + "-" + M.ModuleName,
                                                                    TaskTypeId = T.QuestionTypeId
                                                                }).AsNoTracking().ToListAsync();

                    for (var i = 0; i < setupModule.SetupTaskForm.TaskList.Count(); i++)
                    {
                        if (setupModule.SetupTaskForm.TaskList[i].TaskId == 0) continue;
                        var scorePoints = await GetScorePoints(setupModule.SetupTaskForm.TaskList[i].TaskTypeId ?? default(int), setupModule.SetupTaskForm.TaskList[i].TaskId ?? default(int));
                        setupModule.SetupTaskForm.TaskList[i].Score = scorePoints.Score;
                        setupModule.SetupTaskForm.TaskList[i].Points = scorePoints.Points;
                    }
                }
                else
                {
                    var asssesmentLists = await DB.Assesments
                      .Include(x => x.AssesmentSkillChecksNavigation)
                      .ThenInclude(x => x.SkillChecksNavigator)
                      .Where(x => x.GUID == setupModule.AssesmentId)
                      .Select(x => new ResponseAssesmentModel
                      {
                          GUID = x.GUID,
                          Name = x.Name,
                          EnumRemedialOption = x.EnumRemedialOption,
                          EnumScoringMethod = x.EnumScoringMethod,
                          LearningTime = x.LearningTime,
                          EnumTrainingType = x.TrainingTypeID,
                          IsByOption = x.IsByOption,
                          RemedialLimit = x.RemedialLimit,
                          SkillCheck = x.AssesmentSkillChecksNavigation.ToList()
                      }).ToListAsync();
                    setupModule.AssesmentList = asssesmentLists;
                }
            }

            return result;
        }

        public async Task<int> CreateSetupCourse(SetupCourseFormModel model)
        {
            using (var transaction = await DB.Database.BeginTransactionAsync())
            {
                var updateCourseModel = await this.DB.Courses.FindAsync(model.CourseId);

                updateCourseModel.IsRecommendedForYou = model.IsRecommendedForYou;
                updateCourseModel.IsPopular = model.IsPopular;
                updateCourseModel.SetupCourseCreatedBy = updateCourseModel.SetupCourseCreatedBy ?? GetUserLogin();
                updateCourseModel.SetupCourseCreatedAt = updateCourseModel.SetupCourseCreatedAt ?? DateTime.UtcNow.ToIndonesiaTimeZone();
                updateCourseModel.SetupCourseUpdatedBy = GetUserLogin();
                updateCourseModel.SetupCourseUpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                updateCourseModel.SetupCourseApprovedAt = null;

                // delete release training

                var trainingToDelete = await this.DB.Trainings.AsNoTracking().Where(Q => Q.CourseId == model.CourseId).ToListAsync();

                for (int i = 0; i < trainingToDelete.Count(); i++)
                {
                    var approvalId3 = await this.DB.ApprovalToTrainings.Where(Q => Q.TrainingId == trainingToDelete[i].TrainingId).Select(Q => Q.ApprovalId).FirstOrDefaultAsync();

                    if (approvalId3 != 0)
                    {
                        var deleteModel2 = await this.DB.ApprovalToTrainings.FindAsync(approvalId3);
                        this.DB.ApprovalToTrainings.Remove(deleteModel2);

                        var deleteModel3 = await this.DB.Approvals.FindAsync(approvalId3);
                        this.DB.Approvals.Remove(deleteModel3);
                    }

                    var update = await this.DB.Trainings.FindAsync(trainingToDelete[i].TrainingId);
                    update.IsDeleted = true;
                    update.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    update.UpdatedBy = GetUserLogin();
                }

                await this.DB.SaveChangesAsync();

                // delete & insert course training type mapping

                var deleteCourseTrainingTypes = await DB.CourseTrainingTypeMappings.Where(Q => Q.CourseId == model.CourseId).ToAsyncEnumerable().ToList();
                if (deleteCourseTrainingTypes != null)
                {
                    DB.CourseTrainingTypeMappings.RemoveRange(deleteCourseTrainingTypes);
                }
                await DB.SaveChangesAsync();

                List<CourseTrainingTypeMappings> addCourseTrainingTypes = new List<CourseTrainingTypeMappings>();
                foreach (CourseTrainingTypeMappingModel addModel in model.ListCourseTrainingTypeMappings)
                {
                    addCourseTrainingTypes.Add(new CourseTrainingTypeMappings
                    {
                        CourseId = addModel.CourseId,
                        TrainingTypeId = addModel.TrainingTypeId,
                        MinimumScore = addModel.MinimumScore,
                        WorkloadRequirement = addModel.WorkloadRequirement,
                        Percentage = addModel.Percentage
                    });
                }
                this.DB.CourseTrainingTypeMappings.AddRange(addCourseTrainingTypes);
                await this.DB.SaveChangesAsync();

                // delete & insert course prerequisites

                var deleteCoursePrerequisites = await DB.CoursePrerequisiteMappings.Where(Q => Q.PrevCourseId == model.CourseId).ToAsyncEnumerable().ToList();
                if (deleteCoursePrerequisites != null)
                {
                    DB.CoursePrerequisiteMappings.RemoveRange(deleteCoursePrerequisites);
                }
                await DB.SaveChangesAsync();

                List<CoursePrerequisiteMappings> addCoursePrerequisites = new List<CoursePrerequisiteMappings>();
                foreach (CoursePrerequisiteViewModel addModel in model.ListCoursePrerequisiteMappings)
                {
                    addCoursePrerequisites.Add(new CoursePrerequisiteMappings
                    {
                        PrevCourseId = model.CourseId,
                        NextCourseId = addModel.NextCourseId,
                        NextSetupModuleId = addModel.NextSetupModuleId
                    });
                }
                this.DB.CoursePrerequisiteMappings.AddRange(addCoursePrerequisites);
                await this.DB.SaveChangesAsync();

                // delete & insert course learning objectives

                var deleteCourseLearningObjectives = await DB.CourseLearningObjectives.Where(Q => Q.CourseId == model.CourseId).ToAsyncEnumerable().ToList();
                if (deleteCourseLearningObjectives != null)
                {
                    DB.CourseLearningObjectives.RemoveRange(deleteCourseLearningObjectives);
                }
                //await DB.SaveChangesAsync();

                List<CourseLearningObjectives> addCourseLearningObjectives = new List<CourseLearningObjectives>();
                foreach (CourseLearningObjectiveModel addModel in model.ListCourseLearningObjectives)
                {
                    addCourseLearningObjectives.Add(new CourseLearningObjectives
                    {
                        CourseId = addModel.CourseId,
                        LearningObjectiveName = addModel.LearningObjectiveName
                    });
                }
                this.DB.CourseLearningObjectives.AddRange(addCourseLearningObjectives);
                await this.DB.SaveChangesAsync();

                // delete & insert course's setup module


                var deleteSetupModuleId = await DB.SetupModules.Where(Q => Q.CourseId == model.CourseId).Select(Q => Q.SetupModuleId).ToAsyncEnumerable().ToList();

                var deleteSetupModules = await DB.SetupModules.Where(Q => Q.CourseId == model.CourseId).ToAsyncEnumerable().ToList();
                foreach (var del in deleteSetupModules)
                {
                    del.IsDeleted = true;
                }
                await DB.SaveChangesAsync();

                List<SetupModuleFormModel> addSetupModules = new List<SetupModuleFormModel>();
                foreach (SetupModuleFormModel addModel in model.ListSetupModules)
                {

                    if (String.IsNullOrWhiteSpace(addModel.AssesmentId))
                    {
                        addModel.AssesmentId = null;
                    }

                    var newSetupModule = new SetupModules
                    {

                        CourseId = addModel.CourseId,
                        IsRecommendedForYou = addModel.IsRecommendedForYou.GetValueOrDefault(),
                        IsPopular = addModel.IsPopular.GetValueOrDefault(),
                        MinimumScore = addModel.MinimumScore,
                        TrainingTypeId = addModel.TrainingTypeId,
                        IsForRemedial = addModel.IsForRemedial,
                        TimePointId = addModel.TimePointId,
                        ModuleId = addModel.ModuleId,
                        AssesmentId = addModel.AssesmentId,
                        Order = addModel.Order,
                        EnumRemedialOption = addModel.EnumRemedialOption,
                        EnumScoringMethod = addModel.EnumScoringMethod,
                        IsByOption = addModel.IsByOption,
                        RemedialLimit = addModel.RemedialLimit,
                        EnumCategoryPreDuringPost = addModel.EnumCategoryPreDuringPost,

                    };

                    this.DB.SetupModules.Add(newSetupModule);
                    await this.DB.SaveChangesAsync();

                    if (addModel.SetupTaskForm != null)
                    {
                        var newInsertSetupTask = new SetupTasks
                        {
                            TestTime = addModel.SetupTaskForm.TestTime.GetValueOrDefault(),
                            IsGrouping = addModel.SetupTaskForm.IsGrouping.GetValueOrDefault(),
                            CompetencyId = addModel.SetupTaskForm.CompetencyId,
                            ModuleId = addModel.SetupTaskForm.ModuleId,
                            TotalParticipant = addModel.SetupTaskForm.TotalParticipant,
                            TotalQuestion = addModel.SetupTaskForm.TotalQuestion,
                            SetupModuleId = newSetupModule.SetupModuleId,
                            QuestionPerParticipant = addModel.SetupTaskForm.QuestionPerParticipant
                        };

                        this.DB.SetupTasks.Add(newInsertSetupTask);
                        this.DB.SaveChanges();

                        var listInsertTaskCode = new List<SetupTaskCodes>();

                        if (addModel.SetupTaskForm.TaskList != null && addModel.SetupTaskForm.TaskList.Count() != 0)
                        {
                            for (int a = 0; a < addModel.SetupTaskForm.TaskList.Count; a++)
                            {
                                listInsertTaskCode.Add(new SetupTaskCodes
                                {
                                    SetupTaskId = newInsertSetupTask.SetupTaskId,
                                    TaskId = addModel.SetupTaskForm.TaskList[a].TaskId.GetValueOrDefault(),
                                    QuestionNumber = a + 1
                                });
                            }
                            await this.DB.SetupTaskCodes.AddRangeAsync(listInsertTaskCode);
                        }

                        await this.DB.SaveChangesAsync();
                    }

                }


                //Checking if setup learning already exist
                var setupLearning = await DB.SetupLearning.Where(Q => Q.CourseId == model.CourseId).ToAsyncEnumerable().FirstOrDefault();

                if (setupLearning == null)
                {
                    if (model.CourseId != 0)
                    {
                        setupLearning = new SetupLearning
                        {
                            CourseId = model.CourseId,
                            AssesmentId = null,
                            LearningName = model.CourseName,
                            LearningCategoryName = LearningCategoryNameEnum.Course,
                            ProgramTypeName = model.ProgramTypeName,
                            CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                            ApprovalStatus = model.ApprovalId == ApprovalStatusesEnum.DraftId ? ApprovalStatusesEnum.Draft : ApprovalStatusesEnum.Approve,
                            Order = model.Order,

                        };

                        DB.SetupLearning.Add(setupLearning);
                    }
                    else
                    {
                        setupLearning = new SetupLearning
                        {
                            AssesmentId = model.AssesmentId,
                            CourseId = null,
                            LearningName = model.AssesemntName,
                            LearningCategoryName = LearningCategoryNameEnum.Assesment,
                            ProgramTypeName = model.ProgramTypeName,
                            CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                            ApprovalStatus = model.ApprovalId == ApprovalStatusesEnum.DraftId ? ApprovalStatusesEnum.Draft : ApprovalStatusesEnum.Approve,
                            Order = model.Order,
                        };
                        DB.SetupLearning.Add(setupLearning);
                    }
                }
                else
                {
                    if (model.CourseId != 0)
                    {
                        setupLearning.CourseId = model.CourseId;
                        setupLearning.LearningCategoryName = LearningCategoryNameEnum.Course;
                        setupLearning.ProgramTypeName = model.ProgramTypeName;
                        setupLearning.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                        setupLearning.Order = model.Order;
                    }
                    else
                    {
                        setupLearning.AssesmentId = model.AssesmentId;
                        setupLearning.LearningCategoryName = LearningCategoryNameEnum.Assesment;
                        setupLearning.ProgramTypeName = model.ProgramTypeName;
                        setupLearning.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                        setupLearning.Order = model.Order;

                    }
                }

                var approvalId = await this.DB.ApprovalToSetupCourses.Where(Q => Q.CourseId == model.CourseId).Select(Q => Q.ApprovalId).FirstOrDefaultAsync();

                //Approval does not exist -> Create new Approval
                if (approvalId == 0)
                {
                    var newApproval = new ApprovalCreateFormModel
                    {
                        ApprovalStatusId = model.ApprovalId == ApprovalStatusesEnum.DraftId ? ApprovalStatusesEnum.DraftId : ApprovalStatusesEnum.ApproveId,
                        ContentCategory = ContentCategoryEnums.SetupCourse,
                        ContentId = model.CourseId,
                        ContentName = model.CourseName,
                        PageEnum = PageEnums.SetupCourse
                    };

                    var approvals = await this.ApprovalMan.CreateNewApproval(newApproval);

                    setupLearning.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();

                    if (approvals.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
                    {
                        updateCourseModel.SetupCourseApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                        setupLearning.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                        setupLearning.ApprovalStatus = ApprovalStatusesEnum.Approve;
                    }
                    else
                    {
                        if (approvals.ApprovalStatusId == ApprovalStatusesEnum.WaitingId)
                        {
                            setupLearning.ApprovalStatus = ApprovalStatusesEnum.Waiting;
                        }
                        else
                        {
                            setupLearning.ApprovalStatus = ApprovalStatusesEnum.Draft;
                        }
                    }
                }
                else
                {
                    var updateApproval = new ApprovalUpdateFormModel
                    {
                        ApprovalId = approvalId,
                        ApprovalStatusId = model.ApprovalId == ApprovalStatusesEnum.DraftId ? ApprovalStatusesEnum.DraftId : ApprovalStatusesEnum.ApproveId,
                        ContentName = model.CourseName,
                        PageEnum = PageEnums.SetupCourse,
                        ContentId = model.CourseId,
                        ContentCategory = ContentCategoryEnums.SetupCourse
                    };


                    var approvals = await this.ApprovalMan.UpdateNewApproval(updateApproval);

                    if (approvals.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
                    {
                        updateCourseModel.SetupCourseApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                        setupLearning.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                        setupLearning.ApprovalStatus = ApprovalStatusesEnum.Approve;
                    }
                    else
                    {
                        if (approvals.ApprovalStatusId == ApprovalStatusesEnum.WaitingId)
                        {
                            setupLearning.ApprovalStatus = ApprovalStatusesEnum.Waiting;
                        }
                        else
                        {
                            setupLearning.ApprovalStatus = ApprovalStatusesEnum.Draft;
                        }
                    }
                }

                //check training certification mappings
                var getTrainingList = await this.DB.Trainings.Where(Q => Q.CourseId == model.CourseId).ToListAsync();

                foreach (var item in getTrainingList)
                {
                    var deleteTrainingCertificationMapping = await this.DB.TrainingCertifications.Where(q => q.TrainingId == item.TrainingId || q.RelatedTrainingId == item.TrainingId).FirstOrDefaultAsync();
                    if (deleteTrainingCertificationMapping != null)
                    {
                        DB.TrainingCertifications.RemoveRange(deleteTrainingCertificationMapping);
                    }
                }

                await this.DB.SaveChangesAsync();

                transaction.Commit();

                return model.CourseId;
            }
        }


        public async Task<ScorePointsModel> GetApiScorePoints(TaskScorePointFilteredModel req)
        {
            var model = new ScorePointsModel();
            var taskTypeId = req.TaskTypeId;
            var taskId = req.TaskId;
            //1   True / False
            if (taskTypeId == 1)
            {
                var timePointId = await DB.TaskTrueFalseTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).FirstOrDefaultAsync();
                model.Score = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Score).FirstOrDefaultAsync() ?? default(int);
                model.Points = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            //2   Matching
            if (taskTypeId == 2)
            {
                var timePointId = await DB.TaskMatchingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).FirstOrDefaultAsync();
                model.Score = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Score).FirstOrDefaultAsync() ?? default(int);
                model.Points = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            //3   Sequence
            if (taskTypeId == 3)
            {
                var timePointIds = await DB.TaskSequenceChoices.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).ToListAsync();
                //var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Score).ToListAsync();
                var scores = await (from tp in DB.TimePoints
                                    join tsc in DB.TaskSequenceChoices on tp.TimePointId equals tsc.Score
                                    where tsc.TaskId == taskId
                                    select tp.Score).ToListAsync();
                model.Score = scores.Sum() ?? default(int);
                var points = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Points).ToListAsync();
                model.Points = points.Sum();
            }
            //4   Tebak Gambar
            if (taskTypeId == 4)
            {
                var timePointId = await DB.TaskTebakGambarTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).FirstOrDefaultAsync();
                model.Score = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Score).FirstOrDefaultAsync() ?? default(int);
                model.Points = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            //5   Hot Spot
            if (taskTypeId == 5)
            {
                var timePointIds = await DB.TaskHotSpotAnswers.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).ToListAsync();
                var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Score).ToListAsync();
                model.Score = scores.Sum() ?? default(int);
                var points = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Points).ToListAsync();
                model.Points = points.Sum();
            }
            //6   Short Answer
            //7   Open Question/ Essay
            //11  File Upload
            if (taskTypeId == 6 || taskTypeId == 7 || taskTypeId == 11)
            {
                model.Score = 100;
                model.Points = 0;
            }
            //8   Checklist
            if (taskTypeId == 8)
            {
                var timePointIds = await DB.TaskChecklistChoices.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).ToListAsync();
                //var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Score).ToListAsync();
                //model.Score = scores.Sum() ?? default(int);
                model.Score = timePointIds.Sum();
                var points = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Points).ToListAsync();
                model.Points = points.Sum();
            }
            //9   Rating
            if (taskTypeId == 9)
            {
                var timePointIds = new List<int>
                    {
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score0To20).FirstOrDefaultAsync(),
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score21To40).FirstOrDefaultAsync(),
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score41To60).FirstOrDefaultAsync(),
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score61To80).FirstOrDefaultAsync(),
                        await DB.TaskRatingTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score81To100).FirstOrDefaultAsync()
                    };
                var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => new { Score = Q.Score, Point = Q.Points }).ToListAsync();
                var choises = await DB.TaskRatingChoices.Where(Q => Q.TaskId == taskId).CountAsync();
                model.Score = scores.Max(q => q.Score) * choises ?? default(int);
                model.Points = scores.Max(q => q.Point) * choises;
                //model.Points = await DB.TimePoints.Where(Q => Q.Score == scores.Max()).Select(Q => Q.Points * choises).FirstOrDefaultAsync();
            }
            //10  Multiple Choice
            if (taskTypeId == 10)
            {
                var timePointId = await DB.TaskMultipleChoiceTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.Score).FirstOrDefaultAsync();
                model.Score = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Score).FirstOrDefaultAsync() ?? default(int);
                model.Points = await DB.TimePoints.Where(Q => Q.TimePointId == timePointId).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            //12  Matrix
            if (taskTypeId == 12)
            {
                var timePointIds = new List<int>
                    {
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn1).FirstOrDefaultAsync(),
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn2).FirstOrDefaultAsync(),
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn3).FirstOrDefaultAsync(),
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn4).FirstOrDefaultAsync(),
                        await DB.TaskMatrixTypes.Where(Q => Q.TaskId == taskId).Select(Q => Q.ScoreColumn5).FirstOrDefaultAsync()
                    };
                //var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => Q.Score).ToListAsync();
                var scores = await DB.TimePoints.Where(Q => timePointIds.Contains(Q.TimePointId)).Select(Q => new { Score = Q.Score, Point = Q.Points }).ToListAsync();
                var choises = await DB.TaskMatrixQuestions.Where(Q => Q.TaskId == taskId).CountAsync();
                model.Score = scores.Max(q => q.Score) * choises ?? default(int);
                model.Points = scores.Max(q => q.Point) * choises;
                //model.Score = scores.Max() * choises ?? default(int);
                //model.Points = await DB.TimePoints.Where(Q => Q.Score == model.Score).Select(Q => Q.Points).FirstOrDefaultAsync();
            }
            return model;
        }

    }
}
