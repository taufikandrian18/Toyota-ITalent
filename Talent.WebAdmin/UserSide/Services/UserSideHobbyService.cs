using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.UserSide.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideHobbyService
    {
        private readonly TalentContext DB;

        public UserSideHobbyService(TalentContext talentContext)
        {
            this.DB = talentContext;
        }

        public async Task<List<UserSideHobbyModel>> GetHobbies()
        {
            var dataHobbies = await this
                .DB
                .Hobbies
                .AsNoTracking()
                .Select(Q => new UserSideHobbyModel
                {
                    HobbyId = Q.HobbyId,
                    Name = Q.Name,
                    Description = Q.Description
                }).ToListAsync();

            return dataHobbies;
        }
    }
}