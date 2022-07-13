using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Entities.DbQueryModels
{
    public class GetUserSidePeopleWhoTookTheCourseListModel
    {
        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string Position { get; set; }

        public Guid?  BlobId { get; set; }
        public string MIME { get; set; }
    }
}
