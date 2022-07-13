using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class DealerDropdownModel
    {
        public string DealerId { get; set; }
        public string DealerName { get; set; }
        public bool IsOther { get; set; }
    }

    public class OutletDropdownModel
    {
        public string OutletId { get; set; }
        public string OutletName { get; set; }
    }

    public class PositionDropdownModel
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public bool IsOther { get; set; }
        public bool IsTamPeople { get; set; }
    }

    public class EmployeeDropdownModel
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }

    public class RewardTypeDropdownModel
    {
        public int RewardTypeId { get; set; }
        public string RewardTypeName { get; set; }
    }

    public class ModuleDropdownModel
    {
        public int? ModuleId { get; set; }
        public string ModuleName { get; set; }
    }

    public class CoachDropdownModel
    {
        public int CoachId { get; set; }
        public string CoachName { get; set; }
    }

    public class EventDropdownModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
    }

    public class RewardPointTypeDropdown
    {
        public int RewardPointTypeId { get; set; }
        public string RewardPointTypeName { get; set; }
    }

    public class MaterialTypeDropdownModel
    {
        public int MaterialTypeId { get; set; }
        public string MaterialTypeName { get; set; }
    }

    public class TopicDropdownModel
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
    }

    public class ApprovalStatusDropdownModel
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
