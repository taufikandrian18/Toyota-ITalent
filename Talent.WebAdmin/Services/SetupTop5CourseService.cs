using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class SetupTop5CourseService
    {
        private readonly TalentContext _Db;

        public SetupTop5CourseService(TalentContext talentContext)
        {
            this._Db = talentContext;
        }

        public IQueryable<SetupTop5CourseJoinedModel> SetupTop5CourseJoinTable()
        {
            var query = (from t in this._Db.Trainings
                         join c in this._Db.Courses on t.CourseId equals c.CourseId
                         join sl in this._Db.SetupLearning on t.CourseId equals sl.CourseId
                         select new SetupTop5CourseJoinedModel
                         {
                             TrainingId = t.TrainingId,
                             CourseId = t.CourseId,
                             CourseName = c.CourseName,
                             ProgramTypeName = sl.ProgramTypeName,
                             CreatedBy = t.CreatedBy,
                             Date = t.UpdatedAt,
                             IsDeleted = t.IsDeleted,
                             Batch = t.Batch,
                             ApprovedAt = t.ApprovedAt,
                             Top5Course = t.Top5Course
                         }).AsNoTracking().AsQueryable();

            return query;
        }

        public async Task<SetupTop5CourseViewModel> GetAllSetupTop5CoursePaginate(SetupTop5CourseSearch search)
        {
            var query = SetupTop5CourseJoinTable();

            if (string.IsNullOrEmpty(search.CourseName) == false)
            {
                query = query.Where(Q => Q.CourseName.Contains(search.CourseName));
            }

            var resultQuery = await query.Where(Q => Q.IsDeleted == false && Q.ApprovedAt != null && Q.Top5Course > 0).OrderBy(Q => Q.Top5Course).ToListAsync();
            var totalData = resultQuery.Count();

            var setupTop5CourseResult = new SetupTop5CourseViewModel
            {
                ListSetupTop5Course = resultQuery,
                TotalItem = totalData
            };

            return setupTop5CourseResult;
        }

        //TODO: NANDO
        public async Task<SetupTop5CourseDropdownList> GetCourseNameDropdown()
        {
            var query = (from t in this._Db.Trainings
                         join c in this._Db.Courses on t.CourseId equals c.CourseId
                         select new SetupTop5CourseDropdownData
                         {
                             TrainingId = t.TrainingId,
                             CourseName = c.CourseName,
                             CourseId = t.CourseId,
                             IsDeleted = t.IsDeleted,
                             ApprovedAt = t.ApprovedAt,
                             Top5Course = t.Top5Course,
                             Batch = t.Batch
                         }).AsNoTracking().AsQueryable();

            var resultQuery = await query.Where(Q => Q.IsDeleted == false && Q.ApprovedAt != null && Q.Top5Course == 0).OrderBy(Q => Q.CourseName).ToListAsync();

            var setupTop5CourseResult = new SetupTop5CourseDropdownList
            {
                CourseNameDropdown = resultQuery
            };

            return setupTop5CourseResult;
        }

        public async Task<bool> InsertIntoTop5Course(SetupTop5CourseDropdownData model)
        {
            var query = (from t in this._Db.Trainings
                         select new SetupTop5CourseDropdownData
                         {
                             Top5Course = t.Top5Course
                         }).AsNoTracking().AsQueryable();

            var getTop5CourseTotalData = await query.Where(Q => Q.Top5Course > 0).CountAsync();
            var currentCount = 5 - getTop5CourseTotalData;

            var searchData = await this._Db.Trainings.Where(Q => Q.TrainingId == model.TrainingId && Q.Batch == model.Batch).FirstOrDefaultAsync();

            if (getTop5CourseTotalData < 5)
            {
                searchData.Top5Course = currentCount;
            }
            else
            {
                var top5CourseList = await this._Db.Trainings.Where(Q => Q.Top5Course > 0).ToListAsync();

                var removeFromTop5Course = top5CourseList.Where(Q => Q.Top5Course == 5).FirstOrDefault();
                top5CourseList.Remove(removeFromTop5Course);
                removeFromTop5Course.Top5Course = 0;
                searchData.Top5Course = 1;

                for (int i = 0; i < 4; i++)
                {
                    top5CourseList[i].Top5Course += 1;
                }
            }

            await this._Db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RearrangeTop5Course(SetupTop5CourseViewModel model)
        {
            var newTop5CourseList = await this._Db.Trainings.Where(Q => Q.Top5Course > 0).ToListAsync();

            for (int i = 0; i < model.ListSetupTop5Course.Count(); i++)
            {
                var updateTop5CourseData = newTop5CourseList.Where(Q => Q.TrainingId == model.ListSetupTop5Course[i].TrainingId).FirstOrDefault();
                updateTop5CourseData.Top5Course = i + 1;
            }

            await this._Db.SaveChangesAsync();
            return true;
        }
    }
}
