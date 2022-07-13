using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideCertificateService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;

        public UserSideCertificateService(TalentContext talentContext, IFileStorageService fileService)
        {
            this.DB = talentContext;
            this.FileService = fileService;
        }

        /// <summary>
        /// Get all certificate data by employee id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of certificate data in <seealso cref="List{UserSideCertificateViewModel}"/> format.</returns>
        public async Task<List<UserSideCertificateModel>> GetCertificateByEmployeeId(string id)
        {
            var queryCertificates = await (from e in DB.Employees
                                           join ec in DB.EmployeeCertificates
                                           on e.EmployeeId equals ec.EmployeeId
                                           join b in DB.Blobs
                                           on ec.BlobId equals b.BlobId into ab
                                           from b in ab.DefaultIfEmpty()
                                           where e.EmployeeId == id && ec.BlobId != null
                                           select new
                                           {
                                               Certificate = ec,
                                               Blob = b
                                           })
                                    .ToListAsync();

            var dataCertificates = queryCertificates.Select(async Q => new UserSideCertificateModel{
                EmployeeCertificateId = Q.Certificate.EmployeeCertificateId,
                ContentType = Q.Blob?.Mime,
                EventDate = Q.Certificate.EventDate,
                FileName = Q.Blob?.Name,
                Host = Q.Certificate.Host,
                ImageUrl = Q.Blob != null ? await this.FileService.GetFileAsync(Q.Blob.BlobId.ToString(), Q.Blob.Mime) : "",
                Title = Q.Certificate.Title,
                Header =Q.Certificate.CertificationHeader,
                TrainingName = Q.Certificate.TrainingName,
                Type = Q.Certificate.Type,
                Venue = Q.Certificate.Venue
            }).Select(Q => Q.Result).ToList();

            var queryAssesment = await (from a in this.DB.Assessments
            join b in this.DB.Blobs on a.BlobId equals b.BlobId into lb
            from b in lb.DefaultIfEmpty()
            where a.EmployeeId.ToLower() == id.ToLower() && a.IsDeleted == false
            select new {
                Assesment= a,
                Blob = b
            }).ToListAsync();

            var dataAssesment = queryAssesment.Select(async Q => new UserSideCertificateModel{
                AssesmentId = Q.Assesment.AssessmentId,
                ImageUrl = Q.Blob != null ? await this.FileService.GetFileAsync(Q.Blob.BlobId.ToString(), Q.Blob.Mime) : "",
                FileName = Q.Blob?.Name,
                ContentType = Q.Blob?.Mime,
                Title = Q.Assesment.Name,
                EventDate = Q.Assesment.UpdatedAt
            }).Select(Q => Q.Result).ToList();

            dataCertificates.AddRange(dataAssesment);
            dataCertificates.OrderBy(Q => Q.EventDate);

            return dataCertificates;
        }

        /// <summary>
        /// Create new upload certificate.
        /// </summary>
        /// <param name="newUploadCertificate"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task CreateUploadCertificate(UserSideCertificateFormModel newUploadCertificate, string employeeId)
        {
            //if (newUploadCertificate.UploadedFile != null)
            if (newUploadCertificate.FileUploaded != null)
            {
                //string fileName, fileExtension;
                //fileName = newUploadCertificate.UploadedFile.Files[0].FileName;
                //fileExtension = newUploadCertificate.UploadedFile.Files[0].ContentType.Substring(newUploadCertificate.UploadedFile.Files[0].ContentType.IndexOf('/') + 1, newUploadCertificate.UploadedFile.Files[0].ContentType.Length - newUploadCertificate.UploadedFile.Files[0].ContentType.IndexOf('/') - 1);
                //var newGuid = this.FileService.InsertBlob(fileName, fileExtension);
                //// Sorry God for this sin T.T supaya jalan aja dulu (albert harus bener ke yang baruu ya)
                //await DB.SaveChangesAsync();
                //fileName = newGuid.ToString();
                //using (var data = newUploadCertificate.UploadedFile.Files[0].OpenReadStream())
                //{
                //    await this.FileService.UploadFile(fileName, fileExtension, data);
                //}
                if (string.IsNullOrEmpty(newUploadCertificate.FileUploaded.Base64) == false)
                {
                    // Obtain file extension from file name due the misleading Upload File function.
                    var fileExtension = Path.GetExtension(newUploadCertificate.FileUploaded.FileName);
                    newUploadCertificate.FileUploaded.ContentType = fileExtension.Substring(1, fileExtension.Length - 1);
                    var newGuid = await FileService.UploadFileFromBase64(newUploadCertificate.FileUploaded);

                    var dataEmployeeCertificate = new EmployeeCertificates
                    {
                        EmployeeId = employeeId,
                        Type = newUploadCertificate.Type,
                        Title = newUploadCertificate.Title,
                        TrainingName = newUploadCertificate.TrainingName,
                        EventDate = newUploadCertificate.EventDate,
                        Host = newUploadCertificate.Host,
                        Venue = newUploadCertificate.Venue,
                        BlobId = newGuid
                    };

                    this.DB.Add(dataEmployeeCertificate);
                    await this.DB.SaveChangesAsync();
                }


            }
        }
        /// <summary>
        /// Get single certificate data by employee id and certificate id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="certificateId"></param>
        /// <returns>Single </returns>
        public async Task<String> GetSingleCertificate(string employeeId, int certificateId)
        {
            var blob = await (from e in DB.Employees
                              join ec in DB.EmployeeCertificates
                              on e.EmployeeId equals ec.EmployeeId
                              join b in DB.Blobs
                              on ec.BlobId equals b.BlobId into ab
                              from b in ab.DefaultIfEmpty()
                              where e.EmployeeId == employeeId && ec.EmployeeCertificateId == certificateId
                              select new BlobModel
                              {
                                  BlobId = b.BlobId,
                                  Mime = b.Mime,
                                  Name = b.Name,
                              }).FirstOrDefaultAsync();
            if (blob == null) return null;
            var ImageUrl = await this.FileService.GetFileAsync(blob.BlobId.ToString(), blob.Mime);
            return ImageUrl;
        }
    }
}
