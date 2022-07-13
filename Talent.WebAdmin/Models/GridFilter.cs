using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public abstract class GridFilter
    {
        /// <summary>
        /// sorting by table column
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// current page index in the html
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// takes how many data in 1 page
        /// </summary>
        public int PageSize { get; set; }
    }
}
