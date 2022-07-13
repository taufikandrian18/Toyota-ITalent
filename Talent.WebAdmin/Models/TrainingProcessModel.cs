using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class TrainingProcessViewModel
    {
        public List<TrainingProcessModel> ListTrainingProcess { set; get; }
        public int TotalItem { set; get; }
    }

    public class TrainingProcessModel
    {
        public int TrainingId { set; get; }
        public string CourseName { set; get; }
        public string ProgramType { set; get; }
        public int Batch { set; get; }
        public int? Confirmed { set; get; }
        public int? Quota { set; get; }
        public DateTime? TrainingStartDate { set; get; }
        public DateTime? TrainingEndDate { set; get; }
    }

    public class TrainingProcessFilter : PageAbstract
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string CourseName { get; set; }
        public string ProgramTypeName { get; set; }
        public int? Batch { get; set; }
        public int? Confirmed { get; set; }
        public int? Quota { get; set; }
    }

    public class TrainingProcessDetailViewModel
    {
        public List<TrainingProcessDetailModel> TrainingProcessDetailList { get; set; }
    }

    public class TrainingProcessDetailModel
    {
        public int TrainingInvitationId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeId { get; set; }
        public DateTime CreatedAt{ get; set; }
        public string Gender { get; set; }
        public string Dealer { get; set; }
        public string Outlet { get; set; }
        public string Accommodation { get; set; }
        public string PositionId { get; set; }
        public string PositionName { get; set; }
        public int? Price { get; set; }
        public DateTime? DateofStayStart { get; set; }
        public DateTime? DateofStayEnd { get; set; }
        public bool IsJoined { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
