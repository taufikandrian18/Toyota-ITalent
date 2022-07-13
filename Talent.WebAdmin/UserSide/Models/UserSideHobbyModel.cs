namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideHobbyModel
    {
        public int HobbyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UserSideHobbyProfileFormModel
    {
        public int HobbyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsChecked { get; set; }
    }
}