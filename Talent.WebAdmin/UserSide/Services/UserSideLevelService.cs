using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideLevelService
    {
        private readonly TalentContext DB;

        public UserSideLevelService(TalentContext db)
        {
            this.DB = db;
        }

        /// <summary>
        /// get employee's current experience
        /// from current experience get the next level and the experience needed to reach it
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<EmployeeLevels> GetNextLevel(string employeeId)
        {
            var employeeExperience = await DB.Employees
                .Where(Q => Q.EmployeeId == employeeId)
                .Select(Q => Q.EmployeeExperience)
                .FirstOrDefaultAsync();

            var employeeLevels = await DB.EmployeeLevels
                .Select(Q => Q)
                .ToListAsync();

            // Next level from current experience
            var nextEmployeeLevel = employeeLevels
                .Where(Q => Q.MinValue > employeeExperience)
                .FirstOrDefault();

            // Level max
            var maxEmployeeLevel = employeeLevels
                .Skip(employeeLevels.Count - 1)
                .Take(1)
                .FirstOrDefault();

            return nextEmployeeLevel ?? maxEmployeeLevel;
        }

    }
}
