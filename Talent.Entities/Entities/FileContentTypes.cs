using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class FileContentTypes
    {
        [StringLength(255)]
        public string ContentType { get; set; }
        [Column("MIME")]
        [StringLength(255)]
        public string Mime { get; set; }
    }
}
