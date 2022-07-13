using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class TaskService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper HelperMan;

        public TaskService(TalentContext talentcontext, ClaimHelper claimHelper)
        {
            this.DB = talentcontext;
            this.HelperMan = claimHelper;
        }

        /// <summary>
        /// get all tasks before/after filter and sorting
        /// </summary>
        /// <param name="filter">query from the html to filter and sorting the data at the same time</param>
        /// <returns>return task data after the filter and sorting</returns>
        public async Task<TaskPaginationModel> GetAllTaskPaginateAsync(TaskPagingModel filter)
        {
            var query = (from T in this.DB.Tasks
                         join Q in this.DB.QuestionTypes on T.QuestionTypeId equals Q.QuestionTypeId
                         join M in this.DB.Modules on T.ModuleId equals M.ModuleId
                         join C in this.DB.Competencies on T.CompetencyId equals C.CompetencyId
                         join CT in this.DB.CompetencyTypes on C.CompetencyTypeId equals CT.CompetencyTypeId
                         join ET in this.DB.EvaluationTypes on T.EvaluationTypeId equals ET.EvaluationTypeId
                         join E in this.DB.Employees on T.CreatedBy equals E.EmployeeId
                         where T.IsDeleted == false
                         select new
                         {
                             TaskCode = CT.CompetencyTypeName.Substring(0, 1) + "-" + C.PrefixCode + "-" + ET.EvaluationTypeName + "-" + T.TaskNumber + "-" + M.ModuleName,
                             QuestionTypeId = Q.QuestionTypeId,
                             QuestionTypeName = Q.QuestionTypeName,
                             ModuleName = M.ModuleName,
                             CreatedBy = E.EmployeeName,
                             CreatedAtDate = T.CreatedAt,
                             UpdateAtDate = T.UpdatedAt,
                             TaskId = T.TaskId,
                         }).AsNoTracking().AsQueryable();

            //search
            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAtDate >= startDate && Q.CreatedAtDate < endDate) ||
                                         (Q.UpdateAtDate >= startDate && Q.UpdateAtDate < endDate));
            }

            if (string.IsNullOrEmpty(filter.TaskCode) == false)
            {
                query = query.Where(Q => Q.TaskCode.Contains(filter.TaskCode));
            }

            if (filter.QuestionTypeId != null)
            {
                query = query.Where(Q => Q.QuestionTypeId == filter.QuestionTypeId);
            }

            if (string.IsNullOrEmpty(filter.ModuleName) == false)
            {
                query = query.Where(Q => Q.ModuleName.Contains(filter.ModuleName));
            }

            if (string.IsNullOrEmpty(filter.CreatedBy) == false)
            {
                query = query.Where(Q => Q.CreatedBy.Contains(filter.CreatedBy));
            }

            //sort
            switch (filter.SortBy)
            {
                case "TaskAsc":
                    query = query.OrderBy(Q => Q.TaskCode);
                    break;
                case "QuestionTypeIdAsc":
                    query = query.OrderBy(Q => Q.QuestionTypeName);
                    break;
                case "ModuleNameAsc":
                    query = query.OrderBy(Q => Q.ModuleName);
                    break;
                case "CreatedByAsc":
                    query = query.OrderBy(Q => Q.CreatedBy);
                    break;
                case "CreatedDateAsc":
                    query = query.OrderBy(Q => Q.CreatedAtDate);
                    break;
                case "UpdatedDateAsc":
                    query = query.OrderBy(Q => Q.UpdateAtDate);
                    break;
                case "TaskDesc":
                    query = query.OrderByDescending(Q => Q.TaskCode);
                    break;
                case "QuestionTypeIdDesc":
                    query = query.OrderByDescending(Q => Q.QuestionTypeName);
                    break;
                case "ModuleNameDesc":
                    query = query.OrderByDescending(Q => Q.ModuleName);
                    break;
                case "CreatedByDesc":
                    query = query.OrderByDescending(Q => Q.CreatedBy);
                    break;
                case "CreatedDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAtDate);
                    break;
                case "UpdatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdateAtDate);
                    break;
                case "":
                    query = query.OrderByDescending(Q => Q.UpdateAtDate);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdateAtDate);
                    break;
            }

            var allDdata = await query.Select(Q => new TaskViewModel
            {
                TaskCode = Q.TaskCode,
                QuestionTypeId = Q.QuestionTypeId,
                ModuleName = Q.ModuleName,
                CreatedBy = Q.CreatedBy,
                CreatedAtDate = Q.CreatedAtDate,
                UpdateAtDate = Q.UpdateAtDate,
                TaskId = Q.TaskId,
                UpdateAt = Q.UpdateAtDate == null ? "" : Q.UpdateAtDate.ToString("dd/MM/yyyy"),
                CreatedAt = Q.CreatedAtDate == null ? "" : Q.CreatedAtDate.ToString("dd/MM/yyyy"),
            }).Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalData = await query.CountAsync();

            return new TaskPaginationModel
            {
                TaskData = allDdata,
                TotalData = totalData
            };

        }

        /// <summary>
        /// delete selected task data
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>return true if successfully delete data</returns>
        public async Task<bool> DeleteTaskByIdAsync(int taskId)
        {
            var task = await this.DB.Tasks.Where(Q => Q.TaskId == taskId).FirstOrDefaultAsync();
            var userId = this.HelperMan.GetLoginUserId();

            if (task == null)
            {
                return false;
            }

            task.IsDeleted = true;
            task.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            task.UpdatedBy = userId;

            await this.DB.SaveChangesAsync();

            return true;
        }

        public async Task<List<CompetencyMappingJoinModel>> GetAllTaskCode()
        {
            var query = from cm in DB.CompetencyEvaluationMappings
                        join c in DB.Competencies on cm.CompetencyId equals c.CompetencyId
                        join ct in DB.CompetencyTypes on c.CompetencyTypeId equals ct.CompetencyTypeId
                        join et in DB.EvaluationTypes on cm.EvaluationTypeId equals et.EvaluationTypeId
                        select new CompetencyMappingJoinModel
                        {
                            CompetencyId = c.CompetencyId,
                            CompetencyTypeId = c.CompetencyTypeId,
                            PrefixCode = c.PrefixCode,
                            EvaluationTypeId = cm.EvaluationTypeId,
                            CompetencyTypeName = ct.CompetencyTypeName,
                            EvaluationTypeName = et.EvaluationTypeName
                        };
            var result = await query.AsNoTracking().AsQueryable().ToListAsync();
            return result;
        }

        public async Task<CompetencyMappingJoinModel> GetTaskCodeByTaskId(int id)
        {
            var query = from t in DB.Tasks
                        join c in DB.Competencies on t.CompetencyId equals c.CompetencyId
                        join cm in DB.CompetencyEvaluationMappings on c.CompetencyId equals cm.CompetencyId
                        join ct in DB.CompetencyTypes on c.CompetencyTypeId equals ct.CompetencyTypeId
                        join et in DB.EvaluationTypes on t.EvaluationTypeId equals et.EvaluationTypeId
                        where t.TaskId == id
                        select new CompetencyMappingJoinModel
                        {
                            CompetencyId = c.CompetencyId,
                            CompetencyTypeId = c.CompetencyTypeId,
                            PrefixCode = c.PrefixCode,
                            EvaluationTypeId = t.EvaluationTypeId,
                            CompetencyTypeName = ct.CompetencyTypeName,
                            EvaluationTypeName = et.EvaluationTypeName
                        };
            var result = await query.AsNoTracking().AsQueryable().FirstOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// Get All Modules for Inserting Task
        /// </summary>
        /// <returns></returns>
        public async Task<List<ModuleForTaskModel>> GetAllModules()
        {
            var result = await this.DB.Modules.Where(Q => Q.IsDeleted == false).Select(Q => new ModuleForTaskModel
            {
                ModuleId = Q.ModuleId,
                ModuleName = Q.ModuleName
            }).AsNoTracking().ToListAsync();

            return result;
        }

        /// <summary>
        /// Get Time Points for Task
        /// </summary>
        /// <returns></returns>
        public async Task<List<TimePointTaskModel>> GetAllTaskTimePoints()
        {
            var result = await this.DB.TimePoints.Where(Q => Q.PointTypeId == 5 && Q.IsDeleted == false).Select(Q => new TimePointTaskModel
            {
                Points = Q.Points,
                Score = Q.Score,
                TimePointId = Q.TimePointId
            }).AsNoTracking().ToListAsync();

            return result;
        }
        /// <summary>
        /// Get All Time Point + Time Point From Task Score Id
        /// </summary>
        /// <returns></returns>
        public async Task<List<TimePointTaskModel>> GetAllTaskTimePointsUpdate(List<int> TimePointScore)
        {
            var result = await this.DB.TimePoints.Where(Q => (Q.PointTypeId == 5 && Q.IsDeleted == false) || TimePointScore.Contains(Q.Score.GetValueOrDefault())).Select(Q => new TimePointTaskModel
            {
                Points = Q.Points,
                Score = Q.Score,
                TimePointId = Q.TimePointId
            }).AsNoTracking().ToListAsync();

            return result;
        }

        /// <summary>
        /// Get Current Number for new Inserted Task
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> GetNumber(GetNumberTaskModel model)
        {
            var query = from t in DB.Tasks
                        join c in DB.Competencies on t.CompetencyId equals c.CompetencyId
                        where c.CompetencyId == model.CompetencyId && t.EvaluationTypeId == model.EvaluationTypeId && t.ModuleId == model.ModuleId
                        select new
                        {
                            TaskId = t.TaskId,
                        };
            var countData = await query.CountAsync();
            var totalNumber = countData + 1;
            return totalNumber;
        }

        public async Task<int> Count()
        {
            return await this.DB.Tasks.CountAsync() + 1;
        }

        public async Task<TaskInsertModel> GetTaskById(int id)
        {
            var data = await this.DB.Tasks.Where(Q => Q.TaskId == id).Select(Q => new TaskInsertModel
            {
                BlobId = Q.BlobId,
                TaskId = Q.TaskId,
                CompetencyId = Q.CompetencyId,
                EvaluationTypeId = Q.EvaluationTypeId,
                LayoutType = Q.LayoutType,
                ModuleId = Q.ModuleId,
                QuestionTypeId = Q.QuestionTypeId,
                StoryLineDescription = Q.StoryLineDescription,
                TaskNumber = Q.TaskNumber
            }).FirstOrDefaultAsync();
            return data;
        }
    }
}
