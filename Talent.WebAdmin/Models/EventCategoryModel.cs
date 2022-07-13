using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class EventCategoryModel
    {
        public int EventCategoryId { get; set; }
        public string EventCategoryName { get; set; }
        public string EventCategoryDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class EventCategoryViewModel
    {
        public List<EventCategoryModel> ListEventCategoryModel { get; set; }
        public int TotalItem { get; set; }
    }

    public class EventCategoryFormModel
    {
        public int EventCategoryId { get; set; }
        [Required]
        [StringLength(64)]
        public string EventCategoryName { get; set; }
        [StringLength(1024)]
        public string EventCategoryDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class EventCategoryFilter : PageAbstract
    {
        public string EventCategoryName { get; set; }
    }
}
