using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideAccessService
    {
        private readonly TalentContext DB;

        public UserSideAccessService(TalentContext db)
        {
            this.DB = db;
        }

        public async Task<int> StartAccess(string employeeId)
        {
            var user = this.DB.Employees
                .FirstOrDefault(Q => Q.EmployeeId == employeeId);

            // Update user's last seen at
            user.LastSeenAt = DateTime.UtcNow.ToIndonesiaTimeZone();

            // Add new access time
            var newEmployeeAccessTimes = new EmployeeAccessTimes
            {
                EmployeeId = employeeId,
                StartTime = DateTime.UtcNow.ToIndonesiaTimeZone(),
            };

            this.DB.EmployeeAccessTimes.Add(newEmployeeAccessTimes);

            await this.DB.SaveChangesAsync();

            return newEmployeeAccessTimes.EmployeeAccessTimeId;
        }

        public async Task EndAccess(int employeeAccessTimeId)
        {
            var access = this.DB.EmployeeAccessTimes
                .Where(Q => Q.EmployeeAccessTimeId == employeeAccessTimeId)
                .FirstOrDefault();

            // Update employee's end access time
            access.EndTime = DateTime.UtcNow.ToIndonesiaTimeZone();

            await this.DB.SaveChangesAsync();
        }

    }
}
