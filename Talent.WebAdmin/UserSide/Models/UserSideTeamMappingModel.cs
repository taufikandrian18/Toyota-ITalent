using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSidePersonalMappingModel
    {
        public string EmployeeId { get; set; }

        public List<UserSideCompetencyMappingViewModel> ListHardCompetencyModel { get; set; }
        public List<UserSideCompetencyMappingViewModel> ListSoftCompetencyModel { get; set; }
    }

    public class UserSideCompetencyMappingViewModel
    {
        public string PrefixCode { get; set; }
        public string CompetencyName { get; set; }
        public string CompetencyDescription { get; set; }

        public string CompetencyTypeName { get; set; }
        public double ScoreTotal { get; set; }
        public double MaxScoreTotal { get; set; }

        public List<UsersideKeyactionViewModel> ListKeyAction { get; set; }
    }

    public class UsersideKeyactionViewModel
    {
        public int ProficiencyLevel { get; set; }
        public string KeyActionCode { get; set; }
        public string KeyActionName { get; set; }
        public string KeyActionDescription { get; set; }
        public double Score { get; set; }
        public double MaxScore { get; set; }
    }

    public class UserSideTeamMappingModel
    {
        public List<UserSideTeamCompetencyMappingViewModel> ListHardCompetencyModel { get; set; }
        public List<UserSideTeamCompetencyMappingViewModel> ListSoftCompetencyModel { get; set; }
    }


    public class UserSideTeamCompetencyMappingViewModel
    {
        public string PrefixCode { get; set; }
        public string CompetencyName { get; set; }
        public int CompetencyId { get; set; }
        public string CompetencyDescription { get; set; }
        public int CompetencyTypeId { get; set; }
        public string CompetencyTypeName { get; set; }
        public double ScoreTotal { get; set; }
        public double MaxScoreTotal { get; set; }
        public List<UsersideTeamKeyactionViewModel> ListKeyAction { get; set; }
    }

    public class UsersideTeamKeyactionViewModel
    {
        public int PositionId { get; set; }
        public string KeyActionCode { get; set; }
        public string KeyActionName { get; set; }
        public string KeyActionDescription { get; set; }
        public int ProficiencyLevel { get; set; }
        public double ScoreSubTotal { get; set; }
        public double MaxScoreSubTotal { get; set; }
        public List<UserSideTeamProficiencyViewModel> Proficiencies { get; set; }
    }

    public class UserSideTeamProficiencyViewModel
    {
        public string EmployeeId { get; set; }
        //public int ProficiencyLevel { get; set; }
        public string ImageUrl { get; set; }
        public int TeamId { get; set; }
        public int PossitionId { get; set; }
        public double Score { get; set; }
        public double MaxScore { get; set; }
    }
}
