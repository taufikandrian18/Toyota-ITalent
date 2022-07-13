using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.UserSide.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSidePersonalMappingService
    {
        private readonly TalentContext DB;

        public UserSidePersonalMappingService(TalentContext talentContext)
        {
            this.DB = talentContext;
        }

        public async Task<UserSidePersonalMappingModel> GetPersonalMappingByEmployeeId(string id)
        {
            //demanded
            var queryPerson = await this.DB.Employees.Where(Q => Q.EmployeeId == id).Select(Q => Q.EmployeeId).FirstOrDefaultAsync();

            var dataTeamDemanded = queryPerson
                .Select(async X => new UserSidePersonalMappingModel
                {
                    EmployeeId = id,
                    ListHardCompetencyModel =
                    await (from epm in this.DB.EmployeePositionMappings.AsNoTracking()
                           join p in this.DB.Positions.AsNoTracking()
                           on epm.PositionId equals p.PositionId
                           join pcm in this.DB.PositionCompentencyMappings.AsNoTracking()
                           on p.PositionId equals pcm.PositionId
                           join c in this.DB.Competencies.AsNoTracking()
                           on pcm.CompetencyId equals c.CompetencyId

                           join ct in this.DB.CompetencyTypes.AsNoTracking()
                           on c.CompetencyTypeId equals ct.CompetencyTypeId

                           where epm.EmployeeId == id && ct.CompetencyTypeId == (int)Enums.CompetencyType.Hard

                           select new UserSideCompetencyMappingViewModel
                           {
                               PrefixCode = c.PrefixCode,
                               CompetencyName = c.CompetencyName,
                               CompetencyDescription = c.CompetencyDescription,
                               CompetencyTypeName = ct.CompetencyTypeName,
                               ScoreTotal = 0,
                               MaxScoreTotal = 0,
                               ListKeyAction =

                               (from pcm1 in this.DB.PositionCompentencyMappings.AsNoTracking()
                                join ckam in this.DB.CompetencyKeyActionMappings.AsNoTracking()
                                on pcm1.CompetencyId equals ckam.CompetencyId

                                join ka in this.DB.KeyActions.AsNoTracking()
                                on ckam.KeyActionId equals ka.KeyActionId

                                where ckam.CompetencyId == c.CompetencyId && pcm1.PositionId == p.PositionId

                                select new UsersideKeyactionViewModel
                                {
                                    ProficiencyLevel = pcm.ProficiencyLevel,
                                    KeyActionCode = ka.KeyActionCode,
                                    KeyActionName = ka.KeyActionName,
                                    KeyActionDescription = ka.KeyActionDescription,
                                    Score = 0,
                                    MaxScore = 0
                                }).ToList()
                           }).OrderBy(Q => Q.PrefixCode).ToListAsync(),

                    ListSoftCompetencyModel =
                    await (from epm in this.DB.EmployeePositionMappings.AsNoTracking()
                           join p in this.DB.Positions.AsNoTracking()
                           on epm.PositionId equals p.PositionId
                           join pcm in this.DB.PositionCompentencyMappings.AsNoTracking()
                           on p.PositionId equals pcm.PositionId
                           join c in this.DB.Competencies.AsNoTracking()
                           on pcm.CompetencyId equals c.CompetencyId

                           join ct in this.DB.CompetencyTypes.AsNoTracking()
                           on c.CompetencyTypeId equals ct.CompetencyTypeId

                           where epm.EmployeeId == id && ct.CompetencyTypeId == (int)Enums.CompetencyType.Soft

                           select new UserSideCompetencyMappingViewModel
                           {
                               PrefixCode = c.PrefixCode,
                               CompetencyName = c.CompetencyName,
                               CompetencyDescription = c.CompetencyDescription,
                               CompetencyTypeName = ct.CompetencyTypeName,
                               ScoreTotal = 0,
                               MaxScoreTotal = 0,
                               ListKeyAction =

                               (from pcm2 in this.DB.PositionCompentencyMappings.AsNoTracking()
                                join ckam in this.DB.CompetencyKeyActionMappings.AsNoTracking()
                                on pcm2.CompetencyId equals ckam.CompetencyId

                                join ka in this.DB.KeyActions.AsNoTracking()
                                on ckam.KeyActionId equals ka.KeyActionId

                                where ckam.CompetencyId == c.CompetencyId && pcm2.PositionId == p.PositionId

                                select new UsersideKeyactionViewModel
                                {
                                    ProficiencyLevel = pcm2.ProficiencyLevel,
                                    KeyActionCode = ka.KeyActionCode,
                                    KeyActionName = ka.KeyActionName,
                                    KeyActionDescription = ka.KeyActionDescription,
                                    Score = 0,
                                    MaxScore = 0
                                }).ToList()
                           }).OrderBy(Q => Q.PrefixCode).ToListAsync()
                })
                .Select(Q => Q.Result)
                .FirstOrDefault();



            //Evaluated
            var getPersonalMappingList = await DB.GetPersonalMapping(id).ToListAsync();

            var getCompetencyList = getPersonalMappingList
                .GroupBy(Q => Q.PrefixCode)
                .Select(Q => new
                {
                    PrefixCode = Q.Key,
                    CompetencyName = Q.Select(X => X.CompetencyName).FirstOrDefault(),
                    CompetencyDescription = Q.Select(X => X.CompetencyDescription).FirstOrDefault(),
                    CompetencyTypeName = Q.Select(X => X.CompetencyTypeName).FirstOrDefault(),
                    CompetencyTypeId = Q.Select(X => X.CompetencyTypeId).FirstOrDefault(),
                    ListKeyAction = Q.Where(X => X.PrefixCode == Q.Key)
                        .Select(X => new
                        {
                            proficiencyLevel = X.ProficiencyLevel,
                            keyActionCode = X.KeyActionCode,
                            keyActionName = X.KeyActionName,
                            keyActionDescription = X.KeyActionDescription,
                            evaluationTypeId = X.EvaluationTypeId,
                            score = X.EvaluationTypeId == (int)EvaluationTypeEnum.KnowledgeId
                                    ? X.Score * 0.1
                                    : X.EvaluationTypeId == (int)EvaluationTypeEnum.SkillId
                                    ? X.Score * 0.2
                                    : X.EvaluationTypeId == (int)EvaluationTypeEnum.BehaviorId
                                    ? X.Score * 0.7
                                    : X.Score,
                            maxScore = X.EvaluationTypeId == (int)EvaluationTypeEnum.KnowledgeId
                                    ? X.MaxScore * 0.1
                                    : X.EvaluationTypeId == (int)EvaluationTypeEnum.SkillId
                                    ? X.MaxScore * 0.2
                                    : X.EvaluationTypeId == (int)EvaluationTypeEnum.BehaviorId
                                    ? X.MaxScore * 0.7
                                    : X.MaxScore
                        }).ToList()
                }).ToList();

            var dataTeamEvaluated = getCompetencyList
                .Select(Q => new UserSidePersonalMappingModel
                {
                    EmployeeId = id,
                    ListHardCompetencyModel = getCompetencyList
                    .Where(X => X.CompetencyTypeId == (int)Enums.CompetencyType.Hard)
                    .Select(X => new UserSideCompetencyMappingViewModel
                    {
                        PrefixCode = X.PrefixCode,
                        CompetencyName = X.CompetencyName,
                        CompetencyDescription = X.CompetencyDescription,
                        CompetencyTypeName = X.CompetencyTypeName,
                        ScoreTotal = X.ListKeyAction.Select(Y => Y.score).Sum(),
                        MaxScoreTotal = X.ListKeyAction.Select(Y => Y.maxScore).Sum(),
                        ListKeyAction = X.ListKeyAction
                        .GroupBy(Y => new { Y.proficiencyLevel, Y.keyActionCode, Y.keyActionName, Y.keyActionDescription })
                        .Select(Y => new UsersideKeyactionViewModel
                        {
                            ProficiencyLevel = Y.Select(Z => Z.proficiencyLevel).FirstOrDefault(),
                            KeyActionCode = Y.Select(Z => Z.keyActionCode).FirstOrDefault(),
                            KeyActionName = Y.Select(Z => Z.keyActionName).FirstOrDefault(),
                            KeyActionDescription = Y.Select(Z => Z.keyActionDescription).FirstOrDefault(),
                            Score = Y.Select(Z => Z.score).Sum(),
                            MaxScore = Y.Select(Z => Z.maxScore).Sum()
                        }).ToList()
                    }).ToList(),
                    ListSoftCompetencyModel = getCompetencyList
                    .Where(X => X.CompetencyTypeId == (int)Enums.CompetencyType.Soft)
                    .Select(X => new UserSideCompetencyMappingViewModel
                    {
                        PrefixCode = X.PrefixCode,
                        CompetencyName = X.CompetencyName,
                        CompetencyDescription = X.CompetencyDescription,
                        CompetencyTypeName = X.CompetencyTypeName,
                        ScoreTotal = X.ListKeyAction.Select(Y => Y.score).Sum(),
                        MaxScoreTotal = X.ListKeyAction.Select(Y => Y.maxScore).Sum(),
                        ListKeyAction = X.ListKeyAction
                        .GroupBy(Y => new { Y.proficiencyLevel, Y.keyActionCode, Y.keyActionName, Y.keyActionDescription })
                        .Select(Y => new UsersideKeyactionViewModel
                        {
                            ProficiencyLevel = Y.Select(Z => Z.proficiencyLevel).FirstOrDefault(),
                            KeyActionCode = Y.Select(Z => Z.keyActionCode).FirstOrDefault(),
                            KeyActionName = Y.Select(Z => Z.keyActionName).FirstOrDefault(),
                            KeyActionDescription = Y.Select(Z => Z.keyActionDescription).FirstOrDefault(),
                            Score = Y.Select(Z => Z.score).Sum(),
                            MaxScore = Y.Select(Z => Z.maxScore).Sum()
                        })
                        .ToList()
                    }).ToList(),
                }).FirstOrDefault();


            //combined demanded with evaluated (separated by score and max score)
            var dataTeams = dataTeamDemanded;

            if (dataTeamEvaluated != null)
            {
                if (dataTeamEvaluated.ListSoftCompetencyModel.Count() != 0)
                {
                    foreach (var item in dataTeamEvaluated.ListSoftCompetencyModel)
                    {
                        var getDataIndex = dataTeams.ListSoftCompetencyModel.FindIndex(Q => Q.PrefixCode == item.PrefixCode);
                        dataTeams.ListSoftCompetencyModel[getDataIndex] = item;
                    }
                }

                if (dataTeamEvaluated.ListHardCompetencyModel.Count() != 0)
                {
                    foreach (var item in dataTeamEvaluated.ListHardCompetencyModel)
                    {
                        var getDataIndex = dataTeams.ListHardCompetencyModel.FindIndex(Q => Q.PrefixCode == item.PrefixCode);
                        dataTeams.ListHardCompetencyModel[getDataIndex] = item;
                    }
                }
            }

            return dataTeams;
        }


    }
}
