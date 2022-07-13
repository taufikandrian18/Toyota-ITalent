using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class StagingOrganizationObject
    {
        [Column("ID")]
        public int Id { get; set; }
        [Key]
        [StringLength(250)]
        public string Code { get; set; }
        [StringLength(10)]
        public string ObjectType { get; set; }
        [Column("ObjectID")]
        [StringLength(20)]
        public string ObjectId { get; set; }
        [StringLength(50)]
        public string Abbreviation { get; set; }
        [StringLength(250)]
        public string ObjectText { get; set; }
        [StringLength(250)]
        public string ObjectDescription { get; set; }
        [StringLength(250)]
        public string State { get; set; }
    }
}
