using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing news detail data.
    /// </summary>
    public class UserSideNewsDetailModel
    {
        public UserSideNewsModel News { set; get; }
        public List<UserSideNewsModel> RelatedNews { set; get; }
    }
}
