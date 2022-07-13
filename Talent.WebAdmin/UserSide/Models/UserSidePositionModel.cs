namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSidePositionModel
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionDescription { get; set; }

        public bool IsOtherDealer { get; set; }
    }
}