using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class PushNotificationCategories
    {

        public PushNotificationCategories()
        {
            PushNotifications = new HashSet<PushNotifications>();
        }

        [Key]
        [StringLength(64)]
        public string Guid { get; set; }
        [StringLength(64)]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<PushNotifications> PushNotifications { get; set; }

    }
}
