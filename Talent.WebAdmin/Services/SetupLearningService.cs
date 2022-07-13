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
    public class SetupLearningService
    {
        private readonly TalentContext _Db;
        private readonly IContextualService ContextMan;
        private readonly ApprovalService ApprovalMan;

        public SetupLearningService(TalentContext talentContext, IContextualService contextual, ApprovalService approvalService)
        {
            this._Db = talentContext;
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

        public IQueryable<SetupLearningModels> JoinTable()
        {

            //LEFT JOIN
            //into cnull from c1 in cnull.DefaultIfEmpty()

            var query = (from sl in this._Db.SetupLearning
                         join c in this._Db.Courses on sl.CourseId equals c.CourseId into cnull
                         from slc in cnull.DefaultIfEmpty()
                         join pq in this._Db.PopQuizzes on sl.PopQuizId equals pq.PopQuizId into pqnull
                         from slpq in pqnull.DefaultIfEmpty()
                         join sm in this._Db.SetupModules on sl.SetupModuleId equals sm.SetupModuleId into smnull
                         from slsm in smnull.DefaultIfEmpty()
                         join m in this._Db.Modules on slsm.ModuleId equals m.ModuleId into mnull
                         from slsmm in mnull.DefaultIfEmpty()
                         where slsmm.IsDeleted == false
                         select new SetupLearningModels
                         {
                             SetupLearningId = sl.SetupLearningId,
                             CourseId = sl.CourseId,
                             PopQuizId = sl.PopQuizId,
                             SetupModuleId = sl.SetupModuleId,
                             ApprovalStatus = sl.ApprovalStatus,
                             CreatedAt = sl.CreatedAt,
                             UpdatedAt = sl.UpdatedAt,
                             //IsPublished = sl.IsPublished,
                             LearningCategoryName = sl.LearningCategoryName,
                             ProgramTypeName = sl.ProgramTypeName,
                             LearningName = sl.CourseId == null ? (sl.SetupModuleId == null ? slpq.PopQuizName : slsmm.ModuleName) : slc.CourseName
                         }).AsNoTracking().AsQueryable();

            return query;
        }

        public async Task<SetupLearningPaginate> GetAllSetupLearning(SetupLearningGridFilter filter)
        {
            var query = JoinTable();

            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt < endDate) ||
                                         (Q.UpdatedAt >= startDate && Q.UpdatedAt < endDate));
            }

            if (filter.ProgramType != null)
            {
                query = query.Where(Q => Q.ProgramTypeName == filter.ProgramType);
            }

            if (filter.LearningCategory != null)
            {
                query = query.Where(Q => Q.LearningCategoryName == filter.LearningCategory);
            }

            if (filter.ProgramType != null)
            {
                query = query.Where(Q => Q.ProgramTypeName == filter.ProgramType);
            }

            if (filter.LearningName != null)
            {
                query = query.Where(Q => Q.LearningName.ToUpper().Contains(filter.LearningName.ToUpper()));
            }

            if (filter.ApprovalStatus != null)
            {
                query = query.Where(Q => Q.ApprovalStatus.ToUpper() == filter.ApprovalStatus.ToUpper());
            }

            //sort
            switch (filter.SortBy)
            {
                case "LearningNameAsc":
                    query = query.OrderBy(Q => Q.LearningName);
                    break;
                case "ProgramTypeNameAsc":
                    query = query.OrderBy(Q => Q.ProgramTypeName);
                    break;
                case "LearningCategoryNameAsc":
                    query = query.OrderBy(Q => Q.LearningCategoryName);
                    break;
                case "ApprovalStatusAsc":
                    query = query.OrderBy(Q => Q.ApprovalStatus);
                    break;
                case "CreatedAtAsc":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "UpdatedAtAsc":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "LearningNameDesc":
                    query = query.OrderByDescending(Q => Q.LearningName);
                    break;
                case "ProgramTypeNameDesc":
                    query = query.OrderByDescending(Q => Q.ProgramTypeName);
                    break;
                case "LearningCategoryNameDesc":
                    query = query.OrderByDescending(Q => Q.LearningCategoryName);
                    break;
                case "ApprovalStatusDesc":
                    query = query.OrderByDescending(Q => Q.ApprovalStatus);
                    break;
                case "CreatedAtDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "UpdatedAtDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }
            var setupLearning = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalData = await query.CountAsync();

            return new SetupLearningPaginate
            {
                Data = setupLearning,
                TotalData = totalData
            };
        }

        public async Task<List<string>> GetAllProgramType()
        {
            var data = await this._Db.ProgramTypes.Select(Q => Q.ProgramTypeName).ToListAsync();
            return data;
        }
        public async Task<List<string>> GetAllApprovalStatus()
        {
            var data = await this._Db.ApprovalStatus.Select(Q => Q.ApprovalName).ToListAsync();
            return data;
        }

        public async Task<SetupLearningCourseLockUnlock> GetAllCourseLockUnlock(int id)
        {

            var response =  new SetupLearningCourseLockUnlock();

            var setupLearning = await (
                    from c in this._Db.Courses
                    join sm in this._Db.SetupModules on c.CourseId equals sm.CourseId
                    join m in this._Db.Modules on sm.ModuleId equals m.ModuleId into sms
                    from m in sms.DefaultIfEmpty()
                    join a in this._Db.Assesments on sm.AssesmentId equals a.GUID into ams
                    from a in ams.DefaultIfEmpty()
                    join pt in this._Db.ProgramTypes on c.ProgramTypeId equals pt.ProgramTypeId
                    where c.CourseId == id && sm.IsDeleted == false
                    orderby sm.SetupModuleId
                    select new SetupLearningCourseLockUnlockGet
                    {
                        AssesmentGUID = a.GUID,
                        AssesmentName = a.Name,
                        CourseId = c.CourseId,
                        CourseName = c.CourseName,
                        ProgramTypeName = pt.ProgramTypeName,
                        ModuleId = m.ModuleId,
                        ModuleName = m.ModuleName,
                        IsPublishedModule = sm.IsPublished
                    }
                ).ToListAsync();

            if (setupLearning.Count > 0){
                var data = new SetupLearningCourseLockUnlock
                {
                    CourseId = setupLearning.FirstOrDefault().CourseId,
                    CourseName = setupLearning.FirstOrDefault().CourseName,
                    ProgramTypeName = setupLearning.FirstOrDefault().ProgramTypeName,
                    Module = setupLearning.Where(x=> x.ModuleId != null).Select(Q => new SetupLearningModuleLockUnlock
                    {
                        ModuleId = Q.ModuleId,
                        IsPublishedModule = Q.IsPublishedModule,
                        ModuleName = Q.ModuleName
                    }).ToList(),
                    Assesment = setupLearning.Where(x=> x.AssesmentGUID != null).Select(Q => new SetupLearningModuleLockUnlock
                    {
                        AssesmentId = Q.AssesmentGUID,
                        IsPublishedModule = Q.IsPublishedModule,
                        ModuleName = Q.AssesmentName
                    }).ToList(),
                };
                
            response = data;
            }

      
            return response;
        }

        /// <summary>
        /// Lock & unlock modules based on the requested course IDs.
        /// </summary>
        /// <param name="slclu">The requested object that store list of course ID.</param>
        /// <returns>Return true if success.</returns>
        public async Task<bool> CourseLockUnlockModule(SetupLearningCourseLockUnlock slclu)
        {
            var moduleIds = slclu
                .Module
                .Select(Q => Q.ModuleId)
                .ToList();

            var assesmentsIds = slclu.Assesment.Select(Q=> Q.AssesmentId).ToList();

            var data = await this
                ._Db
                .SetupModules
                .Where(Q => moduleIds.Contains(Q.ModuleId.Value)
                && Q.CourseId == slclu.CourseId
                && Q.IsDeleted == false)
                .ToListAsync();

                if (data.Count > 0){
                    for (var i = 0; i < data.Count(); i++)
                    {
                        data[i].IsPublished = slclu.Module[i].IsPublishedModule;
                    }
                }

              var dataAssesments = await this
                ._Db
                .SetupModules
                .Where(Q => assesmentsIds.Contains(Q.AssesmentId)
                && Q.CourseId == slclu.CourseId
                && Q.IsDeleted == false)
                .ToListAsync();

            if (dataAssesments.Count() > 0){
                for (var i = 0; i < dataAssesments.Count(); i++)
                {
                    dataAssesments[i].IsPublished = slclu.Assesment[i].IsPublishedModule;
                }
            }
            await this._Db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveCourse(int courseId)
        {
            try
            {
                // delete release training

                var trainingToDelete = await this._Db.Trainings.AsNoTracking().Where(Q => Q.CourseId == courseId).ToListAsync();

                for (int i = 0; i < trainingToDelete.Count(); i++)
                {
                    var approvalId3 = await this._Db.ApprovalToTrainings.Where(Q => Q.TrainingId == trainingToDelete[i].TrainingId).Select(Q => Q.ApprovalId).FirstOrDefaultAsync();

                    if (approvalId3 != 0)
                    {
                        var deleteModel2 = await this._Db.ApprovalToTrainings.FindAsync(approvalId3);
                        this._Db.ApprovalToTrainings.Remove(deleteModel2);

                        var deleteModel3 = await this._Db.Approvals.FindAsync(approvalId3);
                        this._Db.Approvals.Remove(deleteModel3);
                    }

                    var update = await this._Db.Trainings.FindAsync(trainingToDelete[i].TrainingId);

                    if (update.Top5Course != 0)
                    {
                        update.Top5Course = 0;
                        var findTop5 = await this._Db.Trainings.Where(Q => Q.IsDeleted == false && Q.Top5Course != 0).Take(5).OrderBy(Q => Q.Top5Course).ToListAsync();
                        findTop5.Remove(update);
                        var currentPost = 5;
                        for (int a = 0; a < findTop5.Count; a++)
                        {
                            findTop5[a].Top5Course = currentPost;
                            currentPost--;
                        }
                    }

                    update.IsDeleted = true;
                    update.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
                    update.UpdatedBy = GetUserLogin();
                }

                await this._Db.SaveChangesAsync();

                var approvalToSetupCourse = await this._Db.ApprovalToSetupCourses.Where(Q => Q.CourseId == courseId).FirstOrDefaultAsync();

                if (approvalToSetupCourse == null)
                {
                    return false;
                }
                this._Db.ApprovalToSetupCourses.Remove(approvalToSetupCourse);
                var isDeleted = await this.ApprovalMan.DeleteApproval(approvalToSetupCourse.ApprovalId);

                if (isDeleted == false)
                {
                    return false;
                }

                //Hard delete di Setup Learning
                var findSetupLearning = await this._Db.SetupLearning.Where(Q => Q.CourseId == courseId).FirstOrDefaultAsync();
                this._Db.SetupLearning.Remove(findSetupLearning);

                var setupModule = await this._Db.SetupModules.Where(Q => Q.CourseId == courseId).ToListAsync();
                setupModule.ForEach(Q => Q.IsDeleted = true);

                var setupCourse = await this._Db.Courses.Where(Q => Q.CourseId == courseId).FirstOrDefaultAsync();
                setupCourse.SetupCourseCreatedAt = null;
                setupCourse.SetupCourseCreatedBy = null;

                //Hard delete di PrerequisiteMapping

                var findPrerequisite = await this._Db.CoursePrerequisiteMappings.Where(Q => Q.PrevCourseId == courseId || Q.NextCourseId == courseId).ToListAsync();
                this._Db.CoursePrerequisiteMappings.RemoveRange(findPrerequisite);

                await this._Db.SaveChangesAsync();
            }
            catch(Exception e)
            {
                var error = e.InnerException;
                return false;
            }
            return true;
        }
    }
}
