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
    public class ScoreConfigService : Controller
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        IFileStorageService FileService;
        private readonly BlobService BlobService;

        public ScoreConfigService(TalentContext db, ClaimHelper claimHelper, IFileStorageService fileService, BlobService blobService)
        {
            this.DB = db;
            this.ClaimMan = claimHelper;
            this.BlobService = blobService;
            this.FileService = fileService;
        }


        public async Task<ActionResult<BaseResponse>> GetDetail(ParamScoreConfigModel request)
        {
            try
            {   
                var query = DB.SkillChecksScoreConfigs.AsQueryable();
                if (request.SkillCheckGUID != ""){
                    query = query.Where(x=> x.SkillCheckGUID == request.SkillCheckGUID);
                }

                if (request.Query != ""){
                    query = query.Where(x=> request.Query.Contains(x.Text));
                }

                 if (request.GUID != ""){
                    query = query.Where(x=> request.Query.Contains(x.GUID));
                }

                var response = await query.OrderBy(x=> x.CreatedAt).Skip(request.Page).Take(request.Limit).Select(x=>new ResponseScoreConfigModel{
                        GUID = x.GUID,
                        Point = x.Point,
                        Score = x.Score,
                        SkillCheckGUID = x.SkillCheckGUID,
                        Text = x.Text
                }).ToListAsync();
                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetList(ParamScoreConfigModel request)
        {
            try
            {   var query = DB.SkillChecksScoreConfigs.AsQueryable();
                if (request.SkillCheckGUID != ""){
                    query = query.Where(x=> x.SkillCheckGUID == request.SkillCheckGUID);
                }

                if (request.Query != ""){
                    query = query.Where(x=> request.Query.Contains(x.Text));
                }

                var response = await query.OrderBy(x=> x.CreatedAt).Skip(request.Page).Take(request.Limit).Select(x=>new ResponseScoreConfigModel{
                        GUID = x.GUID,
                        Point = x.Point,
                        Score = x.Score,
                        SkillCheckGUID = x.SkillCheckGUID,
                        Text = x.Text,
                        
                }).ToListAsync();
                return StatusCode(200, BaseResponse.ResponseOk(response));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Create(RequestScoreConfigModel request)
        {
            try
            {     
                var data =  new SkillChecksScoreConfigs(); 
                data.GUID = Guid.NewGuid().ToString();
                data.CreatedAt = DateTime.UtcNow.AddHours(7);
                data.UpdatedAt = DateTime.UtcNow.AddHours(7);
                data.Point = request.Point;
                data.Score = request.Score;
                data.SkillCheckGUID = request.SkillCheckGUID;
                data.Text = request.Text;
                data.Description = request.Description;

                DB.SkillChecksScoreConfigs.Add(data);
                await DB.SaveChangesAsync();
                
                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Update(RequestScoreConfigModel request)
        {
            try
            {    
               var data = await DB.SkillChecksScoreConfigs.FirstOrDefaultAsync(x=> x.GUID == request.GUID);
               if (data == null){
                     return StatusCode(200, BaseResponse.ResponseOk("Data Not Found"));
               }

                data.UpdatedAt = DateTime.UtcNow.AddHours(7);
                data.Point = request.Point;
                data.Score = request.Score;
                data.SkillCheckGUID = request.SkillCheckGUID;
                data.Text = request.Text;
                data.Description = request.Description;
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
               var data = await DB.SkillChecksScoreConfigs.FirstOrDefaultAsync(x=> x.GUID == request);
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
