using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
    public class SurveysModel
    {
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public string Position { get; set; }
        public string Outlet { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class SurveysPaginate
    {
        public List<SurveysModel> Data { get; set; }
        public int TotalData { get; set; }
    }
    public class SurveysGridFilter : GridFilter
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string Title { get; set; }
        public string Position { get; set; }
        public string Outlet { get; set; }
        public int? StatusId { get; set; }
    }

    //Choice
    public class SurveyChoice
    {
        public string Choice { get; set; }
        public Guid? BlobId { get; set; }
        public FileContent FileContent { get; set; }
    }
    public class SurveyMatchingChoiceFormModel
    {
        public string Type { get; set; }
        public int? SurveyMatchingChoiceId { get; set; }
        [Required]
        [StringLength(3)]
        public string SurveyMatchingChoiceCode { get; set; }
        [StringLength(64)]
        public string Text { get; set; }
        public Guid? BlobId { get; set; }
        public FileContent ImageUpload { get; set; }
    }
    public class SurveyMatchingChoice
    {
        public List<SurveyMatchingChoiceFormModel> LeftChoices { get; set; }
        public List<SurveyMatchingChoiceFormModel> RightChoices { get; set; }
    }
    //Question
    public class SurveyQuestion
    {
        public int SurveyQuestionTypeId { get; set; }
        public int QuestionNumber { get; set; }
        public string Question { get; set; }
        public Guid? BlobId { get; set; }
        public FileContent FileContent { get; set; }
        public List<SurveyChoice> Choice { get; set; }
        public SurveyMatrixModel MatrixChoice { get; set; }
        public SurveyMatchingChoice MatchingChoices { get; set; }
    }

    public class SurveyMatrixModel
    {
        public List<SurveyMatrixChoicesModel> MatrixChoice { get; set; }
        public List<SurveyMatrixQuestionsModel> MatrixQuestion { get; set; }
    }

    public class SurveyMatrixChoicesModel
    {
        public int SurveyMatrixChoiceId { get; set; }
        public int? SurveyQuestionId { get; set; }
        public string Text { get; set; }
        public int Number { get; set; }
    }

    public class SurveyMatrixQuestionsModel
    {
        public int SurveyMatrixQuestionId { get; set; }
        public int? SurveyQuestionId { get; set; }
        public int? Number { get; set; }
        public string Question { get; set; }
        // model khusus matching survey.
    }


    //Model Insert
    public class SurveyInsert
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<PositionViewModel> Position { get; set; }
        public List<OutletViewModel> Outlet { get; set; }
        public List<SurveyQuestion> Question { get; set; }
    }

    public class SurveyQuestionType
    {
        public int SurveyQuestionTypeId { get; set; }
        public string Name { get; set; }
    }

    public class SurveyGet
    {
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SurveyQuestionId { get; set; }
        public int SurveyQuestionTypeId { get; set; }
        public int QuestionNumber { get; set; }
        public string Question { get; set; }
        public Guid? QuestionBlobId { get; set; }
        public int? SurveyChoiceId { get; set; }
        public string Choice { get; set; }
        public Guid? ChoiceBlobId { get; set; }
        public string QuestionFileName { get; set; }
        public string FileName { get; set; }
        public string Mime { get; set; }
        //Matrix Purpose
        public int? SurveyMatrixChoiceId { get; set; }
        public string TextMatrix { get; set; }
        public int? SurveyMatrixQuestionId { get; set; }
        public int? NumberMatrix { get; set; }
        public int? NumberMatrixChoice { get; set; }
        public string QuestionMatrix { get; set; }
    }

    //Model Get
    public class SurveyInitialize
    {
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<PositionViewModel> Position { get; set; }
        public List<OutletViewModel> Outlet { get; set; }
        public List<SurveyQuestionGet> Question { get; set; }
        public List<AreaViewModel> Area { get; set; }
        public List<ProvinceViewModel> Province { get; set; }
        public List<CityViewModel> City { get; set; }
        public List<DealerViewModel> Dealer { get; set; }
    }

    //QuestionGet
    public class SurveyQuestionGet
    {
        public int SurveyQuestionId { get; set; }
        public int SurveyQuestionTypeId { get; set; }
        public int QuestionNumber { get; set; }
        public string Question { get; set; }
        public Guid? BlobId { get; set; }
        public FileContent FileContent { get; set; }
        public string FileName { get; set; }
        public List<SurveyChoiceGet> Choice { get; set; }
        //Matrix Purpose
        public SurveyMatrixModel MatrixChoice { get; set; }
        public SurveyMatchingChoice MatchingChoices { get; set; }
    }

    public class SurveyChoiceGet
    {
        public int SurveyChoiceId { get; set; }
        public string Choice { get; set; }
        public Guid? BlobId { get; set; }
        public string FileName { get; set; }
        public FileContent FileContent { get; set; }

    }
}
