using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSidePopQuizQuery : UserSidePopQuizModel
    {

    }

    public class UserSidePopQuizModel
    {
        public int PopQuizId { get; set; }
        public string PopQuizName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TestTime { get; set; }
    }

    public class UserSidePopQuizFilterModel
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        /// <summary>
        /// SorBy Value :
        /// name => sort CourseName ASC.
        /// nameDesc => sort CourseName DESC.
        /// createdAt => sort CreatedAt ASC.
        /// createdAtDesc => sort CreatedAt DESC.
        /// </summary>
        public string SortBy { get; set; }
    }

    public class UserSidePopQuizService
    {
        private readonly TalentContext DB;

        public UserSidePopQuizService(TalentContext talentContext)
        {
            this.DB = talentContext;
        }

        public async Task<List<UserSidePopQuizModel>> GetAllPopQuiz(UserSidePopQuizFilterModel filter, string employeeId)
        {
            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            //// This variable gets the user's pop quiz id which he/she done already
            //var popQuizDoneIds = await DB.LearningHistories
            //    .Where(Q => Q.EmployeeId == employeeId)
            //    .Select(Q => Q.PopQuizId)
            //    .Distinct()
            //    .ToListAsync();
            //popQuizDoneIds.Remove(null);

            var query = (from pq in DB.PopQuizzes
                         join st in DB.SetupTasks on pq.PopQuizId equals st.PopQuizId
                         //where !popQuizDoneIds.Contains(pq.PopQuizId)
                         where pq.IsDeleted == false && pq.ApprovedAt != null
                         select new UserSidePopQuizModel
                         {
                             PopQuizId = pq.PopQuizId,
                             PopQuizName = pq.PopQuizName,
                             CreatedAt = pq.CreatedAt,
                             TestTime = st.TestTime,
                         })
                         .AsNoTracking();

            switch (filter.SortBy)
            {
                case "name":
                    query = query.OrderBy(Q => Q.PopQuizName);
                    break;

                case "nameDesc":
                    query = query.OrderByDescending(Q => Q.PopQuizName);
                    break;

                case "date":
                    query = query.OrderBy(Q => Q.CreatedAt);
                    break;

                case "dateDesc":
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;

                default:
                    query = query.OrderByDescending(Q => Q.CreatedAt);
                    break;
            }

            var popQuizes = await query.Select(Q => new UserSidePopQuizModel
            {
                PopQuizId = Q.PopQuizId,
                PopQuizName = Q.PopQuizName,
                CreatedAt = Q.CreatedAt,
                TestTime = Q.TestTime,
            }).Skip((int)skipCount)
              .Take(filter.PageSize)
              .ToListAsync();

            return popQuizes;
        }
    }
}
