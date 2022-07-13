using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class PushNotificationRecipients
    {

      
        [Key]
        [StringLength(64)]
        public string Guid { get; set; }
        [StringLength(64)]
        public string RecipientEmployeeId { get; set; }
        public string NotificationId { get; set; }
        public DateTime ReadDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        [ForeignKey("RecipientEmployeeId")]
        public virtual Employees Employee { get; set; }
        [ForeignKey("NotificationId")]
        public virtual PushNotifications Notification { get; set; }
    }
}
