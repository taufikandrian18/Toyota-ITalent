using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class TopicCreateModel
    {
        /// <summary>
        /// topic name for insert
        /// </summary>
        [Required]
        public string TopicName { get; set; }

        /// <summary>
        /// topic desc for insert
        /// </summary>
        public string TopicDesc { get; set; }

        /// <summary>
        /// topic ebadge for insert
        /// </summary>
        [Required]
        public int EBadge { get; set; }

        /// <summary>
        /// topic minimum point for insert
        /// </summary>
        [Required]
        public int MinPoint { get; set; }

        /// <summary>
        /// topic file upload for insert
        /// </summary>
        public FileContent FileContent { get; set; }
    }
}
