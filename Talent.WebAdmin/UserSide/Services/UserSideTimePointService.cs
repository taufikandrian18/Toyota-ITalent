using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideTimePointService
    {
        private readonly TalentContext _Db;

        public UserSideTimePointService(TalentContext talentContext)
        {
            this._Db = talentContext;
        }

        public async Task<int> GetPointByTimePointId(int id)
        {
            var point = await _Db.TimePoints.Where(Q => Q.TimePointId == id).Select(Q => Q.Points).FirstOrDefaultAsync();
            return point;
        }
    }
}
