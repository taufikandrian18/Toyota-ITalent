using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class EvaluationTypesService
    {
        private readonly TalentContext _Db;

        public EvaluationTypesService(TalentContext talentContext)
        {
            this._Db = talentContext;
        }

        public async Task<List<EvaluationTypesViewModel>> GetAllEvaluationTypesAsync()
        {
            return await this._Db.EvaluationTypes.Select(Q => new EvaluationTypesViewModel
            {
                EvaluationTypeId = Q.EvaluationTypeId,
                EvaluationTypeName = Q.EvaluationTypeName
            }).AsNoTracking().ToListAsync();
        }
    }
}
