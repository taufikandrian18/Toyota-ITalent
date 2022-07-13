using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Services;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideTrainingProcessModel
    {
    }

    public class UserSideTrainingProcessFilterModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsOrderedByDateAscending { get; set; }
    }

    /// <summary>
    /// Model class for storing user's training process data.
    /// </summary>
    public class UserSideTrainingProcessViewModel
    {
        public string ImageUrl { get; set; }
        public int TrainingInvId { get; set; }
        public string TrainingName { get; set; }
        public string ProgramType { get; set; }
        public int Batch { get; set; }

        public DateTime? TrainingStartDate { get; set; }
        public DateTime? TrainingEndDate { get; set; }

        //public string EnrollStartDate { get; set; }
        //public string EnrollEndDate { get; set; }
        public DateTime? EnrollDate { get; set; }

        public string TrainingStatus { get; set; }
        public bool IsRescheluded { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsRejected { get; set; }
        public bool IsAccommodate { get; set; }

    }

    public class UserSideTrainingProcessDetailModel
    {
        public int TrainingInvId { get; set; }
        public int TrainingId { get; set; }

        public string TrainingName { get; set; }
        public int Batch { get; set; }
        public DateTime? TrainingStartDate { get; set; }
        public DateTime? TrainingEndDate { get; set; }
        //public TimeSpan TrainingStartTime { get; set; }
        //public TimeSpan TrainingEndTime { get; set; }
        public string Location { get; set; }

        public string TrainingStatus { get; set; }
        public bool IsRescheluded { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsRejected { get; set; }


        public string EmployeeName { get; set; }
        public string EmployeePosition { get; set; }

        public decimal Price { get; set; }
        public List<UserSideAcomodationListModel> AccomodationList { get; set; }

        public bool NeedAccomodation { get; set; }
        public int SelectedAccomodation { get; set; }
        public DateTime? StayStartDate { get; set; }
        public DateTime? StayEndDate { get; set; }

    }

    public class UserSideTrainingProcessConfirmModel
    {
        public int TrainingInvId { get; set; }
        //confirm => yes, reject => no
        public string Answer { get; set; }
        public bool NeedAccomodation { get; set; }
        public UserSideAcomodationConfirmModel Accomodation { get; set; }
    }
}
