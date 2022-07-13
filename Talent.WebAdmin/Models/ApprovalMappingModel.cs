using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ApprovalMappingModel
    {
        public int ApprovalMappingid { get; set; }
        public string PageId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ApprovalLevelId { get; set; }
        public string PositionName { get; set; }
        public string PageName { get; set; }
        public string Description { get; set; }
    }

    public class ApprovalMappingFormModel
    {
        [Key]
        public int? ApprovalMappingid { get; set; }
        [Required]
        [StringLength(8)]
        public string PageId { get; set; }
        public int ApprovalLevelId { get; set; }
    }

    public class ApprovalMappingFilterModel : PageAbstract
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int? Level { get; set; }
        public string PageName { get; set; }
        public string PositionName { get; set; }

    }

    public class ApprovalMappingViewModel
    {
        public List<ApprovalMappingModel> ListApprovalMapping { get; set; }
        public int TotalData { get; set; }
    }

    public class PagesModel
    {
        public string PageId { get; set; }
        public string Name { get; set; }
        public bool NeedApproval { get; set; }
    }

    public class ApprovalLevelModel
    {
        public int ApprovalLevelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }


}
