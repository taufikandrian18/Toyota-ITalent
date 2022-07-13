using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing news filter data.
    /// </summary>
    public class UserSideNewsFilterModel
    {
        public string SortBy { get; set; }
        public int ItemPerPage { get; set; }
        public int PageIndex { get; set; }
        public string Author { set; get; }
        public string Category { set; get; }
    }
}
