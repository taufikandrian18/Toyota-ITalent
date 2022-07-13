using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
    public class ApprovalUpgradeUserService : Controller
    {
        private readonly TalentContext DB;


        public ApprovalUpgradeUserService(TalentContext talentContext)
        {
            this.DB = talentContext;
        }

        public async Task<ActionResult<BaseResponse>> Create(UpgradeAccountApprovals model)
        {
            try
            {
                var data = DB.UpgradeAccountApprovals.Add(model);
                await DB.SaveChangesAsync();
                return BaseResponse.Created(null);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Update(UpgradeAccountApprovals model)
        {
            try
            {
                var existingData = DB.UpgradeAccountApprovals.FirstOrDefault(Q => Q.Guid == model.Guid);
                if (existingData == null)
                {
                    var message = new Message();

                    message.En = "Data not found";
                    message.Id = "Data tidak ditemukan";
                    return BaseResponse.BadRequest(null, message);
                }

                existingData = model;
                await DB.SaveChangesAsync();
                return BaseResponse.ResponseOk(existingData);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Delete(string guid)
        {
            try
            {
                var existingData = DB.UpgradeAccountApprovals.FirstOrDefault(Q => Q.Guid == guid);
                if (existingData == null)
                {
                    var message = new Message();

                    message.En = "Data not found";
                    message.Id = "Data tidak ditemukan";
                    return BaseResponse.BadRequest(null, message);
                }

                DB.Remove(existingData);
                await DB.SaveChangesAsync();
                return BaseResponse.ResponseOk(null);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Get(string guid)
        {
            try
            {
                var data = await DB.UpgradeAccountApprovals.Where(Q => Q.EmployeeId == guid).Include(Q => Q.Employee).ToListAsync();
                return BaseResponse.ResponseOk(data);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }


    }
}
