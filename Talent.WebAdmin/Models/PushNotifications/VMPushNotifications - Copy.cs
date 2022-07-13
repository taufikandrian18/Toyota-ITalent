using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;

namespace Talent.WebAdmin.Models
{
    public class VMPushNotificationList
    {
        public string Guid { get; set; }
        public string SenderName { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsPublished { get; set; }
        public string Category { get; set; }
        public DateTime SendDate { get; set; }
    }

    public class VMPushNotificationAdd
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string SenderId { get; set; }
        public bool IsPublished { get; set; }
        public List<string> GroupPositions { get; set; }
        public List<string> ManPowerPosition { get; set; }

        public List<string> SpecifiedEmployeeId { get; set; }

    }
    public class VMPushNotificationDataAdd
    {
        public string Category { get; set; }
        public int? DataID { get; set; }
        public int? DataSecondId { get; set; }
    }

    public class VMPushNotificationDataAddMyTools
    {
        public string Category { get; set; }
        public Guid DataID { get; set; }
        public Guid? DataSecondId { get; set; }
    }

    public class VMPushNotificationUpdate
    {
        public string Guid { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsPublished { get; set; }
        public string SenderId { get; set; }
        public List<string> GroupPositions { get; set; }
        public List<string> ManPowerPosition { get; set; }

    }

    public class VMPushNotificationDetail
    {
        public string Guid { get; set; }
        public string SenderName { get; set; }
        public string SenderId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsPublished { get; set; }

        public List<VMRecipients> Recipients { get; set; }
        public string Category { get; set; }
        public string CategoryID { get; set; }
        public DateTime SendDate { get; set; }
    }

    public class VMRecipients
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? Readdate { get; set; }
    }
}
