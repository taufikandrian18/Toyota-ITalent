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
    public class SetupModuleService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        private readonly InboxService InboxMan;
        private readonly ApprovalService ApprovalMan;

        public SetupModuleService(TalentContext talentContext, ClaimHelper claimHelper, InboxService inboxService, ApprovalService approvalService)
        {
            this.DB = talentContext;
            this.ClaimMan = claimHelper;
            this.InboxMan = inboxService;
            this.ApprovalMan = approvalService;
        }

        public async Task<List<SetupModuleModuleViewModel>> GetModulesForSetupModule(string moduleName)
        {
        
                var getModules = await (from m in this.DB.Modules
                                        where !this.DB.SetupModules.Any(Q => Q.ModuleId == m.ModuleId && Q.CourseId == null && Q.IsDeleted == false) && m.ApprovedAt != null && m.IsDeleted == false
                                        select new SetupModuleModuleViewModel
                                        {
                                            ModuleName = m.ModuleName,
                                            ModuleDescription = m.ModuleDescription,
                                            ModuleId = m.ModuleId
                                        }).AsNoTracking().ToListAsync();
                return getModules;
        }

        public async Task<List<SetupModuleModuleViewModel>> GetAllModulesForSetupModuleUpdate(string moduleName)
        {

            var getModules = await this.DB.Modules.Where(Q => Q.ApprovedAt != null).Select(Q => new SetupModuleModuleViewModel
            {
                ModuleName = Q.ModuleName,
                ModuleDescription = Q.ModuleDescription,
                ModuleId = Q.ModuleId
            }).Where(Q => Q.ModuleName.StartsWith(moduleName)).AsNoTracking().ToListAsync();

            return getModules;
        }

        public async Task<List<TimePointTaskModel>> GetAllLearningTimePoints()
        {
            var getAllLearningTimePoints = await this.DB.TimePoints.Where(Q => Q.PointTypeId == 1 && Q.IsDeleted == false).Select(Q => new TimePointTaskModel
            {
                Points = Q.Points,
                Score = Q.Score,
                TimePointId = Q.TimePointId,
                Time = Q.Time
            }).AsNoTracking().ToListAsync();

            return getAllLearningTimePoints;
        }

        public async Task<List<ModuleForSetupModel>> GetModuleByName(string moduleName)
        {
            var getAllModule = await this.DB.Modules.Where(Q => Q.ModuleName.StartsWith(moduleName)).Select(Q => new ModuleForSetupModel
            {
                ModuleName = Q.ModuleName,
                ModuleId = Q.ModuleId
            }).ToListAsync();

            return getAllModule;
        }

        public async Task<List<SetupModuleModuleViewModel>> GetModuleTaskByName(string moduleName)
        {

            var getAllModule = await (from t in DB.Tasks
                                      join m in DB.Modules on t.ModuleId equals m.ModuleId
                                      where t.IsDeleted == false
                                      select new SetupModuleModuleViewModel
                                      {
                                          ModuleName = m.ModuleName,
                                          ModuleId = m.ModuleId,
                                          ModuleDescription = m.ModuleDescription
                                      }).Where(Q => Q.ModuleName.StartsWith(moduleName)).AsQueryable().Distinct().ToListAsync();

            return getAllModule;
        }

        public async Task<List<CompetencyMappingJoinModel>> GetAllCompetencies(string search)
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
            var result = await query.AsNoTracking().AsQueryable().Distinct().Where(Q => Q.CompetencyNameMapping.ToLower().Contains(search.ToLower())).ToListAsync();

            return result;
        }

        public async Task<List<TaskForSetupModuleModel>> GetAllTaskCode(string taskCode)
        {
            var query = (from T in this.DB.Tasks
                         join Q in this.DB.QuestionTypes on T.QuestionTypeId equals Q.QuestionTypeId
                         join M in this.DB.Modules on T.ModuleId equals M.ModuleId
                         join C in this.DB.Competencies on T.CompetencyId equals C.CompetencyId
                         join CT in this.DB.CompetencyTypes on C.CompetencyTypeId equals CT.CompetencyTypeId
                         join ET in this.DB.EvaluationTypes on T.EvaluationTypeId equals ET.EvaluationTypeId
                         where T.IsDeleted == false
                         select new
                         {
                             TaskCode = CT.CompetencyTypeName.Substring(0, 1) + "-" + C.PrefixCode + "-" + ET.EvaluationTypeName + "-" + T.TaskNumber + "-" + M.ModuleName,
                             TaskId = T.TaskId,
                         }).AsNoTracking().AsQueryable();
            var result = await query.Where(Q => Q.TaskCode.Contains(taskCode)).Select(Q => new TaskForSetupModuleModel
            {
                TaskCode = Q.TaskCode,
                TaskId = Q.TaskId
            }).ToListAsync();

            return result;
        }

        public async Task<List<CompetencyMappingJoinModel>> GetCompetencyAutoSuggest(string competencyName)
        {
            var query = from t in DB.Tasks
                        join c in DB.Competencies on t.CompetencyId equals c.CompetencyId
                        join ct in DB.CompetencyTypes on c.CompetencyTypeId equals ct.CompetencyTypeId
                        where t.IsDeleted == false
                        select new
                        {
                            CompetencyNameMapping = ct.CompetencyTypeName.Substring(0, 1) + "-" + c.PrefixCode,
                            CompetencyId = c.CompetencyId,
                            CompetencyTypeId = c.CompetencyTypeId,
                            PrefixCode = c.PrefixCode,
                            CompetencyTypeName = ct.CompetencyTypeName,
                        };
            var result = await query.Where(Q => Q.CompetencyNameMapping.Contains(competencyName)).Select(Q => new CompetencyMappingJoinModel
            {
                CompetencyId = Q.CompetencyId,
                CompetencyTypeId = Q.CompetencyTypeId,
                PrefixCode = Q.PrefixCode,
                CompetencyTypeName = Q.CompetencyTypeName,
                CompetencyNameMapping = Q.CompetencyNameMapping
            }).AsNoTracking().AsQueryable().Distinct().ToListAsync();
            return result;
        }

        public async Task<List<TaskForSetupModuleModel>> GetAllTaskCodeWithFilter(TaskCodeFilteredFormModel model)
        {
            var query = (from T in this.DB.Tasks
                         join Q in this.DB.QuestionTypes on T.QuestionTypeId equals Q.QuestionTypeId
                         join M in this.DB.Modules on T.ModuleId equals M.ModuleId
                         join C in this.DB.Competencies on T.CompetencyId equals C.CompetencyId
                         join CT in this.DB.CompetencyTypes on C.CompetencyTypeId equals CT.CompetencyTypeId
                         join ET in this.DB.EvaluationTypes on T.EvaluationTypeId equals ET.EvaluationTypeId
                         where T.IsDeleted == false
                         select new
                         {
                             TaskCode = CT.CompetencyTypeName.Substring(0, 1) + "-" + C.PrefixCode + "-" + ET.EvaluationTypeName + "-" + T.TaskNumber + "-" + M.ModuleName,
                             TaskId = T.TaskId,
                             CompetencyId = C.CompetencyId,
                             ModuleId = M.ModuleId
                         }).AsNoTracking().AsQueryable();

            if (model.CompetencyId != null)
            {
                query = query.Where(Q => Q.CompetencyId == model.CompetencyId);
            }

            if (model.ModuleId != null)
            {
                query = query.Where(Q => Q.ModuleId == model.ModuleId);
            }

            var result = await query.Where(Q => Q.TaskCode.Contains(model.TaskCode)).Select(Q => new TaskForSetupModuleModel
            {
                TaskCode = Q.TaskCode,
                TaskId = Q.TaskId
            }).ToListAsync();

            return result;
        }

        public async Task<List<TaskForSetupModuleModel>> GenerateTaskCode(int totalTask, TaskCodeFilteredFormModel model)
        {
            var query = (from T in this.DB.Tasks
                         join Q in this.DB.QuestionTypes on T.QuestionTypeId equals Q.QuestionTypeId
                         join M in this.DB.Modules on T.ModuleId equals M.ModuleId
                         join C in this.DB.Competencies on T.CompetencyId equals C.CompetencyId
                         join CT in this.DB.CompetencyTypes on C.CompetencyTypeId equals CT.CompetencyTypeId
                         join ET in this.DB.EvaluationTypes on T.EvaluationTypeId equals ET.EvaluationTypeId
                         where T.IsDeleted == false
                         select new
                         {
                             TaskCode = CT.CompetencyTypeName.Substring(0, 1) + "-" + C.PrefixCode + "-" + ET.EvaluationTypeName + "-" + T.TaskNumber + "-" + M.ModuleName,
                             TaskId = T.TaskId,
                             CompetencyId = C.CompetencyId,
                             ModuleId = M.ModuleId
                         }).AsNoTracking().AsQueryable();

            if (model.CompetencyId != null)
            {
                query = query.Where(Q => Q.CompetencyId == model.CompetencyId);
            }

            if (model.ModuleId != null)
            {
                query = query.Where(Q => Q.ModuleId == model.ModuleId);
            }

            var count = await query.CountAsync();

            if (totalTask > count)
            {
                totalTask = count;
            }

            var result = await query.Select(Q => new TaskForSetupModuleModel
            {
                TaskCode = Q.TaskCode,
                TaskId = Q.TaskId
            }).Take(totalTask).ToListAsync();

            return result;
        }

        public async Task<bool> InsertSetupModule(SetupModuleFormModel model)
        {
            using (var transaction = await this.DB.Database.BeginTransactionAsync())
            {
                try
                {
                    var userLogIn = this.ClaimMan.GetLoginUserId();

                    var newSetupModule = new SetupModules
                    {
                        IsRecommendedForYou = model.IsRecommendedForYou.GetValueOrDefault(),
                        IsPopular = model.IsPopular.GetValueOrDefault(),
                        TimePointId = model.TimePointId,
                        ModuleId = model.ModuleId,
                        CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                        UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                        CreatedBy = userLogIn,
                        UpdatedBy = userLogIn
                    };


                    this.DB.SetupModules.Add(newSetupModule);
                    await this.DB.SaveChangesAsync();

                    if (model.SetupTaskForm.TaskList != null)
                    {
                        var newInsertSetupTask = new SetupTasks
                        {
                            TestTime = model.SetupTaskForm.TestTime.GetValueOrDefault(),
                            IsGrouping = model.SetupTaskForm.IsGrouping.GetValueOrDefault(),
                            CompetencyId = model.SetupTaskForm.CompetencyId,
                            ModuleId = model.SetupTaskForm.ModuleId,
                            TotalParticipant = model.SetupTaskForm.TotalParticipant,
                            TotalQuestion = model.SetupTaskForm.TotalQuestion,
                            SetupModuleId = newSetupModule.SetupModuleId,
                            QuestionPerParticipant = model.SetupTaskForm.QuestionPerParticipant
                        };

                        this.DB.SetupTasks.Add(newInsertSetupTask);
                        this.DB.SaveChanges();

                        var listInsertTaskCode = new List<SetupTaskCodes>();

                        for (int a = 0; a < model.SetupTaskForm.TaskList.Count; a++)
                        {
                            listInsertTaskCode.Add(new SetupTaskCodes
                            {
                                SetupTaskId = newInsertSetupTask.SetupTaskId,
                                TaskId = model.SetupTaskForm.TaskList[a].TaskId.GetValueOrDefault(),
                                QuestionNumber = a + 1
                            });
                        }
                        await this.DB.SetupTaskCodes.AddRangeAsync(listInsertTaskCode);
                        await this.DB.SaveChangesAsync();
                    }

                    var newSetupLearning = new SetupLearning
                    {
                        //Insert Setup Learning Approval also
                        SetupModuleId = newSetupModule.SetupModuleId,
                        LearningCategoryName = "Module",
                        LearningName = model.ModuleName,
                        CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                        UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    };

                    var approvalStatusId = model.InputType == 1 ? ApprovalStatusesEnum.DraftId : ApprovalStatusesEnum.ApproveId;

                    var newApproval = new ApprovalCreateFormModel
                    {
                        ApprovalStatusId = approvalStatusId,
                        ContentCategory = ContentCategoryEnums.SetupModule,
                        ContentName = model.ModuleName,
                        PageEnum = PageEnums.SetupModule,
                        ContentId = newSetupModule.SetupModuleId
                    };

                    var approvals = await this.ApprovalMan.CreateNewApproval(newApproval);

                    if (approvals.ApprovalStatusId == ApprovalStatusesEnum.ApproveId)
                    {
                        newSetupModule.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                        newSetupLearning.ApprovalStatus = ApprovalStatusesEnum.Approve;
                        newSetupLearning.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    }
                    else if (approvals.ApprovalStatusId == ApprovalStatusesEnum.DraftId)
                    {
                        newSetupLearning.ApprovalStatus = ApprovalStatusesEnum.Draft;
                    }
                    else
                    {
                        newSetupLearning.ApprovalStatus = ApprovalStatusesEnum.Waiting;
                    }


                    this.DB.SetupLearning.Add(newSetupLearning);
                    await this.DB.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    var error = e.Data;
                    return false;
                }
            }

            return true;
        }

        public async Task<SetupModuleFormModel> GetSetupModuleFormModel(int id)
        {
            var setupModule = new SetupModuleFormModel();

            setupModule = await (
                from SM in this.DB.SetupModules
                join TP in this.DB.TimePoints on SM.TimePointId equals TP.TimePointId
                join M in this.DB.Modules on SM.ModuleId equals M.ModuleId

                where SM.SetupModuleId == id
                select new SetupModuleFormModel
                {
                    SetupModuleId = SM.SetupModuleId,
                    TimePointId = SM.TimePointId,
                    ModuleName = M.ModuleName,
                    ModuleId = M.ModuleId,
                    ModuleDescription = M.ModuleDescription,
                    IsRecommendedForYou = SM.IsRecommendedForYou,
                    IsPopular = SM.IsPopular,
                    Score = TP.Score,
                    Points = TP.Points,
                    SetupTaskForm = new SetupTaskFormModel()
                }).AsNoTracking().FirstOrDefaultAsync();

            if (setupModule == null)
            {
                return new SetupModuleFormModel();
            }

            setupModule.SetupTaskForm = await (from ST in this.DB.SetupTasks

                                               join M in this.DB.Modules on ST.ModuleId equals M.ModuleId into LM
                                               from M in LM.DefaultIfEmpty()

                                               join C in this.DB.Competencies on ST.CompetencyId equals C.CompetencyId into LCID
                                               from C in LCID.DefaultIfEmpty()

                                               join CT in this.DB.CompetencyTypes on C.CompetencyTypeId equals CT.CompetencyTypeId into LCTID
                                               from CT in LCTID.DefaultIfEmpty()

                                               where ST.SetupModuleId == id

                                               select new SetupTaskFormModel
                                               {
                                                   SetupTaskId = ST.SetupTaskId,
                                                   TestTime = ST.TestTime,
                                                   IsGrouping = ST.IsGrouping,
                                                   CompetencyNameMapping = ST.CompetencyId != null ? CT.CompetencyTypeName.Substring(0, 1) + "-" + C.PrefixCode : "",
                                                   ModuleName = ST.ModuleId != null ? M.ModuleName : "",
                                                   TotalParticipant = ST.TotalParticipant,
                                                   TotalQuestion = ST.TotalQuestion,
                                                   QuestionPerParticipant = ST.QuestionPerParticipant,
                                                   TaskList = new List<SetupTaskCodesFormModel>(),
                                                   CompetencyId = C.CompetencyId,
                                                   CompetencyTypeId = C.CompetencyTypeId,
                                                   PrefixCode = C.PrefixCode,
                                                   CompetencyTypeName = CT.CompetencyTypeName,
                                                   ModuleDescription = M.ModuleDescription,
                                                   ModuleId = M.ModuleId
                                               }).AsNoTracking().FirstOrDefaultAsync();

            if (setupModule.SetupTaskForm == null)
            {
                setupModule.SetupTaskForm = new SetupTaskFormModel();
                return setupModule;
            }

            setupModule.SetupTaskForm.TaskList = await (from STC in this.DB.SetupTaskCodes

                                                        join T in this.DB.Tasks on STC.TaskId equals T.TaskId

                                                        join Q in this.DB.QuestionTypes on T.QuestionTypeId equals Q.QuestionTypeId
                                                        join M in this.DB.Modules on T.ModuleId equals M.ModuleId
                                                        join C in this.DB.Competencies on T.CompetencyId equals C.CompetencyId
                                                        join CT in this.DB.CompetencyTypes on C.CompetencyTypeId equals CT.CompetencyTypeId
                                                        join ET in this.DB.EvaluationTypes on T.EvaluationTypeId equals ET.EvaluationTypeId

                                                        where STC.SetupTaskId == setupModule.SetupTaskForm.SetupTaskId

                                                        select new SetupTaskCodesFormModel
                                                        {
                                                            SetupTaskId = STC.SetupTaskId,
                                                            TaskId = STC.TaskId,
                                                            QuestionNumber = STC.QuestionNumber,
                                                            TaskName = CT.CompetencyTypeName.Substring(0, 1) + "-" + C.PrefixCode + "-" + ET.EvaluationTypeName + "-" + T.TaskNumber + "-" + M.ModuleName,
                                                            TaskCode = CT.CompetencyTypeName.Substring(0, 1) + "-" + C.PrefixCode + "-" + ET.EvaluationTypeName + "-" + T.TaskNumber + "-" + M.ModuleName,
                                                        }).AsNoTracking().ToListAsync();

            return setupModule;
        }

        public async Task<bool> UpdateSetupModule(SetupModuleFormModel model)
        {
            using (var transaction = await this.DB.Database.BeginTransactionAsync())
            {
                var userLogIn = this.ClaimMan.GetLoginUserId();

                var findSetupModule = await this.DB.SetupModules.Where(Q => Q.SetupModuleId == model.SetupModuleId && Q.IsDeleted == false).FirstOrDefaultAsync();

                var findSetupLearning = await this.DB.SetupLearning.Where(Q => Q.SetupModuleId == model.SetupModuleId).FirstOrDefaultAsync();

                if (findSetupModule == null)
                {
                    return false;
                }

                var isDuplicateModule = await this.DB.SetupModules.Where(Q => Q.ModuleId == model.ModuleId && Q.Course == null && Q.IsDeleted == false).FirstOrDefaultAsync();

                if (isDuplicateModule != null && isDuplicateModule.SetupModuleId != findSetupLearning.SetupModuleId)
                {
                    return false;
                }

                var checkApprovalMappings = await this.DB.ApprovalMappings.Where(Q => Q.PageId == PageEnums.SetupModule).FirstOrDefaultAsync();
                var findApprovalToSetupModule = await this.DB.ApprovalToSetupModules.Where(Q => Q.SetupModuleId == findSetupModule.SetupModuleId).FirstOrDefaultAsync();
                var findApproval = await this.DB.Approvals.Where(Q => Q.ApprovalId == findApprovalToSetupModule.ApprovalId).FirstOrDefaultAsync();

                if (findApproval.ApprovalStatusId == ApprovalStatusesEnum.WaitingId)
                {
                    return false;
                }

                findSetupModule.TimePointId = model.TimePointId;
                findSetupModule.IsPopular = model.IsPopular.GetValueOrDefault();
                findSetupModule.IsRecommendedForYou = model.IsRecommendedForYou.GetValueOrDefault();
                findSetupModule.ModuleId = model.ModuleId;
                findSetupModule.UpdatedBy = userLogIn;
                findSetupModule.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();

                //await this.DB.SaveChangesAsync();

                var findSetupTask = await this.DB.SetupTasks.Where(Q => Q.SetupTaskId == model.SetupTaskForm.SetupTaskId).FirstOrDefaultAsync();

                if (findSetupTask == null)
                {
                    if (model.SetupTaskForm.TaskList != null)
                    {
                        var newSetupTask = new SetupTasks
                        {
                            CompetencyId = model.SetupTaskForm.CompetencyId,
                            IsGrouping = model.SetupTaskForm.IsGrouping.GetValueOrDefault(),
                            ModuleId = model.SetupTaskForm.ModuleId,
                            QuestionPerParticipant = model.SetupTaskForm.QuestionPerParticipant,
                            SetupModuleId = model.SetupModuleId,
                            TestTime = model.SetupTaskForm.TestTime.GetValueOrDefault(),
                            TotalParticipant = model.SetupTaskForm.TotalParticipant,
                            TotalQuestion = model.SetupTaskForm.TotalQuestion
                        };

                        this.DB.SetupTasks.Add(newSetupTask);
                        await this.DB.SaveChangesAsync();

                        var newSetupTaskCode = new List<SetupTaskCodes>();
                        for (int a = 0; a < model.SetupTaskForm.TaskList.Count; a++)
                        {
                            newSetupTaskCode.Add(new SetupTaskCodes
                            {
                                QuestionNumber = a + 1,
                                SetupTaskId = newSetupTask.SetupTaskId,
                                TaskId = model.SetupTaskForm.TaskList[a].TaskId.GetValueOrDefault()
                            });
                        }
                        this.DB.SetupTaskCodes.AddRange(newSetupTaskCode);
                        await this.DB.SaveChangesAsync();
                    }
                }
                else
                {
                    if (model.SetupTaskForm.TaskList == null || model.SetupTaskForm.TaskList.Count() <= 0)
                    {
                        //Remove Task Code
                        this.DB.SetupTaskCodes.RemoveRange(this.DB.SetupTaskCodes.Where(Q => Q.SetupTaskId == model.SetupTaskForm.SetupTaskId));

                        //Remove Setup Task
                        var removeSetupTask = await this.DB.SetupTasks.Where(Q => Q.SetupTaskId == model.SetupTaskForm.SetupTaskId).FirstOrDefaultAsync();
                        this.DB.SetupTasks.Remove(removeSetupTask);

                        await this.DB.SaveChangesAsync();
                    }
                    else
                    {
                        findSetupTask.SetupModuleId = model.SetupModuleId;
                        findSetupTask.CompetencyId = model.SetupTaskForm.CompetencyId;
                        findSetupTask.IsGrouping = model.SetupTaskForm.IsGrouping.GetValueOrDefault();
                        findSetupTask.ModuleId = model.SetupTaskForm.ModuleId;
                        findSetupTask.QuestionPerParticipant = model.SetupTaskForm.QuestionPerParticipant;
                        findSetupTask.TotalQuestion = model.SetupTaskForm.TotalQuestion;
                        findSetupTask.TotalParticipant = model.SetupTaskForm.TotalParticipant;
                        findSetupTask.TestTime = model.SetupTaskForm.TestTime.GetValueOrDefault();

                        this.DB.SetupTaskCodes.RemoveRange(this.DB.SetupTaskCodes.Where(Q => Q.SetupTaskId == model.SetupTaskForm.SetupTaskId));

                        await this.DB.SaveChangesAsync();

                        var taskCodes = new List<SetupTaskCodes>();

                        for (int a = 0; a < model.SetupTaskForm.TaskList.Count; a++)
                        {
                            taskCodes.Add(new SetupTaskCodes
                            {
                                QuestionNumber = a + 1,
                                SetupTaskId = findSetupTask.SetupTaskId,
                                TaskId = model.SetupTaskForm.TaskList[a].TaskId.GetValueOrDefault()
                            });
                        }

                        this.DB.SetupTaskCodes.AddRange(taskCodes);
                        await this.DB.SaveChangesAsync();
                    }
                }

                findSetupLearning.LearningName = model.ModuleName;
                findSetupLearning.LearningCategoryName = LearningCategoryNameEnum.Module;
                findSetupLearning.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                //findSetupLearning.IsPublished = false;

                var newApproval = new ApprovalUpdateFormModel
                {
                    ApprovalId = findApproval.ApprovalId,
                    ApprovalStatusId = model.InputType == 1 ? ApprovalStatusesEnum.DraftId : ApprovalStatusesEnum.ApproveId,
                    ContentName = model.ModuleName,
                    PageEnum = PageEnums.SetupModule,
                    ContentId = findSetupModule.SetupModuleId,
                    ContentCategory = ContentCategoryEnums.SetupModule
                };

                var approvals = await this.ApprovalMan.UpdateNewApproval(newApproval);

                if (approvals.ApprovalStatusId == ApprovalStatusesEnum.DraftId)
                {
                    findSetupLearning.ApprovalStatus = ApprovalStatusesEnum.Draft;
                    findSetupModule.ApprovedAt = null;
                }
                else if (approvals.ApprovalStatusId == ApprovalStatusesEnum.WaitingId)
                {
                    findSetupLearning.ApprovalStatus = ApprovalStatusesEnum.Waiting;
                    findSetupModule.ApprovedAt = null;
                    findSetupLearning.ApprovedAt = null;
                }
                else
                {
                    findSetupLearning.ApprovalStatus = ApprovalStatusesEnum.Approve;
                    findSetupLearning.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    findSetupModule.ApprovedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                }


                await this.DB.SaveChangesAsync();

                transaction.Commit();
            }

            return true;
        }


        public async Task<bool> RemoveSetupModule(int setupModuleId)
        {
            try
            {
                var userLogin = this.ClaimMan.GetLoginUserId();
                var findSetupModule = await this.DB.SetupModules.Where(Q => Q.SetupModuleId == setupModuleId).FirstOrDefaultAsync();

                if (findSetupModule == null)
                {
                    return false;
                }

                var findApprovalToSetupModule = await this.DB.ApprovalToSetupModules.Where(Q => Q.SetupModuleId == setupModuleId).FirstOrDefaultAsync();

                if (findApprovalToSetupModule == null)
                {
                    return false;
                }

                this.DB.ApprovalToSetupModules.Remove(findApprovalToSetupModule);

                var isDeleted = await this.ApprovalMan.DeleteApproval(findApprovalToSetupModule.ApprovalId);

                if (isDeleted == false)
                {
                    return false;
                }

                var findSetupLearning = await this.DB.SetupLearning.Where(Q => Q.SetupModuleId == setupModuleId).FirstOrDefaultAsync();
                this.DB.SetupLearning.Remove(findSetupLearning);

                findSetupModule.IsDeleted = true;
                findSetupModule.UpdatedBy = userLogin;
                findSetupModule.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();

                var findPrerequisite = await this.DB.CoursePrerequisiteMappings.Where(Q => Q.NextSetupModuleId == setupModuleId).ToListAsync();
                this.DB.CoursePrerequisiteMappings.RemoveRange(findPrerequisite);

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
