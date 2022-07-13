using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.UserSide.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideInterestService
    {
        private readonly TalentContext DB;

        public UserSideInterestService(TalentContext talentContext)
        {
            this.DB = talentContext;
        }

        public async Task<List<UserSideInterestModel>> GetInterest()
        {
            var dataInterests = await this
                .DB
                .Topics
                .AsNoTracking()
                .Select(Q => new UserSideInterestModel
                {
                    TopicId = Q.TopicId,
                    TopicName = Q.TopicName,
                    TopicDescription = Q.TopicDescription
                }).ToListAsync();

            return dataInterests;
        }
    }
}