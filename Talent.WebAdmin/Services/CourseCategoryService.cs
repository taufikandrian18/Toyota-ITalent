using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class CourseCategoryService
    {
        private readonly TalentContext DB;

        public CourseCategoryService(TalentContext db)
        {
            this.DB = db;
        }

        public async Task<CourseCategoryViewModel> GetAllCourseCategories()
        {

            var allCourseCategories = await this.DB.CourseCategories.Select(Q => new CourseCategoryModel
            {
                CourseCategoryId = Q.CourseCategoryId,
                CourseCategoryName = Q.CourseCategoryName
            }).ToListAsync();

            var totalItem = await this.DB.CourseCategories.CountAsync();

            var result = new CourseCategoryViewModel
            {
                ListCourseCategoryModel = allCourseCategories,
                TotalItem = totalItem
            };

            return result;
        }

        public async Task<CourseCategoryModel> GetCourseCategoryById(int id)
        {
            var result = await DB.CourseCategories.AsNoTracking()
                .Select(Q => new CourseCategoryModel
                {
                    CourseCategoryId = Q.CourseCategoryId,
                    CourseCategoryName = Q.CourseCategoryName
                })
                .Where(Q => Q.CourseCategoryId == id).FirstOrDefaultAsync(); //Q.IsDeleted == false && 

            if (result == null)
            {
                result = new CourseCategoryModel();
            }

            return result;
        }
    }
}
