using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class TopicViewModel
    {
        /// <summary>
        /// store topic id for view
        /// may not be changed
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// store topic name for view
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// store topic description for view
        /// </summary>
        public string TopicDesc { get; set; }

        /// <summary>
        /// store topic ebadge for view
        /// </summary>
        public int? EBadge { get; set; }

        /// <summary>
        /// store topic minimum point for view
        /// </summary>
        public int MinPoint { get; set; }

        /// <summary>
        /// store date when topic is created
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// store date when topic is updated
        /// </summary>
        public string UpdatedAt { get; set; }
    }
}
