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
    public class HobbyService
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;

        public HobbyService(TalentContext context, ClaimHelper claim)
        {
            this.DB = context;
            this.ClaimMan = claim;
        }

        public async Task<HobbyGridModel> GetAllHobbyAsync(HobbyFilterModel filter)
        {
            var query = this.DB.Hobbies.AsNoTracking();

            var totalData = await query.CountAsync();

            if (String.IsNullOrEmpty(filter.HobbyName) == false)
            {
                query = query.Where(Q => Q.Name.ToLower().Contains(filter.HobbyName.ToLower()));
            }

            var totalFilterData = await query.CountAsync();

            switch (filter.SortBy)
            {
                case "name":
                    query = query.OrderBy(Q => Q.Name);
                    break;
                case "nameDesc":
                    query = query.OrderByDescending(Q => Q.Name);
                    break;
                case "description":
                    query = query.OrderBy(Q => Q.Description);
                    break;
                case "descriptionDesc":
                    query = query.OrderByDescending(Q => Q.Description);
                    break;
                case "createdAt":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;
                case "createdAtDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
                case "updatedAt":
                    query = query.OrderBy(Q => Q.UpdatedAt);
                    break;
                case "updatedAtDesc":
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(Q => Q.UpdatedAt);
                    break;
            }

            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);
            query = query.Skip((int)skipCount).Take(filter.PageSize);

            var hobbies = await query.Select(Q => new HobbyGridViewModel
            {
                HobbyId = Q.HobbyId,
                HobbyName = Q.Name,
                HobbyDescription = Q.Description,
                CreatedAt = Q.CreatedAt,
                UpdatedAt = Q.UpdatedAt
            }).ToListAsync();

            var grid = new HobbyGridModel
            {
                Grid = hobbies,
                TotalData = totalData,
                TotalFilterData = totalFilterData
            };

            return grid;
        }

        public async Task<bool> InsertHobby(HobbyCreateModel model)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var isExist = await this.DB.Hobbies.AnyAsync(Q => Q.Name.ToLower() == model.HobbyName.ToLower());

            if (isExist)
            {
                return false;
            }

            var newHobby = new Hobbies
            {
                Name = model.HobbyName,
                Description = model.HobbyDescription,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                CreatedBy = ClaimMan.GetLoginUserId(),
                UpdatedBy = ClaimMan.GetLoginUserId(),
            };

            this.DB.Hobbies.Add(newHobby);
            await this.DB.SaveChangesAsync();
            return true;
        }

        public async Task<HobbyViewModel> GetHobbyByIdAsync(int hobbyId)
        {
            var hobby = await this.DB.Hobbies.Where(Q => Q.HobbyId == hobbyId).Select(Q => new HobbyViewModel
            {
                HobbyId = Q.HobbyId,
                HobbyName = Q.Name,
                HobbyDescription = Q.Description
            }).FirstOrDefaultAsync();

            return hobby;
        }

        public async Task<bool> UpdateHobbyAsync(HobbyUpdateModel model)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();
            var dataHobby = await this
                .DB
                .Hobbies
                .Where(Q => Q.HobbyId == model.HobbyId)
                .FirstOrDefaultAsync();

            var isChanged = dataHobby.Name.ToLower() != model.HobbyName.ToLower();
            var isExist = await this.DB.Hobbies.AnyAsync(Q => Q.Name == model.HobbyName);

            if (isChanged && isExist)
            {
                return false;
            }

            dataHobby.Name = model.HobbyName;
            dataHobby.Description = model.HobbyDescription;
            dataHobby.UpdatedAt = thisDate;
            dataHobby.UpdatedBy = ClaimMan.GetLoginUserId();

            await this.DB.SaveChangesAsync();
            return true;
        }

        public async Task DeleteHobbyAsync(int hobbyId)
        {

            var dataEmployeeHobbyMapping = await this
                .DB
                .EmployeeHobbyMappings
                .Where(Q => Q.HobbyId == hobbyId)
                .ToListAsync();

            this.DB.EmployeeHobbyMappings.RemoveRange(dataEmployeeHobbyMapping);


            var dataHobby = await this
            .DB
            .Hobbies
            .Where(Q => Q.HobbyId == hobbyId)
            .FirstOrDefaultAsync();

            this.DB.Hobbies.Remove(dataHobby);
            await this.DB.SaveChangesAsync();
        }
    }
}
