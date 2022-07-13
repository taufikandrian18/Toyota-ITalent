namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing rank data.
    /// </summary>
    public class UserSideRankModel
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

    public class UserSideRankProfileHome
    {
        public int EmployeePoint { get; set; }
        public int Level { get; set; }
        public int TotalExperience { get; set; }
        public int Rank { set; get; }
    }
}