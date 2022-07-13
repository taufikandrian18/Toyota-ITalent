using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Enums;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Talent.WebAdmin.UserSide.Services
{
    /// <summary>
    /// Service class for providing various methods for interacting with banner data.
    /// </summary>
    public class UserSideUserDownloadService : Controller
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;

        public UserSideUserDownloadService(TalentContext talentContext, IFileStorageService fileService)
        {
            this.DB = talentContext;
            this.FileService = fileService;
        }


        public async Task<ActionResult<BaseResponse>> Create(VMEmployeeDownload model)
        {
            try
            {
                if (model.Category.ToLower() != "news")
                {

                    var jsonDescDeserialize = JsonConvert.DeserializeObject<Root>(model.Description);

                    string relatedID = !string.IsNullOrEmpty(jsonDescDeserialize.learningModel.trainingId.ToString()) ? jsonDescDeserialize.learningModel.trainingId.ToString() : jsonDescDeserialize.learningModel.setupModuleId.ToString();

                    var responseData = new EmployeeDownload();
                    var existingData = DB.EmployeeDownload.Where(Q => Q.EmployeeId == model.EmployeeID && Q.RelatedId == relatedID).FirstOrDefault();

                    if (existingData == null)
                    {
                        responseData = new EmployeeDownload
                        {
                            Guid = Guid.NewGuid().ToString(),
                            CreatedBy = "System",
                            UpdatedBy = "System",
                            URL = model.URL,
                            ThumbnailURL = model.Thumbnail,
                            Category = model.Category,
                            EmployeeId = model.EmployeeID,
                            Title = model.Title,
                            Description = model.Description,
                            RelatedId = relatedID,
                        };

                        await DB.EmployeeDownload.AddAsync(responseData);
                    }
                    else
                    {
                        existingData.RelatedId = model.RelatedId;
                        existingData.ThumbnailURL = model.Thumbnail;
                        existingData.Description = model.Description;
                        existingData.Category = model.Category;
                        existingData.EmployeeId = model.EmployeeID;
                    }
                    await DB.SaveChangesAsync();
                    return StatusCode(200, BaseResponse.ResponseOk(responseData));
                }
                else {
                    var jsonDescDeserialize = JsonConvert.DeserializeObject<RootNews>(model.Description);

                    string relatedID = jsonDescDeserialize.NewsId.ToString();

                    var responseData = new EmployeeDownload();
                    var existingData = DB.EmployeeDownload.Where(Q => Q.EmployeeId == model.EmployeeID && Q.RelatedId == relatedID).FirstOrDefault();

                    if (existingData == null)
                    {
                        responseData = new EmployeeDownload
                        {
                            Guid = Guid.NewGuid().ToString(),
                            CreatedBy = "System",
                            UpdatedBy = "System",
                            URL = model.URL,
                            ThumbnailURL = model.Thumbnail,
                            Category = model.Category,
                            EmployeeId = model.EmployeeID,
                            Title = model.Title,
                            Description = model.Description,
                            RelatedId = relatedID,
                        };

                        await DB.EmployeeDownload.AddAsync(responseData);
                    }
                    else
                    {
                        existingData.RelatedId = model.RelatedId;
                        existingData.ThumbnailURL = model.Thumbnail;
                        existingData.Description = model.Description;
                        existingData.Category = model.Category;
                        existingData.EmployeeId = model.EmployeeID;
                    }
                    await DB.SaveChangesAsync();
                    return StatusCode(200, BaseResponse.ResponseOk(responseData));
                }
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetData(VMEmployeeDownloadParameter parameters)
        {
            if (parameters.Limit == 0)
            {
                parameters.Limit = 10;
            }

            if (parameters.Page == 0)
            {
                parameters.Page = 1;
            }

            parameters.Page = (parameters.Page - 1) * parameters.Limit;

            try
            {
                var query = DB.EmployeeDownload.AsQueryable();
                if (parameters.Category != null)
                {
                    query = query.Where(Q => parameters.Category.Contains(Q.Category));
                }

                if (!String.IsNullOrEmpty(parameters.EmployeeID))
                {
                    query = query.Where(Q => Q.EmployeeId == parameters.EmployeeID);
                }

                if (!String.IsNullOrEmpty(parameters.GUID))
                {
                    query = query.Where(Q => Q.Guid == parameters.GUID);
                }

                if (!String.IsNullOrEmpty(parameters.RelatedId))
                {
                    query = query.Where(Q => Q.RelatedId == parameters.RelatedId);
                }

                foreach (var q in query)
                {
                    if (q.Category.ToLower() != "news")
                    {
                        var jsonDescDeserialize = JsonConvert.DeserializeObject<Root>(q.Description);
                        if (!string.IsNullOrEmpty(q.RelatedId) && q.RelatedId != "null")
                        {
                            if (!string.IsNullOrEmpty(jsonDescDeserialize.learningModel.trainingId.ToString()))
                            {
                                var queryCourse = await (from t in DB.Trainings
                                                         join c in DB.Courses on t.CourseId equals c.CourseId
                                                         join b in DB.Blobs on c.BlobId equals b.BlobId
                                                         where t.TrainingId == int.Parse(q.RelatedId)
                                                         select new
                                                         {
                                                             BlobID = b.BlobId,
                                                             MimeFile = b.Mime,
                                                             Title = c.CourseName
                                                         }).FirstOrDefaultAsync();

                                var courseThumbnail = await FileService.GetFileAsync(queryCourse.BlobID.ToString(), queryCourse.MimeFile);

                                foreach (var courseModule in jsonDescDeserialize.downloadModel)
                                {
                                    var queryGetModule = await (from m in DB.Modules
                                                                join b in DB.Blobs on m.BlobId equals b.BlobId
                                                                join mb in DB.Blobs on m.MaterialBlobId equals mb.BlobId
                                                                where m.ModuleName == courseModule.name
                                                                select new
                                                                {
                                                                    moduleBlobID = m.BlobId,
                                                                    moduleMimeFile = b.Mime,
                                                                    moduleContentBlobID = m.MaterialBlobId,
                                                                    moduleContentMimeFile = mb.Mime,
                                                                    moduleTitle = m.ModuleName
                                                                }).FirstOrDefaultAsync();

                                    if (queryGetModule != null)
                                    {
                                        courseModule.courseImageUrl = courseThumbnail;
                                        courseModule.moduleImageUrl = await FileService.GetFileAsync(queryGetModule.moduleBlobID.ToString(), queryGetModule.moduleMimeFile);
                                        courseModule.imageUrl = courseModule.moduleImageUrl;
                                        if (queryGetModule.moduleContentMimeFile.ToLower() == "mp4" || queryGetModule.moduleContentMimeFile.ToLower() == "wmv" || queryGetModule.moduleContentMimeFile.ToLower() == "vob" || queryGetModule.moduleContentMimeFile.ToLower() == "mkv" || queryGetModule.moduleContentMimeFile.ToLower() == "m4v" || queryGetModule.moduleContentMimeFile.ToLower() == "ifo")
                                        {
                                            courseModule.url = await FileService.GetDownloadFileAsync(queryGetModule.moduleContentBlobID.ToString(), queryGetModule.moduleContentMimeFile);

                                        }
                                        else
                                        {
                                            courseModule.url = await FileService.GetFileAsync(queryGetModule.moduleContentBlobID.ToString(), queryGetModule.moduleContentMimeFile);

                                        }
                                    }
                                }

                                jsonDescDeserialize.learningModel.imageUrl = courseThumbnail;
                                q.ThumbnailURL = courseThumbnail;
                                q.Title = queryCourse.Title;

                            }
                            else
                            {
                                var queryModule = await (from sm in DB.SetupModules
                                                         join m in DB.Modules on sm.ModuleId equals m.ModuleId
                                                         join b in DB.Blobs on m.BlobId equals b.BlobId
                                                         join mb in DB.Blobs on m.MaterialBlobId equals mb.BlobId
                                                         where sm.SetupModuleId == int.Parse(q.RelatedId)
                                                         select new
                                                         {
                                                             BlobID = b.BlobId,
                                                             MimeFile = b.Mime,
                                                             Title = m.ModuleName,
                                                             moduleContentBlobID = m.MaterialBlobId,
                                                             moduleContentMimeFile = mb.Mime,
                                                         }).FirstOrDefaultAsync();

                                var moduleThumbnail = await FileService.GetFileAsync(queryModule.BlobID.ToString(), queryModule.MimeFile);

                                foreach (var moduleContent in jsonDescDeserialize.downloadModel)
                                {
                                    moduleContent.courseImageUrl = moduleThumbnail;
                                    moduleContent.moduleImageUrl = moduleThumbnail;
                                    moduleContent.imageUrl = moduleContent.moduleImageUrl;
                                    if (queryModule.moduleContentMimeFile.ToLower() == "mp4" || queryModule.moduleContentMimeFile.ToLower() == "wmv" || queryModule.moduleContentMimeFile.ToLower() == "vob" || queryModule.moduleContentMimeFile.ToLower() == "mkv" || queryModule.moduleContentMimeFile.ToLower() == "m4v" || queryModule.moduleContentMimeFile.ToLower() == "ifo")
                                    {
                                        moduleContent.url = await FileService.GetDownloadFileAsync(queryModule.moduleContentBlobID.ToString(), queryModule.moduleContentMimeFile);

                                    }
                                    else
                                    {
                                        moduleContent.url = await FileService.GetFileAsync(queryModule.moduleContentBlobID.ToString(), queryModule.moduleContentMimeFile);
                                    }
                                }

                                jsonDescDeserialize.learningModel.imageUrl = moduleThumbnail;
                                q.ThumbnailURL = moduleThumbnail;
                                q.Title = queryModule.Title;

                            }
                        }

                     

                        var jsonDescSerialize = JsonConvert.SerializeObject(jsonDescDeserialize);

                        q.Description = jsonDescSerialize;
                    }
                    else
                    {
                        var jsonDescDeserialize = JsonConvert.DeserializeObject<RootNews>(q.Description);
                        if (!string.IsNullOrEmpty(q.RelatedId) && q.RelatedId != "null")
                        {

                            var queryNews = await (from n in DB.News
                                                     join b in DB.Blobs on n.ThumbnailBlobId equals b.BlobId
                                                     where n.NewsId == int.Parse(q.RelatedId)
                                                     select new
                                                     {
                                                         BlobID = b.BlobId,
                                                         MimeFile = b.Mime,
                                                     }).FirstOrDefaultAsync();

                            var queryNewsLink = await (from n in DB.News
                                                       join b in DB.Blobs on n.FileBlobId equals b.BlobId
                                                       where n.NewsId == int.Parse(q.RelatedId)
                                                       select new
                                                       {
                                                           BlobID = b.BlobId,
                                                           MimeFile = b.Mime,
                                                       }).FirstOrDefaultAsync();

                            var newsThumbnail = await FileService.GetFileAsync(queryNews.BlobID.ToString(), queryNews.MimeFile);
                            var newsContent = await FileService.GetFileAsync(queryNewsLink.BlobID.ToString(), queryNewsLink.MimeFile);

                       

                            jsonDescDeserialize.ThumbnailUrl = newsThumbnail;
                           

                            jsonDescDeserialize.Link = newsContent;

                            
                        }

                        var jsonDescSerialize = JsonConvert.SerializeObject(jsonDescDeserialize);
                        q.URL = jsonDescDeserialize.Link;
                        q.ThumbnailURL = jsonDescDeserialize.ThumbnailUrl;
                        //q.Description = jsonDescSerialize;

                    }

                } 



                var data = await  query.Take(parameters.Limit).Skip(parameters.Page).ToListAsync();

                return StatusCode(200, BaseResponse.ResponseOk(data));
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> Delete(string  id)
        {
            try
            {
                var existingData = DB.EmployeeDownload.Where(Q => Q.Guid == id).FirstOrDefault();
                if (existingData == null) {
                    var msg = new Message();
                    msg.En = "User not found";
                    msg.Id = "User tidak ditemukan";
                    return StatusCode(400, BaseResponse.BadRequest(msg));
                }

                DB.EmployeeDownload.Remove(existingData);
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
