using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin
{
    public class TopicUpdateModel
    {
        /// <summary>
        /// store topic id and may not be changed
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// topic name for update
        /// </summary>
        [Required]
        public string TopicName { get; set; }

        /// <summary>
        /// topic description for update
        /// </summary>
        public string TopicDesc { get; set; }

        /// <summary>
        /// topic ebadge for update
        /// </summary>
        [Required]
        public int EBadge { get; set; }

        /// <summary>
        /// topic minimun point for update
        /// </summary>
        [Required]
        public int MinPoint { get; set; }

        public Guid BlobId { get; set; }

        /// <summary>
        /// topic file upload for update
        /// </summary>
        public string BlobName { get; set; }

        public string BlobFileType { get; set; }

        public FileContent FileContent { get; set; }
    }
}
