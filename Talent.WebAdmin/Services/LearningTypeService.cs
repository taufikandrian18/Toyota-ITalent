using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class LearningTypeService
    {
        private readonly TalentContext DB;

        public LearningTypeService(TalentContext db)
        {
            this.DB = db;
        }

        public async Task<LearningTypeViewModel> GetAllLearningTypes()
        {

            var allLearningTypes = await this.DB.LearningTypes.Select(Q => new LearningTypeModel
            {
                LearningTypeId = Q.LearningTypeId,
                LearningName = Q.LearningName
            }).ToListAsync();

            var totalItem = await this.DB.CompetencyTypes.CountAsync();

            var result = new LearningTypeViewModel
            {
                ListLearningTypeModel = allLearningTypes,
                TotalItem = totalItem
            };

            return result;
        }

        public async Task<LearningTypeModel> GetLearningTypeById(int id)
        {
            var result = await DB.LearningTypes.AsNoTracking()
                .Select(Q => new LearningTypeModel
                {
                    LearningTypeId = Q.LearningTypeId,
                    LearningName = Q.LearningName
                })
                .Where(Q => Q.LearningTypeId == id).FirstOrDefaultAsync(); //Q.IsDeleted == false && 

            if (result == null)
            {
                result = new LearningTypeModel();
            }

            return result;
        }
    }
}
