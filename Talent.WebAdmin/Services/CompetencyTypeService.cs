using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class CompetencyTypeService
    {
        private readonly TalentContext DB;

        public CompetencyTypeService(TalentContext db)
        {
            this.DB = db;
        }

        public async Task<CompetencyTypeViewModel> GetAllCompetencyTypes()
        {

            var allCompetencyTypes = await this.DB.CompetencyTypes.Select(Q => new CompetencyTypeModel
            {
                CompetencyTypeId = Q.CompetencyTypeId,
                CompetencyTypeName = Q.CompetencyTypeName
            }).ToListAsync();

            var totalItem = await this.DB.CompetencyTypes.CountAsync();

            var result = new CompetencyTypeViewModel
            {
                ListCompetencyTypeModel = allCompetencyTypes,
                TotalItem = totalItem
            };

            return result;
        }

        public async Task<CompetencyTypeModel> GetCompetencyTypeById(int id)
        {
            var result = await DB.CompetencyTypes.AsNoTracking()
                .Select(Q => new CompetencyTypeModel
                {
                    CompetencyTypeId = Q.CompetencyTypeId,
                    CompetencyTypeName = Q.CompetencyTypeName
                })
                .Where(Q => Q.CompetencyTypeId == id).FirstOrDefaultAsync(); //Q.IsDeleted == false && 

            if (result == null)
            {
                return new CompetencyTypeModel();
            }

            return result;
        }
    }
}
