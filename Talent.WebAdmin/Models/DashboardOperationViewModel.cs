using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{

    public class DashboardApprovalListViewModel
    {
        public List<DashboardApprovalData> ListApproval { get; set; }
    }

    public class DashboardApprovalData
    {
        public string ApprovalContent { get; set; }
        public int Remaining { get; set; }
        public int Completed { get; set; }
    }

    public class DashboardUsersThisYearListViewModel
    {
        public int JanuaryUsersData { get; set; }
        public int FebruaryUsersData { get; set; }
        public int MarchUsersData { get; set; }
        public int AprilUsersData { get; set; }
        public int MayUsersData { get; set; }
        public int JuneUsersData { get; set; }
        public int JulyUsersData { get; set; }
        public int AugustUsersData { get; set; }
        public int SeptemberUsersData { get; set; }
        public int OctoberUsersData { get; set; }
        public int NovemberUsersData { get; set; }
        public int DecemberUsersData { get; set; }
    }

    public class DashboardUsersThisYearData
    {
        public string EmployeeId { get; set; }
        public int Month { get; set; }
    }

    public class DashboardClassListViewModel
    {
        public List<DashboardClassData> ThisWeekClassList { get; set; }
        public int TotalThisWeekClass { get; set; }
        public List<DashboardClassData> NextWeekClassList { get; set; }
        public int TotalNextWeekClass { get; set; }
    }

    public class DashboardClassData
    {
        public string CourseName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class DashboardTop5TopicViewModel
    {
        public List<DashboardTop5TopicData> TopicsList { get; set; }
    }

    public class DashboardTop5TopicData
    {
        public string TopicName { get; set; }
        public int TopicTotal { get; set; }
    }

    public class DashboardTop5LearningViewModel
    {
        public List<DashboardTop5LearningData> LearningsList { get; set; }
    }

    public class DashboardTop5LearningData
    {
        public string CourseName { get; set; }
        public float Rating { get; set; }
    }

    public class DashboardTotalLearningLibraryViewModel
    {
        public int Total { get; set; }
        public int TotalCourses { get; set; }
        public int TotalTask { get; set; }
        public int TotalModules { get; set; }
        public int TotalPopQuiz { get; set; }
    }

    public class DashboardTotalLearningLibraryData
    {
        public string CategoryName { get; set; }
        public int Total { get; set; }
    }

    public class DashboardTop5NewsViewModel
    {
        public List<DashboardTop5NewsData> TopNewsList { get; set; }
    }

    public class DashboardTop5NewsData
    {
        public string Title { get; set; }
    }

    public class DashboardTop5EventsViewModel
    {
        public List<DashboardTop5EventsData> TopEventsList { get; set; }
    }

    public class DashboardTop5EventsData
    {
        public string Title { get; set; }
    }

    public class DashboardTop5CoachViewModel
    {
        public List<DashboardTop5CoachData> CoachList { get; set; }
    }

    public class DashboardTop5CoachData
    {
        public string CoachName { get; set; }
        public float Rating { get; set; }
    }

    public class DashboardTop5RewardTypeViewModel
    {
        public List<DashboardTop5RewardTypeData> TopRewardTypeList { get; set; }
    }

    public class DashboardTop5RewardTypeData
    {
        public string RewardType { get; set; }
        public int TotalReward { get; set; }
    }

    public class DashboardNPSReportViewModel
    {
        public float Promotor { get; set; }
        public float Passive { get; set; }
        public float Detractors { get; set; }
    }

    public class DashboardMyInsightViewModel
    {
        public DashboardMyInsightData ThisMonth { get; set; }
        public DashboardMyInsightData LastMonth { get; set; }
        public DashboardMyInsightData Last2Month { get; set; }
    }

    public class DashboardMyInsightData
    {
        public string GetMonth { get; set; }
        public int TotalInsight { get; set; }
    }

}
