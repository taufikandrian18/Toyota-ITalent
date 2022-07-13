using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class ApprovalStatusService
    {
        private readonly TalentContext DB;

        public ApprovalStatusService(TalentContext db)
        {
            this.DB = db;
        }

        public async Task<ApprovalStatusViewModel> GetAllApprovalStatuses()
        {

            var allApprovalStatuses = await this.DB.ApprovalStatus.Select(Q => new ApprovalStatusModel
            {
                ApprovalId = Q.ApprovalStatusId,
                ApprovalName = Q.ApprovalName
            }).ToListAsync();

            var totalItem = await this.DB.ApprovalStatus.CountAsync();

            var result = new ApprovalStatusViewModel
            {
                ListApprovalStatusModel = allApprovalStatuses,
                TotalItem = totalItem
            };

            return result;
        }

        public async Task<ApprovalStatusModel> GetApprovalStatusById(int? id)
        {
            var result = await DB.ApprovalStatus.AsNoTracking()
                .Select(Q => new ApprovalStatusModel
                {
                    ApprovalId = Q.ApprovalStatusId,
                    ApprovalName = Q.ApprovalName
                })
                .Where(Q => Q.ApprovalId == id).FirstOrDefaultAsync(); //Q.IsDeleted == false && 

            if (result == null)
            {
                result = new ApprovalStatusModel();
            }

            return result;
        }
    }
}
