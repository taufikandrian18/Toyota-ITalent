using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.Services
{
    public class PushNotificationService : Controller
    {
        private readonly TalentContext DB;


        public PushNotificationService(TalentContext talentContext)
        {
            this.DB = talentContext;
        }

        public async Task<ActionResult<BaseResponse>> CreatePushNotifications(VMPushNotificationAdd model)
        {
            try
            {

                var insertData = new PushNotifications {
                    Guid = Guid.NewGuid().ToString(),
                    Title = model.Title,
                    Body = model.Body,
                    IsPublished = model.IsPublished,
                    SenderEmployeeId = model.SenderId,
                    SendDate = DateTime.Now,
                    NotificationCategoryId = "ANNOUNCEMENT",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                var data = DB.PushNotifications.Add(insertData);
                await DB.SaveChangesAsync();

                if (insertData.IsPublished)
                {
                    await SendNotiifications(model);
                }
                return BaseResponse.Created(null);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
        public async Task<ActionResult<BaseResponse>> CreatePushNotificationRemedialScores(VMPushNotificationAdd model, VMPushNotificationDataAdd dataJson)
        {
            try
            {

                var insertData = new PushNotifications
                {
                    Guid = Guid.NewGuid().ToString(),
                    Title = model.Title,
                    Body = model.Body,
                    IsPublished = model.IsPublished,
                    SenderEmployeeId = model.SenderId,
                    SendDate = DateTime.Now,
                    NotificationCategoryId = "REMEDIAL",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                var data = DB.PushNotifications.Add(insertData);
                await DB.SaveChangesAsync();

                if (insertData.IsPublished)
                {
                    if (dataJson.Category.Trim() == "Remedial")
                    {
                        await SendNotiificationRemedials(model, dataJson);
                        insertData.Body += ";" + dataJson.Category + ";" + dataJson.DataID + ";" + dataJson.DataSecondId;
                    }
                }
                return BaseResponse.Created(null);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
        public async Task<ActionResult<BaseResponse>> CreatePushNotificationRemedials(VMPushNotificationAdd model)
        {
            try
            {

                var insertData = new PushNotifications
                {
                    Guid = Guid.NewGuid().ToString(),
                    Title = model.Title,
                    Body = model.Body,
                    IsPublished = model.IsPublished,
                    SenderEmployeeId = model.SenderId,
                    SendDate = DateTime.Now,
                    NotificationCategoryId = "REMEDIAL",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                var data = DB.PushNotifications.Add(insertData);
                await DB.SaveChangesAsync();

                if (insertData.IsPublished)
                {
                    await SendNotiifications(model);
                }
                return BaseResponse.Created(null);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> CreatePushNotificationMyTools(VMPushNotificationAdd model, VMPushNotificationDataAddMyTools dataJson)
        {
            try
            {
                var insertData = new PushNotifications
                {
                    Guid = Guid.NewGuid().ToString(),
                    Title = model.Title,
                    Body = model.Body,
                    IsPublished = model.IsPublished,
                    SenderEmployeeId = model.SenderId,
                    SendDate = DateTime.Now,
                    NotificationCategoryId = "MYTOOLS",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                var data = DB.PushNotifications.Add(insertData);

                if (insertData.IsPublished)
                {
                    if(dataJson.Category.Trim() == "Product")
                    {

                        Thread pushNotificationThread = new Thread(() => SendNotificationMyTools(model, dataJson));
                        pushNotificationThread.Start();
                       
                        insertData.Body += ";" + dataJson.Category + ";" + dataJson.DataID + ";" + dataJson.DataSecondId;
                    }
                }
                await DB.SaveChangesAsync();
                return BaseResponse.Created(null);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> UpdatePushNotifications(VMPushNotificationUpdate model)
        {
            try
            {
                var existingData = DB.PushNotifications.FirstOrDefault(Q => Q.Guid == model.Guid);
                if (existingData == null)
                {
                    var message = new Message();

                    message.En = "Data not found";
                    message.Id = "Data tidak ditemukan";
                    return BaseResponse.BadRequest(null, message);
                }

                var temp = existingData.IsPublished;
                existingData.Title = model.Title;
                existingData.Body = model.Body;
                existingData.IsPublished = model.IsPublished;
                existingData.UpdatedAt = DateTime.Now;
                await DB.SaveChangesAsync();

                if (model.IsPublished && temp == false)
                {
                    await SendNotiifications(new VMPushNotificationAdd { 
                        Body = existingData.Body,
                        Title = existingData.Title,
                    
                    });
                }
                return BaseResponse.Created(null);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> DeletePushNotifications(string guid)
        {
            try
            {
                var existingData = DB.PushNotifications.FirstOrDefault(Q => Q.Guid == guid);
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

        public async Task<ActionResult<BaseResponse>> GetListPushNotifications(VMParameterPushNotification model)
        {
            try
            {
                if (model.Limit == 0)
                {
                    model.Limit = 10;
                }

                if (model.Page == 0)
                {
                    model.Page = 1;
                }

                model.Page = (model.Page - 1) * model.Limit;
                var count = 0;

                var query = DB.PushNotifications.Include(Q => Q.Employee).Include(Q => Q.Category).Where(q => q.Category.Guid == "ANNOUNCEMENT").AsQueryable();

                if (!String.IsNullOrWhiteSpace(model.Id))
                {
                    query = query.Where(Q => Q.Guid == model.Id).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(model.CategoryId))
                {
                    query = query.Where(Q => Q.NotificationCategoryId == model.CategoryId).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(model.SenderId))
                {
                    query = query.Where(Q => Q.SenderEmployeeId == model.SenderId).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(model.Query))
                {
                    query = query.Where(Q => Q.Title.Contains(model.Query) || Q.Body.Contains(model.Query) || Q.Employee.EmployeeName.Contains(model.Query));
                }

                if (!String.IsNullOrWhiteSpace(model.Sort))
                {
                    if (model.Sort.ToLower() == "desc")
                    {
                        query = query.OrderByDescending(Q => Q.SendDate).AsQueryable();
                    }
                    if (model.Sort.ToLower() == "asc")
                    {
                        query = query.OrderBy(Q => Q.SendDate).AsQueryable();
                    }
                }

                count = query.Count();
                var data = await query.Select(Q => new VMPushNotificationList
                {
                    Guid = Q.Guid,
                    SenderName = Q.Employee.EmployeeName,
                    Title = Q.Title,
                    Body = Q.Body,
                    IsPublished = Q.IsPublished,
                    Category = Q.Category.Name,
                    SendDate = Q.SendDate,
                }).Skip(model.Page).Take(model.Limit).ToListAsync();

                return BaseResponse.ResponseOk(data.OrderByDescending(q => q.SendDate), count);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> GetPushNotifications(string id)
        {
            try
            {

                var query = await DB.PushNotifications.Include(Q => Q.Category).Include(Q => Q.Employee).Include(Q => Q.PushNotificationRecipients).ThenInclude(Q => Q.Employee).Select(Q =>
                 new VMPushNotificationDetail
                 {
                     Guid = Q.Guid,
                     CategoryID = Q.NotificationCategoryId,
                     Category = Q.Category.Name,
                     Title = Q.Title,
                     SenderName = Q.Employee.EmployeeName,
                     SenderId = Q.SenderEmployeeId,
                     SendDate = Q.SendDate,
                     Body = Q.Body,
                     IsPublished = Q.IsPublished
                     //Recipients = Q.PushNotificationRecipients.Select(Y => new VMRecipients
                     //{
                     //    EmployeeID = Y.RecipientEmployeeId,
                     //    Readdate = Y.ReadDate,
                     //    EmployeeName = Y.Employee.EmployeeName,
                     //}).ToList(),
                 }).FirstOrDefaultAsync(Q => Q.Guid == id);

                return BaseResponse.ResponseOk(query);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }
        public async Task<ActionResult<BaseResponse>> CreatePushNotificationCategories(PushNotificationCategories model)
        {
            try
            {
                var data = DB.PushNotificationCategories.Add(model);
                await DB.SaveChangesAsync();
                return BaseResponse.Created(null);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> UpdatePushNotificationCategories(PushNotificationCategories model)
        {
            try
            {
                var existingData = DB.PushNotificationCategories.FirstOrDefault(Q => Q.Guid == model.Guid);
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

        public async Task<ActionResult<BaseResponse>> DeletePushNotificationCategories(string guid)
        {
            try
            {
                var existingData = DB.PushNotificationCategories.FirstOrDefault(Q => Q.Guid == guid);
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

        public async Task<ActionResult<BaseResponse>> GetPushNotificationCategories(string guid)
        {
            try
            {
                var data = await DB.PushNotificationCategories.Where(Q => Q.Guid == guid).ToListAsync();
                return BaseResponse.ResponseOk(data);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }


        public async Task<ActionResult<BaseResponse>> CreatePushNotificationRecipients(PushNotificationRecipients model)
        {
            try
            {

                var data = DB.PushNotificationRecipients.Add(model);
                await DB.SaveChangesAsync();
                return BaseResponse.Created(null);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task<ActionResult<BaseResponse>> UpdatePushNotificationRecipients(PushNotificationRecipients model)
        {
            try
            {
                var existingData = DB.PushNotificationRecipients.FirstOrDefault(Q => Q.Guid == model.Guid);
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

        public async Task<ActionResult<BaseResponse>> DeletePushNotificationRecipients(string guid)
        {
            try
            {
                var existingData = DB.PushNotificationRecipients.FirstOrDefault(Q => Q.Guid == guid);
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

        public async Task<ActionResult<BaseResponse>> GetPushNotificationRecipients(string guid)
        {
            try
            {
                var data = await DB.PushNotificationRecipients.Where(Q => Q.Guid == guid).Include(Q => Q.Employee).Include(Q => Q.Notification).ToListAsync();
                return BaseResponse.ResponseOk(data);
            }
            catch (Exception x)
            {
                return StatusCode(500, BaseResponse.Error(null, x));
            }
        }

        public async Task SendNotiifications(VMPushNotificationAdd model) {
            var listOfToken = await DB.UserFcmTokens.ToListAsync();

            if (model.SpecifiedEmployeeId != null && model.SpecifiedEmployeeId.Count() > 0){
                listOfToken = listOfToken.Where(x=> model.SpecifiedEmployeeId.Contains(x.EmployeeId)).ToList();
            }

            foreach (var item in listOfToken)
            {
                if (!String.IsNullOrEmpty(item.Token))
                {
                    var data = await PushNotificationHelpers.SendNotificationJSON(deviceId: item.Token, title: model.Title, body: model.Body, "", "", Guid.Empty, Guid.Empty);
                }
            }
        }
        public async Task SendNotiificationRemedials(VMPushNotificationAdd model, VMPushNotificationDataAdd dataJson)
        {
            try
            {
                //var listOfToken = await DB.UserFcmTokens.ToListAsync();
                var listOfToken = await DB.UserFcmTokens.Where(x => x.EmployeeId == model.SpecifiedEmployeeId[0]).ToListAsync();

                //if (model.SpecifiedEmployeeId != null && model.SpecifiedEmployeeId.Count() > 0)
                //{
                //    listOfToken = listOfToken.Where(x => model.SpecifiedEmployeeId.Contains(x.EmployeeId.ToLower())).ToList();
                //}

                foreach (var item in listOfToken)
                {
                    if (!String.IsNullOrEmpty(item.Token))
                    {
                        var data = await PushNotificationHelpers.SendNotificationJSONRemed(deviceId: item.Token, title: model.Title, body: model.Body, "FLUTTER_NOTIFICATION_CLICK", dataJson.Category, dataJson.DataID, dataJson.DataSecondId);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendNotificationMyTools(VMPushNotificationAdd model, VMPushNotificationDataAddMyTools dataJson)
        {
            var listOfToken = await DB.UserFcmTokens.ToListAsync();

            foreach (var item in listOfToken)
            {
                if (!String.IsNullOrEmpty(item.Token))
                {
                    var data = await PushNotificationHelpers.SendNotificationJSON(deviceId: item.Token, title: model.Title, body: model.Body, "FLUTTER_NOTIFICATION_CLICK", dataJson.Category,dataJson.DataID,dataJson.DataSecondId.HasValue ? dataJson.DataSecondId.Value : Guid.Empty);
                }
            }
        }

        public async Task<ActionResult<BaseResponse>> PushNotifWebAdmin()
        {
            var NotifWebAdminList = new List<VMPushNotificationWebAdminModel>();

            var queryProducts = await DB.Products.Where(x => x.ApprovedAt == null).ToListAsync();
            if (queryProducts.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryProducts.Count() + " Product Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryProductTypes = await DB.ProductTypes.Where(x => x.ApprovedAt == null).ToListAsync();
            if (queryProductTypes.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryProductTypes.Count() + " Product Type Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryProductCustomer = await DB.ProductCustomers.Where(x => x.ApprovedAt == null).ToListAsync();
            if (queryProductCustomer.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryProductCustomer.Count() + " Product Customer Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryProductCompetitor = await DB.ProductCompetitorMappings.Where(x => x.ApprovedAt == null).ToListAsync();
            if (queryProductCompetitor.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryProductCompetitor.Count() + " Product Competitor Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryProductFeature = await DB.ProductFeatureMappings.Where(x => x.ProductFeatureMappingApprovalStatus != "Published").ToListAsync();
            if(queryProductFeature.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryProductFeature.Count() + " Product Feature Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryProductTip = await DB.ProductTips.Where(x => x.IsApproved != "Published").ToListAsync();
            if (queryProductTip.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryProductTip.Count() + " Product Tips Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryProductSPWAs = await DB.ProductSPWAMappings.Where(x => x.ApprovedAt == null).ToListAsync();
            if (queryProductSPWAs.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryProductSPWAs.Count() + " Product SPWA Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryProductPrograms = await DB.ProductProgramMappings.Where(x => x.ApprovedAt == null).ToListAsync();
            if (queryProductPrograms.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryProductPrograms.Count() + " Product Program Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryProductGalleries = await DB.ProductGalleries.Where(x => x.IsApproved != "Published").ToListAsync();
            if (queryProductGalleries.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryProductGalleries.Count() + " Product Gallery Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryProductPhotos = await DB.ProductPhotos.Where(x => x.ApprovedAt == null).ToListAsync();
            if (queryProductPhotos.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryProductPhotos.Count() + " Product Photos Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryProductFAQs = await DB.ProductFAQs.Where(x => x.ApprovedAt == null).ToListAsync();
            if (queryProductFAQs.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryProductFAQs.Count() + " Product FAQ Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryProductKPOM = await DB.KeyPieceOfMinds.Where(x => x.ApprovedAt == null).ToListAsync();
            if (queryProductKPOM.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryProductKPOM.Count() + " Key Piece of Mind Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryDictionaries = await DB.Dictionaries.Where(x => x.ApprovedAt == null).ToListAsync();
            if (queryDictionaries.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryDictionaries.Count() + " Dictionary Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryKodawaris = await DB.Kodawaris.Where(x => x.ApprovedAt == null).ToListAsync();
            if (queryKodawaris.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryKodawaris.Count() + " Kodawari Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryServiceTips = await DB.ServiceTips.Where(x => x.ApprovedAt == null).ToListAsync();
            if (queryServiceTips.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryServiceTips.Count() + " Service Tips Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            var queryHOGuidelines = await DB.HOGuidelines.Where(x => x.IsApproved == false).ToListAsync();
            if (queryHOGuidelines.Count() > 0)
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = queryHOGuidelines.Count() + "HO Guidelines Need to be Approved";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);
            }

            if (NotifWebAdminList.Count() > 0)
            {
                return BaseResponse.ResponseOk(NotifWebAdminList);
            }
            else
            {
                var NotifWebAdminObj = new VMPushNotificationWebAdminModel();
                NotifWebAdminObj.NotificationText = "There are no data updates";
                NotifWebAdminObj.CreatedAt = DateTime.Now;

                NotifWebAdminList.Add(NotifWebAdminObj);

                return BaseResponse.ResponseOk(NotifWebAdminList);
            }
        }

    }
}
