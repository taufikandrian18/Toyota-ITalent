using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class KeyPieceOfMindTypeModel
    {
        public Guid KeyPieceOfMindTypeId { get; set; }
        public Guid? BlobId { get; set; }
        public string KeyPieceOfMindTypeName { get; set; }
        public string KeyPieceOfMindTypeDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class KeyPieceOfMindTypePaginate
    {
        public List<KeyPieceOfMindTypeModel> KeyPieceOfMindTypes { get; set; }
        public int TotalKeyPieceOfMindTypes { get; set; }
    }
    public class KeyPieceOfMindTypeGridFilter : GridFilter
    {
        public string KeyPieceOfMindTypeName { get; set; }
    }
    public class KeyPieceOfMindTypeCreateModel
    {
        /// <summary>
        /// store product name to be created
        /// </summary>
        [Required]
        public string KeyPieceOfMindTypeName { get; set; }

        public FileContent ProductFileContent { get; set; }

        public string KeyPieceOfMindTypeDescription { get; set; }

    }
    public class KeyPieceOfMindTypeUpdateModel
    {
        [Required]
        public Guid KeyPieceOfMindTypeId { get; set; }
        /// <summary>
        /// store product name to be created
        /// </summary>
        [Required]
        public string KeyPieceOfMindTypeName { get; set; }

        public FileContent ProductFileContent { get; set; }

        public string KeyPieceOfMindTypeDescription { get; set; }

    }
    public class DeleteKeyPieceOfMindTypeModel
    {
        public Guid KeyPieceOfMindTypeId { get; set; }
        public bool isDeleteKeyPieceOfMindType { get; set; }
    }
}
