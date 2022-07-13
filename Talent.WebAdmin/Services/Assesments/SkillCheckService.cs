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
  public class SkillCheckService : Controller
  {
    private readonly TalentContext DB;
    private readonly ClaimHelper ClaimMan;
    IFileStorageService FileService;
    private readonly BlobService BlobService;

    public SkillCheckService(TalentContext db, ClaimHelper claimHelper, IFileStorageService fileService, BlobService blobService)
    {
      this.DB = db;
      this.ClaimMan = claimHelper;
      this.BlobService = blobService;
      this.FileService = fileService;
    }


    public async Task<ActionResult<BaseResponse>> GetDetail(ParamSkillCheckModel request)
    {
      try
      {
        var query = DB.SkillChecks.AsQueryable();
        var response = await query.Where(x => x.GUID == request.GUID).Select(x => new ResponseSkillCheckModel
        {
          GUID = x.GUID,
          Title = x.Title,
          Description = x.Description,
          EnumFeedbackScore = x.EnumFeedbackScore,
          IsQuestion = x.IsQuestion,
          MediaBlobId = x.MediaBlobId,
          MinimumScore = x.MinimumScore,
          Name = x.Name,
          CreatedBy = x.CreatedBy,
          UpdatedBy = x.UpdatedBy,
          CreatedAt = x.CreatedAt,
          UpdatedAt = x.UpdatedAt,
        }).ToListAsync();
        return StatusCode(200, BaseResponse.ResponseOk());
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }

    public async Task<ActionResult<BaseResponse>> GetList(ParamSkillCheckModel request)
    {
      try
      {

        if (request.Limit == 0)
        {
          request.Limit = 10;
        }

        if (request.Page == 0)
        {
          request.Page = 1;
        }

        request.Page = request.Page - 1;

        var query = DB.SkillChecks.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Query))
        {
          query = query.Where(x => x.Name.Contains(request.Query)).AsNoTracking();
        }

        if (!string.IsNullOrWhiteSpace(request.ScoringMethod))
        {
          query = query.Where(x => x.EnumFeedbackScore.Contains(request.ScoringMethod)).AsNoTracking();
        }

        if (request.CreatedAt != null)
        {
          query = query.Where(x => x.CreatedAt.Value.Date == request.CreatedAt.Value.Date).AsNoTracking();
        }

        if (!string.IsNullOrWhiteSpace(request.CreatedBy))
        {
          query = query.Where(x => x.CreatedBy.Contains(request.CreatedBy)).AsNoTracking();
        }

        if (request.StartDate != null && request.EndDate != null)
        {
          var newStartDate = request.StartDate.Value.UtcDateTime.ToIndonesiaTimeZone();
          var startDate = new DateTime(newStartDate.Year, newStartDate.Month, newStartDate.Day, 0, 0, 0);

          var newEndDate = request.EndDate.Value.UtcDateTime.ToIndonesiaTimeZone();
          var endDate = new DateTime(newEndDate.Year, newEndDate.Month, newEndDate.Day, 23, 59, 59);

          query = query.Where(Q => (Q.CreatedAt >= startDate && Q.CreatedAt < endDate));
        }

        if (!string.IsNullOrEmpty(request.GUID))
        {
          query = query.Where(x => x.GUID == request.GUID).AsNoTracking();
        }

        var count = query.Count();

        var response = await query.Where(x => x.DeletedAt == null).OrderByDescending(x => x.CreatedAt).Include(x => x.SkillCheckScoreConfigsNavigation).Skip(request.Page * request.Limit).Take(request.Limit).Select(x => new ResponseSkillCheckModel
        {
          GUID = x.GUID,
          Title = x.Title,
          Description = x.Description,
          EnumFeedbackScore = x.EnumFeedbackScore,
          IsQuestion = x.IsQuestion,
          MediaBlobId = x.MediaBlobId,
          MinimumScore = x.MinimumScore,
          Name = x.Name,
          ScoringConfig = x.SkillCheckScoreConfigsNavigation.ToList(),
          CreatedAt = x.CreatedAt,
          UpdatedAt = x.UpdatedAt,
          CreatedBy = x.CreatedBy
        }).ToListAsync();

        var index = 0;
        foreach (var datum in response)
        {

          var imageURL = "";
          var blobData = new BlobModel();

          if (!String.IsNullOrEmpty(datum.MediaBlobId))
          {
            blobData = await this.BlobService.GetBlobById(Guid.Parse(datum.MediaBlobId));

            imageURL = await this.FileService.GetFileAsync(blobData.BlobId.ToString(), blobData.Mime);
          }

          response[index].MediaUrl = imageURL;
          response[index].MediaBlobData = blobData;

          index++;
        }

        return StatusCode(200, BaseResponse.ResponseOk(response, count));
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }

    public async Task<ActionResult<BaseResponse>> Create(RequestSkillCheckModel request)
    {
      try
      {
        var data = new SkillChecks();
        var scoringDatas = new List<SkillChecksScoreConfigs>();

        Guid? getUploadBlob = null;

        // Insert media
        if (string.IsNullOrEmpty(request.MediaContent.Base64) == false)
        {
          getUploadBlob = await this.FileService.UploadFileFromBase64(request.MediaContent);
        }

        data.GUID = Guid.NewGuid().ToString();
        data.CreatedAt = DateTime.UtcNow.AddHours(7);
        data.UpdatedAt = DateTime.UtcNow.AddHours(7);
        data.IsQuestion = request.IsQuestion;
        data.MediaBlobId = getUploadBlob.ToString();
        data.MinimumScore = request.MinimumScore;
        data.Name = request.Name;
        data.Title = request.Title;
        data.EnumFeedbackScore = request.EnumFeedbackScore;
        data.Description = request.Description;

        // if (!string.IsNullOrEmpty(request.CreatedBy))
        // {
        data.CreatedBy = this.ClaimMan.GetLoginUserName();
        data.UpdatedBy = this.ClaimMan.GetLoginUserName();
        // data.CreatedBy = "SYSTEM";
        // }

        DB.SkillChecks.Add(data);

        foreach (var item in request.ScoringConfig)
        {
          var scoringData = new SkillChecksScoreConfigs()
          {
            CreatedAt = DateTime.UtcNow.AddHours(7),
            SkillCheckGUID = data.GUID,
            Point = item.Point,
            Score = item.Score,
            Text = item.Text,
            GUID = Guid.NewGuid().ToString(),
            Description = item.Description,
          };

          scoringDatas.Add(scoringData);
        }

        await DB.SkillChecksScoreConfigs.AddRangeAsync(scoringDatas);
        await DB.SaveChangesAsync();
        return StatusCode(200, BaseResponse.ResponseOk());
      }
      catch (Exception x)
      {
        return StatusCode(500, BaseResponse.Error(null, x));
      }
    }

    public async Task<ActionResult<BaseResponse>> Update(RequestSkillCheckModel request)
    {
      try
      {
        var data = await DB.SkillChecks.Include(v => v.SkillCheckScoreConfigsNavigation).FirstOrDefaultAsync(x => x.GUID == request.GUID);
        if (data == null)
        {
          return StatusCode(200, BaseResponse.ResponseOk("Data Not Found"));
        }

        Guid? getUploadBlob = null;

        // Update media
        if (string.IsNullOrEmpty(request.MediaContent.Base64) == false)
        {
          getUploadBlob = await this.FileService.UploadFileFromBase64(request.MediaContent);
        }
        else if (string.IsNullOrEmpty(request.MediaBlobId) == false)
        {
          getUploadBlob = Guid.Parse(request.MediaBlobId);
        }

        data.UpdatedAt = DateTime.UtcNow.AddHours(7);
        data.IsQuestion = request.IsQuestion;
        data.MediaBlobId = getUploadBlob.ToString();
        data.MinimumScore = request.MinimumScore;
        data.Name = request.Name;
        data.Title = request.Title;
        data.EnumFeedbackScore = request.EnumFeedbackScore;
        data.Description = request.Description;

        // if (!string.IsNullOrEmpty(request.UpdatedBy))
        // {
        //   data.UpdatedBy = "SYSTEM";
        // }

        data.UpdatedBy = this.ClaimMan.GetLoginUserName();


        // Delete children
        foreach (var existingChild in data.SkillCheckScoreConfigsNavigation.ToList())
        {
          if (!request.ScoringConfig.Any(c => c.GUID == existingChild.GUID))
            DB.SkillChecksScoreConfigs.Remove(existingChild);
        }

        // Update and Insert children
        foreach (var childModel in request.ScoringConfig)
        {
          var existingChild = data.SkillCheckScoreConfigsNavigation
              .Where(c => c.GUID == childModel.GUID)
              .SingleOrDefault();

          if (existingChild != null)
            // Update child
            DB.Entry(existingChild).CurrentValues.SetValues(childModel);
          else
          {
            // Insert child
            var newChild = new SkillChecksScoreConfigs
            {
              CreatedAt = DateTime.UtcNow.AddHours(7),
              SkillCheckGUID = data.GUID,
              Point = childModel.Point,
              Score = childModel.Score,
              Text = childModel.Text,
              GUID = Guid.NewGuid().ToString(),
              Description = childModel.Description,
            };
            data.SkillCheckScoreConfigsNavigation.Add(newChild);
          }
        }

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
        var data = await DB.SkillChecks.FirstOrDefaultAsync(x => x.GUID == request);
        if (data == null)
        {
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
