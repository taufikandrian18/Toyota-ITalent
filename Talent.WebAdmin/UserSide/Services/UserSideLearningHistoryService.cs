using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideLearningHistoryService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;

        public UserSideLearningHistoryService(TalentContext talentContext, IFileStorageService fileService)
        {
            this.DB = talentContext;
            this.FileService = fileService;
        }

        public async Task<UserSideLearningHistoryResponseModel> GetLearningHistory(UserSideLearningHistoryFilterModel filter, string employeeId)
        {
            var queryLearning = this.DB.GetAllLearningHistories()
                                        .Where(Q => Q.EmployeeId.ToLower() == employeeId.ToLower())
                                        .OrderByDescending(Q => Q.CreatedAt)
                                        .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                queryLearning = queryLearning.Where(Q => Q.Name.ToLower().Contains(filter.Keyword.ToLower()));
            }

            if (filter.LearningTypeIds != null)
            {
                queryLearning = queryLearning.Where(Q => filter.LearningTypeIds.Contains(Q.LearningTypeId));
            }

            if (filter.MaterialTypeIds != null)
            {
                var satu = filter.MaterialTypeIds[0].ToString();
                var dua = filter.MaterialTypeIds.Count >= 2 ? filter.MaterialTypeIds[1].ToString() : satu;
                var tiga = filter.MaterialTypeIds.Count >= 3 ? filter.MaterialTypeIds[2].ToString() : satu;

                queryLearning = queryLearning.Where(Q => Q.MaterialTypeId.Contains(satu) || Q.MaterialTypeId.Contains(dua) || Q.MaterialTypeId.Contains(tiga));
            }

            if (filter.ProgramTypeIds != null)
            {
                queryLearning = queryLearning.Where(Q => filter.ProgramTypeIds.Contains(Q.ProgramTypeId));
            }

            if (filter.IsFree || filter.IsNotFree)
            {
                if (filter.IsFree && !filter.IsNotFree)
                {
                    queryLearning = queryLearning.Where(Q => Q.CoursePrice == null || Q.CoursePrice.Value == 0);
                }
                else if (!filter.IsFree && filter.IsNotFree)
                {
                    queryLearning = queryLearning.Where(Q => Q.CoursePrice != null && Q.CoursePrice.Value != 0);
                }
            }

            var totalData = await queryLearning.CountAsync();

            if (totalData <= filter.PageSize)
            {
                filter.PageIndex = 1;
            };

            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            var learnings = await queryLearning.Select(Q => new UserSideLearningHistoryModel
            {
                CourseId = Q.CourseId.HasValue ? Q.CourseId.Value : 0,
                TrainingId = Q.TrainingId.HasValue ? Q.TrainingId.Value : 0,
                LearningHistoryId = Q.LearningHistoryId,
                LearningHistoryName = Q.Name,
                SetupModuleId = Q.SetupModuleId.HasValue ? Q.SetupModuleId.Value : 0,
                CourseName = Q.CourseName,
                RatingScore = Q.RatingScore.HasValue ? Q.RatingScore.Value / (double)2 : 0,
                CreatedAt = Q.CreatedAt,
                ImageUrl = GenerateImageUrl(Q.BlobId, Q.Mime).Result,
                SetupCourseApprovedAt = Q.SetupCourseApprovedAt
            }).Skip((int)skipCount)
            .Take(filter.PageSize)
            .ToListAsync();

            var responseLearnings = new UserSideLearningHistoryResponseModel
            {
                EmployeeId = employeeId,
                Learnings = learnings,
                TotalData = totalData
            };

            return responseLearnings;
        }
        public async Task<UserSideLearningHistoryResponseModel> GetCompleteLearningHistory(UserSideLearningHistoryFilterModel filter, string employeeId)
        {

            var releaseTraining = await this.DB.Trainings
                .Where(Q => Q.IsDeleted == false && Q.ApprovedAt != null)
                .Select(Q => Q.CourseId).ToListAsync();

            var releaseLearning = await this.DB.SetupLearning
                .Where(Q => Q.ApprovalStatus.ToLower() == WebAdmin.Enums.ApprovalStatusesEnum.Approve.ToLower())
                //.Where(Q => Q.ApprovalStatus.ToLower() == "approved")
                .Select(Q => Q.CourseId.GetValueOrDefault()).ToListAsync();

            var tempList = new List<int>();
            tempList.AddRange(releaseTraining);
            tempList.AddRange(releaseLearning);

            var queryLearning = await (from c in DB.Courses

                                       join sm in DB.SetupModules on c.CourseId equals sm.CourseId

                                       join m in DB.Modules on sm.ModuleId equals m.ModuleId

                                       join pt in DB.ProgramTypes on c.ProgramTypeId equals pt.ProgramTypeId
                                       join b in DB.Blobs on c.BlobId equals b.BlobId

                                       join t in DB.Trainings on c.CourseId equals t.CourseId

                                       join tr in DB.TrainingRatings on t.TrainingId equals tr.TrainingId into ctr
                                       from tr in ctr.DefaultIfEmpty()

                                       join el in DB.EnrollLearnings on t.TrainingId equals el.TrainingId

                                       join elt in DB.EnrollLearningTimes on el.EnrollLearningId equals elt.EnrollLearningId into elelt
                                       from elt in elelt.DefaultIfEmpty()

                                       join e in DB.Employees on el.EmployeeId equals e.EmployeeId

                                       join atc in DB.ApprovalToCourses on c.CourseId equals atc.CourseId

                                       join a in DB.Approvals on atc.ApprovalId equals a.ApprovalId

                                       where a.ApprovalStatusId == WebAdmin.Enums.ApprovalStatusesEnum.ApproveId && tempList.Contains(c.CourseId) && elt.StartTime != null && e.EmployeeId.ToLower() == employeeId.ToLower()
                                       orderby t.ApprovedAt descending
                                       select new
                                       {
                                           t.TrainingId,
                                           t.Batch,
                                           t.StartDate,
                                           t.EndDate,
                                           RatingScore = tr.RatingScore == null ? 0 : tr.RatingScore,
                                           c.CourseId,
                                           c.CourseName,
                                           pt.ProgramTypeName,
                                           c.BlobId,
                                           b.Mime,
                                           c.CreatedAt,
                                           c.UpdatedAt,
                                           c.LearningTypeId,
                                           m.MaterialTypeId,
                                           c.ProgramTypeId,
                                           c.CoursePrice,
                                           sm.SetupModuleId

                                       }).AsNoTracking()
                                  .ToListAsync();

            //var totalData = query2.Count();

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                queryLearning = queryLearning.Where(Q => Q.CourseName != null).ToList();
                if (queryLearning.Count() > 0)
                {
                    queryLearning = queryLearning.Where(Q => Q.CourseName.ToLower().Contains(filter.Keyword.ToLower()) == true).ToList();
                }
            }

            if (filter.LearningTypeIds != null)
            {
                filter.LearningTypeIds = filter.LearningTypeIds.Where(Q => Q != null).ToList();
                if (filter.LearningTypeIds.Count() > 0)
                {
                    queryLearning = queryLearning.Where(Q => filter.LearningTypeIds.Contains(Q.LearningTypeId)).ToList();
                }
            }

            if (filter.MaterialTypeIds != null)
            {
                filter.MaterialTypeIds = filter.MaterialTypeIds.Where(Q => Q != null).ToList();
                if (filter.MaterialTypeIds.Count() > 0)
                {
                    queryLearning = queryLearning.Where(Q => filter.MaterialTypeIds.Contains(Q.MaterialTypeId)).ToList();
                }
            }

            if (filter.ProgramTypeIds != null)
            {
                filter.ProgramTypeIds = filter.ProgramTypeIds.Where(Q => Q != null).ToList();
                if (filter.ProgramTypeIds.Count() > 0)
                {
                    queryLearning = queryLearning.Where(Q => filter.ProgramTypeIds.Contains(Q.ProgramTypeId)).ToList();
                }
            }

            if (filter.IsFree || filter.IsNotFree)
            {
                if (filter.IsFree && !filter.IsNotFree)
                {
                    queryLearning = queryLearning.Where(Q => Q.CoursePrice == null || Q.CoursePrice.Value == 0).ToList();
                }
                else if (!filter.IsFree && filter.IsNotFree)
                {
                    queryLearning = queryLearning.Where(Q => Q.CoursePrice != null && Q.CoursePrice.Value != 0).ToList();
                }
            }

            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);


            var learnings = queryLearning
                .GroupBy(Q => Q.TrainingId)
            .Select(async X => new UserSideLearningHistoryModel
            {
                CourseId = X.First().CourseId,
                SetupModuleId = X.First().SetupModuleId,
                CourseName = X.First().CourseName,
                RatingScore = X.First().RatingScore,
                SetupCourseApprovedAt = X.First().CreatedAt,
                ImageUrl = X.First().BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(X.First().BlobId.ToString(), X.First().Mime)
            })
            .Select(Q => Q.Result)
            .ToList();

            var totalData = learnings.Count();

            if (totalData <= filter.PageSize)
            {
                filter.PageIndex = 1;
            };

            learnings = learnings
                .Skip((int)skipCount)
                .Take(filter.PageSize)
                .ToList();

            var responseLearnings = new UserSideLearningHistoryResponseModel
            {
                EmployeeId = employeeId,
                Learnings = learnings,
                TotalData = totalData
            };

            return responseLearnings;
        }


        public async Task<UserSideDetailLearningHistoryModel> GetDetailLearningHistory(string employeeid, int learningHistoryId)
        {

            var trainingId = await this.DB.LearningHistories.Where(Q => Q.LearningHistoryId == learningHistoryId).Select(Q => Q.TrainingId).FirstOrDefaultAsync();

            var allAnswer = await (from t in this.DB.Trainings
                                   join c in this.DB.Courses on t.CourseId equals c.CourseId
                                   join ta in this.DB.TaskAnswers on t.TrainingId equals ta.TrainingId into ps
                                   from ta in ps.DefaultIfEmpty()
                                   join sm in this.DB.SetupModules on ta.SetupModuleId equals sm.SetupModuleId into ps2
                                   from sm in ps2.DefaultIfEmpty()
                                   join m in this.DB.Modules on sm.ModuleId equals m.ModuleId into ps3
                                   from m in ps3.DefaultIfEmpty()
                                   join tad in this.DB.TaskAnswerDetails on ta.TaskAnswerId equals tad.TaskAnswerId into ps4
                                   from tad in ps4.DefaultIfEmpty()
                                   join tas in this.DB.Tasks on tad.TaskId equals tas.TaskId into ps5
                                   from tas in ps5.DefaultIfEmpty()
                                   select new
                                   {
                                       t.TrainingId,
                                       c.CourseId,
                                       c.CourseName,
                                       ta.SetupModuleId,
                                       m.ModuleId,
                                       m.ModuleName,
                                       ta.EmployeeId,
                                       tas.TaskId,
                                       tad.Score,
                                       tas.EvaluationTypeId
                                   }).Where(Q => Q.TrainingId == trainingId && Q.EmployeeId.ToLower() == employeeid.ToLower()).ToListAsync();

            var result = new UserSideDetailLearningHistoryModel{};

            if (allAnswer.Count() != 0 ){
                result.CourseId = allAnswer.FirstOrDefault().CourseId;
                result.CourseName = allAnswer.FirstOrDefault().CourseName;
                result.EmployeeId = allAnswer.FirstOrDefault().EmployeeId;
            }

            var taskIds = allAnswer.Select(Q => Q.TaskId).Distinct().ToList();
            var taskScore = await this.DB.GetAllTaskScore().Where(Q => taskIds.Contains(Q.TaskId)).ToListAsync();

            var listModule = new List<UserSideModuleLearningHistoryModel>();

            var moduleIds = allAnswer.Select(Q => Q.ModuleId).Distinct().ToList();

            foreach (var moduleId in moduleIds)
            {
                var moduleAnswer = allAnswer.Where(Q => Q.ModuleId == moduleId).ToList();

                var module = new UserSideModuleLearningHistoryModel
                {
                    ModuleId = moduleAnswer.FirstOrDefault().ModuleId,
                    ModuleName = moduleAnswer.FirstOrDefault().ModuleName
                };

                var listEvaluation = new List<UserSideEvaluationLearningHistoryModel>();

                var knowledgeTaskAnswer = moduleAnswer.Where(Q => Q.EvaluationTypeId == EvaluationTypeEnum.KnowledgeId).ToList();
                var knowledgeTaskIds = knowledgeTaskAnswer.Select(Q => Q.TaskId).ToList();
                var knowledgeScoreEarned = knowledgeTaskAnswer.Sum(Q => Q.Score);
                var knowledgeTask = taskScore.Where(Q => knowledgeTaskIds.Contains(Q.TaskId)).ToList();
                var knowledgeMaxScore = knowledgeTask.Sum(Q => Q.Score);
                float knowledgeEvaluationScore = (float)knowledgeScoreEarned / (float)knowledgeMaxScore;

                var knowledgeEvaluation = new UserSideEvaluationLearningHistoryModel
                {
                    EvaluationTypeName = EvaluationTypeEnum.Knowledge,
                    EvaluationTypeId = EvaluationTypeEnum.KnowledgeId,
                    EvaluationScore = float.IsNaN(knowledgeEvaluationScore) ? 0 : knowledgeEvaluationScore
                };

                var skillTaskAnswer = moduleAnswer.Where(Q => Q.EvaluationTypeId == EvaluationTypeEnum.SkillId).ToList();
                var skillTaskIds = skillTaskAnswer.Select(Q => Q.TaskId).ToList();
                var skillScoreEarned = skillTaskAnswer.Sum(Q => Q.Score);
                var skillTask = taskScore.Where(Q => skillTaskIds.Contains(Q.TaskId)).ToList();
                var skillMaxScore = skillTask.Sum(Q => Q.Score);
                float skillEvaluationScore = (float)skillScoreEarned / (float)skillMaxScore;

                var skillEvaluation = new UserSideEvaluationLearningHistoryModel
                {
                    EvaluationTypeName = EvaluationTypeEnum.Skill,
                    EvaluationTypeId = EvaluationTypeEnum.SkillId,
                    EvaluationScore = float.IsNaN(skillEvaluationScore) ? 0 : skillEvaluationScore
                };

                var behaviourTaskAnswer = moduleAnswer.Where(Q => Q.EvaluationTypeId == EvaluationTypeEnum.BehaviorId).ToList();
                var behaviourTaskIds = behaviourTaskAnswer.Select(Q => Q.TaskId).ToList();
                var behaviourScoreEarned = behaviourTaskAnswer.Sum(Q => Q.Score);
                var behaviourTask = taskScore.Where(Q => behaviourTaskIds.Contains(Q.TaskId)).ToList();
                var behaviourMaxScore = behaviourTask.Sum(Q => Q.Score);
                float beheaviourEvaluationScore = (float)behaviourScoreEarned / (float)behaviourMaxScore;

                var behaviourEvaluation = new UserSideEvaluationLearningHistoryModel
                {
                    EvaluationTypeName = EvaluationTypeEnum.Behavior,
                    EvaluationTypeId = EvaluationTypeEnum.BehaviorId,
                    EvaluationScore = float.IsNaN(beheaviourEvaluationScore) ? 0 : beheaviourEvaluationScore
                };

                listEvaluation.Add(knowledgeEvaluation);
                listEvaluation.Add(skillEvaluation);
                listEvaluation.Add(behaviourEvaluation);

                module.Evaluations = listEvaluation;
                listModule.Add(module);
            }

            result.Modules = listModule;

            return result;
        }

        public async Task<UserSideLearningHistoryFilterValueModel> GetlearningHistoryFilterValue()
        {
            var dataProgramType = await this.DB
                .ProgramTypes
                .Select(Q => new UserSideLearningHistoryFilterValueTypeModel
                {
                    Id = Q.ProgramTypeId,
                    Name = Q.ProgramTypeName
                })
                .ToListAsync();

            var dataLearningType = await this.DB
                .LearningTypes
                .Select(Q => new UserSideLearningHistoryFilterValueTypeModel
                {
                    Id = Q.LearningTypeId,
                    Name = Q.LearningName
                })
                .ToListAsync();

            var dataMaterialtype = await this.DB
                .MaterialTypes
                .Select(Q => new UserSideLearningHistoryFilterValueTypeModel
                {
                    Id = Q.MaterialTypeId,
                    Name = Q.MaterialTypeName
                })
                .ToListAsync();

            var data = new UserSideLearningHistoryFilterValueModel
            {
                ProgramTypes = dataProgramType,
                LearningTypes = dataLearningType,
                MaterialTypes = dataMaterialtype
            };

            return data;
        }

        private async Task<string> GenerateImageUrl(Guid? BlobId, string BlobMime)
        {
            if (BlobId == null || BlobMime == null || BlobId == Guid.Empty)
            {
                return null;
            }

            return await this.FileService.GetFileAsync(BlobId.ToString(), BlobMime);
        }
    }
}