using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class HobbyCreateModel
    {
        [Required]
        public string HobbyName { get; set; }
        public string HobbyDescription { get; set; }
    }

    public class HobbyUpdateModel
    {
        public int HobbyId { get; set; }
        [Required]
        public string HobbyName { get; set; }
        public string HobbyDescription { get; set; }
    }

    public class HobbyViewModel
    {
        public int HobbyId { get; set; }
        public string HobbyName { get; set; }
        public string HobbyDescription { get; set; }
    }

    public class HobbyGridViewModel
    {
        public int HobbyId { get; set; }
        public string HobbyName { get; set; }
        public string HobbyDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class HobbyGridModel
    {
        public List<HobbyGridViewModel> Grid { get; set; }
        public int TotalFilterData { get; set; }
        public int TotalData { get; set; }
    }

    public class HobbyFilterModel: GridFilter
    {
        public string HobbyName { get; set; }
    }
}
