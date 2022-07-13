using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class PositionMappingService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;

        public PositionMappingService(TalentContext db, ClaimHelper claimHelper)
        {
            this.DB = db;
            this.ClaimMan = claimHelper;
        }

        public async Task<ResponsePositionMapModel> GetPositionMapPaginateAsync(PositionMapFilterModel filter)
        {
            var query = DB.PositionCompentencyMappings
                        .Select(Q => new PositionMapViewModel
                            {
                                PosCompetencyId = Q.PositionCompentencyMappingId,
                                PositionId = Q.PositionId,
                                PositionName = Q.Position.PositionName,
                                CompetencyName = Q.Competency.CompetencyName,
                                Priority = Q.Priority,
                                ProficiencyLevel = Q.ProficiencyLevel,
                                CreatedAt = Q.CreatedAt,
                                UpdateAt = Q.UpdatedAt
                            });

            var totalData2 = await query.CountAsync();

            //filtering section
            //PositionName
            if ((string.IsNullOrEmpty(filter.PositionName) || string.IsNullOrWhiteSpace(filter.PositionName)) == false)
            {
                query = query.Where(Q => Q.PositionName.ToLower().Contains(filter.PositionName.ToLower()));
            }
            // CompetencyName
            if ((string.IsNullOrEmpty(filter.CompetencyName)) == false)
            {
                query = query.Where(Q => Q.CompetencyName.ToLower().Contains(filter.CompetencyName.ToLower()));
            }
            // date
            if (filter.StartDate != null && filter.EndDate != null)
            {
                var newStartDate = filter.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

                var newEndDate = filter.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
                var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

                query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt < endDate) ||
                                         (Q.UpdateAt >= startDate && Q.UpdateAt < endDate));
            }
            //Priority
            if ((string.IsNullOrEmpty(filter.Priority) || string.IsNullOrWhiteSpace(filter.Priority)) == false)
            {
                query = query.Where(Q => Q.Priority.ToLower().Contains(filter.Priority.ToLower()));
            }
            //ProficiencyLevel
            if (filter.ProficiencyLevel > 0)
            {
                query = query.Where(Q => Q.ProficiencyLevel == filter.ProficiencyLevel);
            }

            // --------------- Order by ------------------

            switch (filter.OrderBy)
            {
                case "PositionNameUp":
                    {
                        query = query.OrderByDescending(Q => Q.PositionName);
                        break;
                    }

                case "PositionNameDown":
                    {
                        query = query.OrderBy(Q => Q.PositionName);
                        break;
                    }

                case "CompetencyNameUp":
                    {
                        query = query.OrderByDescending(Q => Q.CompetencyName);
                        break;
                    }

                case "CompetencyNameDown":
                    {
                        query = query.OrderBy(Q => Q.CompetencyName);
                        break;
                    }

                case "PriorityUp":
                    {
                        query = query.OrderByDescending(Q => Q.Priority);
                        break;
                    }

                case "PriorityDown":
                    {
                        query = query.OrderBy(Q => Q.Priority);
                        break;
                    }
                case "ProficiencyLevelUp":
                    {
                        query = query.OrderByDescending(Q => Q.ProficiencyLevel);
                        break;
                    }

                case "ProficiencyLevelDown":
                    {
                        query = query.OrderBy(Q => Q.ProficiencyLevel);
                        break;
                    }

                case "CreatedDateUp":
                    {
                        query = query.OrderByDescending(Q => Q.CreatedAt);
                        break;
                    }

                case "CreatedDateDown":
                    {
                        query = query.OrderBy(Q => Q.CreatedAt);
                        break;
                    }

                case "UpdatedDateUp":
                    {
                        query = query.OrderByDescending(Q => Q.UpdateAt);
                        break;
                    }

                case "UpdatedDateDown":
                    {
                        query = query.OrderBy(Q => Q.UpdateAt);
                        break;
                    }

                case "":{
                    query = query.OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdateAt);
                    break;
                }
                case null:{
                    query = query.OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdateAt);
                    break;
                }

                default:{
                    query = query.OrderByDescending(Q => Q.CreatedAt).ThenByDescending(Q => Q.UpdateAt);
                    break;
                }
            }

            // ------------ final --------------------
            var totalData = await query.CountAsync();

            var listData = await query
                .Skip((filter.Page - 1) * filter.ItemPage)
                .Take(filter.ItemPage)
                .AsNoTracking()
                .ToListAsync();

            var responseModel = new ResponsePositionMapModel
            {
                ContentList = listData,
                TotalData = totalData
            };

            return responseModel;

        }

        public async Task<PositionMapFormModel> getDetailPosMappingAsync(int id){

            var list = await DB.PositionCompentencyMappings
                .Where(Q => Q.PositionCompentencyMappingId == id)
                .Select(Q => new PosCompetencyModel{
                    CompetencyId = Q.CompetencyId,
                    CompetencyName = Q.Competency.CompetencyName,
                    Priority = Q.Priority,
                    ProficiencyLevel = Q.ProficiencyLevel,
                })
                .ToListAsync();

            var query = await DB.PositionCompentencyMappings
                .Where(Q => Q.PositionCompentencyMappingId == id)
                .Select(Q => new PositionMapFormModel
                {
                    PositionMapId = Q.PositionCompentencyMappingId,
                    PositionId = Q.PositionId,
                    PositionName = Q.Position.PositionName,
                    PositionDescription = Q.Position.PositionDescription,
                    CompetencyList = list
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return query;    
        }

        public async Task<PositionMapFormModel> getDetailPositionCompetencyAsync(int positionId){

            var list = await DB.PositionCompentencyMappings
                .Where(Q => Q.PositionId == positionId)
                .Select(Q => new PosCompetencyModel{
                    CompetencyId = Q.CompetencyId,
                    CompetencyName = Q.Competency.CompetencyName,
                    Priority = Q.Priority,
                    ProficiencyLevel = Q.ProficiencyLevel,
                })
                .ToListAsync();

            var query = await DB.PositionCompentencyMappings
                .Where(Q => Q.PositionId == positionId)
                .Select(Q => new PositionMapFormModel
                {
                    PositionMapId = Q.PositionCompentencyMappingId,
                    PositionId = Q.PositionId,
                    PositionName = Q.Position.PositionName,
                    PositionDescription = Q.Position.PositionDescription,
                    CompetencyList = list
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return query;    
        }

        public async Task<string> InsertPosMappingAsync(PositionMapFormModel model){

            var getPosition = await DB.Positions.Where(Q => Q.PositionId == model.PositionId).FirstOrDefaultAsync();
            var checkPositionExist = await DB.PositionCompentencyMappings.Where(Q => Q.PositionId == model.PositionId).AnyAsync();

            if(checkPositionExist == true){
                // return ResponseMessageEnum.FailedBaseString + "Position you want insert already exist, please update it instead insert it";
                return ResponseMessageEnum.FailedBaseString + "Data Already Exist.";
            }

            getPosition.PositionDescription = model.PositionDescription;
            
            var listCompetency = new List<PositionCompentencyMappings>();

            foreach(var data in model.CompetencyList){
                var temp = new PositionCompentencyMappings {
                    CompetencyId = data.CompetencyId,
                    PositionId = getPosition.PositionId,
                    Priority = data.Priority,
                    ProficiencyLevel = data.ProficiencyLevel,
                    
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    CreatedBy = ClaimMan.GetLoginUserId(),
                    UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    UpdatedBy = ClaimMan.GetLoginUserId()
                };

                listCompetency.Add(temp);
            }

            DB.Positions.Update(getPosition);
            DB.PositionCompentencyMappings.AddRange(listCompetency);

            await DB.SaveChangesAsync();
            try{
            }catch(Exception ){
                return ResponseMessageEnum.FailedSave;
            }

            return ResponseMessageEnum.SuccessAddSave;
        }

        public async Task<string> UpdatePosMappingAsync(PositionMapFormModel model){
            

            var getPosition = await DB.Positions.Where(Q => Q.PositionId == model.PositionId).FirstOrDefaultAsync();
            getPosition.PositionDescription = model.PositionDescription;

            var getListCompetency = await DB.PositionCompentencyMappings
                .Where(Q => Q.PositionId == model.PositionId)
                .ToListAsync();

            DB.PositionCompentencyMappings.RemoveRange(getListCompetency);

            var listCompetency = new List<PositionCompentencyMappings>();

            foreach(var data in model.CompetencyList){
                var temp = new PositionCompentencyMappings {
                    CompetencyId = data.CompetencyId,
                    PositionId = getPosition.PositionId,
                    Priority = data.Priority,
                    ProficiencyLevel = data.ProficiencyLevel,
                    
                    CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    CreatedBy = ClaimMan.GetLoginUserId(),
                    UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                    UpdatedBy = ClaimMan.GetLoginUserId()
                };

                listCompetency.Add(temp);
            }

            DB.PositionCompentencyMappings.AddRange(listCompetency);

            DB.Positions.Update(getPosition);

            await DB.SaveChangesAsync();
            try{
            }catch(Exception ){
                return ResponseMessageEnum.FailedSave;
            }

            return ResponseMessageEnum.SuccessEditSave;

        }

        public async Task<List<DropDownModel>> GetCompetencyListAsync(int positionId){
            var getList = await DB.PositionCompentencyMappings
            .Where(Q => Q.Position.PositionId == positionId)
            .Select(Q => new DropDownModel{
                Id = Q.PositionCompentencyMappingId,
                Name = Q.Competency.CompetencyName
            })
            .AsNoTracking()
            .ToListAsync();

            return getList;
        }

        public async Task<string> DeletePosMappingAsync(int id){

            var list = await DB.PositionCompentencyMappings
                .Where(Q => Q.PositionCompentencyMappingId == id)
                .ToListAsync();

            DB.PositionCompentencyMappings.RemoveRange(list);

            await DB.SaveChangesAsync();

            try{
            }catch(Exception ){
                return ResponseMessageEnum.FailedDelete;
            }

            return ResponseMessageEnum.SuccessDelete;   
        }

        public async Task<string> DeletePositionCompetencyAsync(PositionMapDeleteModel deleteModel){

            if(deleteModel.CompetencyListId.Count <= 0){
                return ResponseMessageEnum.FailedBaseString + "nothing to delete";
            }

            var list = await DB.PositionCompentencyMappings
                .Where(Q => 
                    Q.PositionId == deleteModel.PositionId &&
                    deleteModel.CompetencyListId.Contains(Q.PositionCompentencyMappingId)
                )
                .ToListAsync();

            DB.PositionCompentencyMappings.RemoveRange(list);

            await DB.SaveChangesAsync();

            try{
            }catch(Exception ){
                return ResponseMessageEnum.FailedDelete;
            }

            return ResponseMessageEnum.SuccessDelete;   
        }

    }
}
