using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Events
    {
        public Events()
        {
            ApprovalToEvents = new HashSet<ApprovalToEvents>();
            EmployeeEventMappings = new HashSet<EmployeeEventMappings>();
            EventOutletMappings = new HashSet<EventOutletMappings>();
            EventPositionMappings = new HashSet<EventPositionMappings>();
            Rewards = new HashSet<Rewards>();
        }

        [Key]
        public int EventId { get; set; }
        public int? EventCategoryId { get; set; }
        public Guid? BlobId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        [Required]
        [StringLength(255)]
        public string HostName { get; set; }
        public string Description { get; set; }
        [Required]
        [StringLength(255)]
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public int? TotalView { get; set; }
        public bool IsDeleted { get; set; }
        [StringLength(64)]
        public string Source { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("Events")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("EventCategoryId")]
        [InverseProperty("Events")]
        public virtual EventCategories EventCategory { get; set; }
        [InverseProperty("Event")]
        public virtual ICollection<ApprovalToEvents> ApprovalToEvents { get; set; }
        [InverseProperty("Event")]
        public virtual ICollection<EmployeeEventMappings> EmployeeEventMappings { get; set; }
        [InverseProperty("Event")]
        public virtual ICollection<EventOutletMappings> EventOutletMappings { get; set; }
        [InverseProperty("Event")]
        public virtual ICollection<EventPositionMappings> EventPositionMappings { get; set; }
        [InverseProperty("Event")]
        public virtual ICollection<Rewards> Rewards { get; set; }
    }
}
