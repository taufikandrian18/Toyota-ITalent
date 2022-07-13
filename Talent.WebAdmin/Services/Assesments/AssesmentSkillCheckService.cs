using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
    public class AssesmentSkillCheckService : Controller
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        IFileStorageService FileService;
        private readonly BlobService BlobService;

        public AssesmentSkillCheckService(TalentContext db, ClaimHelper claimHelper, IFileStorageService fileService, BlobService blobService)
        {
            this.DB = db;
            this.ClaimMan = claimHelper;
            this.BlobService = blobService;
            this.FileService = fileService;
        }


        public async Task<ActionResult<BaseResponse>> GetDetail(ParamAssesmentSkillCheckModel request)
        {
           try
            {   
                var query = DB.AssesmentSkillChecks.AsQueryable();
                if (request.GUID != ""){
                    query = query.Where(x=> x.GUID == request.GUID);
                }

                if (request.AssesmentGUID != ""){
                    query = query.Where(x=> request.AssesmentGUID.Contains(x.AssesmentGUID));
                }
                if (request.SkillCheckGUID != ""){
                    query = query.Where(x=> request.SkillCheckGUID.Contains(x.SkillCheckGUID));
                }

                var response = await query.Include(x=> x.AssesmentsNavigator).Include(x=> x.SkillChecksNavigator).OrderBy(x=> x.CreatedAt).Skip(request.Page).Take(request.Limit).Select(x=>new ResponseAssesmentSkillCheckModel{
                      GUID = x.GUID,
                      AssesmentGUID = x.AssesmentGUID,
                      SkillCheckGUID = x.SkillCheckGUID,
                      Order = x.Order,
                }).ToListAsync();
                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetList(ParamAssesmentSkillCheckModel request)
        {
            try
            {      
                var query = DB.AssesmentSkillChecks.AsQueryable();
                if (request.GUID != ""){
                    query = query.Where(x=> x.GUID == request.GUID);
                }

                if (request.AssesmentGUID != ""){
                    query = query.Where(x=> request.AssesmentGUID.Contains(x.AssesmentGUID));
                }
                if (request.SkillCheckGUID != ""){
                    query = query.Where(x=> request.SkillCheckGUID.Contains(x.SkillCheckGUID));
                }

                var response = await query.OrderBy(x=> x.CreatedAt).Skip(request.Page).Take(request.Limit).Select(x=>new ResponseAssesmentSkillCheckModel{
                      GUID = x.GUID,
                      AssesmentGUID = x.AssesmentGUID,
                      SkillCheckGUID = x.SkillCheckGUID,
                      Order = x.Order,
                }).ToListAsync();
                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Create(RequestAssesmentSkillCheckModel request)
        {
            try
            {      
                var data =  new AssesmentSkillChecks(); 
                data.GUID = Guid.NewGuid().ToString();
                data.CreatedAt = DateTime.UtcNow.AddHours(7);
                data.UpdatedAt = DateTime.UtcNow.AddHours(7);
                data.Order = request.Order;
                data.SkillCheckGUID = request.SkillCheckGUID;
                data.AssesmentGUID = request.AssesmentGUID;

                DB.AssesmentSkillChecks.Add(data);
                await DB.SaveChangesAsync();
                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Update(RequestAssesmentSkillCheckModel request)
        {
          try
            {      
               var data = await DB.AssesmentSkillChecks.FirstOrDefaultAsync(x=> x.GUID == request.GUID);
               if (data == null){
                     return StatusCode(200, BaseResponse.ResponseOk("Data Not Found"));
               }

                data.UpdatedAt = DateTime.UtcNow.AddHours(7);
                data.Order = request.Order;
                data.SkillCheckGUID = request.SkillCheckGUID;
                data.AssesmentGUID = request.AssesmentGUID;
                await DB.SaveChangesAsync();  
                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Delete(string request)
        {
            try
            {      
                var data = await DB.AssesmentSkillChecks.FirstOrDefaultAsync(x=> x.GUID == request);
               if (data == null){
                     return StatusCode(200, BaseResponse.ResponseOk("Data Not Found"));
               }

                data.DeletedAt = DateTime.UtcNow.AddHours(7);
            
                await DB.SaveChangesAsync();  
                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

    }


}
