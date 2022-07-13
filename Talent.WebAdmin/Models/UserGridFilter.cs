using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class UserGridFilter : GridFilter
    {
        /// <summary>
        /// module name filter
        /// </summary>
        public string UserRole { get; set; }

        /// <summary>
        /// material type id for filter
        /// </summary>
        public bool? TypeofPeople { get; set; }

        /// <summary>
        /// topic name for filter
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// date start range filter
        /// nullable
        /// </summary>
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// date end range filter
        /// nullable
        /// </summary>
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
