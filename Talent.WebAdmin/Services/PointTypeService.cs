using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class PointTypeService
    {
        private readonly TalentContext _Db;

        public PointTypeService(TalentContext talentContext)
        {
            this._Db = talentContext;
        }

        public async Task<List<PointTypeViewModel>> GetAllPointType()
        {
            return await this._Db.PointTypes.Select(Q => new PointTypeViewModel
            {
                PointTypeId = Q.PointTypeId,
                PointTypeName = Q.PointTypeName
            }).AsNoTracking().ToListAsync();
        }

        public async Task<string> GetPointTypeById(int id)
        {
            return await this._Db.PointTypes.Where(Q => Q.PointTypeId == id).Select(Q => Q.PointTypeName).FirstOrDefaultAsync();
        }
    }
}
