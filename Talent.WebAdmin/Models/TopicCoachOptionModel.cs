using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class TopicCoachOptionModel
    {
        /// <summary>
        /// store topic Id for option on coach
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// store topic name for option on coach
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// store topic ebadge id for option on coach
        /// </summary>
        public int EbadgeId { get; set; }

        /// <summary>
        /// store topic ebadge name for option on coach
        /// </summary>
        public string EbadgeName { get; set; }
    }
}
