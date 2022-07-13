using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class SetupPopQuizService
    {
        private readonly TalentContext DB;
        private readonly InboxService InboxMan;
        private readonly ApprovalService ApprovalMan;

        public SetupPopQuizService(TalentContext talentContext, InboxService inboxService, ApprovalService approvalService)
        {
            this.DB = talentContext;
            this.InboxMan = inboxService;
            this.ApprovalMan = approvalService;
        }

        public async Task<List<CompetencySetupModel>> GetCompetencyOptionAsync(string name)
        {
            var data = await (from t in DB.Tasks
                              join c in DB.Competencies on t.CompetencyId equals c.CompetencyId
                              join ct in DB.CompetencyTypes on c.CompetencyTypeId equals ct.CompetencyTypeId
                              where t.IsDeleted == false
                              select new CompetencySetupModel
                              {
                                  CompetencyId = c.CompetencyId,
                                  PrefixCode = c.PrefixCode,
                                  CompetencyTypeId = ct.CompetencyTypeId,
                                  CompetencyTypeName = ct.CompetencyTypeName
                              }).Distinct().ToListAsync();

            if (!String.IsNullOrEmpty(name))
            {
                data = data.Where(Q => Q.PrefixCode.ToLower().Contains(name.ToLower()) || Q.CompetencyTypeName.ToLower().Contains(name.ToLower())).ToList();
            }

            return data;
        }

        public async Task<List<ModuleSetupModel>> GetModuleOptionAsync(string name)
        {
            var data = await (from t in DB.Tasks
                              join m in DB.Modules on t.ModuleId equals m.ModuleId
                              where t.IsDeleted == false
                              select new ModuleSetupModel
                              {
                                  ModuleId = t.ModuleId,
                                  ModuleName = m.ModuleName
                              }).Distinct().ToListAsync();

            if (!String.IsNullOrEmpty(name))
            {
                data = data.Where(Q => Q.ModuleName.ToLower().Contains(name.ToLower())).ToList();
            }

            return data;
        }

        public async Task<List<TaskCodeSetupModel>> GetTaskCodeOptionAsync(TaskCodeFilterModel filter)
        {
            var query = from t in DB.Tasks
                        join e in DB.EvaluationTypes on t.EvaluationTypeId equals e.EvaluationTypeId
                        join c in DB.Competencies on t.CompetencyId equals c.CompetencyId
                        join ct in DB.CompetencyTypes on c.CompetencyTypeId equals ct.CompetencyTypeId
                        join m in DB.Modules on t.ModuleId equals m.ModuleId
                        where t.IsDeleted == false
                        select new TaskCodeSetupModel
                        {
                            TaskId = t.TaskId,
                            CompetencyId = c.CompetencyId,
                            CompetencyTypeId = ct.CompetencyTypeId,
                            ModuleId = m.ModuleId,
                            EvaluationTypeId = e.EvaluationTypeId,
                            TaskNumber = t.TaskNumber,
                            PrefixCode = c.PrefixCode,
                            CompetencyTypeName = ct.CompetencyTypeName,
                            EvaluationTypeName = e.EvaluationTypeName,
                            ModuleName = m.ModuleName,
                            TaskCode = ct.CompetencyTypeName.Substring(0, 1) + "-" + c.PrefixCode + "-" + e.EvaluationTypeName + "-" + t.TaskNumber + "-" + m.ModuleName
                        };

            if (String.IsNullOrEmpty(filter.FilterName) == false)
            {
                query = query.Where(Q => Q.TaskCode.Contains(filter.FilterName));
            }

            if (filter.CompetencyId != null)
            {
                query = query.Where(Q => Q.CompetencyId == filter.CompetencyId);
            }

            if (filter.ModuleId != null)
            {
                query = query.Where(Q => Q.ModuleId == filter.ModuleId);
            }

            query = query.OrderBy(Q => Q.CompetencyId).ThenBy(Q => Q.EvaluationTypeId).ThenBy(Q => Q.TaskNumber).ThenBy(Q => Q.ModuleId);

            var data = await query.ToListAsync();

            return data;
        }

        public async Task<bool> InsertPopUpQuizAsync(SetupPopUpQuizCreateModel model)
        {
            var thisDate = DateTime.UtcNow.AddHours(7);
            var isExist = await this.DB.PopQuizzes.AsNoTracking().Where(Q => Q.IsDeleted == false).AnyAsync(Q => Q.PopQuizName.ToLower() == model.QuizTitle.ToLower());

            if (isExist)
            {
                return false;
            }

            var dataPopQuiz = new PopQuizzes
            {
                PopQuizName = model.QuizTitle,
                //IsPublished = false,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
            };

            this.DB.PopQuizzes.Add(dataPopQuiz);

            var dataSetupTask = new SetupTasks
            {
                SetupModuleId = null,
                PopQuizId = dataPopQuiz.PopQuizId,
                CompetencyId = model.CompetencyId,
                ModuleId = model.ModuleId,
                TestTime = model.TestTime,
                IsGrouping = model.IsGrouping,
                TotalParticipant = model.TotalParticipant,
                TotalQuestion = model.TotalQuestion,
                QuestionPerParticipant = model.QuestionPerParticipant
            };

            this.DB.SetupTasks.Add(dataSetupTask);

            List<SetupTaskCodes> dataSetupTaskCodes = new List<SetupTaskCodes>();
            var questionNumber = 1;

            if(model.TaskIds != null && model.TaskIds.Count() != 0)
            {
                foreach (var taskId in model.TaskIds)
                {
                    dataSetupTaskCodes.Add(new SetupTaskCodes
                    {
                        QuestionNumber = questionNumber,
                        SetupTaskId = dataSetupTask.SetupTaskId,
                        TaskId = taskId
                    });

                    questionNumber++;
                }

                this.DB.SetupTaskCodes.AddRange(dataSetupTaskCodes);
            }

            var dataLearning = new SetupLearning
            {
                PopQuizId = dataPopQuiz.PopQuizId,
                LearningName = model.QuizTitle,
                LearningCategoryName = LearningCategoryNameEnum.PopQuiz,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                ApprovalStatus = model.ApprovalId == ApprovalStatusesEnum.DraftId ? ApprovalStatusesEnum.Draft : ApprovalStatusesEnum.Approve
            };

            this.DB.SetupLearning.Add(dataLearning);

            var newApproval = new ApprovalCreateFormModel
            {
                ApprovalStatusId = model.ApprovalId == ApprovalStatusesEnum.DraftId ? ApprovalStatusesEnum.DraftId : ApprovalStatusesEnum.ApproveId,
                ContentCategory = ContentCategoryEnums.SetupPopQuiz,
                ContentId = dataPopQuiz.PopQuizId,
                ContentName = model.QuizTitle,
                PageEnum = PageEnums.SetupPopQuiz
            };

            var approvals = await this.ApprovalMan.CreateNewApproval(newApproval);

            //if (approvals.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
            //{
            //    dataLearning.ApprovedAt =thisDate;
            //    dataLearning.ApprovalStatus = ApprovalStatusesEnum.Approve;
            //    dataPopQuiz.ApprovedAt = thisDate;
            //}
            //else if (approvals.ApprovalStatusId == ApprovalStatusesEnum.DraftId)
            //{
            //    dataLearning.ApprovalStatus = ApprovalStatusesEnum.Draft;
            //    dataLearning.ApprovedAt = null;
            //    dataPopQuiz.ApprovedAt = null;
            //}
            //else
            //{
            //    dataLearning.ApprovalStatus = ApprovalStatusesEnum.Waiting;
            //    dataLearning.ApprovedAt = null;
            //    dataPopQuiz.ApprovedAt = null;
            //}

            if (approvals.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
            {
                dataLearning.ApprovedAt = (DateTime?)thisDate;
                dataPopQuiz.ApprovedAt = (DateTime?)thisDate;
                dataLearning.ApprovalStatus = ApprovalStatusesEnum.Approve;
            }
            else
            {
                if (approvals.ApprovalStatusId == ApprovalStatusesEnum.DraftId)
                {
                    dataLearning.ApprovalStatus = ApprovalStatusesEnum.Draft;
                }
                else
                {
                    dataLearning.ApprovalStatus = ApprovalStatusesEnum.Waiting;
                }
                dataLearning.ApprovedAt = null;
                dataPopQuiz.ApprovedAt = null;
            }

            await this.DB.SaveChangesAsync();

            return true;
        }

        public async Task<SetupPopQuizDetailModel> GetPopQuizDetailAsync(int popQuizId)
        {
            var data = await (from pq in DB.PopQuizzes
                              join st in DB.SetupTasks on pq.PopQuizId equals st.PopQuizId
                              where pq.PopQuizId == popQuizId
                              select new SetupPopQuizDetailModel
                              {
                                  PopQuizId = pq.PopQuizId,
                                  QuizTitle = pq.PopQuizName,
                                  SetupTaskId = st.SetupTaskId,
                                  TestTime = st.TestTime,
                                  IsGrouping = st.IsGrouping,
                                  CompetencyId = st.CompetencyId,
                                  ModuleId = st.ModuleId,
                                  TotalParticipant = st.TotalParticipant,
                                  TotalQuestion = st.TotalQuestion,
                                  QuestionPerParticipant = st.QuestionPerParticipant,
                                  TaskIds = new List<TaskCodeSetupModel>()
                              }).FirstOrDefaultAsync();

            data.TaskIds = await (from t in DB.Tasks
                                  join stc in DB.SetupTaskCodes on t.TaskId equals stc.TaskId
                                  join e in DB.EvaluationTypes on t.EvaluationTypeId equals e.EvaluationTypeId
                                  join c in DB.Competencies on t.CompetencyId equals c.CompetencyId
                                  join ct in DB.CompetencyTypes on c.CompetencyTypeId equals ct.CompetencyTypeId
                                  join m in DB.Modules on t.ModuleId equals m.ModuleId
                                  where t.IsDeleted == false && stc.SetupTaskId == data.SetupTaskId
                                  orderby stc.QuestionNumber
                                  select new TaskCodeSetupModel
                                  {
                                      TaskId = t.TaskId,
                                      CompetencyId = c.CompetencyId,
                                      CompetencyTypeId = ct.CompetencyTypeId,
                                      ModuleId = m.ModuleId,
                                      EvaluationTypeId = e.EvaluationTypeId,
                                      TaskNumber = t.TaskNumber,
                                      PrefixCode = c.PrefixCode,
                                      CompetencyTypeName = ct.CompetencyTypeName,
                                      EvaluationTypeName = e.EvaluationTypeName,
                                      ModuleName = m.ModuleName,
                                      TaskCode = ct.CompetencyTypeName.Substring(0, 1) + "-" + c.PrefixCode + "-" + e.EvaluationTypeName + "-" + t.TaskNumber + "-" + m.ModuleName
                                  }).ToListAsync();

            return data;
        }

        public async Task<bool> UpdatePopQuizAsync(SetupPopQuizUpdateModel model)
        {
            var thisDate = DateTime.UtcNow.AddHours(7);

            var dataCheck = await this.DB.PopQuizzes.AsNoTracking().Where(Q => Q.PopQuizId == model.PopQuizId).FirstOrDefaultAsync();
            var isChange = model.QuizTitle != dataCheck.PopQuizName;
            var isExist = await this.DB.PopQuizzes.AsNoTracking().AnyAsync(Q => Q.PopQuizName.ToLower() == model.QuizTitle.ToLower());

            if (isChange && isExist)
            {
                return false;
            }

            var dataPopQuiz = await this.DB.PopQuizzes.Where(Q => Q.PopQuizId == model.PopQuizId).FirstOrDefaultAsync();

            dataPopQuiz.PopQuizName = model.QuizTitle;
            dataPopQuiz.UpdatedAt = thisDate;

            var dataSetupTask = await this.DB.SetupTasks.Where(Q => Q.PopQuizId == model.PopQuizId).FirstOrDefaultAsync();

            dataSetupTask.CompetencyId = model.CompetencyId;
            dataSetupTask.ModuleId = model.ModuleId;
            dataSetupTask.TestTime = model.TestTime;
            dataSetupTask.IsGrouping = model.IsGrouping;
            dataSetupTask.TotalParticipant = model.TotalParticipant;
            dataSetupTask.TotalQuestion = model.TotalQuestion;
            dataSetupTask.QuestionPerParticipant = model.QuestionPerParticipant;

            var oldTaskCodes = await this.DB.SetupTaskCodes.Where(Q => Q.SetupTaskId == dataSetupTask.SetupTaskId).ToListAsync();

            this.DB.SetupTaskCodes.RemoveRange(oldTaskCodes);

            List<SetupTaskCodes> dataSetupTaskCodes = new List<SetupTaskCodes>();
            var questionNumber = 1;

            if (model.TaskIds != null && model.TaskIds.Count() != 0)
            {
                foreach (var taskId in model.TaskIds)
                {
                    dataSetupTaskCodes.Add(new SetupTaskCodes
                    {
                        QuestionNumber = questionNumber,
                        SetupTaskId = dataSetupTask.SetupTaskId,
                        TaskId = taskId
                    });

                    questionNumber++;
                }
                this.DB.SetupTaskCodes.AddRange(dataSetupTaskCodes);
            }


            var dataLearning = await this.DB.SetupLearning.Where(Q => Q.PopQuizId == model.PopQuizId).FirstOrDefaultAsync();
            dataLearning.LearningName = model.QuizTitle;
            dataLearning.UpdatedAt = thisDate;

            var dataApproval = await (from ap in DB.ApprovalToPopQuizzes
                                      join a in DB.Approvals on ap.ApprovalId equals a.ApprovalId
                                      where ap.PopQuizId == model.PopQuizId
                                      select a).FirstOrDefaultAsync();

            var updateApproval = new ApprovalUpdateFormModel
            {
                ApprovalId = dataApproval.ApprovalId,
                ApprovalStatusId = model.ApprovalId == ApprovalStatusesEnum.DraftId ? ApprovalStatusesEnum.DraftId : ApprovalStatusesEnum.ApproveId,
                ContentName = model.QuizTitle,
                PageEnum = PageEnums.SetupPopQuiz,
                ContentId = dataPopQuiz.PopQuizId,
                ContentCategory = ContentCategoryEnums.SetupPopQuiz
            };

            var approvals = await this.ApprovalMan.UpdateNewApproval(updateApproval);

            if (approvals.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
            {
                dataPopQuiz.ApprovedAt = (DateTime?)thisDate;
                dataLearning.ApprovalStatus = ApprovalStatusesEnum.Approve;
                dataLearning.ApprovedAt = (DateTime?)thisDate;
            }
            else
            {
                if (approvals.ApprovalStatusId == ApprovalStatusesEnum.DraftId)
                {
                    dataLearning.ApprovalStatus = ApprovalStatusesEnum.Draft;
                }
                else
                {
                    dataLearning.ApprovalStatus = ApprovalStatusesEnum.Waiting;
                }
                dataLearning.ApprovedAt = null;
                dataPopQuiz.ApprovedAt = null;
            }

            await this.DB.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemovePopQuiz(int popQuizId)
        {
            try
            {
                var findPopQuiz = await this.DB.PopQuizzes.Where(Q => Q.PopQuizId == popQuizId).FirstOrDefaultAsync();

                if (findPopQuiz == null)
                {
                    return false;
                }


                var findApprovalToPopQuizzes = await this.DB.ApprovalToPopQuizzes.Where(Q => Q.PopQuizId == popQuizId).FirstOrDefaultAsync();

                if (findApprovalToPopQuizzes == null)
                {
                    return false;
                }

                this.DB.ApprovalToPopQuizzes.Remove(findApprovalToPopQuizzes);

                var isDeleted = await this.ApprovalMan.DeleteApproval(findApprovalToPopQuizzes.ApprovalId);

                if (isDeleted == false)
                {
                    return false;
                }

                var findSetupLearning = await this.DB.SetupLearning.Where(Q => Q.PopQuizId == popQuizId).FirstOrDefaultAsync();
                this.DB.SetupLearning.Remove(findSetupLearning);

                findPopQuiz.IsDeleted = true;

                await this.DB.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
