using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Talent.WebAdmin.Services;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class EmployeeService
    {
        private readonly TalentContext DB;

        public EmployeeService(TalentContext db)
        {
            this.DB = db;
        }

        public async Task<string> GetName(string employeeId)
        {
            string data = await DB.Employees.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.EmployeeName).FirstOrDefaultAsync();

            return data;
        }

        public async Task<EmployeeListViewModel> GetMultiEmployee(string name)
        {
            var searchEmployee = await this.DB.Employees.Select(Q => new EmployeeViewModel
            {
                EmployeeName = Q.EmployeeName,
                EmployeeId = Q.EmployeeId,
                CreatedAt = Q.CreatedAt
            }).Where(Q => Q.EmployeeName.StartsWith(name)).ToListAsync();

            if (searchEmployee == null)
            {
                searchEmployee = new List<EmployeeViewModel>();
            }

            var searchResult = new EmployeeListViewModel
            {
                EmployeeList = searchEmployee
            };

            return searchResult;
        }

        public async Task<EmployeeListViewModel> GetMultipleEmployeeForDigitalSignature(string name)
        {
            var getAllEmployeeId = from e in DB.Employees
                                   where !DB.DigitalSignatures.Any(Q => Q.EmployeeId == e.EmployeeId && Q.IsDeleted == false)
                                   select new EmployeeViewModel
                                   {
                                       EmployeeName = e.EmployeeName,
                                       EmployeeId = e.EmployeeId,
                                       CreatedAt = e.CreatedAt
                                   };

            var result = await getAllEmployeeId.Where(Q => Q.EmployeeName.StartsWith(name) ).ToListAsync();

            if (result == null)
            {
                result = new List<EmployeeViewModel>();
            }

            var searchResult = new EmployeeListViewModel
            {
                EmployeeList = result
            };

            return searchResult;
        }

        public async Task<bool> InsertEmployee(EmployeeFormModel model)
        {

            var insertEmployee = await this.DB.Employees.AddAsync(new Employees
            {
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy,
                EmployeeEmail = model.EmployeeEmail,
                EmployeeExperience = model.EmployeeExperience,
                EmployeeId = model.EmployeeId,
                EmployeeName = model.EmployeeName,
                EmployeePhone = model.EmployeePhone,
                EmployeeUsername = model.EmployeeUsername,
                IsDeleted = model.IsDeleted,
                LastSeenAt = model.LastSeenAt,
                OutletId = model.OutletId,
                UpdatedAt = model.UpdatedAt,
                UpdatedBy = model.UpdatedBy

            });

            await DB.SaveChangesAsync();
            return true;
        }

    }
}
