using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Class for storing detail employee for My Profile.
    /// </summary>
    public class ProfileViewModel
    {
        public UserSideEmployeeModel Employee { get; set; }

        public List<UserSidePositionModel> Position { get; set; }

        public UserSideOutletModel Outlet { get; set; }

        public UserSideAreaModel Area { get; set; }

        public ProvinceViewModel Province { get; set; }

        public UserSideDealerModel Dealer { get; set; }

        public List<UserSideHobbyModel> Hobbies { get; set; }

        public List<UserSideInterestModel> Interests { get; set; }

        public List<UserSideCertificateModel> Certificates { get; set; }

        public UserSideTotalBadgeModel TotalBadge { get; set; }

        public bool? isDataMember { get; set; }
    }

    public class ProvinceViewModel
    {
        public string ProvinceId { get; set; }
        public string ProvinceName { get; set; }
    }

    public class ProfileUpdateModel
    {
        public string NickName { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string BioDescription { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        //file image content
        public FileContent FileUploaded { get; set; }
    }

    public class ProfileHobbyInterestModel
    {
        public List<UserSideHobbyModel> Hobbies { get; set; }

        public List<UserSideInterestModel> Interests { get; set; }
    }

    public class ProfileHomePage
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNickName { get; set; }
        public bool IsCoach { get; set; }
        public string ImageUrl { get; set; }
        public string Type { get; set; }

        public UserSideRankProfileHome RankUser { get; set; }
    }

    public class EmployeeLevelModel
    {
        public int LevelId { get; set; }
        public int? Exp { get; set; }
    }

    public class ProfileRankModel
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string PositionName { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int EmployeePoint { get; set; }
        public int Level { get; set; }
        public int TotalExperience { get; set; }
        public int Rank { set; get; }
        public bool IsLoggedInUser { set; get; }
        public string AreaName { set; get; }
        public string DealerName { set; get; }
    }
}