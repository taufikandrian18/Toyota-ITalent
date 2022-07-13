using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public partial class Courses
    {
        public Courses()
        {
            ApprovalToCourses = new HashSet<ApprovalToCourses>();
            ApprovalToSetupCourses = new HashSet<ApprovalToSetupCourses>();
            CourseLearningObjectives = new HashSet<CourseLearningObjectives>();
            CoursePrerequisiteMappingsNextCourse = new HashSet<CoursePrerequisiteMappings>();
            CoursePrerequisiteMappingsPrevCourse = new HashSet<CoursePrerequisiteMappings>();
            CourseTrainingTypeMappings = new HashSet<CourseTrainingTypeMappings>();
            EmployeeCertificates = new HashSet<EmployeeCertificates>();
            SetupLearning = new HashSet<SetupLearning>();
            SetupModules = new HashSet<SetupModules>();
            TrainingRatings = new HashSet<TrainingRatings>();
            Trainings = new HashSet<Trainings>();
            FinalScores = new HashSet<FinalScores>();
        }

        [Key]
        public int CourseId { get; set; }
        public int ProgramTypeId { get; set; }
        public int LevelId { get; set; }
        public int CourseCategoryId { get; set; }
        public int LearningTypeId { get; set; }
        public Guid BlobId { get; set; }
        [Required]
        [StringLength(255)]
        public string CourseName { get; set; }
        public int? CoursePrice { get; set; }
        public string CourseDescription { get; set; }
        [StringLength(255)]
        public string Others { get; set; }
        public bool? IsRecommendedForYou { get; set; }
        public bool? IsPopular { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CourseApprovedAt { get; set; }
        public DateTime? SetupCourseApprovedAt { get; set; }
        public DateTime? SetupCourseCreatedAt { get; set; }
        [StringLength(64)]
        public string SetupCourseCreatedBy { get; set; }
        public DateTime? SetupCourseUpdatedAt { get; set; }
        [StringLength(64)]
        public string SetupCourseUpdatedBy { get; set; }

        [ForeignKey("BlobId")]
        [InverseProperty("Courses")]
        public virtual Blobs Blob { get; set; }
        [ForeignKey("CourseCategoryId")]
        [InverseProperty("Courses")]
        public virtual CourseCategories CourseCategory { get; set; }
        [ForeignKey("LearningTypeId")]
        [InverseProperty("Courses")]
        public virtual LearningTypes LearningType { get; set; }
        [ForeignKey("LevelId")]
        [InverseProperty("Courses")]
        public virtual Levels Level { get; set; }
        [ForeignKey("ProgramTypeId")]
        [InverseProperty("Courses")]
        public virtual ProgramTypes ProgramType { get; set; }
        [InverseProperty("Course")]
        public virtual ICollection<ApprovalToCourses> ApprovalToCourses { get; set; }
        [InverseProperty("Course")]
        public virtual ICollection<ApprovalToSetupCourses> ApprovalToSetupCourses { get; set; }
        [InverseProperty("Course")]
        public virtual ICollection<CourseLearningObjectives> CourseLearningObjectives { get; set; }
        [InverseProperty("NextCourse")]
        public virtual ICollection<CoursePrerequisiteMappings> CoursePrerequisiteMappingsNextCourse { get; set; }
        [InverseProperty("PrevCourse")]
        public virtual ICollection<CoursePrerequisiteMappings> CoursePrerequisiteMappingsPrevCourse { get; set; }
        [InverseProperty("Course")]
        public virtual ICollection<CourseTrainingTypeMappings> CourseTrainingTypeMappings { get; set; }
        [InverseProperty("Course")]
        public virtual ICollection<EmployeeCertificates> EmployeeCertificates { get; set; }
        [InverseProperty("Course")]
        public virtual ICollection<SetupLearning> SetupLearning { get; set; }
        [InverseProperty("Course")]
        public virtual ICollection<SetupModules> SetupModules { get; set; }
        [InverseProperty("Course")]
        public virtual ICollection<TrainingRatings> TrainingRatings { get; set; }
        [InverseProperty("Course")]
        public virtual ICollection<Trainings> Trainings { get; set; }
        [InverseProperty("Course")]
        public virtual ICollection<FinalScores> FinalScores { get; set; }
       
    }
}
