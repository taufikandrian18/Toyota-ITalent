using System;
using System.Collections.Generic;
using System.Text;

namespace TAM.Talent.Certificate.Lib.Models
{
    public class PdfDataModel
    {
        public string CertificateNumber { get; set; }
        public string Host { get; set; }
        public string EmployeeName { get; set; }
        public string TrainingName { get; set; }
        public string Location { get; set; }
        public DateTime? EventDate { get; set; }
        public string EmployeeId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Position { get; set; }
        public int ProgramTypeId { get; set; }
        public Byte[] SignatureBytes { get; set; }
        public string SignatureEmployeeName { get; set; }
        public string EnumCertificate {get;set;}

    }

    public class SignatureEmployeeModel
    {
        public string EmployeeName { get; set; }
        public string PositionName { get; set; }
    }
}
