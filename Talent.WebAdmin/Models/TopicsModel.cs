using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class TopicsModel
    {
        public int TopicId { get; set; }
        public int? EbadgeId { get; set; }
        public Guid? BlobId { get; set; }
        [Required]
        [StringLength(64)]
        public string TopicName { get; set; }
        public int TopicMinimumPoints { get; set; }
        [StringLength(1024)]
        public string TopicDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class EbadgeModel
    {

        public int EbadgeId { get; set; }
        [Required]
        [StringLength(64)]
        public string EbadgeName { get; set; }

    }

    public class TopicsEbadgeJoinModelForCoach
    {
        public int EbadgeId { get; set; }
        public string EbadgeName { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
    }

}
