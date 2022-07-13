using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talent.WebAdmin.Models
{
    public class KeyPieceOfMindMenuModel
    {
        public Guid KeyPieceOfMindMenuId { get; set; }
        public string KeyPieceOfMindMenuName { get; set; }
        public int KeyPieceOfMindMenuSequence { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class KeyPieceOfMindMenuPaginate
    {
        public List<KeyPieceOfMindMenuModel> KeyPieceOfMindMenus { get; set; }
        public int TotalKeyPieceOfMindMenus { get; set; }
    }

    public class KeyPieceOfMindMenuGridFilter : GridFilter
    {
        public string KeyPieceOfMindMenuName { get; set; }
    }

    public class KeyPieceOfMindMenuCreateModel
    {
        /// <summary>
        /// store product name to be created
        /// </summary>
        [Required]
        public string KeyPieceOfMindMenuName { get; set; }

        [Required]
        public int KeyPieceOfMindMenuSequence { get; set; }

    }
    public class KeyPieceOfMindMenuUpdateModel
    {
        [Required]
        public Guid KeyPieceOfMindMenuId { get; set; }
        /// <summary>
        /// store product name to be created
        /// </summary>
        [Required]
        public string KeyPieceOfMindMenuName { get; set; }

        [Required]
        public int KeyPieceOfMindMenuSequence { get; set; }

    }
    public class DeleteKeyPieceOfMindMenuModel
    {
        public Guid KeyPieceOfMindMenuId { get; set; }
        public bool isDeleteKeyPieceOfMindMenu { get; set; }
    }
}
