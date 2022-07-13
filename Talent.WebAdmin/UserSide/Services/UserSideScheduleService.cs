using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.UserSide.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideScheduleService
    {
        private readonly TalentContext DB;

        public UserSideScheduleService(TalentContext talentContext)
        {
            this.DB = talentContext;
        }

        /// <summary>
        /// Get data schedule by employee ID.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>List of schedule data in <seealso cref="List{UserSideCoachScheduleViewModel}"/> format.</returns>
        public async Task<List<UserSideCoachScheduleViewModel>> GetSchedule(string employeeId)
        {
            var querySchedule = await (from c in this.DB.Coaches.AsNoTracking()
                                       join cs in this.DB.CoachSchedules.AsNoTracking()
                                       on c.CoachId equals cs.CoachId
                                       where c.EmployeeId == employeeId
                                       select new
                                       {
                                           Coach = c,
                                           CoachSchedule = cs
                                       })
                                       .Select(Q => new UserSideCoachScheduleViewModel
                                       {
                                           CoachId = Q.CoachSchedule.CoachId,
                                           ScheduleName = Q.CoachSchedule.ScheduleName,
                                           StartDateTime = Q.CoachSchedule.StartDateTime,
                                           EndDateTime = Q.CoachSchedule.EndDateTime,
                                           CoachScheduleId = Q.CoachSchedule.CoachScheduleId
                                       }).ToListAsync();

            return querySchedule;
        }

        /// <summary>
        /// Create new coach schedule.
        /// </summary>
        /// <param name="newUserSideSchedule"></param>
        /// <param name="coachId"></param>
        /// <returns></returns>
        public async Task CreateSchedule(UserSideCoachScheduleFormModel newUserSideSchedule, int coachId)
        {
            var dataSchedule = new CoachSchedules
            {
                CoachId = coachId,
                ScheduleName = newUserSideSchedule.ScheduleName,
                StartDateTime = newUserSideSchedule.StartDateTime,
                EndDateTime = newUserSideSchedule.EndDateTime
            };

            this.DB.Add(dataSchedule);
            await this.DB.SaveChangesAsync();
        }

        /// <summary>
        /// Update schedule data.
        /// </summary>
        /// <param name="updateSchedule"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSchedule(UserSideCoachScheduleUpdateModel updateSchedule)
        {
            var dataSchedule = await this
                .DB
                .CoachSchedules
                .Where(Q => Q.CoachScheduleId == updateSchedule.CoachScheduleId)
                .FirstOrDefaultAsync();

            if (dataSchedule == null)
            {
                return false;
            }

            dataSchedule.ScheduleName = updateSchedule.ScheduleName;
            dataSchedule.StartDateTime = updateSchedule.StartDateTime;
            dataSchedule.EndDateTime = updateSchedule.EndDateTime;

            await this.DB.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Delete schedule data by coach schedule id.
        /// </summary>
        /// <param name="coachScheduledId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSchedule(int coachScheduleId)
        {
            var scheduleData = await this
                .DB
                .CoachSchedules
                .Where(Q => Q.CoachScheduleId == coachScheduleId)
                .FirstOrDefaultAsync();

            if (scheduleData == null)
            {
                return false;
            }

            this.DB.Remove(scheduleData);
            await this.DB.SaveChangesAsync();

            return true;
        }
    }
}