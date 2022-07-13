using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class GuideTypeService
    {
        private readonly TalentContext DB;

        public GuideTypeService(TalentContext db)
        {
            this.DB = db;
        }

        public async Task<GuideTypeViewModel> GetAllGuideTypes()
        {

            var allGuideTypes = await this.DB.GuideTypes.Select(Q => new GuideTypeModel
            {
                GuideTypeId = Q.GuideTypeId,
                Name = Q.Name
            }).ToListAsync();

            var totalItem = await this.DB.GuideTypes.CountAsync();

            var result = new GuideTypeViewModel
            {
                ListGuideTypeModel = allGuideTypes,
                TotalItem = totalItem
            };

            return result;
        }

        public async Task<GuideTypeModel> GetGuideTypeById(int id)
        {
            var result = await DB.GuideTypes.AsNoTracking()
                .Select(Q => new GuideTypeModel
                {
                    GuideTypeId = Q.GuideTypeId,
                    Name = Q.Name
                })
                .Where(Q => Q.GuideTypeId == id).FirstOrDefaultAsync(); //Q.IsDeleted == false && 

            if (result == null)
            {
                result = new GuideTypeModel();
            }

            return result;
        }
    }
}
