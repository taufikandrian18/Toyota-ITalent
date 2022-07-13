using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideUserAccessTimesService
    {
        private readonly TalentContext _Db;

        public UserSideUserAccessTimesService(TalentContext talentContext)
        {
            this._Db = talentContext;
        }

        public async Task<int> EmployeeStartDate(string employeeId)
        {
            //var accessTime = await this._Db.EmployeeAccessTimes.Where(Q => Q.EmployeeId == employeeId).FirstOrDefaultAsync();

            //if (accessTime != null)
            //{
            //    accessTime.StartTime = DateTime.UtcNow.ToIndonesiaTimeZone();
            //    await this._Db.SaveChangesAsync();
            //    return accessTime.EmployeeAccessTimeId;
            //}
            var newData = new EmployeeAccessTimes
            {
                EmployeeId = employeeId,
                StartTime = DateTime.UtcNow.ToIndonesiaTimeZone()
            };
            this._Db.Add(newData);
            await this._Db.SaveChangesAsync();
            return newData.EmployeeAccessTimeId;
        }

        public async Task<int?> EmployeeEndDate(int employeeAccesTimeId, string employeeId)
        {
            var accessTime = await this._Db.EmployeeAccessTimes.Where(Q => Q.EmployeeAccessTimeId == employeeAccesTimeId).FirstOrDefaultAsync();

            //Supaya yang bisa logout cuma diri sendirii
            if(accessTime.EmployeeId != employeeId)
            {
                return null;
            }

            var dateNow = DateTime.UtcNow.ToIndonesiaTimeZone();
            accessTime.EndTime = dateNow;

            var employee = await this._Db.Employees.Where(Q => Q.EmployeeId == accessTime.EmployeeId).FirstOrDefaultAsync();
            employee.LastSeenAt = dateNow;

            await this._Db.SaveChangesAsync();
            return accessTime.EmployeeAccessTimeId;
        }
    }
}
