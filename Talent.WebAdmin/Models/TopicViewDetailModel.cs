using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin
{
    public class TopicViewDetailModel
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
        public int? EBadgeId { get; set; }

        /// <summary>
        /// store topic minimum point for view
        /// </summary>
        public int MinPoint { get; set; }

        public Guid? BlobId { get; set; }

        public string BlobName { get; set; }

        public string BlobFileType { get; set; }
    }
}
