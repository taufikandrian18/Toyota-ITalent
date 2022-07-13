using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public abstract class PageAbstract
    {
        public string SortBy { get; set; }
        public int PageNumber { get; set; }
    }
}
