using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing certificate data.
    /// </summary>
    public class UserSideCertificateModel
    {
        /// <summary>
        /// Certificate Title.
        /// </summary>
        public string Title { get; set; }

        public string Header {get;set;}

        /// <summary>
        /// Certificate EmployeeSertificateId.
        /// </summary>
        public int? EmployeeCertificateId { get; set; }

        /// <summary>
        /// Certificate Type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Certificate Date.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Certificate Tranning Name.
        /// </summary>
        public string TrainingName { get; set; }

        /// <summary>
        /// Certificate Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Certificate Venue
        /// </summary>
        public string Venue { get; set; }

        /// <summary>
        /// Certificate Blob Id (Link / Image Url)
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// certificate Blob Name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// certificate blob mime
        /// </summary>
        public string ContentType { get; set; }
        public int? AssesmentId { get; set; }
    }

    public class UserSideCertificateFormModel
    {
        /// <summary>
        /// Certificate Title.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        /// <summary>
        /// Certificate Type.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Type { get; set; }

        /// <summary>
        /// Certificate Date.
        /// </summary>
        [Required]
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Certificate Tranning Name.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string TrainingName { get; set; }

        /// <summary>
        /// Certificate Host
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Host { get; set; }

        /// <summary>
        /// Certificate Venue
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        [Required]
        public FileContent FileUploaded { get; set; }
    }
}
