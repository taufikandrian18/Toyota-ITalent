using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class TopicGridFilter : GridFilter
    {
        /// <summary>
        /// date start range filter
        /// nullable
        /// </summary>
        public DateTimeOffset? DateStart { get; set; }

        /// <summary>
        /// date end range filter
        /// nullable
        /// </summary>
        public DateTimeOffset? DateEnd { get; set; }

        /// <summary>
        /// topic name filter
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// topic ebadge filter
        /// nullable
        /// </summary>
        public int? Ebadge { get; set; }

        /// <summary>
        /// topic minimum point filter
        /// nullable
        /// </summary>
        public int? MinPoint { get; set; }
    }
}
