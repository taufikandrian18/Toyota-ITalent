using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class LevelService
    {
        private readonly TalentContext DB;

        public LevelService(TalentContext db)
        {
            this.DB = db;
        }

        public async Task<LevelViewModel> GetAllLevels()
        {

            var allLevels = await this.DB.Levels.Select(Q => new LevelModel
            {
                LevelId = Q.LevelId,
                LevelName = Q.LevelName
            }).ToListAsync();

            var totalItem = await this.DB.Levels.CountAsync();

            var result = new LevelViewModel
            {
                ListLevelModel = allLevels,
                TotalItem = totalItem
            };

            return result;
        }

        public async Task<LevelModel> GetLevelById(int id)
        {
            var result = await DB.Levels.AsNoTracking()
                .Select(Q => new LevelModel
                {
                    LevelId = Q.LevelId,
                    LevelName = Q.LevelName
                })
                .Where(Q => Q.LevelId == id).FirstOrDefaultAsync(); //Q.IsDeleted == false && 

            if (result == null)
            {
                result = new LevelModel();
            }

            return result;
        }
    }
}
