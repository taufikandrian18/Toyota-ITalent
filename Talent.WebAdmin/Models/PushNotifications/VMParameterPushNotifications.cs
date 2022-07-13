using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Models
{
    public class VMParameterPushNotification
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string CategoryId { get; set; }
        public string SortField { get; set; }
        public string Sort { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public string Query { get; set; }
    }

    public class VMPushNotificationWebAdminModel
    {
        public string NotificationText { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
