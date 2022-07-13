using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Certificate.Lib;
using TAM.Talent.Commons.Core.Interfaces;
using TAM.Talent.Commons.Core.Models;
using TAM.Talent.Certificate.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Talent.WebAdmin.UserSide.Enums;
using System.Text.RegularExpressions;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideGenerateCertificatePDFService
    {
        private readonly TalentContext DB;
        private readonly PDFCertificateGeneratorService PdfGenerator;
        private readonly IFileStorageService FileMan;

        /// <summary>
        /// buat default number certificate, jadi gak perlu query berkali-kali ._. .
        /// </summary>
        private int certifNumberCount = 1;

        public UserSideGenerateCertificatePDFService(TalentContext talentContext, PDFCertificateGeneratorService pdfGenerator, IFileStorageService fileStorageService)
        {
            this.DB = talentContext;
            this.PdfGenerator = pdfGenerator;
            this.FileMan = fileStorageService;
        }

        //public async Task<bool> GeneratePDF()
        //{
        //    var data = new PdfDataModel
        //    {
        //        CertificateNumber = "number number",
        //        EmployeeName = "EmployeeName",
        //        TrainingName = "trainingName",
        //        Host = "._.",
        //        Location = "Location",
        //        EventDate = DateTime.Now
        //    };

        //    var dataStream = PdfGenerator.ExportPdfStreamData(data);

        //    var sampleContent = new FileContent()
        //    {
        //        Base64 = "",
        //        ContentType = "pdf",
        //        FileName = "asd"
        //    };

        //    var a = await FileMan.UploadCertificateFile(dataStream, sampleContent);

        //    return true;
        //}

        public async Task<bool> GenerateCertificateAfterTrainingAsync(int trainingId, string employeeId)
        {
            var getAllCurrentTrainingCertifsList = await (from t in DB.Trainings
                                                          join ec in DB.EmployeeCertificates
                                                          on t.CourseId equals ec.CourseId
                                                          where t.TrainingId == trainingId
                                                          select new
                                                          {
                                                              ec.EmployeeId,
                                                              ec.CertificateNumber
                                                          }).ToListAsync();

            if (getAllCurrentTrainingCertifsList.Any(Q => Q.EmployeeId == employeeId) == true)
            {
                return false;
            }

            var getLastNumber = getAllCurrentTrainingCertifsList.Max(Q => Q.CertificateNumber);

            if (string.IsNullOrEmpty(getLastNumber) == false)
            {
                //nge split certif format untuk nge-get number
                var stringSplit = getLastNumber.Split("/");

                //split lagi untuk get number depannya aja
                var getNumber = stringSplit[2].Split("-");

                //convert string into int
                try
                {
                    int lastNumber = (int.Parse(getNumber[0]) + 1);
                    certifNumberCount = lastNumber;
                }
                catch
                {
                    certifNumberCount = 1;
                }

            }
            
            var getDigitalSignatureData = await DB.DigitalSignatures.Where(Q => Q.IsDeleted == false).OrderByDescending(Q => Q.UpdatedAt).FirstOrDefaultAsync();

            var getEmployeeSignatureData = getDigitalSignatureData == null
                ? null
                : await (from e in DB.Employees
                         join epm in DB.EmployeePositionMappings
                         on e.EmployeeId equals epm.EmployeeId

                         join p in DB.Positions
                         on epm.PositionId equals p.PositionId

                         where e.EmployeeId == getDigitalSignatureData.EmployeeId

                         select new SignatureEmployeeModel
                         {
                             EmployeeName = e.EmployeeName,
                             PositionName = p.PositionName
                         }).FirstOrDefaultAsync();

            var getDigitalSignatureBytes = getDigitalSignatureData == null
                ? (null, "", "")
                : await FileMan.DownloadFileAsync(getDigitalSignatureData.BlobId.ToString());

            var getDataCertificate = await (from t in DB.Trainings
                                            join el in DB.EnrollLearnings
                                            on t.TrainingId equals el.TrainingId into elJoin

                                            from el in elJoin
                                            join ec in DB.EmployeeCertificates
                                            on t.CourseId equals ec.CourseId into ecJoin
                                            from ec in ecJoin.DefaultIfEmpty()

                                            join e in DB.Employees
                                            on el.EmployeeId equals e.EmployeeId

                                            join c in DB.Courses
                                            on t.CourseId equals c.CourseId

                                            where
                                            el.TrainingId == trainingId
                                            && el.EmployeeId == employeeId

                                            select new PdfDataModel
                                            {
                                                CertificateNumber = ec == null ? GenerateCertifNumber(c.CourseName, t.StartDate != null ? (DateTime?)t.StartDate.Value : null) : ec.CertificateNumber,
                                                EmployeeName = el.Employee.EmployeeName,
                                                TrainingName = t.Course.CourseName,
                                                Host = "Toyota Training Center",
                                                Location = t.Location,
                                                EventDate = t.CreatedAt,
                                                CourseName = c.CourseName,
                                                CourseId = t.CourseId,
                                                EmployeeId = el.EmployeeId,
                                                //Position = p.PositionName,
                                                Position = getEmployeeSignatureData == null ? "" : getEmployeeSignatureData.PositionName,
                                                SignatureEmployeeName = getEmployeeSignatureData == null ? "" : getEmployeeSignatureData.EmployeeName,
                                                ProgramTypeId = c.ProgramTypeId,
                                                SignatureBytes = getDigitalSignatureBytes.Item1 ?? null,
                                                EnumCertificate = t.EnumCertificate
                                            }).AsNoTracking().FirstOrDefaultAsync();
            
            if(getDataCertificate == null){
                return false;
            }

            var dataStream = PdfGenerator.ExportPdfStreamData(getDataCertificate);
            var sampleContent = new FileContent()
            {
                Base64 = "",
                ContentType = "pdf",
                FileName = "Certificate - " + getDataCertificate.CertificateNumber
            };

            var fileGuid = await FileMan.UploadCertificateFile(dataStream, sampleContent);
            
            var employeeCertificate = new EmployeeCertificates
            {
                EmployeeId = getDataCertificate.EmployeeId,
                //BlobId = fileGuid,
                CourseId = getDataCertificate.CourseId,
                Type = CertficateTypeEnum.Internal,
                CertificateNumber = getDataCertificate.CertificateNumber,
                Title = getDataCertificate.TrainingName,
                TrainingName = getDataCertificate.TrainingName,
                Host = getDataCertificate.Host,
                Venue = getDataCertificate.Location ?? "-",
                EventDate = getDataCertificate.EventDate.GetValueOrDefault(),
                CertificationHeader = "Certification Of Completion",
            };

            if(getDataCertificate.EnumCertificate !="Default"){
                employeeCertificate.CertificationHeader = getDataCertificate.EnumCertificate;
            }

            if (getDataCertificate.EventDate != null)
            {
                employeeCertificate.EventDate = getDataCertificate.EventDate.Value;
            }

            DB.EmployeeCertificates.Add(employeeCertificate);

            await DB.SaveChangesAsync();

            return true;
        }

         public async Task<bool> GenerateCertificateAfterTrainingFromFinalScoresAsync(int trainingId, string employeeId)
        {
            var getAllCurrentTrainingCertifsList = await (from t in DB.Trainings
                                                          join ec in DB.EmployeeCertificates
                                                          on t.CourseId equals ec.CourseId
                                                          where ec.TrainingId == trainingId && t.IsDeleted == false
                                                          select new
                                                          {
                                                              ec.EmployeeId,
                                                              ec.CertificateNumber
                                                          }).ToListAsync();

            if (getAllCurrentTrainingCertifsList.Any(Q => Q.EmployeeId == employeeId) == true)
            {
                return false;
            }

            var getLastNumber = getAllCurrentTrainingCertifsList.Max(Q => Q.CertificateNumber);

            if (string.IsNullOrEmpty(getLastNumber) == false)
            {
                //nge split certif format untuk nge-get number
                var stringSplit = getLastNumber.Split("/");

                //split lagi untuk get number depannya aja
                var getNumber = stringSplit[2].Split("-");

                //convert string into int
                try
                {
                    int lastNumber = (int.Parse(getNumber[0]) + 1);
                    certifNumberCount = lastNumber;
                }
                catch
                {
                    certifNumberCount = 1;
                }

            }
            
            var getDigitalSignatureData = await DB.DigitalSignatures.Where(Q => Q.IsDeleted == false).OrderByDescending(Q => Q.UpdatedAt).FirstOrDefaultAsync();

            var getEmployeeSignatureData = getDigitalSignatureData == null
                ? null
                : await (from e in DB.Employees
                         join epm in DB.EmployeePositionMappings
                         on e.EmployeeId equals epm.EmployeeId

                         join p in DB.Positions
                         on epm.PositionId equals p.PositionId

                         where e.EmployeeId == getDigitalSignatureData.EmployeeId

                         select new SignatureEmployeeModel
                         {
                             EmployeeName = e.EmployeeName,
                             PositionName = p.PositionName
                         }).FirstOrDefaultAsync();

            var getDigitalSignatureBytes = getDigitalSignatureData == null
                ? (null, "", "")
                : await FileMan.DownloadFileAsync(getDigitalSignatureData.BlobId.ToString());
            
            var x = await DB.Trainings.Where(z=> z.TrainingId==trainingId).Include(z=> z.Course).FirstOrDefaultAsync();
            var emp = await DB.Employees.Where(z=> z.EmployeeId == employeeId).FirstOrDefaultAsync();
            var getDataCertificate = 
                                            new PdfDataModel
                                            {
                                                CertificateNumber = GenerateCertifNumber(x.Course.CourseName, x.StartDate != null ? (DateTime?) x.StartDate.Value : null),
                                                EmployeeName = emp.EmployeeName,
                                                TrainingName = x.Course.CourseName,
                                                Host = "Toyota Training Center",
                                                Location =  x.Location,
                                                EventDate =  x.EndDate.Value.AddDays(1),
                                                CourseName =  x.Course.CourseName,
                                                CourseId =  x.CourseId,
                                                EmployeeId = emp.EmployeeId,
                                                //Position = p.PositionName,
                                                Position = getEmployeeSignatureData == null ? "" : getEmployeeSignatureData.PositionName,
                                                SignatureEmployeeName = getEmployeeSignatureData == null ? "" : getEmployeeSignatureData.EmployeeName,
                                                ProgramTypeId =  x.Course.ProgramTypeId,
                                                SignatureBytes = getDigitalSignatureBytes.Item1 ?? null,
                                                EnumCertificate =  x.EnumCertificate
                                            };
            
            if(getDataCertificate == null){
                return false;
            }

            var dataStream = PdfGenerator.ExportPdfStreamData(getDataCertificate);
            var sampleContent = new FileContent()
            {
                Base64 = "",
                ContentType = "pdf",
                FileName = "Certificate - " + getDataCertificate.CertificateNumber
            };

            var fileGuid = await FileMan.UploadCertificateFile(dataStream, sampleContent);
            
            var employeeCertificate = new EmployeeCertificates
            {
                EmployeeId = getDataCertificate.EmployeeId,
                BlobId = fileGuid,
                CourseId = getDataCertificate.CourseId,
                Type = CertficateTypeEnum.Internal,
                CertificateNumber = getDataCertificate.CertificateNumber,
                Title = getDataCertificate.TrainingName,
                TrainingName = getDataCertificate.TrainingName,
                Host = getDataCertificate.Host,
                Venue = getDataCertificate.Location ?? "-",
                EventDate = x.EndDate.Value.AddDays(1),
                CertificationHeader = getDataCertificate.TrainingName,
                TrainingId = trainingId
            };

            if(getDataCertificate.EnumCertificate !="Default"){
                employeeCertificate.CertificationHeader = getDataCertificate.EnumCertificate;
            }

            if (getDataCertificate.EventDate != null)
            {
                employeeCertificate.EventDate = getDataCertificate.EventDate.Value;
            }

            DB.EmployeeCertificates.Add(employeeCertificate);

            await DB.SaveChangesAsync();

            return true;
        }

        //public async Task<bool> GenerateCertficateFromTrainingAsync(int trainingId)
        //{
        //    var getListPassedEmployee = await DB.EnrollLearnings
        //        .Where(Q => Q.TrainingId == trainingId && Q.IsPassed == true)
        //        .Select(Q => Q.EmployeeId)
        //        .AsNoTracking()
        //        .ToListAsync();

        //    // v.1.0
        //    // get last certif number
        //    var getExistingCertificate = await (from t in DB.Trainings
        //                                        join ec in DB.EmployeeCertificates
        //                                        on t.CourseId equals ec.CourseId
        //                                        where t.TrainingId == trainingId
        //                                        select new
        //                                        {
        //                                            ec.EmployeeId,
        //                                            ec.CertificateNumber
        //                                        })
        //                         .ToListAsync();
        //    var getLastNumber = getExistingCertificate.Max(Q => Q.CertificateNumber);

        //    if (string.IsNullOrEmpty(getLastNumber) == false)
        //    {
        //        //nge split certif format untuk nge-get number
        //        var stringSplit = getLastNumber.Split("/");
        //        //convert string into int
        //        try
        //        {
        //            int lastNumber = (int.Parse(stringSplit[2]) + 1);
        //            certifNumberCount = lastNumber;
        //        }
        //        catch
        //        {
        //            certifNumberCount = 1;
        //        }

        //    }

        //    var getDigitalSignatureData = await DB.DigitalSignatures.OrderByDescending(Q => Q.UpdatedAt).FirstOrDefaultAsync();

        //    var getEmployeeSignatureData = await (from e in DB.Employees
        //                                          join epm in DB.EmployeePositionMappings
        //                                          on e.EmployeeId equals epm.EmployeeId

        //                                          join p in DB.Positions
        //                                          on epm.PositionId equals p.PositionId

        //                                          where e.EmployeeId == getDigitalSignatureData.EmployeeId

        //                                          select new SignatureEmployeeModel
        //                                          {
        //                                              EmployeeName = e.EmployeeName,
        //                                              PositionName = p.PositionName
        //                                          }).FirstOrDefaultAsync();

        //    var getDigitalSignatureBytes = await FileMan.DownloadFileAsync(getDigitalSignatureData.BlobId.ToString());

        //    var getListDataCertificate = await (from t in DB.Trainings
        //                                        join el in DB.EnrollLearnings
        //                                        on t.TrainingId equals el.TrainingId into elJoin

        //                                        from el in elJoin
        //                                        join ec in DB.EmployeeCertificates
        //                                        on t.CourseId equals ec.CourseId into ecJoin
        //                                        from ec in ecJoin.DefaultIfEmpty()

        //                                        join e in DB.Employees
        //                                        on el.EmployeeId equals e.EmployeeId

        //                                        //join epm in DB.EmployeePositionMappings
        //                                        //on e.EmployeeId equals epm.EmployeeId

        //                                        //join p in DB.Positions
        //                                        //on epm.PositionId equals p.PositionId

        //                                        join c in DB.Courses
        //                                        on t.CourseId equals c.CourseId

        //                                        where
        //                                        el.TrainingId == trainingId &&
        //                                        getListPassedEmployee.Contains(el.EmployeeId)
        //                                        //&& ec != null ? getExistingCertificate.FindIndex(Q => Q.EmployeeId == ec.EmployeeId) == -1 : true

        //                                        select new PdfDataModel
        //                                        {
        //                                            CertificateNumber = ec == null ? GenerateCertifNumber(c.CourseName, t.StartDate != null ? (DateTime?)t.StartDate.Value : null) : ec.CertificateNumber,
        //                                            EmployeeName = el.Employee.EmployeeName,
        //                                            TrainingName = t.Course.CourseName,
        //                                            Host = "Toyota Training Center",
        //                                            Location = t.Location,
        //                                            EventDate = t.StartDate != null ? (DateTime?)t.StartDate.Value : null,
        //                                            CourseName = c.CourseName,
        //                                            CourseId = t.CourseId,
        //                                            EmployeeId = el.EmployeeId,
        //                                            //Position = p.PositionName,
        //                                            Position = getEmployeeSignatureData.PositionName,
        //                                            SignatureEmployeeName = getEmployeeSignatureData.EmployeeName,
        //                                            ProgramTypeId = c.ProgramTypeId,
        //                                            SignatureBytes = getDigitalSignatureBytes.Item1
        //                                        })
        //                                 .AsNoTracking()
        //                                 .ToListAsync()
        //                                 ;

        //    List<EmployeeCertificates> newEmployeeCertificateList = new List<EmployeeCertificates>();

        //    foreach (var data in getListDataCertificate)
        //    {
        //        if (getExistingCertificate.Any(Q => Q.EmployeeId == data.EmployeeId) == true)
        //        {
        //            continue;
        //        }
        //        else
        //        {
        //            var dataStream = PdfGenerator.ExportPdfStreamData(data);
        //            var sampleContent = new FileContent()
        //            {
        //                Base64 = "",
        //                ContentType = "pdf",
        //                FileName = "asd - " + data.CertificateNumber
        //            };
        //            var fileGuid = await FileMan.UploadCertificateFile(dataStream, sampleContent);
        //            var employeeCertificate = new EmployeeCertificates
        //            {
        //                EmployeeId = data.EmployeeId,
        //                BlobId = fileGuid,
        //                CourseId = data.CourseId,
        //                Type = CertficateTypeEnum.Internal,
        //                CertificateNumber = data.CertificateNumber,
        //                Title = data.TrainingName,
        //                TrainingName = data.TrainingName,
        //                Host = data.Host,
        //                Venue = data.Location,
        //            };

        //            if (data.EventDate != null)
        //            {
        //                employeeCertificate.EventDate = data.EventDate.Value;
        //            }

        //            newEmployeeCertificateList.Add(employeeCertificate);
        //        }
        //    }

        //    DB.EmployeeCertificates.AddRange(newEmployeeCertificateList);

        //    await DB.SaveChangesAsync();
        //    //try
        //    //{
        //    //}
        //    //catch
        //    //{
        //    //    return false;
        //    //}

        //    return true;
        //}

        private string GenerateCertifNumber(string courseName, DateTime? eventDate)
        {
            var getYear = "";
            if (eventDate != null)
            {
                getYear = eventDate.Value.ToString("yy");
            }
            else
            {
                getYear = DateTime.Now.ToString("yy");
            }

            string[] words = courseName.Trim().Split(' ');
            string courseNameCode = "";
            var regexItem = new Regex("^[a-zA-Z]*$");

            foreach (string item in words)
            {
                string getLetter = item.Substring(0, 1).ToUpper();

                if (regexItem.IsMatch(getLetter))
                {
                    courseNameCode += getLetter;
                }
            }

            var newGuid = Guid.NewGuid().ToString().Substring(0, 8);

            var certifNumber = "";
            if (certifNumberCount < 10)
            {
                certifNumber = courseNameCode + "/" + getYear + "/000" + certifNumberCount + "-" + newGuid;
            }
            else if (certifNumberCount < 100)
            {
                certifNumber = courseNameCode + "/" + getYear + "/00" + certifNumberCount + "-" + newGuid;
            }
            else if (certifNumberCount < 100)
            {
                certifNumber = courseNameCode + "/" + getYear + "/0" + certifNumberCount + "-" + newGuid;
            }
            else
            {
                certifNumber = courseNameCode + "/" + getYear + "/" + certifNumberCount + "-" + newGuid;
            }
            certifNumberCount++;

            return certifNumber;
        }


        //public async Task<UserSideGenerateCertificatePDFModel> GetCertificateData()
        //{
        //    var query = await (from 
        //                       select
        //                        ).ToListAsync();
        //}
    }
}
