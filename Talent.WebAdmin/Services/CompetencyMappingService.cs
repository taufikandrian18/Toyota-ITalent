using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class CompetencyMappingService
    {
        private readonly TalentContext _Db;
        private readonly ClaimHelper ClaimMan;

        public CompetencyMappingService(TalentContext talentContext, ClaimHelper claimHelper)
        {
            this._Db = talentContext;
            this.ClaimMan = claimHelper;
        }

        /// <summary>
        /// Query untuk menjoin table competencies, competency types, competency evalutaion mappings dan evaluation types
        /// </summary>
        /// <returns></returns>
        public IQueryable<CompetencyMapping> JoinTable()
        {
            var query = (from c in this._Db.Competencies
                         join ct in this._Db.CompetencyTypes on c.CompetencyTypeId equals ct.CompetencyTypeId
                         join cem in this._Db.CompetencyEvaluationMappings on c.CompetencyId equals cem.CompetencyId
                         join et in this._Db.EvaluationTypes on cem.EvaluationTypeId equals et.EvaluationTypeId
                         select new CompetencyMapping
                         {
                             CompetencyId = c.CompetencyId,
                             EvaluationTypeId = et.EvaluationTypeId,
                             CompetencyMappingCode = (ct.CompetencyTypeName.Substring(0, 1)) + "-" + c.PrefixCode + "-" + et.EvaluationTypeName,
                             CompetencyName = c.CompetencyName,
                             TypeofEvaluation = et.EvaluationTypeName,
                             BonusScoreLt50 = cem.BonusScoreLt50,
                             BonusScoreMte50 = cem.BonusScoreMte50,
                             CreatedAt = cem.CreatedAt,
                             UpdatedAt = cem.UpdatedAt
                         }).AsNoTracking().AsQueryable();
            return query;
        }

        /// <summary>
        /// Function untuk menampilkan data berdasarkan parameter search yang sudah di paginate
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<CompetencyMappingPaginate> GetAllCompetencyMappingPaginateAsync(CompetencyMappingGridFilter filter)
        {

            var query = JoinTable();

            //search
            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt <= endDate) || (Q.UpdatedAt >= startDate && Q.UpdatedAt <= endDate));
            }

            if (filter.CompetencyName != null)
            {
                query = query.Where(Q => Q.CompetencyName.Contains(filter.CompetencyName));
            }

            if (filter.CompetencyMappingCode != null)
            {
                query = query.Where(Q => Q.CompetencyMappingCode.Contains(filter.CompetencyMappingCode));
            }

            if (filter.TypeofEvaluation != null)
            {
                query = query.Where(Q => Q.TypeofEvaluation.Contains(filter.TypeofEvaluation));
            }

            //sort
            switch (filter.SortBy)
            {
                case "CompetencyMappingCodeAsc":
                    query = query.OrderBy(Q => Q.CompetencyMappingCode);
                    break;
                case "CompetencyNameAsc":
                    query = query.OrderBy(Q => Q.CompetencyName);
                    break;
                case "TypeofEvaluationAsc":
                    query = query.OrderBy(Q => Q.TypeofEvaluation);
                    break;
                case "CreatedDateAsc":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "UpdatedDateAsc":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;

                case "CompetencyMappingCodeDesc":
                    query = query.OrderByDescending(Q => Q.CompetencyMappingCode);
                    break;
                case "CompetencyNameDesc":
                    query = query.OrderByDescending(Q => Q.CompetencyName);
                    break;
                case "TypeofEvaluationDesc":
                    query = query.OrderByDescending(Q => Q.TypeofEvaluation);
                    break;
                case "CreatedDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "UpdatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var competencyMapping = await query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalData = await query.CountAsync();

            return new CompetencyMappingPaginate
            {
                Data = competencyMapping,
                TotalData = totalData
            };
        }

        public async Task<bool> CompetencyEvaluationMappingsIsExist(int CompetencyId, int EvaluationTypeId)
        {
            var data = await this._Db.CompetencyEvaluationMappings.FirstOrDefaultAsync(Q => Q.CompetencyId == CompetencyId && Q.EvaluationTypeId == EvaluationTypeId);

            if(data == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Blom FIX Bagian competencymappingcode
        /// </summary>
        /// <param name="insert"></param>
        /// <returns></returns>
        public async Task<bool> InsertCompetencyMappingAsync(CompetencyMappingInsertModel insert)
        {
            if(insert.BonusScoreLt50 < 0 || insert.BonusScoreMte50 < 0)
            {
                return false;
            }

            var exist = await this.CompetencyEvaluationMappingsIsExist(insert.CompetencyId, insert.EvaluationTypeId);

            if (exist == true)
            {
                return false;
            }

            var thisDate = DateTime.UtcNow.AddHours(7);

            await this._Db.CompetencyEvaluationMappings.AddAsync(new CompetencyEvaluationMappings
            {
                CompetencyId = insert.CompetencyId,
                EvaluationTypeId = insert.EvaluationTypeId,
                BonusScoreLt50 = insert.BonusScoreLt50,
                BonusScoreMte50 = insert.BonusScoreMte50,
                CreatedAt = thisDate,
                CreatedBy = ClaimMan.GetLoginUserId(),
                UpdatedAt = thisDate,
                UpdatedBy = ClaimMan.GetLoginUserId(),
            });
            await this._Db.SaveChangesAsync();

            return true;
        }

        public async Task<CompetencyMapping> GetAllCompetencyMappingByCompetencyIdandEvaluationIdPaginateAsync(CompetencyMappingGetDataGridFilter filter)
        {
            var query = JoinTable();

            var data = await query.Where(Q => Q.CompetencyId == filter.CompetencyId && Q.EvaluationTypeId == filter.EvaluationTypeId).AsNoTracking().FirstOrDefaultAsync();

            return data;
        }

        public async Task<bool> UpdateCompetencyMappingAsync(CompetencyMappingGetDataGridFilter filter, CompetencyMappingInsertModel update)
        {
            var thisDate = DateTime.UtcNow.AddHours(7);
            if (filter.CompetencyId == update.CompetencyId && filter.EvaluationTypeId == update.EvaluationTypeId)
            {
                if (update.BonusScoreLt50 < 0 || update.BonusScoreMte50 < 0)
                {
                    return false;
                }
                var dataSame = await this._Db.CompetencyEvaluationMappings
                .Where(Q => Q.CompetencyId == filter.CompetencyId && Q.EvaluationTypeId == filter.EvaluationTypeId).FirstOrDefaultAsync();

                dataSame.BonusScoreLt50 = update.BonusScoreLt50;
                dataSame.BonusScoreMte50 = update.BonusScoreMte50;
                dataSame.UpdatedAt = thisDate;
                await this._Db.SaveChangesAsync();
                return true;
            }

            if (update.BonusScoreLt50 < 0 || update.BonusScoreMte50 < 0)
            {
                return false;
            }

            var exist = await this.CompetencyEvaluationMappingsIsExist(update.CompetencyId, update.EvaluationTypeId);
            if (exist == true)
            {
                return false;
            }

            var data = await this._Db.CompetencyEvaluationMappings
                .Where(Q => Q.CompetencyId == filter.CompetencyId && Q.EvaluationTypeId == filter.EvaluationTypeId).FirstOrDefaultAsync();

            this._Db.Remove(data);
            await this._Db.SaveChangesAsync();



            data.CompetencyId = update.CompetencyId;
            data.EvaluationTypeId = update.EvaluationTypeId;
            data.BonusScoreLt50 = update.BonusScoreLt50;
            data.BonusScoreMte50 = update.BonusScoreMte50;
            data.UpdatedAt = thisDate;
            data.CreatedAt = data.CreatedAt;
            data.UpdatedBy = ClaimMan.GetLoginUserId();

            this._Db.Add(data);
            await this._Db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCompetencyMappingAsync(CompetencyMappingGetDataGridFilter filter)
        {
            var delete = await this._Db.CompetencyEvaluationMappings.Where(Q => Q.CompetencyId == filter.CompetencyId && Q.EvaluationTypeId == filter.EvaluationTypeId).FirstOrDefaultAsync();

            //delete.IsDeleted = true;
            this._Db.CompetencyEvaluationMappings.Remove(delete);
            await this._Db.SaveChangesAsync();

            return true;
        }
    }
}

