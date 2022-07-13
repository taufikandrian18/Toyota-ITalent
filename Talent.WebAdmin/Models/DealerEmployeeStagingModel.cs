using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.Models
{
    public class DealerEmployeeStagingModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal OutletId { get; set; }
        public string ManpowerTypeId { get; set; }
        public string LastManpowerPositionTypeId { get; set; }
        public string ManpowerStatusTypeId { get; set; }
        public string EmployeeId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
    }
}
