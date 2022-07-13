using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class GridTopicModel
    {
        /// <summary>
        /// contain all the topic data before or after the filter
        /// </summary>
        public List<TopicViewModel> Topics { get; set; }

        /// <summary>
        /// total data in topic table
        /// </summary>
        public int TotalData { get; set; }

        /// <summary>
        /// total data after filter
        /// </summary>
        public int TotalDataFilter { get; set; }
    }
}
