using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class DashboardTop5TopicData
    {
        public string TopicName { get; set; }
        public int Total { get; set; }
    }

    public class DashboardTop5LearningData
    {
        public string CourseName { get; set; }
        public double Rating { get; set; }
    }

    public class DashboardTop5CoachData
    {
        public string CoachName { get; set; }
        public double Rating { get; set; }
    }

    public class DashboardTop5EventData
    {
        public string Name { get; set; }
    }

    public class DashboardTop5RewardTypeData
    {
        public string RewardType { get; set; }
        public int TotalReward { get; set; }
    }

    public class DashboardNPSReportData
    {
        public int Promotor { get; set; }
        public int Passive { get; set; }
        public int Detractors { get; set; }
    }

    public class LearningViewModel
    {
        public int TrainingId { get; set; }
        public int TrainingBatch { get; set; }
        public string ImageUrl { get; set; }
        public string TrainingName { get; set; }
        public double? Rating { get; set; }
        public string ProgramTypeName { get; set; }
        public bool IsPopular { get; set; }
        public string Mime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class DashboardEvaluatedCompetencyMappingModel
    {
        public string PrefixCode { get; set; }
        public int CompetencyTypeId { get; set; }
        public int ProficiencyLevel { get; set; }
        public string KeyActionCode { get; set; }
        public int EvaluationTypeId { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
    }

    public class ReportCompetencyTracking
    {
        public string Program { get; set; }
        public string Course { get; set; }
        public string EmployeeID { get; set; }
        public string Dealer { get; set; }
        public string Outlet { get; set; }
        public string Area { get; set; }
        public string AreaSpecialist { get; set; }
        public string Position { get; set; }
        public string EmployeeName { get; set; }
        public string Points { get; set; }
        public string PointsDuring { get; set; }
        public string PointsPost { get; set; }
        public ReportTrackingCompetencyPoints[] PointsJSON { get; set; }
        public ReportTrackingCompetencyPoints[] PointsDuringJSON { get; set; }
        public ReportTrackingCompetencyPoints[] PointsPostJSON { get; set; }
        public string Video { get; set; }
    }

    public class ReportTrackingCompetencyPoints
    {
        public string SetupModuleID { get; set; }
        public int TrainingTypeID { get; set; }
        public string ModuleName { get; set; }
        public string Point { get; set; }
    }

    public class ReportKPITracking
    {
        public string OutletID { get; set; }
        public string Dealer { get; set; }
        public string Outlet { get; set; }
        public string Area { get; set; }
        public string ResultTotal { get; set; }
        public int Cycle2Target { get; set; }
        public int Cycle2Participant { get; set; }
        public string Cycle2ParticipantPercentage { get; set; }
        public int Cycle2Passed { get; set; }
        public string Cycle2PassedPercentage { get; set; }
        public int Cycle2Video { get; set; }
        public string Cycle2VideoPercentage { get; set; }
        public int Cycle2Certified { get; set; }
        public string Cycle2CertifiedPercentage { get; set; }
        public int Cycle3Target { get; set; }
        public int Cycle3Participant { get; set; }
        public string Cycle3ParticipantPercentage { get; set; }
        public int Cycle3Passed { get; set; }
        public string Cycle3PassedPercentage { get; set; }
        public int Cycle3Video { get; set; }
        public string Cycle3VideoPercentage { get; set; }
        public int Cycle3Certified { get; set; }
        public string Cycle3CertifiedPercentage { get; set; }
        public string ResultSPV { get; set; }
        public string ResultBM { get; set; }
    }

    public class ReportKPITracking2
    {
        public string OutletID { get; set; }
        public string Dealer { get; set; }
        public string Outlet { get; set; }
        public string Area { get; set; }
        public string ResultTotal { get; set; }
        public ReportKPITracking2ResultFormated [] ResultTotalFormated { get; set; }
    }

    public class ReportKPITracking2ResultFormated
    {
        public string ModuleName { get; set; }
        public string ModuleType { get; set; }
        public string IsPercent { get; set; }
        public string ModuleValue { get; set; }
    }
}
