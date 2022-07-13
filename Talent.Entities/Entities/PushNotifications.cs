using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class PushNotifications
    {

        public PushNotifications()
        {
            PushNotificationRecipients = new HashSet<PushNotificationRecipients>();
        }
        [Key]
        [StringLength(64)]
        public string Guid { get; set; }
        [StringLength(64)]
        public string SenderEmployeeId { get; set; }
        public string NotificationCategoryId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsPublished { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        [ForeignKey("SenderEmployeeId")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("NotificationCategoryId")]
        public virtual PushNotificationCategories Category { get; set; }

        public virtual ICollection<PushNotificationRecipients> PushNotificationRecipients { get; set; }
    }
}
