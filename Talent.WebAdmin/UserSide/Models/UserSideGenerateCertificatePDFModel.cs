using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    public class UserSideGenerateCertificatePDFModel
    {
        public string CertificateNumber { get; set; }
        public string HostName { get; set; }
        public string Name { get; set; }
        public string TrainingName { get; set; }
        public string Place { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DigitalSignatureBlobId { get; set; }
        public string PositionName { get; set; }

    }
}
