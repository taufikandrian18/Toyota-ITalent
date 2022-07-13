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
    public class EventCategoryService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;

        public EventCategoryService(TalentContext talentContext, ClaimHelper claim)
        {
            this.DB = talentContext;
            this.ClaimMan = claim;
        }

        public async Task<EventCategoryViewModel> GetAllEventCategories(EventCategoryFilter filter)
        {
            var query = (
                from ec in DB.EventCategories.AsNoTracking()
                select new EventCategoryModel
                {
                    EventCategoryId = ec.EventCategoryId,
                    EventCategoryName = ec.Name,
                    EventCategoryDescription = ec.Description,
                    CreatedAt = ec.CreatedAt,
                    UpdatedAt = ec.UpdatedAt
                });

            if (string.IsNullOrEmpty(filter.EventCategoryName) == false)
            {
                query = query.Where(Q => Q.EventCategoryName.ToLower().Contains(filter.EventCategoryName.ToLower()));
            }

            query = query.OrderByDescending(Q => Q.UpdatedAt);

            switch (filter.SortBy)
            {
                case "name":
                    query = query.OrderBy(Q => Q.EventCategoryName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "nameDesc":
                    query = query.OrderByDescending(Q => Q.EventCategoryName).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "description":
                    query = query.OrderBy(Q => Q.EventCategoryDescription).ThenByDescending(Q => Q.UpdatedAt);
                    break;
                case "descriptionDesc":
                    query = query.OrderByDescending(Q => Q.EventCategoryDescription).ThenByDescending(Q => Q.UpdatedAt);
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

            var result = new EventCategoryViewModel
            {
                ListEventCategoryModel = resultQuery,
                TotalItem = totalData
            };

            return result;
        }

        public async Task<List<EventCategoryModel>> GetAllEventCategoriesNoFilter()
        {
            var result = await DB.EventCategories.Select(Q => new EventCategoryModel
            {
                EventCategoryId = Q.EventCategoryId,
                EventCategoryName = Q.Name,
                EventCategoryDescription = Q.Description,
                CreatedAt = Q.CreatedAt,
                UpdatedAt = Q.UpdatedAt
            }).ToListAsync();

            return result;
        }

        public async Task<EventCategoryModel> GetEventCategoryById(int id)
        {
            var result = await this.DB.EventCategories.Select(Q => new EventCategoryModel
            {
                EventCategoryId = Q.EventCategoryId,
                EventCategoryName = Q.Name,
                EventCategoryDescription = Q.Description,
                CreatedAt = Q.CreatedAt,
                UpdatedAt = Q.UpdatedAt
            }).Where(Q => Q.EventCategoryId == id).FirstOrDefaultAsync();

            if (result == null)
            {
                result = new EventCategoryModel();
            }

            return result;
        }

        public async Task<int?> CreateEventCategory(EventCategoryFormModel model)
        {
            var exist = await this.DB.EventCategories.Where(Q => Q.Name.ToLower() == model.EventCategoryName.ToLower()).FirstOrDefaultAsync();

            if(exist != null)
            {
                return null;
            }

            var createModel = new EventCategories
            {
                Name = model.EventCategoryName,
                Description = model.EventCategoryDescription,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = ClaimMan.GetLoginUserId(),
                UpdatedBy = ClaimMan.GetLoginUserId(),
            };

            this.DB.EventCategories.Add(createModel);
            await this.DB.SaveChangesAsync();

            return createModel.EventCategoryId;
        }

        public async Task<bool> UpdateEventCategory(EventCategoryFormModel model)
        {
            var updateModel = await this.DB.EventCategories.FindAsync(model.EventCategoryId);

            var findWithName = await this.DB.EventCategories.Where(Q => Q.Name.ToLower() == model.EventCategoryName.ToLower()).FirstOrDefaultAsync();

            if(findWithName != null){
                if(findWithName != updateModel){
                    return false;
                }
            }

            if (updateModel == null)
            {
                return false;
            }

            updateModel.Name = model.EventCategoryName;
            updateModel.Description = model.EventCategoryDescription;
            updateModel.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();
            updateModel.UpdatedBy = ClaimMan.GetLoginUserId();
            await this.DB.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteEventCategory(int id)
        {
            var deleteModel = await this.DB.EventCategories.FindAsync(id);

            if (deleteModel == null)
            {
                return false;
            }

            var checkEvent = await this.DB.Events.Where(Q => Q.EventCategoryId == id).FirstOrDefaultAsync();

            if (checkEvent != null)
            {
                return false;
            }

            this.DB.EventCategories.Remove(deleteModel);
            await this.DB.SaveChangesAsync();
            return true;
        }
    }
}
