using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    [Table("ITalentReports")]
    public partial class ItalentReports
    {
        [Column("ITalentReportId")]
        public int ItalentReportId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
    }
}
