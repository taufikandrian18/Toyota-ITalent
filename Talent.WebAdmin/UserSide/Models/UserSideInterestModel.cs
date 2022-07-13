namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideInterestModel
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string TopicDescription { get; set; }
    }

    public class UserSideInterestProfileFormModel
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string TopicDescription { get; set; }
        public bool IsChecked { get; set; }
    }
}