using System;
using System.ComponentModel.DataAnnotations;

namespace Talent.Entities.Entities
{
    public partial class UserLogActivities
    {
        [Key]
        [StringLength(64)]
        public string Guid { get; set; }
        [StringLength(64)]
        public string EmployeeId { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public string ContentId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
