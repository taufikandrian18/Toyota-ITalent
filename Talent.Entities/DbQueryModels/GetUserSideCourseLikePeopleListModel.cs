using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class GetUserSideCourseLikePeopleListModel
    {
        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string PositionName { get; set; }
        public Guid? BlobId { get; set; }
        public string Mime { get; set; }

    }
}
