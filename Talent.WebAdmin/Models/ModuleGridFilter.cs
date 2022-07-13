using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class ModuleGridFilter : GridFilter
    {
        /// <summary>
        /// module name filter
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// material type id for filter
        /// </summary>
        public int MaterialTypeId { get; set; }

        /// <summary>
        /// topic name for filter
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// approval status for filter
        /// </summary>
        public int ApprovalStatus { get; set; }

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
    }
}
