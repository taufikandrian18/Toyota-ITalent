using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class ModuleCreateModel
    {
        /// <summary>
        /// store module name to be created
        /// </summary>
        [Required]
        public string ModuleName { get; set; }

        public FileContent ModuleFileContent { get; set; }

        public string ModuleDesc { get; set; }

        [Required]
        public int MaterialTypeId { get; set; }

        public FileContent MaterialFileContent { get; set; }

        public string Link { get; set; }

        [Required]
        public List<int> TopicId { get; set; }

        [Required]
        public bool Downloadable { get; set; }

        [Required]
        public int ApprovalStatusId { get; set; }
    }

    public class ModuleGridModel
    {
        public List<ModuleGridViewModel> Modules { get; set; }
        public int TotalFilterData { get; set; }
    }

    public class ModuleGridViewModel
    {
        /// <summary>
        /// store module name for view
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// store module id for view
        /// used for edit, view, and delete
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// store module material type for view
        /// </summary>
        public string TypeMaterialName { get; set; }

        /// <summary>
        /// store topic name for view
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// store approval status for view
        /// </summary>
        public string ApprovalStatus { get; set; }

        /// <summary>
        /// store date when module is created for view
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// store date when module is updated for view
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    public class ModuleViewDetailModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public Guid ModuleBlobId { get; set; }
        public string ModuleBlobName { get; set; }
        public string ModuleBlobMIME { get; set; }
        public MaterialTypeDropdownModel MaterialType { get; set; }
        public Guid? MaterialBlobId { get; set; }
        public string MaterialBlobName { get; set; }
        public string MaterialBlobMIME { get; set; }
        public string MaterialLink { get; set; }
        public bool isDownloadable { get; set; }
        public List<TopicDropdownModel> Topics { get; set; }
        public int ApprovalStatusId { get; set; }
        public string RelatedTrainingName {get;set;}
        public string RelatedTrainingId {get;set;}
    }

    public class ModuleUpdateModel
    {
        public int ModuleId { get; set; }
        /// <summary>
        /// store module name to be created
        /// </summary>
        [Required]
        public string ModuleName { get; set; }

        public FileContent ModuleFileContent { get; set; }

        public string ModuleDesc { get; set; }

        [Required]
        public int MaterialTypeId { get; set; }

        public FileContent MaterialFileContent { get; set; }

        public string Link { get; set; }

        [Required]
        public List<int> TopicId { get; set; }

        [Required]
        public bool Downloadable { get; set; }

        [Required]
        public int ApprovalStatusId { get; set; }
    }

    public class DeleteModuleModel
    {
        public int ModuleId { get; set; }
        public bool isDeleteModule { get; set; }
        public List<int> TopicIds { get; set; }
    }
}
