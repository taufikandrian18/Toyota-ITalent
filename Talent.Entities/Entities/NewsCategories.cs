using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class NewsCategories
    {
        public NewsCategories()
        {
            News = new HashSet<News>();
        }

        [Key]
        public int NewsCategoryId { get; set; }
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [InverseProperty("NewsCategory")]
        public virtual ICollection<News> News { get; set; }
    }
}
