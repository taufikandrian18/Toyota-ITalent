using System;
using System.Collections.Generic;

namespace Talent.WebAdmin.Models
{
    //Filter Search
    public class  UserPrivilegeSettingsGridFilter : GridFilter
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string MenuId { get; set; }
        public string UserRole { get; set; }
        public string PageId { get; set; }
    }
    
    public class UserPrivilegeSettingsModel
    {
        public int PrivilegeSettingsId { get; set; }
        public int UserRoleId { get; set; }
        public string UserRole { get; set; }
        public string MenuId { get; set; }
        public string Menu { get; set; }
        public string PageId { get; set; }
        public string Page { get; set; }
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    //UserPrivilegeSettingPaginate
    public class UserPrivilegeSettingsPaginateModel
    {
        public List<UserPrivilegeSettingsModel> Data { get; set; }
        public int TotalData { get; set; }
    }

    //Add
    public class UserPrivilegeSettingsInsertModel
    {
        public int UserRoleId { get; set; }
        public string PageId { get; set; }
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }

    //UserRole
    public class UserPrivilegeSettingsUserRoleModel
    {
        public int UserRoleId { get; set; }
        public string UserRole { get; set; }
    }

    //Menu
    public class UserPrivilegeSettingsMenuModel
    {
        public string MenuId { get; set; }
        public string Menu { get; set; }
    }

    //Page
    public class UserPrivilegeSettingsPageModel
    {
        public string PageId { get; set; }
        public string Page { get; set; }
    }

    //Page CRUD
    public class UserPrivilegeSettingPageCRUD
    {
        public bool IsPageCheck { get; set; }
        public string PageId { get; set; }
        public string Page { get; set; }
        public string MenuId { get; set; }
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }

    //GetByUserRoleId

    public class UserPrivilegeSettingGetByUserId
    {
        public int UserRoleId { get; set; }
        public List<UserPrivilegeSettingPageCRUD> MenuPage { get; set; }
    }

    public class UserAccessCRUD
    {
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }


}
