using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class ProgramTypeService
    {
        private readonly TalentContext DB;

        public ProgramTypeService(TalentContext db)
        {
            this.DB = db;
        }

        public async Task<ProgramTypeViewModel> GetAllProgramTypes()
        {

            var allProgramTypes = await this.DB.ProgramTypes.Select(Q => new ProgramTypeModel
            {
                ProgramTypeId = Q.ProgramTypeId,
                ProgramTypeName = Q.ProgramTypeName
            }).ToListAsync();

            var totalItem = await this.DB.ProgramTypes.CountAsync();

            var result = new ProgramTypeViewModel
            {
                ListProgramTypeModel = allProgramTypes,
                TotalItem = totalItem
            };

            return result;
        }

        public async Task<ProgramTypeModel> GetProgramTypeById(int id)
        {
            var result = await DB.ProgramTypes.AsNoTracking()
                .Select(Q => new ProgramTypeModel
                {
                    ProgramTypeId = Q.ProgramTypeId,
                    ProgramTypeName = Q.ProgramTypeName
                })
                .Where(Q => Q.ProgramTypeId == id).FirstOrDefaultAsync(); //Q.IsDeleted == false && 

            if (result == null)
            {
                result = new ProgramTypeModel();
            }

            return result;
        }
    }
}
