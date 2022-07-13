using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/user-privilege-settings")]
    public class UserPrivilegeSettingsApiController : Controller
    {
        private readonly UserPrivilegeSettingsService _PrivilegeMan;

        public UserPrivilegeSettingsApiController(UserPrivilegeSettingsService userPrivilegeSettingsService)
        {
            this._PrivilegeMan = userPrivilegeSettingsService;
        }

        [HttpGet("get-all-user-privilege-settings-paginate")]
        public async Task<ActionResult<UserPrivilegeSettingsPaginateModel>> GetAllUserPrivilegeSettingsPaginate(UserPrivilegeSettingsGridFilter filter)
        {
            var data = await this._PrivilegeMan.GetAllUserPrivilegeSettingsPaginate(filter);
            return data;
        }

        //userrolenotset
        [HttpGet("get-all-user-privilege-settings-user-role-not-yet-set")]
        public async Task<ActionResult<List<UserPrivilegeSettingsUserRoleModel>>> GetAllUserPrivilegeSettingsUserRoleNotYetSet()
        {
            var data = await this._PrivilegeMan.GetAllUserPrivilegeSettingsUserRoleNotYetSet();
            return data;
        }

        //userrolenotset
        [HttpGet("get-all-user-privilege-settings-user-role-not-yet-set-except-self/{id}")]
        public async Task<ActionResult<List<UserPrivilegeSettingsUserRoleModel>>> GetAllUserPrivilegeSettingsUserRoleNotYetSetExceptSelf(int id)
        {
            var data = await this._PrivilegeMan.GetAllUserPrivilegeSettingsUserRoleNotYetSetExceptSelf(id);
            return data;
        }

        //userrole
        //[HttpGet("get-all-user-privilege-settings-user-role")]
        //public async Task<ActionResult<List<UserPrivilegeSettingsUserRoleModel>>> GetAllUserPrivilegeSettingsUserRole(string name)
        //{
        //    var data = await this._PrivilegeMan.GetAllUserPrivilegeSettingsUserRole(name);
        //    return data;
        //}

        //menu
        [HttpGet("get-all-user-privilege-settings-menu")]
        public async Task<ActionResult<List<UserPrivilegeSettingsMenuModel>>> GetAllUserPrivilegeSettingsMenu()
        {
            var data = await this._PrivilegeMan.GetAllUserPrivilegeSettingsMenu();
            return data;
        }

        //page
        [HttpGet("get-all-user-privilege-settings-page")]
        public async Task<ActionResult<List<UserPrivilegeSettingsPageModel>>> GetAllUserPrivilegeSettingsPage()
        {
            var data = await this._PrivilegeMan.GetAllUserPrivilegeSettingsPage();
            return data;
        }

        //MenuPage
        [HttpGet("get-all-user-privilege-settings-menu-page-crud-by-id")]
        public async Task<ActionResult<List<UserPrivilegeSettingPageCRUD>>> GetAllUserPrivilegeSettingsMenuPageCrudById(string id)
        {
            var data = await this._PrivilegeMan.GetAllUserPrivilegeSettingsMenuPageCrudById(id);
            return data;
        }

        [HttpGet("get-user-privilege-settings-by-user-role-id/{id}")]
        public async Task<ActionResult<UserPrivilegeSettingGetByUserId>> GetUserPrivilegeSettingsByUserRoleId(int id)
        {
            var data = await this._PrivilegeMan.GetUserPrivilegeSettingsById(id);
            return Ok(data);
        }

        [HttpGet("get-user-role-id-from-privilege-page-mappings-id/{id}")]
        public async Task<ActionResult<int>> GetUserRoleIdFromPrivilegePageMappingsId(int id)
        {
            var data = await this._PrivilegeMan.GetUserRoleIdFromPrivilegePageMappingsId(id);
            return data;
        }

        [HttpPost("insert-user-privilege-settings")]
        public async Task<ActionResult<bool>> InsertUserPrivilegeSettings([FromBody]List<UserPrivilegeSettingsInsertModel> insert)
        {
            var data = await this._PrivilegeMan.InsertUserPrivilegeSettings(insert);
            return Ok(true);
        }

        [HttpPost("update-user-privilege-settings")]
        public async Task<ActionResult<bool>> UpdateUserPrivilegeSettings(int id,[FromBody]List<UserPrivilegeSettingsInsertModel> update)
        {
            var data = await this._PrivilegeMan.UpdateUserPrivilegeSettings(id,update);
            return Ok(true);
        }

        [HttpDelete("delete-user-privilege-settings")]
        public async Task<ActionResult<bool>> Delete(int userRoleId,List<string> id)
        {
            var data = await this._PrivilegeMan.DeleteUserPrivilegeSettings(userRoleId, id);
            return Ok(data);
        }

        [HttpGet("crud-access-page")]
        public ActionResult<UserAccessCRUD> CRUDAccessPage(string pageId)
        {
            var data =  this._PrivilegeMan.CRUDAccessPage(pageId);
            return data;
        }

        [HttpGet("page-access")]
        public ActionResult<List<string>> PageAccess()
        {
            var data = this._PrivilegeMan.PageAccess();
            return data;
        }
    }
}
