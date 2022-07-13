using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talent.Entities.Entities
{
    public class AssesmentQuestions
    {

        public AssesmentQuestions()
        {
            AssesmentAnswerImagesNavigation = new HashSet<AssesmentQuestionAnswerAnswerImages>();
            AssesmentAnswerTrueFalsesNavigation = new HashSet<AssesmentQuestionAnswerTrueFalses>();
            AssesmentQuestionAnswerShortAnswersNavigation = new HashSet<AssesmentQuestionAnswerShortAnswers>();
            AssesmentAnswerRatingsNavigation = new HashSet<AssesmentQuestionAnswerAnswerRatings>();
            AssesmentAnswerMatrixesNavigation = new HashSet<AssesmentQuestionAnswerMatrixes>();
            AssesmentAnswerHotspotsNavigation = new HashSet<AssesmentQuestionAnswerHotspots>();
            AssesmentAnswersFileUploadsNavigation = new HashSet<AssesmentQuestionAnswerFileUploads>();
            AssesmentEssaysNavigation = new HashSet<AssesmentQuestionAnswerEssays>();
            AssesmentAnswerChecklistsNavigation = new HashSet<AssesmentQuestionAnswerChecklists>();
            AssesmentAnswersDropdownsNavigation = new HashSet<AssesmentQuestionAnswerDropdowns>();
            AssesmentAnswerMultipleChoicesNavigation = new HashSet<AssesmentQuestionAnswerAnswerMultipleChoices>();
            AssesmentAnswerAnswerMatchingsNavigation = new HashSet<AssesmentQuestionAnswerAnswerMathcings>();
            LiveAssesmentSkillCheckQuestionNavigation = new HashSet<LiveAssesmentSkillCheckQuestions>();
        }

        [Key]
        public String GUID {get;set;}
        [StringLength(128)]
        public String GUIDQuestionType {get; set;}
        public String QuestionCode {get;set;}
        public String Question {get;set;}
        public String BlobId {get;set;}
        public String EnumStoryLine {get;set;}
        public float TotalScore {get;set;}
        public float TotalPoint {get;set;}
        public DateTime? CreatedAt {get;set;}
        public DateTime? UpdatedAt {get;set;}
        public DateTime? DeletedAt {get;set;}

        
        [ForeignKey("GUIDQuestionType")]
        [InverseProperty("QuestionsNavigation")]
        public virtual AssesmentQuestionTypes QuestionsTypeNavigator { get; set; }

        [InverseProperty("QuestionsNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerTrueFalses> AssesmentAnswerTrueFalsesNavigation { get; set; }
        [InverseProperty("QuestionsNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerShortAnswers> AssesmentQuestionAnswerShortAnswersNavigation { get; set; }
        [InverseProperty("QuestionsNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerAnswerRatings> AssesmentAnswerRatingsNavigation { get; set; }
        [InverseProperty("QuestionsNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerMatrixes> AssesmentAnswerMatrixesNavigation { get; set; }
        [InverseProperty("QuestionsNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerHotspots> AssesmentAnswerHotspotsNavigation { get; set; }
        [InverseProperty("QuestionsNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerFileUploads> AssesmentAnswersFileUploadsNavigation { get; set; }
        [InverseProperty("QuestionsNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerEssays> AssesmentEssaysNavigation { get; set; }
        [InverseProperty("QuestionsNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerDropdowns> AssesmentAnswersDropdownsNavigation { get; set; }
        [InverseProperty("QuestionsNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerChecklists> AssesmentAnswerChecklistsNavigation { get; set; }
        [InverseProperty("QuestionsNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerAnswerMultipleChoices> AssesmentAnswerMultipleChoicesNavigation { get; set; }
        [InverseProperty("QuestionsNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerAnswerMathcings> AssesmentAnswerAnswerMatchingsNavigation { get; set; }
        [InverseProperty("QuestionsNavigator")]
        public virtual ICollection<AssesmentQuestionAnswerAnswerImages> AssesmentAnswerImagesNavigation { get; set; }

        [InverseProperty("AssesmentQuestionNavigator")]
        public virtual ICollection<LiveAssesmentSkillCheckQuestions> LiveAssesmentSkillCheckQuestionNavigation { get; set; }
        [InverseProperty("AssesmentsQuestionsNavigator")]
        public virtual ICollection<SkillChecksQuestions> AssesmentsQuestionsNavigation { get; set; }

        


    }
}