using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class PrivilegePageMappingData
    {
        public string PageId { get; set; }
        public bool IsCreate { set; get; }
        public bool IsRead { set; get; }
        public bool IsUpdate { set; get; }
        public bool IsDelete { set; get; }
    }
}
