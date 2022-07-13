using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using TAM.Talent.Commons.Core.Models;

namespace Talent.WebAdmin.Models
{
  public class RequestSkillCheckModel
  {
    public string GUID { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public bool IsQuestion { get; set; }
    public float MinimumScore { get; set; }
    public string EnumFeedbackScore { get; set; }
    public string Description { get; set; }
    public string MediaBlobId { get; set; }
    public string CreatedBy { get; set; }

    public string UpdatedBy { get; set; }

    public FileContent MediaContent { get; set; }

    public List<RequestScoreConfigModel> ScoringConfig { get; set; }
  }

  public class ResponseSkillCheckModel
  {
    public string GUID { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public bool IsQuestion { get; set; }
    public float MinimumScore { get; set; }
    public string EnumFeedbackScore { get; set; }
    public string Description { get; set; }
    public string MediaBlobId { get; set; }
    public BlobModel MediaBlobData { get; set; }
    public string MediaUrl { get; set; }
    public string CreatedBy { get; set; }

    public string UpdatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<SkillChecksScoreConfigs> ScoringConfig { get; set; }
  }

  public class ParamSkillCheckModel
  {
    public string GUID { get; set; }
    public string Query { get; set; }
    public string CreatedBy { get; set; }
    public string ScoringMethod { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string SortKey { get; set; }
    public string SortType { get; set; }

    public int Limit { get; set; }
    public int Page { get; set; }
  }
}