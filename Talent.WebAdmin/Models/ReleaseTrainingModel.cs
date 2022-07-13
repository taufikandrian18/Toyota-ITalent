using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
  //public class ReleaseTrainingModel
  //{
  //    public int TrainingId { get; set; }
  //    public int CourseId { get; set; }
  //    public int Batch { get; set; }
  //    public DateTime StartDate { get; set; }
  //    public DateTime EndDate { get; set; }
  //    public int Quota { get; set; }
  //    public string Location { get; set; }
  //    public bool IsAccommodate { get; set; }
  //    public bool IsParticipantTrainee { get; set; }
  //    public bool IsPublished { get; set; }
  //    public string CreatedBy { get; set; }
  //    public DateTime CreatedAt { get; set; }
  //    public DateTime UpdatedAt { get; set; }
  //    public DateTime? ApprovedAt { get; set; }
  //}

  public class ReleaseTrainingFormModel
  {
    public int? TrainingId { get; set; }
    public CourseReleaseTrainingModel Course { get; set; }
    public int Batch { get; set; }
    [Column(TypeName = "date")]
    public int? Quota { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Location { get; set; }
    public bool? IsAccommodate { get; set; }
    public bool IsParticipantTrainee { get; set; }
    public bool IsParticipantPermanent { get; set; }
    public string EnumCertificationTtrigger { get; set; }
    public string EnumCertificate { get; set; }
    public bool IsCertificate { get; set; }

    //public bool IsPublished { get; set; }
    //[Required]
    //[StringLength(64)]
    //public string CreatedBy { get; set; }
    //public DateTime CreatedAt { get; set; }
    //public DateTime UpdatedAt { get; set; }
    //public DateTime? ApprovedAt { get; set; }

    public List<TrainingModuleFormModel> ListSetupModule { get; set; }
    public List<TrainingOutletFormModel> ListOutlet { get; set; }
    public List<TrainingPositionModel> ListPosition { get; set; }
    public List<TrainingPositionModel> ListPositionOnlyView { get; set; }
    public int InputType { get; set; }

    public List<Certifications> ListCertifications { get; set; }

  }

    public class Certifications {
        public int CourseId { get; set; }
    }

  public class TrainingModuleFormModel
  {
    public int? TrainingModuleMappingId { get; set; }
    public int? TrainingId { get; set; }
    public DateTime? TrainingStart { get; set; }
    public DateTime? TrainingEnd { get; set; }
    public TeachingTimepPointsModel TeachingTimePoint { get; set; }
    public CoachForReleaseTraining Coach { get; set; }
    public int? SetupModuleId { get; set; }
    public string TrainingTypesName { get; set; }
    public string ModuleName { get; set; }
  }

  public class TrainingPositionModel
  {
    public int TrainingId { get; set; }
    public int PositionId { get; set; }
  }

  public class TrainingOutletFormModel
  {
    public int TrainingId { get; set; }
    public string OutletId { get; set; }
  }

  public class CourseReleaseTrainingModel
  {
    public int CourseId { get; set; }
    public string AssesmentId { get; set; }
    public string CourseName { get; set; }
    public string ProgramTypeName { get; set; }
    public int LearningTypeId { get; set; }


  }

  public class ReleaseTrainingFilter : PageAbstract
  {
    public int? ProgramTypeId { get; set; }
    public string CourseName { get; set; }
    public int? Batch { get; set; }
    public int? ApprovalStatusId { get; set; }
    public DateTimeOffset? DateFilterStart { get; set; }
    public DateTimeOffset? DateFilterEnd { get; set; }
    public DateTimeOffset? EnrollmentStartDate { get; set; }
    public DateTimeOffset? EnrollmentEndDate { get; set; }
  }

    public class ReleaseTrainingByDealerFilter : PageAbstract
    {
        public int? ProgramTypeId { get; set; }
        public string CourseName { get; set; }
        public int? Batch { get; set; }
        public int? ApprovalStatusId { get; set; }
        public string DealerId { get; set; }
        public DateTimeOffset? DateFilterStart { get; set; }
        public DateTimeOffset? DateFilterEnd { get; set; }
        public DateTimeOffset? EnrollmentStartDate { get; set; }
        public DateTimeOffset? EnrollmentEndDate { get; set; }
    }

    public class ApprovalStatusViewModels
  {
    public int ApprovalId { get; set; }
    public string ApprovalName { get; set; }
  }

  public class TeachingTimepPointsModel
  {
    public int? TimePointId { get; set; }
    public int? Time { get; set; }
    public int? Points { get; set; }
  }

  public class ReleaseTrainingSetupModuleModel
  {
    public string TrainingTypesName { get; set; }
    public string ModuleName { get; set; }
    public int SetupModuleId { get; set; }
    public string Type { get; set; }
  }

  public class CoachForReleaseTraining
  {
    public string EmployeeName { get; set; }
    public string EmployeeId { get; set; }
    public int? CoachId { get; set; }

  }

  public class PositionViewModel
  {
    public int PositionId { get; set; }
    public string PositionName { get; set; }
  }

  public class PositionOnlyViewModel
  {
    public int PositionId { get; set; }
    public string PositionName { get; set; }
  }

  public class AreaViewModel
  {
    public string AreaId { get; set; }
    public string Name { get; set; }
  }

  public class RegionViewModel
  {
    public int? RegionId { get; set; }
    public string RegionName { get; set; }
  }

  public class ProvinceViewModel
  {
    public string ProvinceId { get; set; }
    public string ProvinceName { get; set; }
  }

  public class DealerViewModel
  {
    public string DealerId { get; set; }
    public string DealerName { get; set; }
  }

  public class CityViewModel
  {
    public string CityId { get; set; }
    public string CityName { get; set; }
  }

  public class OutletViewModel
  {
    public string OutletId { get; set; }
    public string Name { get; set; }
  }

  public class OutletCompleteViewModel
  {
    public string OutletId { get; set; }
    public string ProvinceId { get; set; }
    public string AreaId { get; set; }
    public string CityId { get; set; }
    public string DealerId { get; set; }
    public string Name { get; set; }
    public string AreaSpecialistId { get; set; }
  }

  public class FilterForOutletModel
  {
    public List<AreaViewModel> Area { get; set; }
    public List<DealerViewModel> Dealer { get; set; }
    public List<ProvinceViewModel> Province { get; set; }
    public List<CityViewModel> City { get; set; }
  }

  public class FilterIdForOutletModel
  {
    public List<string> Area { get; set; }
    public List<string> Dealer { get; set; }
    public List<string> Province { get; set; }
    public List<string> City { get; set; }
  }


  public class ReleaseTrainingModel
  {
    public int TrainingId { get; set; }
    public string CourseName { get; set; }
    public int Batch { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string ApprovalStatus { get; set; }
  }

  public class ReleaseTrainingViewModel
  {
    public List<ReleaseTrainingModel> ListTraining { get; set; }
    public int TotalData { get; set; }
  }

  public class RelaseTrainingDetailModel
  {
    public int? TrainingId { get; set; }
    public CourseReleaseTrainingModel Course { get; set; }
    public int Batch { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Quota { get; set; }
    public string Location { get; set; }
    public bool? IsAccommodate { get; set; }
    public bool IsParticipantPermanent { get; set; }
    public bool IsParticipantTrainee { get; set; }
    public string EnumCertificationTrigger { get; set; }
    public bool IsCertificate { get; set; }
    public string EnumCertificate { get; set; }
    public List<TrainingModuleFormModel> ListSetupModule { get; set; }
    public List<OutletViewModel> ListOutlet { get; set; }
    public List<PositionViewModel> ListPosition { get; set; }
    public List<PositionOnlyViewModel> ListPositionOnlyView { get; set; }
    public List<AreaViewModel> ListArea { get; set; }
    public List<RegionViewModel> ListRegion { get; set; }
    public List<ProvinceViewModel> ListProvince { get; set; }
    public List<CityViewModel> ListCity { get; set; }
    public List<DealerViewModel> ListDealer { get; set; }

    public List<TrainingCertificateView> TrainingCertificateView {get;set;}


  }

  public class TrainingCertificateView {
      public int? RelatedTrainingId {get;set;}
      public string RelatedTrainingName {get;set;}

  }

  public class TotalCourseDetail
  {
    public int TotalPoints { get; set; }
    public int TotalModule { get; set; }
    public int TotalTime { get; set; }
    public int TotalScore { get; set; }
  }

}
