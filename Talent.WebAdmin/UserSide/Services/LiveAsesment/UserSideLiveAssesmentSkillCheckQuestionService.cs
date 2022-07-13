using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using TAM.Talent.Commons.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;



namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideLiveAssesmentSkillCheckQuestionService : Controller
    {
        private readonly TalentContext DB;
        private readonly ClaimHelper ClaimMan;
        IFileStorageService FileService;
        private readonly BlobService BlobService;

        public UserSideLiveAssesmentSkillCheckQuestionService(TalentContext db, ClaimHelper claimHelper, IFileStorageService fileService, BlobService blobService)
        {
            this.DB = db;
            this.ClaimMan = claimHelper;
            this.BlobService = blobService;
            this.FileService = fileService;
        }

      
        public async Task<ActionResult<BaseResponse>> GetDetail(ParamLiveAssesmentSkillCheckQuestionModel request)
        {
            try
            {      
                  var query = DB.LiveAssesmentSkillCheckQuestions.AsQueryable();
                  var response = await query.Where(x=> x.GUID == request.GUID).Select(x=> new ResponseLiveAssesmentSkillCheckQuestionModel{
                   GUID = x.GUID,
                   Answer = x.Answer,
                   Point = x.Point,
                   Score = x.Score,
                   LiveAssesmentGUID = x.LiveAssesmentGUID,
                   Text = x.Text
                }).ToListAsync();
                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetList(ParamLiveAssesmentSkillCheckQuestionModel request)
        {
            try
            {      
                  var query = DB.LiveAssesmentSkillCheckQuestions.AsQueryable();
                  var response = await query.Where(x=> x.LiveAssesmentGUID == request.LiveAssesmentGUID).Select(x=> new ResponseLiveAssesmentSkillCheckQuestionModel{
                   GUID = x.GUID,
                   Answer = x.Answer,
                   Point = x.Point,
                   Score = x.Score,
                   LiveAssesmentGUID = x.LiveAssesmentGUID,
                   Text = x.Text
                }).ToListAsync();
                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Create(RequestLiveAssesmentSkillCheckQuestionModel request)
        {
            try
            {      
                var data = new LiveAssesmentSkillCheckQuestions();
                data.GUID = Guid.NewGuid().ToString();
                data.CreatedAt = DateTime.UtcNow.AddHours(7);
                data.UpdatedAt = DateTime.UtcNow.AddHours(7);
                data.Score = request.Score;
                data.Point  = request.Point;
                data.Text = request.Text;
                data.LiveAssesmentGUID = request.LiveAssesmentGUID;

                
                
                DB.LiveAssesmentSkillCheckQuestions.Add(data);
                await DB.SaveChangesAsync();
                return StatusCode(200, BaseResponse.ResponseOk());
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Update(RequestLiveAssesmentSkillCheckQuestionModel request)
        {
            try
            {      
               var data = await DB.LiveAssesmentSkillCheckQuestions.FirstOrDefaultAsync(x=> x.GUID == request.GUID);
               if (data == null){
                     return StatusCode(200, BaseResponse.ResponseOk("Data Not Found"));
               }

                data.UpdatedAt = DateTime.UtcNow.AddHours(7);
                data.Score = request.Score;
                data.Point  = request.Point;
                data.Text = request.Text;
                data.LiveAssesmentGUID = request.LiveAssesmentGUID;

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
               var data = await DB.LiveAssesmentSkillCheckQuestions.FirstOrDefaultAsync(x=> x.GUID == request);
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
