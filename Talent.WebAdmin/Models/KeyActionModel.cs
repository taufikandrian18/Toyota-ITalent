using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class KeyActionModel
    {
        public int KeyActionId { get; set; }
        public string KeyActionCode { get; set; }
        public string KeyActionName { get; set; }
        public string KeyActionDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class KeyActionViewModel
    {
        public List<KeyActionModel> ListActionModel { get; set; }
        public int TotalItem { get; set; }
    }

    public class KeyActionFormModel
    {
        public int? KeyActionId { get; set; }
        [Required]
        [StringLength(16)]
        public string KeyActionCode { get; set; }
        [Required]
        [StringLength(255)]
        public string KeyActionName { get; set; }
        [StringLength(1024)]
        public string KeyActionDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class KeyActionFilter : PageAbstract
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string KeyActionCode { get; set; }
        public string KeyActionName { get; set; }
    }

}
