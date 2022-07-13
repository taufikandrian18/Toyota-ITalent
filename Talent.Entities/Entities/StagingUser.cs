using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class StagingUser
    {
        [Column("ID")]
        public int Id { get; set; }
        [Key]
        [StringLength(250)]
        public string Code { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(250)]
        public string NoReg { get; set; }
        [StringLength(150)]
        public string Email { get; set; }
        [Required]
        [StringLength(250)]
        public string State { get; set; }
    }
}
