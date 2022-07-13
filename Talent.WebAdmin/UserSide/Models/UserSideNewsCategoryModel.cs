using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing news category data.
    /// </summary>
    public class UserSideNewsCategoryModel
    {
        public int NewsCategoryId { set; get; }
        public string NewsCategoryName { set; get; }
    }
}
