namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing reward filter data.
    /// </summary>
    public class UserSideRewardFilterModel
    {
        public string SortBy { get; set; }
        public string Name { set; get; }
        public int? TypeId { set; get; }
        public bool IsUseTeachingPoint { set; get; }
        public int ItemPerPage { set; get; }
        public int PageIndex { set; get; }
    }
}
