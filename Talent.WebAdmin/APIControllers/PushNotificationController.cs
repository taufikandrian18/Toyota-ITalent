using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/push-notifications")]
    public class PushNotificationController : Controller
    {
        private readonly PushNotificationService ServiceMan;

        public PushNotificationController(PushNotificationService service)
        {
            this.ServiceMan = service;
        }

     
        [HttpPost("create-push-notifications")]
        public async Task<ActionResult<BaseResponse>> Create([FromBody] VMPushNotificationAdd model)
        {
            var getData = await ServiceMan.CreatePushNotifications(model);
            return getData;
        }

        [HttpGet("get-push-notifications")]
        public async Task<ActionResult<BaseResponse>> Get(VMParameterPushNotification model)
        {
            var getData = await ServiceMan.GetListPushNotifications(model);
            return getData;
        }

        [HttpGet("get-push-notifications-detail")]
        public async Task<ActionResult<BaseResponse>> GetDetail(string  id)
        {
            var getData = await ServiceMan.GetPushNotifications(id);
            return getData;
        }

        [HttpDelete("delete-push-notifications")]
        public async Task<ActionResult<BaseResponse>> Delete(string id)
        {
            var getData = await ServiceMan.DeletePushNotifications(id);
            return getData;
        }

        [HttpPut("update-push-notifications")]
        public async Task<ActionResult<BaseResponse>> Update([FromBody] VMPushNotificationUpdate model)
        {
            var getData = await ServiceMan.UpdatePushNotifications(model);
            return getData;
        }

        [HttpPost("create-push-notifications-categories")]
        public async Task<ActionResult<BaseResponse>> CreateCategories([FromBody] PushNotificationCategories model)
        {
            var getData = await ServiceMan.CreatePushNotificationCategories(model);
            return getData;
        }

        [HttpGet("get-push-notifications-categories")]
        public async Task<ActionResult<BaseResponse>> GetCategories(string id)
        {
            var getData = await ServiceMan.GetPushNotificationCategories(id);
            return getData;
        }

        [HttpDelete("delete-push-notifications-categories")]
        public async Task<ActionResult<BaseResponse>> DeleteCategories(string id)
        {
            var getData = await ServiceMan.DeletePushNotificationCategories(id);
            return getData;
        }

        [HttpPut("update-push-notifications-categories")]
        public async Task<ActionResult<BaseResponse>> UpdateCategories([FromBody] PushNotificationCategories model)
        {
            var getData = await ServiceMan.UpdatePushNotificationCategories(model);
            return getData;
        }

        [HttpPost("create-push-notifications-recipients")]
        public async Task<ActionResult<BaseResponse>> CreateRecipients([FromBody] PushNotificationRecipients model)
        {
            var getData = await ServiceMan.CreatePushNotificationRecipients(model);
            return getData;
        }

        [HttpGet("get-push-notifications-recipients")]
        public async Task<ActionResult<BaseResponse>> GetRecipients(string id)
        {
            var getData = await ServiceMan.GetPushNotificationRecipients(id);
            return getData;
        }

        [HttpDelete("delete-push-notifications-recipients")]
        public async Task<ActionResult<BaseResponse>> DeleteRecipients(string id)
        {
            var getData = await ServiceMan.DeletePushNotificationRecipients(id);
            return getData;
        }

        [HttpPut("update-push-notifications-recipients")]
        public async Task<ActionResult<BaseResponse>> UpdateRecipients([FromBody] PushNotificationRecipients model)
        {
            var getData = await ServiceMan.UpdatePushNotificationRecipients(model);
            return getData;
        }

        [HttpGet("get-push-notification-web-admin-recipients")]
        public async Task<ActionResult<BaseResponse>> GetAllPushNotificationWebAdminRecipients()
        {
            var getData = await ServiceMan.PushNotifWebAdmin();
            return getData;
        }
    }
}
