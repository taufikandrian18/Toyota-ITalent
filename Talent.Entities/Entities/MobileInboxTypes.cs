using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class MobileInboxTypes
    {
        public MobileInboxTypes()
        {
            MobileInboxes = new HashSet<MobileInboxes>();
        }

        [Key]
        public int MobileInboxTypeId { get; set; }
        [StringLength(64)]
        public string Name { get; set; }

        [InverseProperty("MobileInboxType")]
        public virtual ICollection<MobileInboxes> MobileInboxes { get; set; }
    }
}
