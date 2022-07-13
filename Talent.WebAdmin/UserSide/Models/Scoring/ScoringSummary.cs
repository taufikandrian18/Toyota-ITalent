using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models.LiveAssesment
{
    public class ScoringSummaryRequest
    {
        public string SuperiorEmployeeGuid { get; set; }
        public bool IsCoach { get; set; }
        public int TrainingId { get; set; }
        public bool IsTamPeople { get; set; }
        public string EnumCategory {get;set;}
        public string TrainingTypeId {get;set;}

        public int Page {get;set;}
        public int Limit {get;set;}
    }

    public class AttemptTask
    {
        public int attempts { get; set; }
    }

    public class ScoringSummaryResponse
    {
        public double EnrollmentPercentage { get; set; }
        public double CertificationPercentage { get; set; }
        public double XEnroll { get; set; }
        public double YEnroll { get; set; }
        public double XCertification { get; set; }
        public double YCertification { get; set; }
    }

    public class ScoringSummaryDetailResponse {
        public double CertificatePercentage {get;set;}
        public double EnrollmentPercentage {get;set;}
        public List<ScoringSummaryDetailMemberResponse> EnrollmentMembers{get;set;}
        public List<ScoringSummaryDetailMemberResponse> CertificateMembers{get;set;}
    }

    public class ScoringSummaryDetailMemberResponse {
        public string EmployeeName {get;set;}
        public string EnrollmentDate {get;set;}
        public string OutletName {get;set;}
    }

    public class ScoringCourseSummaryResponse
    {
        public double PrePercentage { get; set; }
        public double PreX {get;set;}
        public double PreY {get;set;}
        public double DuringPercentage { get; set; }
        public double DuringX {get;set;}
        public double DuringY {get;set;}
        public double PostPercentage { get; set; }
        public double PostX {get;set;}
        public double PostY {get;set;}
        public bool PostNA {get;set;}
        public bool DuringNA {get;set;}
        public bool PreNA {get;set;}
    }


    public class ScoringCourseSummaryResponseDetail
    {
        public double Percentage { get; set; }

        public List<ScoringCourseSummaryMemberResponse> Data {get;set;}

    }

    public class ScoringCourseSummaryMemberResponse {
        public string EmployeeName {get;set;}
        public double Count{get;set;}
        public string EmployeeId {get;set;}
        public string OutletName {get;set;}
        public List<ScoringCourseSummaryAttemptResponse> Scores {get;set;}
    }

    public class ScoringCourseSummaryAttemptResponse {
        public string Name {get;set;}
        public string GUID {get;set;}
        public bool Status {get;set;}
        public float Orders {get;set;}
        public float ChildOrders { get; set; }
        public string ParentId {get;set;}
        public string Category {get;set;}
        public string Score {get;set;}
        public string EnumStatus {get;set;}
    }

    public class ScoreAndStatus {
        public string Status {get;set;}
        public string Score {get;set;}
    }

    public class ScoringCoachExcel {
        public string Name {get;set;}
        public string OutletName {get;set;}

        public string EnrollmentDate {get;set;}
    }
    


    public class CourseReportRequest
    {
        public string SuperiorEmployeeGuid { get; set; }
        public bool IsCoach { get; set; }
        public bool IsTamPeople {get;set;}

        public bool IsGetAllCourse {get;set;}
        public int? ProgramTypeId {get;set;}

        public int Page {get;set;}
        public int Limit {get;set;}
        public string Search {get;set;}
    }

    public class ResponseCourse
    {
        public int TrainingID { get; set; }
        public int CourseID {get;set;}
        public String CourseName { get; set; }
        public String CourseRating { get; set; }
        public String CourseImageUrl { get; set; }
        public int? CoursePrice { get; set; }
        public int? CoursePercentage { get; set; } = 0;
        public int? CourseTimePoint { get; set; } = 0;
        public bool IsAssesment { get; set; }
        public bool IsFinished { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? RelatedTrainingId {get;set;}
        public int? RelatedCourseId {get;set;}

    }

    public class ResponseListUser {
        public string EmployeeGUID { get; set; }
        public string EmployeeName { get; set; }
        public List<ResponseCourse> Courses { get; set; }
    }

   
}
    