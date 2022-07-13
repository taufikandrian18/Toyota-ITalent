using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Announcements
    {
        public Announcements()
        {
        }

        [Key]
        public string AnnouncementID { get; set; }
        public string  Title { get; set; }
        public string AnnouncementFileID { get; set; }
        public string AnnouncementFileType { get; set; }
        public string  Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
    }
}
