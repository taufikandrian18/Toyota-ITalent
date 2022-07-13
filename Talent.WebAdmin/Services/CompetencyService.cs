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
    public class CompetencyService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        private string UserLogin { get; set; }

        public CompetencyService(TalentContext talentContext, ClaimHelper claim)
        {
            this.DB = talentContext;
            this.ClaimMan = claim;
        }

        public async Task<int> CreateCompetency(CompetencyFormModel model)
        {
            var createModel = new Competencies
            {
                CompetencyTypeId = model.CompetencyTypeId,
                //KeyActionId = model.KeyActionId,
                PrefixCode = model.PrefixCode,
                CompetencyName = model.CompetencyName,
                CompetencyDescription = model.CompetencyDescription,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = ClaimMan.GetLoginUserId(),
                UpdatedBy = ClaimMan.GetLoginUserId(),
            };
            this.DB.Competencies.Add(createModel);

            var listCompetencyKeyActionMappings = new List<CompetencyKeyActionMappings>();
            foreach (CompetencyKeyActionMappingModel ck in model.CompetencyKeyActionMappings)
            {
                listCompetencyKeyActionMappings.Add(new CompetencyKeyActionMappings
                {
                    CompetencyId = createModel.CompetencyId,
                    KeyActionId = ck.KeyActionId
                });
            }

            this.DB.CompetencyKeyActionMappings.AddRange(listCompetencyKeyActionMappings);
            await this.DB.SaveChangesAsync();
            
            return createModel.CompetencyId;
        }

        //TO DO : Erlina > KeyActionId
        public async Task<bool> UpdateCompetency(CompetencyFormModel model)
        {
            var updateModel = await this.DB.Competencies.FindAsync(model.CompetencyId);

            if (updateModel == null)
            {
                return false;
            }

            updateModel.CompetencyTypeId = model.CompetencyTypeId;
            updateModel.PrefixCode = model.PrefixCode;
            updateModel.CompetencyName = model.CompetencyName;
            updateModel.CompetencyDescription = model.CompetencyDescription;
            updateModel.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            updateModel.UpdatedBy = ClaimMan.GetLoginUserId();
            await this.DB.SaveChangesAsync();
            
            var dataToDelete = await DB.CompetencyKeyActionMappings.AsNoTracking().Where(Q => Q.CompetencyId == model.CompetencyId).ToListAsync();
            DB.CompetencyKeyActionMappings.RemoveRange(dataToDelete);

            var listCompetencyKeyActionMappings = new List<CompetencyKeyActionMappings>();
            foreach (CompetencyKeyActionMappingModel ck in model.CompetencyKeyActionMappings)
            {
                listCompetencyKeyActionMappings.Add(new CompetencyKeyActionMappings
                {
                    CompetencyId = ck.CompetencyId,
                    KeyActionId = ck.KeyActionId
                });
            }

            this.DB.CompetencyKeyActionMappings.AddRange(listCompetencyKeyActionMappings);
            await this.DB.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCompetency(int id)
        {
            var dataToDelete = await DB.CompetencyKeyActionMappings.AsNoTracking().Where(Q => Q.CompetencyId == id).ToListAsync();
            DB.CompetencyKeyActionMappings.RemoveRange(dataToDelete);

            var deleteModel = await this.DB.Competencies.FindAsync(id);

            deleteModel.IsDeleted = true;

            await this.DB.SaveChangesAsync();
            return true;
        }

        //TO DO : Erlina > KeyActionId
        public async Task<CompetencyViewModel> GetAllCompetencies()
        {
            var allCompetencies = await this.DB.Competencies.AsNoTracking().Select(Q => new CompetencyModel
            {
                CompetencyId = Q.CompetencyId,
                CompetencyTypeId = Q.CompetencyTypeId,
                PrefixCode = Q.PrefixCode,
                CompetencyName = Q.CompetencyName,
                CompetencyDescription = Q.CompetencyDescription,
                CreatedAt = Q.CreatedAt,
                UpdatedAt = Q.UpdatedAt
            }).ToListAsync();

            var totalItem = await this.DB.Competencies.CountAsync();

            var result = new CompetencyViewModel
            {
                ListCompetencyModel = allCompetencies,
                TotalItem = totalItem
            };

            return result;
        }

        //TO DO : Erlina > KeyActionId
        /// <summary>
        /// all competency without pagination & key action & competencytype
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompetencyModel>> GetAllCompetenciesAsync()
        {
            var data = await (
                from ct in this.DB.CompetencyTypes 
                join c in this.DB.Competencies on ct.CompetencyTypeId equals c.CompetencyTypeId
                where c.IsDeleted == false
                select new CompetencyModel
                {
                    CompetencyId = c.CompetencyId,
                    CompetencyName = c.CompetencyName,
                    CompetencyDescription = c.CompetencyDescription,
                    CompetencyTypeId = c.CompetencyTypeId,
                    PrefixCode = ct.CompetencyTypeName.Substring(0, 1) + '-' + c.PrefixCode,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                }).AsNoTracking().ToListAsync();

            return data;
        }

        /// <summary>
        /// competency without key action & competencytype
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        //TO DO : Erlina > KeyActionId
        public async Task<CompetencyModel> GetCompetencyByIdAsync(int id)
        {
            var data = await (
                from ct in this.DB.CompetencyTypes
                join c in this.DB.Competencies on ct.CompetencyTypeId equals c.CompetencyTypeId
                where c.CompetencyId == id
                select new CompetencyModel
                {
                    CompetencyId = c.CompetencyId,
                    CompetencyName = c.CompetencyName,
                    CompetencyDescription = c.CompetencyDescription,
                    CompetencyTypeId = c.CompetencyTypeId,
                    PrefixCode = ct.CompetencyTypeName.Substring(0, 1) + '-' + c.PrefixCode,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                }).AsNoTracking().FirstOrDefaultAsync();

            return data;
        }
    }

    public class CompetencyJoinService
    {
        private readonly TalentContext DB;

        public CompetencyJoinService(TalentContext db)
        {
            this.DB = db;
        }

        public async Task<CompetencyJoinViewModel> GetAllCompetencies(CompetencyFilter filter)
        {
            var query = (
                from ck in DB.CompetencyKeyActionMappings
                join c in DB.Competencies on ck.CompetencyId equals c.CompetencyId
                join ct in DB.CompetencyTypes on c.CompetencyTypeId equals ct.CompetencyTypeId
                join k in DB.KeyActions on ck.KeyActionId equals k.KeyActionId into emptyKeyAction
                from ek in emptyKeyAction.DefaultIfEmpty()
                select new
                {
                    c.CompetencyId,
                    c.CompetencyTypeId,
                    ct.CompetencyTypeName,
                    ck.KeyActionId,
                    KeyActionCode = ek == null ? "[Removed]" : ek.KeyActionCode,
                    c.PrefixCode,
                    c.CompetencyName,
                    c.CompetencyDescription,
                    c.CreatedAt,
                    c.UpdatedAt
                }).Select(Q => new CompetencyJoinModel
                {
                    CompetencyId = Q.CompetencyId,
                    CompetencyTypeId = Q.CompetencyTypeId,
                    CompetencyTypeName = Q.CompetencyTypeName,
                    KeyActionCode = Q.KeyActionCode,
                    PrefixCode = Q.PrefixCode,
                    CompetencyName = Q.CompetencyName,
                    CompetencyDescription = Q.CompetencyDescription,
                    CreatedAt = Q.CreatedAt,
                    UpdatedAt = Q.UpdatedAt,
                });

            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt < endDate) ||
                                         (Q.UpdatedAt >= startDate && Q.UpdatedAt < endDate));
            }

            if (string.IsNullOrEmpty(filter.CompetencyName) == false)
            {
                query = query.Where(Q => Q.CompetencyName.ToLower().Contains(filter.CompetencyName.ToLower()));
            }

            if (string.IsNullOrEmpty(filter.PrefixCode) == false)
            {
                query = query.Where(Q => (Q.CompetencyTypeName.Substring(0, 1) + "-" + Q.PrefixCode).ToLower().Contains(filter.PrefixCode.ToLower()));
            }

            if (string.IsNullOrEmpty(filter.KeyActionCode) == false)
            {
                query = query.Where(Q => Q.KeyActionCode.Contains(filter.KeyActionCode));
            }

            query = query.OrderByDescending(Q => Q.UpdatedAt);

            switch (filter.SortBy)
            {
                case "prefixCode":
                    query = query.OrderBy(Q => string.Concat(Q.CompetencyTypeName, Q.PrefixCode)).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "prefixCodeDesc":
                    query = query.OrderByDescending(Q => string.Concat(Q.CompetencyTypeName, Q.PrefixCode)).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "competencyName":
                    query = query.OrderBy(Q => Q.CompetencyName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "competencyNameDesc":
                    query = query.OrderByDescending(Q => Q.CompetencyName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "keyActionCode":
                    query = query.OrderBy(Q => Q.KeyActionCode).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "keyActionCodeDesc":
                    query = query.OrderByDescending(Q => Q.KeyActionCode).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "createdDate":
                    query = query.OrderBy(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "createdDateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "updatedDate":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "updatedDateDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var resultQuery = await query.Skip((filter.PageNumber - 1) * 10).Take(10).ToListAsync();
            var totalData = await query.CountAsync();

            var result = new CompetencyJoinViewModel
            {
                ListCompetencyJoinModel = resultQuery,
                TotalItem = totalData
            };

            return result;
        }

        public async Task<CompetencyJoinModel> GetCompetencyJoinById(int id)
        {
            var result = await (
                from c in DB.Competencies.AsNoTracking()
                join ct in DB.CompetencyTypes.AsNoTracking() on c.CompetencyTypeId equals ct.CompetencyTypeId
                where c.CompetencyId == id
                select new CompetencyJoinModel
                {
                    CompetencyId = c.CompetencyId,
                    CompetencyTypeId = c.CompetencyTypeId,
                    CompetencyTypeName = ct.CompetencyTypeName,
                    PrefixCode = c.PrefixCode,
                    CompetencyName = c.CompetencyName,
                    CompetencyDescription = c.CompetencyDescription,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                }).FirstOrDefaultAsync();

            if (result == null)
            {
                result = new CompetencyJoinModel();
            }

            result.CompetencyKeyActionMappings = await (
                from ck in DB.CompetencyKeyActionMappings
                join k in DB.KeyActions on ck.KeyActionId equals k.KeyActionId
                where ck.CompetencyId == id
                select new CompetencyKeyActionMappingJoinModel
                {
                    KeyActionId = k.KeyActionId,
                    KeyActionCode = k.KeyActionCode
                }
            ).ToListAsync();

            return result;
        }
    }
}
