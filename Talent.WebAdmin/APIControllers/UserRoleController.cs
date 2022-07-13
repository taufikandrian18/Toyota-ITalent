using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/user-role")]
    public class UserRoleController : Controller
    {
        private readonly UserRoleService UserRoleService;
        private readonly DropdownService DropdownService;

        public UserRoleController(UserRoleService userRoleService, DropdownService dropdownService)
        {
            this.UserRoleService = userRoleService;
            this.DropdownService = dropdownService;
        }

        /// <summary>
        /// Get User Role with filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("get-role")]
        public async Task<ActionResult<UserRoleGridModel>> GetUserRole([FromQuery] UserGridFilter filter)
        {

            var result = await this.UserRoleService.GetAllUserRoles(filter);

            return Ok(result);
        }

        [HttpPost("post-user-role", Name = "insert-user-role")]
        public async Task<ActionResult> PostUserRole([FromBody] UserRoleModelCreate model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Model is not valid");
            }

            var result = await UserRoleService.InsertUserRole(model);

            if(result == false)
            {
                return BadRequest("Position already has a User Role");
            }

            return Ok();
        }

        [HttpDelete("delete-user-role")]
        public async Task<ActionResult<bool>> DeleteKeyAction(int id)
        {
            var isSuccess = await this.UserRoleService.DeleteUserRole(id);

            return Ok(isSuccess);
        }

        [HttpPost("update-user-role")]
        public async Task<ActionResult> UpdateUserRole([FromBody] UserRoleModelUpdate model)
        {
            if(ModelState.IsValid == false){
                return BadRequest("Model State is not valid");
            }

            var result = await this.UserRoleService.UpdateUserRole(model);

            if(result == false)
            {
                return BadRequest("Position already exist!");
            }

            return Ok();
        }

        [HttpGet("tam-position-dropdown")]
        public async Task<ActionResult<List<PositionDropdownModel>>> TamPositionDropdown()
        {
            var data = await this.DropdownService.GetTamPosition();

            return Ok(data);
        }

        [HttpGet("dealer-position-dropdown")]
        public async Task<ActionResult<List<PositionDropdownModel>>> DealerPositionDropdown()
        {
            var data = await this.DropdownService.GetDealerPosition();

            return Ok(data);
        }

        [HttpGet("user-role-dropdown")]
        public async Task<ActionResult<List<UserRoleDropdownModel>>> UserRoleDropdown()
        {
            var data = await this.DropdownService.GetUserRole();

            return Ok(data);
        }

        [HttpGet("category-dropdown")]
        public async Task<ActionResult<List<CategoryDropdownModel>>> CategoryDropdown()
        {
            var data = await this.DropdownService.GetCategory();

            return Ok(data);
        }
        
        [HttpGet("get-user-role-data")]
        public async Task<ActionResult<UserRoleModelUpdate>> GetUserRoleById([FromQuery] int userRoleId)
        {
            var result = await this.UserRoleService.GetDataUpdate(userRoleId);

            return Ok(result);
        }

        [HttpGet("get-user-role-name")]
        public async Task<bool> GetUserRoleName(string roleName)
        {
            var roleNameIsExistWhenCreate = await this.UserRoleService.CheckRoleName(roleName);

            if (roleNameIsExistWhenCreate == true)
            {
                return true;
            }

            else return false;
        }

        [HttpGet("get-user-role-name-by-id")]
        public async Task<bool> GetUserRoleNameById(int roleId, string roleName)
        {
            var roleNameIsExistWhenUpdate = await this.UserRoleService.CheckRoleNameByRoleId(roleId, roleName);

            if (roleNameIsExistWhenUpdate == true)
            {
                return true;
            }

            return false;
        }

        [HttpGet("view-user-role-detail")]
        public async Task<ActionResult<UserRoleModelViewDetail>> ViewUserRoleDetail([FromQuery] int roleId)
        {
            var roleNameViewDetail = await this.UserRoleService.GetUserRoleDetail(roleId);

            return Ok(roleNameViewDetail);
        }
    }
}
