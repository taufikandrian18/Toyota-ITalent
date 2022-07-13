using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideTeamMappingService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;

        public UserSideTeamMappingService(TalentContext talentContext, IFileStorageService fileService)
        {
            this.DB = talentContext;
            this.FileService = fileService;
        }
        public async Task<List<UserSidePositionModel>> GetTeamPosition(string userId, int teamId)
        {
            var query = await (from td in this.DB.TeamDetails.AsNoTracking()
                               join epm in this.DB.EmployeePositionMappings.AsNoTracking()
                               on td.EmployeeId equals epm.EmployeeId

                               join p in this.DB.Positions.AsNoTracking()
                               on epm.PositionId equals p.PositionId

                               where td.TeamId == teamId && td.EmployeeId != userId

                               select new UserSidePositionModel
                               {
                                   PositionId = p.PositionId,
                                   PositionDescription = p.PositionDescription,
                                   PositionName = p.PositionName
                               }).Distinct()
                               .ToListAsync();
            return query;
        }
        public async Task<List<UserSideTeamCompetencyMappingViewModel>> GetTeamMappingByName(string userId, int teamId, string positionName)
        {
            var qPerson = await (from epm in this.DB.EmployeePositionMappings.AsNoTracking()
                                 join p in this.DB.Positions.AsNoTracking()
                                 on epm.PositionId equals p.PositionId
                                 join pcm in this.DB.PositionCompentencyMappings.AsNoTracking()
                                 on p.PositionId equals pcm.PositionId
                                 join tmd in this.DB.TeamDetails.AsNoTracking()
                                 on epm.EmployeeId equals tmd.EmployeeId

                                 where tmd.TeamId == teamId && p.PositionName.ToLower() == positionName.ToLower()
                                 select new
                                 {
                                     pcm.CompetencyId
                                 })
                                 .Distinct()
                                 .ToListAsync();

            if (qPerson.Count() <= 0)
            {
                return null;
            }

            var qPoss = await this.GetTeamPosition(userId, teamId);

            if (!string.IsNullOrEmpty(positionName))
            {
                qPoss = qPoss.Where(Q => Q.PositionName.ToLower().Contains(positionName.ToLower())).ToList();
                if (qPoss.Count() <= 0)
                {
                    return null;
                }
            }

            var competencyIds = qPerson.Select(Q => Q.CompetencyId).Distinct().ToList();
            var positionId = qPoss.Select(Q => Q.PositionId).FirstOrDefault();

            //var dataTeams = await ( from c in this.DB.Competencies.AsNoTracking()

            //                        join ct in this.DB.CompetencyTypes.AsNoTracking()
            //                        on c.CompetencyTypeId equals ct.CompetencyTypeId

            //                        where competencyIds.Any(Q => Q == c.CompetencyId) == true

            //                        select new UserSideTeamCompetencyMappingViewModel
            //                        {
            //                            PrefixCode = c.PrefixCode,
            //                            CompetencyName = c.CompetencyName,
            //                            CompetencyId = c.CompetencyId,
            //                            CompetencyDescription = c.CompetencyDescription,
            //                            CompetencyTypeName = ct.CompetencyTypeName,
            //                            CompetencyTypeId = c.CompetencyTypeId,

            //                            ListKeyAction =
            //                            (from ckam in this.DB.CompetencyKeyActionMappings.AsNoTracking()
            //                             join ka in this.DB.KeyActions.AsNoTracking()
            //                             on ckam.KeyActionId equals ka.KeyActionId

            //                             from epm in this.DB.EmployeePositionMappings.AsNoTracking()
            //                             join p in this.DB.Positions.AsNoTracking()
            //                             on epm.PositionId equals p.PositionId

            //                             join td in this.DB.TeamDetails.AsNoTracking()
            //                             on epm.EmployeeId equals td.EmployeeId

            //                             where ckam.CompetencyId == c.CompetencyId && td.TeamId == teamId && epm.PositionId == positionId

            //                             select new UsersideTeamKeyactionViewModel
            //                             {
            //                                 PositionId = epm.PositionId,
            //                                 KeyActionCode = ka.KeyActionCode,
            //                                 KeyActionName = ka.KeyActionName,
            //                                 KeyActionDescription = ka.KeyActionDescription,

            //                                 Proficiencies =
            //                                 (from ps in this.DB.PositionCompentencyMappings.AsNoTracking()
            //                                  join epm in this.DB.EmployeePositionMappings.AsNoTracking()
            //                                  on ps.PositionId equals epm.PositionId

            //                                  join e in this.DB.Employees.AsNoTracking()
            //                                  on epm.EmployeeId equals e.EmployeeId

            //                                  join td in this.DB.TeamDetails.AsNoTracking()
            //                                  on e.EmployeeId equals td.EmployeeId

            //                                  where ps.CompetencyId == ckam.CompetencyId && td.TeamId == teamId && epm.PositionId == positionId
            //                                  select new UserSideTeamProficiencyViewModel
            //                                  {
            //                                      EmployeeId = epm.EmployeeId,
            //                                      ProficiencyLevel = ps.ProficiencyLevel,
            //                                      ImageUrl = e.BlobId.HasValue ? e.BlobId.Value.ToString() : null,
            //                                      TeamId = td.TeamId,
            //                                      PossitionId = epm.PositionId
            //                                  }).ToList()
            //                             }).ToList()

            //                        }).ToListAsync();


            //demanded
            var queryDemandedList = await DB.GetDemandedTeamMapping(teamId, positionId, competencyIds).ToListAsync();

            if (queryDemandedList == null) return null;

            var dataDemandedTeams = queryDemandedList
                .GroupBy(Q => Q.PrefixCode)
                .Select(Q => new UserSideTeamCompetencyMappingViewModel
                {
                    PrefixCode = Q.Key,
                    CompetencyName = Q.Select(X => X.CompetencyName).FirstOrDefault(),
                    CompetencyId = Q.Select(X => X.CompetencyId).FirstOrDefault(),
                    CompetencyDescription = Q.Select(X => X.CompetencyDescription).FirstOrDefault(),
                    CompetencyTypeName = Q.Select(X => X.CompetencyTypeName).FirstOrDefault(),
                    CompetencyTypeId = Q.Select(X => X.CompetencyTypeId).FirstOrDefault(),
                    ListKeyAction = Q.Where(X => X.PrefixCode == Q.Key)
                        .GroupBy(X => X.KeyActionCode)
                        .Select(X => new UsersideTeamKeyactionViewModel
                        {
                            ProficiencyLevel = X.Select(Z => Z.ProficiencyLevel).FirstOrDefault(),
                            PositionId = X.Select(Z => Z.PositionId).FirstOrDefault(),
                            KeyActionCode = X.Select(Z => Z.KeyActionCode).FirstOrDefault(),
                            KeyActionName = X.Select(Z => Z.KeyActionName).FirstOrDefault(),
                            KeyActionDescription = X.Select(Z => Z.KeyActionDescription).FirstOrDefault(),
                            Proficiencies = X.Where(Z => Z.KeyActionCode == X.Key)
                                .Select(Z => new UserSideTeamProficiencyViewModel
                                {
                                    PossitionId = Z.PositionId,
                                    TeamId = Z.TeamId,
                                    EmployeeId = Z.EmployeeId,
                                    ImageUrl = Z.BlobId?.ToString()
                                }).ToList()
                        }).ToList()
                }).ToList();

            foreach (var teams in dataDemandedTeams)
            {
                foreach (var keyAction in teams.ListKeyAction)
                {
                    foreach (var proficiency in keyAction.Proficiencies)
                    {
                        var blobId = proficiency.ImageUrl;
                        string image = null;
                        if (!string.IsNullOrEmpty(blobId))
                        {
                            var dataImage = await this.FileService.GetFileDetailAsync(blobId);
                            image = dataImage.FileUrl;
                        }
                        proficiency.ImageUrl = image;
                    }
                }
            }

            

            //evaluated
            var queryEvaluatedList = await DB.GetEvaluatedTeamMapping(teamId, positionId, competencyIds).ToListAsync();

            if (queryEvaluatedList == null) return dataDemandedTeams;
            else
            {
                var calcDataEvaluatedTeams = queryEvaluatedList
                    .GroupBy(Q => Q.PrefixCode)
                    .Select(Q => new
                    {
                        PrefixCode = Q.Key,
                        CompetencyName = Q.Select(X => X.CompetencyName).FirstOrDefault(),
                        CompetencyId = Q.Select(X => X.CompetencyId).FirstOrDefault(),
                        CompetencyDescription = Q.Select(X => X.CompetencyDescription).FirstOrDefault(),
                        CompetencyTypeName = Q.Select(X => X.CompetencyTypeName).FirstOrDefault(),
                        CompetencyTypeId = Q.Select(X => X.CompetencyTypeId).FirstOrDefault(),
                        ListKeyAction = Q.Where(X => X.PrefixCode == Q.Key)
                            .GroupBy(X => new { X.KeyActionCode, X.EmployeeId, X.EvaluationTypeId })
                            .Select(X => new
                            {
                                ProficiencyLevel = X.Select(Z => Z.ProficiencyLevel).FirstOrDefault(),
                                PositionId = X.Select(Z => Z.PositionId).FirstOrDefault(),
                                X.Key.KeyActionCode,
                                KeyActionName = X.Select(Z => Z.KeyActionName).FirstOrDefault(),
                                KeyActionDescription = X.Select(Z => Z.KeyActionDescription).FirstOrDefault(),
                                TeamId = X.Select(Z => Z.TeamId).FirstOrDefault(),
                                EmployeeId = X.Select(Z => Z.EmployeeId).FirstOrDefault(),
                                ImageUrl = X.Select(Z => Z.BlobId).FirstOrDefault()?.ToString(),
                                Score = X.Select(Z => Z.EvaluationTypeId).FirstOrDefault() == (int)EvaluationTypeEnum.KnowledgeId
                                    ? X.Select(Z => Z.Score).FirstOrDefault() * 0.1
                                    : X.Select(Z => Z.EvaluationTypeId).FirstOrDefault() == (int)EvaluationTypeEnum.SkillId
                                    ? X.Select(Z => Z.Score).FirstOrDefault() * 0.2
                                    : X.Select(Z => Z.EvaluationTypeId).FirstOrDefault() == (int)EvaluationTypeEnum.BehaviorId
                                    ? X.Select(Z => Z.Score).FirstOrDefault() * 0.7
                                    : X.Select(Z => Z.Score).FirstOrDefault(),
                                MaxScore = X.Select(Z => Z.EvaluationTypeId).FirstOrDefault() == (int)EvaluationTypeEnum.KnowledgeId
                                    ? X.Select(Z => Z.MaxScore).FirstOrDefault() * 0.1
                                    : X.Select(Z => Z.EvaluationTypeId).FirstOrDefault() == (int)EvaluationTypeEnum.SkillId
                                    ? X.Select(Z => Z.MaxScore).FirstOrDefault() * 0.2
                                    : X.Select(Z => Z.EvaluationTypeId).FirstOrDefault() == (int)EvaluationTypeEnum.BehaviorId
                                    ? X.Select(Z => Z.MaxScore).FirstOrDefault() * 0.7
                                    : X.Select(Z => Z.MaxScore).FirstOrDefault()
                            }).ToList()
                    }).ToList();

                var calcAvgDataEvaluatedTeams = calcDataEvaluatedTeams
                    .Select(Q => new
                    {
                        Q.PrefixCode,
                        Q.CompetencyName,
                        Q.CompetencyId,
                        Q.CompetencyDescription,
                        Q.CompetencyTypeName,
                        Q.CompetencyTypeId,
                        ListKeyAction = Q.ListKeyAction
                            .GroupBy(X => new { X.ProficiencyLevel, X.PositionId, X.KeyActionCode, X.KeyActionName, X.KeyActionDescription })
                            .Select(X => new
                            {
                                ProficiencyLevel = X.Select(Y => Y.ProficiencyLevel).FirstOrDefault(),
                                PositionId = X.Select(Y => Y.PositionId).FirstOrDefault(),
                                KeyActionCode = X.Select(Y => Y.KeyActionCode).FirstOrDefault(),
                                KeyActionName = X.Select(Y => Y.KeyActionName).FirstOrDefault(),
                                KeyActionDescription = X.Select(Y => Y.KeyActionDescription).FirstOrDefault(),
                                ScoreSubTotal = X.GroupBy(Y => Y.EmployeeId).Average(Y => Y.Sum(Z => Z.Score)),
                                MaxScoreSubTotal = X.GroupBy(Y => Y.EmployeeId).Average(Y => Y.Sum(Z => Z.MaxScore)),
                                Proficiencies = X.Select(Y => new
                                {
                                    PossitionId = Y.PositionId,
                                    TeamId = Y.TeamId,
                                    EmployeeId = Y.EmployeeId,
                                    ImageUrl = Y.ImageUrl,
                                    Score = Y.Score,
                                    MaxScore = Y.MaxScore
                                }).ToList()
                            }).ToList()
                    }).ToList();

                var dataEvaluatedTeams = calcAvgDataEvaluatedTeams
                    .Select(Q => new UserSideTeamCompetencyMappingViewModel
                    {
                        PrefixCode = Q.PrefixCode,
                        CompetencyName = Q.CompetencyName,
                        CompetencyId = Q.CompetencyId,
                        CompetencyDescription = Q.CompetencyDescription,
                        CompetencyTypeName = Q.CompetencyTypeName,
                        CompetencyTypeId = Q.CompetencyTypeId,
                        ScoreTotal = Q.ListKeyAction.Select(X => X.ScoreSubTotal).Sum(),
                        MaxScoreTotal = Q.ListKeyAction.Select(X => X.MaxScoreSubTotal).Sum(),
                        ListKeyAction = Q.ListKeyAction
                            .Select(X => new UsersideTeamKeyactionViewModel
                            {
                                ProficiencyLevel = X.ProficiencyLevel,
                                PositionId = X.PositionId,
                                KeyActionCode = X.KeyActionCode,
                                KeyActionName = X.KeyActionName,
                                KeyActionDescription = X.KeyActionDescription,
                                ScoreSubTotal = X.ScoreSubTotal,
                                MaxScoreSubTotal = X.MaxScoreSubTotal,
                                Proficiencies = X.Proficiencies
                                    .GroupBy(Y => new { Y.EmployeeId, Y.ImageUrl, Y.TeamId, Y.PossitionId })
                                    .Select(Y => new UserSideTeamProficiencyViewModel
                                    {
                                        PossitionId = Y.Key.PossitionId,
                                        TeamId = Y.Key.TeamId,
                                        EmployeeId = Y.Key.EmployeeId,
                                        ImageUrl = Y.Key.ImageUrl,
                                        Score = Y.Sum(Z => Z.Score),
                                        MaxScore = Y.Sum(Z => Z.MaxScore)
                                    }).ToList()
                            }).ToList()
                    }).ToList();

                //foreach (var teams in dataEvaluatedTeams)
                //{
                //    foreach (var keyAction in teams.ListKeyAction)
                //    {
                //        foreach (var proficiency in keyAction.Proficiencies)
                //        {
                //            var blobId = proficiency.ImageUrl;
                //            string image = null;
                //            if (!string.IsNullOrEmpty(blobId))
                //            {
                //                var dataImage = await this.FileService.GetFileDetailAsync(blobId);
                //                image = dataImage.FileUrl;
                //            }
                //            proficiency.ImageUrl = image;
                //        }
                //    }
                //}

                var dataTeams = dataDemandedTeams;

                //untuk mengisi nilai score, max score, scoreSubTotal, maxScoreSubTotal, ScroreTotal dan MaxScoreTotal
                if (dataEvaluatedTeams != null)
                {
                    foreach (var item in dataEvaluatedTeams)
                    {
                        var getDataIndex = dataTeams.FindIndex(Q => Q.PrefixCode == item.PrefixCode);
                        dataTeams[getDataIndex].ScoreTotal = item.ScoreTotal;
                        dataTeams[getDataIndex].MaxScoreTotal = item.MaxScoreTotal;

                        foreach (var item1 in item.ListKeyAction)
                        {
                            var getDataIndexListKeyAction = dataTeams[getDataIndex].ListKeyAction.FindIndex(Q => Q.KeyActionCode == item1.KeyActionCode);
                            dataTeams[getDataIndex].ListKeyAction[getDataIndexListKeyAction].ScoreSubTotal = item1.ScoreSubTotal;
                            dataTeams[getDataIndex].ListKeyAction[getDataIndexListKeyAction].MaxScoreSubTotal = item1.MaxScoreSubTotal;

                            foreach (var item2 in item1.Proficiencies)
                            {
                                var getDataIndexProficiency = dataTeams[getDataIndex].ListKeyAction[getDataIndexListKeyAction].Proficiencies.FindIndex(Q => Q.EmployeeId == item2.EmployeeId);
                                dataTeams[getDataIndex].ListKeyAction[getDataIndexListKeyAction].Proficiencies[getDataIndexProficiency].Score = item2.Score;
                                dataTeams[getDataIndex].ListKeyAction[getDataIndexListKeyAction].Proficiencies[getDataIndexProficiency].MaxScore = item2.MaxScore;

                            }
                        }
                    }
                }

                return dataTeams;
            }
        }

        public async Task<UserSideTeamMappingModel> GetTeamMappingByPositionName(string userId, int teamId, string positionName)
        {
            var data = await this.GetTeamMappingByName(userId, teamId, positionName);

            var dataNew = new UserSideTeamMappingModel()
            {
                ListHardCompetencyModel = new List<UserSideTeamCompetencyMappingViewModel>(),
                ListSoftCompetencyModel = new List<UserSideTeamCompetencyMappingViewModel>()
            };
            if (data != null)
            {
                var dataSoft = data.Where(Q => Q.CompetencyTypeId == (int)Enums.CompetencyType.Soft).ToList();
                var dataHard = data.Where(Q => Q.CompetencyTypeId == (int)Enums.CompetencyType.Hard).ToList();

                if (dataSoft != null || dataSoft.Count() > 0)
                {
                    dataNew.ListSoftCompetencyModel = dataSoft;
                    dataNew.ListSoftCompetencyModel.OrderBy(Q => Q.PrefixCode);
                }
                if (dataHard != null || dataHard.Count() > 0)
                {
                    dataNew.ListHardCompetencyModel = dataHard;
                    dataNew.ListHardCompetencyModel.OrderBy(Q => Q.PrefixCode);
                }
            }

            return dataNew;
        }


        public async Task<List<UserSideTeamCompetencyMappingViewModel>> GetTeamMappingById(int teamId, int positionId)
        {
            var qPerson = await (from epm in this.DB.EmployeePositionMappings.AsNoTracking()
                                 join p in this.DB.Positions.AsNoTracking()
                                 on epm.PositionId equals p.PositionId
                                 join pcm in this.DB.PositionCompentencyMappings.AsNoTracking()
                                 on p.PositionId equals pcm.PositionId
                                 join tmd in this.DB.TeamDetails.AsNoTracking()
                                 on epm.EmployeeId equals tmd.EmployeeId

                                 join ps in this.DB.EmployeePositionMappings.AsNoTracking()
                                 on tmd.EmployeeId equals ps.EmployeeId

                                 join pss in this.DB.Positions.AsNoTracking()
                                 on ps.PositionId equals pss.PositionId

                                 where tmd.TeamId == teamId //&& ps.PositionId == positionId
                                 select new
                                 {
                                     pss.PositionName,
                                     pcm.CompetencyId
                                 })
                                 .Distinct()
                                 .ToListAsync();

            if (qPerson.Count() <= 0)
            {
                return null;
            }

            var competencyIds = qPerson.Select(Q => Q.CompetencyId).ToList();

            //var dataTeams = await (from c in this.DB.Competencies.AsNoTracking()

            //                       join ct in this.DB.CompetencyTypes.AsNoTracking()
            //                       on c.CompetencyTypeId equals ct.CompetencyTypeId

            //                       where competencyIds.Any(Q => Q == c.CompetencyId) == true

            //                       select new UserSideTeamCompetencyMappingViewModel
            //                       {
            //                           PrefixCode = c.PrefixCode,
            //                           CompetencyName = c.CompetencyName,
            //                           CompetencyId = c.CompetencyId,
            //                           CompetencyDescription = c.CompetencyDescription,
            //                           CompetencyTypeName = ct.CompetencyTypeName,
            //                           CompetencyTypeId = c.CompetencyTypeId,

            //                           ListKeyAction =
            //                           (from ckam in this.DB.CompetencyKeyActionMappings.AsNoTracking()
            //                            join ka in this.DB.KeyActions.AsNoTracking()
            //                            on ckam.KeyActionId equals ka.KeyActionId

            //                            from epm in this.DB.EmployeePositionMappings.AsNoTracking()
            //                            join p in this.DB.Positions.AsNoTracking()
            //                            on epm.PositionId equals p.PositionId

            //                            join td in this.DB.TeamDetails.AsNoTracking()
            //                            on epm.EmployeeId equals td.EmployeeId

            //                            where ckam.CompetencyId == c.CompetencyId && td.TeamId == teamId && epm.PositionId == positionId

            //                            select new UsersideTeamKeyactionViewModel
            //                            {
            //                                PositionId = epm.PositionId,
            //                                KeyActionCode = ka.KeyActionCode,
            //                                KeyActionName = ka.KeyActionName,
            //                                KeyActionDescription = ka.KeyActionDescription,

            //                                Proficiencies =
            //                               (from ps in this.DB.PositionCompentencyMappings.AsNoTracking()
            //                                join epm in this.DB.EmployeePositionMappings.AsNoTracking()
            //                                on ps.PositionId equals epm.PositionId

            //                                join e in this.DB.Employees.AsNoTracking()
            //                                on epm.EmployeeId equals e.EmployeeId

            //                                join td in this.DB.TeamDetails.AsNoTracking()
            //                                on e.EmployeeId equals td.EmployeeId

            //                                where ps.CompetencyId == ckam.CompetencyId && td.TeamId == teamId && epm.PositionId == positionId
            //                                select new UserSideTeamProficiencyViewModel
            //                                {
            //                                    EmployeeId = epm.EmployeeId,
            //                                    ProficiencyLevel = ps.ProficiencyLevel,
            //                                    ImageUrl = e.BlobId.HasValue ? e.BlobId.Value.ToString() : null,
            //                                    TeamId = td.TeamId,
            //                                    PossitionId = epm.PositionId
            //                                }).ToList()
            //                            }).ToList()

            //                       }).ToListAsync();

            //foreach (var teams in dataTeams)
            //{
            //    foreach (var keyAction in teams.ListKeyAction)
            //    {
            //        foreach (var proficiency in keyAction.Proficiencies)
            //        {
            //            var blobId = proficiency.ImageUrl;
            //            string image = null;
            //            if (!string.IsNullOrEmpty(blobId))
            //            {
            //                var dataImage = await this.FileService.GetFileDetailAsync(blobId);
            //                image = dataImage.FileUrl;
            //            }
            //            proficiency.ImageUrl = image;
            //        }
            //    }
            //}

            //demanded
            var queryDemandedList = await DB.GetDemandedTeamMapping(teamId, positionId, competencyIds).ToListAsync();

            if (queryDemandedList == null) return null;

            var dataDemandedTeams = queryDemandedList
                .GroupBy(Q => Q.PrefixCode)
                .Select(Q => new UserSideTeamCompetencyMappingViewModel
                {
                    PrefixCode = Q.Key,
                    CompetencyName = Q.Select(X => X.CompetencyName).FirstOrDefault(),
                    CompetencyId = Q.Select(X => X.CompetencyId).FirstOrDefault(),
                    CompetencyDescription = Q.Select(X => X.CompetencyDescription).FirstOrDefault(),
                    CompetencyTypeName = Q.Select(X => X.CompetencyTypeName).FirstOrDefault(),
                    CompetencyTypeId = Q.Select(X => X.CompetencyTypeId).FirstOrDefault(),
                    ListKeyAction = Q.Where(X => X.PrefixCode == Q.Key)
                        .GroupBy(X => X.KeyActionCode)
                        .Select(X => new UsersideTeamKeyactionViewModel
                        {
                            ProficiencyLevel = X.Select(Z => Z.ProficiencyLevel).FirstOrDefault(),
                            PositionId = X.Select(Z => Z.PositionId).FirstOrDefault(),
                            KeyActionCode = X.Select(Z => Z.KeyActionCode).FirstOrDefault(),
                            KeyActionName = X.Select(Z => Z.KeyActionName).FirstOrDefault(),
                            KeyActionDescription = X.Select(Z => Z.KeyActionDescription).FirstOrDefault(),
                            Proficiencies = X.Where(Z => Z.KeyActionCode == X.Key)
                                .Select(Z => new UserSideTeamProficiencyViewModel
                                {
                                    PossitionId = Z.PositionId,
                                    TeamId = Z.TeamId,
                                    EmployeeId = Z.EmployeeId,
                                    ImageUrl = Z.BlobId?.ToString()
                                }).ToList()
                        }).ToList()
                }).ToList();

            foreach (var teams in dataDemandedTeams)
            {
                foreach (var keyAction in teams.ListKeyAction)
                {
                    foreach (var proficiency in keyAction.Proficiencies)
                    {
                        var blobId = proficiency.ImageUrl;
                        string image = null;
                        if (!string.IsNullOrEmpty(blobId))
                        {
                            var dataImage = await this.FileService.GetFileDetailAsync(blobId);
                            image = dataImage.FileUrl;
                        }
                        proficiency.ImageUrl = image;
                    }
                }
            }

            //evaluated
            var queryEvaluatedList = await DB.GetEvaluatedTeamMapping(teamId, positionId, competencyIds).ToListAsync();

            if (queryEvaluatedList == null) return dataDemandedTeams;
            else
            {
                var calcDataEvaluatedTeams = queryEvaluatedList
                    .GroupBy(Q => Q.PrefixCode)
                    .Select(Q => new
                    {
                        PrefixCode = Q.Key,
                        CompetencyName = Q.Select(X => X.CompetencyName).FirstOrDefault(),
                        CompetencyId = Q.Select(X => X.CompetencyId).FirstOrDefault(),
                        CompetencyDescription = Q.Select(X => X.CompetencyDescription).FirstOrDefault(),
                        CompetencyTypeName = Q.Select(X => X.CompetencyTypeName).FirstOrDefault(),
                        CompetencyTypeId = Q.Select(X => X.CompetencyTypeId).FirstOrDefault(),
                        ListKeyAction = Q.Where(X => X.PrefixCode == Q.Key)
                            .GroupBy(X => new { X.KeyActionCode, X.EmployeeId, X.EvaluationTypeId })
                            .Select(X => new
                            {
                                ProficiencyLevel = X.Select(Z => Z.ProficiencyLevel).FirstOrDefault(),
                                PositionId = X.Select(Z => Z.PositionId).FirstOrDefault(),
                                X.Key.KeyActionCode,
                                KeyActionName = X.Select(Z => Z.KeyActionName).FirstOrDefault(),
                                KeyActionDescription = X.Select(Z => Z.KeyActionDescription).FirstOrDefault(),
                                TeamId = X.Select(Z => Z.TeamId).FirstOrDefault(),
                                EmployeeId = X.Select(Z => Z.EmployeeId).FirstOrDefault(),
                                ImageUrl = X.Select(Z => Z.BlobId).FirstOrDefault()?.ToString(),
                                Score = X.Select(Z => Z.EvaluationTypeId).FirstOrDefault() == (int)EvaluationTypeEnum.KnowledgeId
                                    ? X.Select(Z => Z.Score).FirstOrDefault() * 0.1
                                    : X.Select(Z => Z.EvaluationTypeId).FirstOrDefault() == (int)EvaluationTypeEnum.SkillId
                                    ? X.Select(Z => Z.Score).FirstOrDefault() * 0.2
                                    : X.Select(Z => Z.EvaluationTypeId).FirstOrDefault() == (int)EvaluationTypeEnum.BehaviorId
                                    ? X.Select(Z => Z.Score).FirstOrDefault() * 0.7
                                    : X.Select(Z => Z.Score).FirstOrDefault(),
                                MaxScore = X.Select(Z => Z.EvaluationTypeId).FirstOrDefault() == (int)EvaluationTypeEnum.KnowledgeId
                                    ? X.Select(Z => Z.MaxScore).FirstOrDefault() * 0.1
                                    : X.Select(Z => Z.EvaluationTypeId).FirstOrDefault() == (int)EvaluationTypeEnum.SkillId
                                    ? X.Select(Z => Z.MaxScore).FirstOrDefault() * 0.2
                                    : X.Select(Z => Z.EvaluationTypeId).FirstOrDefault() == (int)EvaluationTypeEnum.BehaviorId
                                    ? X.Select(Z => Z.MaxScore).FirstOrDefault() * 0.7
                                    : X.Select(Z => Z.MaxScore).FirstOrDefault()
                            }).ToList()
                    }).ToList();

                var calcAvgDataEvaluatedTeams = calcDataEvaluatedTeams
                    .Select(Q => new
                    {
                        Q.PrefixCode,
                        Q.CompetencyName,
                        Q.CompetencyId,
                        Q.CompetencyDescription,
                        Q.CompetencyTypeName,
                        Q.CompetencyTypeId,
                        ListKeyAction = Q.ListKeyAction
                            .GroupBy(X => new { X.ProficiencyLevel, X.PositionId, X.KeyActionCode, X.KeyActionName, X.KeyActionDescription })
                            .Select(X => new
                            {
                                ProficiencyLevel = X.Select(Y => Y.ProficiencyLevel).FirstOrDefault(),
                                PositionId = X.Select(Y => Y.PositionId).FirstOrDefault(),
                                KeyActionCode = X.Select(Y => Y.KeyActionCode).FirstOrDefault(),
                                KeyActionName = X.Select(Y => Y.KeyActionName).FirstOrDefault(),
                                KeyActionDescription = X.Select(Y => Y.KeyActionDescription).FirstOrDefault(),
                                ScoreSubTotal = X.GroupBy(Y => Y.EmployeeId).Average(Y => Y.Sum(Z => Z.Score)),
                                MaxScoreSubTotal = X.GroupBy(Y => Y.EmployeeId).Average(Y => Y.Sum(Z => Z.MaxScore)),
                                Proficiencies = X.Select(Y => new
                                {
                                    PossitionId = Y.PositionId,
                                    TeamId = Y.TeamId,
                                    EmployeeId = Y.EmployeeId,
                                    ImageUrl = Y.ImageUrl,
                                    Score = Y.Score,
                                    MaxScore = Y.MaxScore
                                }).ToList()
                            }).ToList()
                    }).ToList();

                var dataEvaluatedTeams = calcAvgDataEvaluatedTeams
                    .Select(Q => new UserSideTeamCompetencyMappingViewModel
                    {
                        PrefixCode = Q.PrefixCode,
                        CompetencyName = Q.CompetencyName,
                        CompetencyId = Q.CompetencyId,
                        CompetencyDescription = Q.CompetencyDescription,
                        CompetencyTypeName = Q.CompetencyTypeName,
                        CompetencyTypeId = Q.CompetencyTypeId,
                        ScoreTotal = Q.ListKeyAction.Select(X => X.ScoreSubTotal).Sum(),
                        MaxScoreTotal = Q.ListKeyAction.Select(X => X.MaxScoreSubTotal).Sum(),
                        ListKeyAction = Q.ListKeyAction
                            .Select(X => new UsersideTeamKeyactionViewModel
                            {
                                ProficiencyLevel = X.ProficiencyLevel,
                                PositionId = X.PositionId,
                                KeyActionCode = X.KeyActionCode,
                                KeyActionName = X.KeyActionName,
                                KeyActionDescription = X.KeyActionDescription,
                                ScoreSubTotal = X.ScoreSubTotal,
                                MaxScoreSubTotal = X.MaxScoreSubTotal,
                                Proficiencies = X.Proficiencies
                                    .GroupBy(Y => new { Y.EmployeeId, Y.ImageUrl, Y.TeamId, Y.PossitionId })
                                    .Select(Y => new UserSideTeamProficiencyViewModel
                                    {
                                        PossitionId = Y.Key.PossitionId,
                                        TeamId = Y.Key.TeamId,
                                        EmployeeId = Y.Key.EmployeeId,
                                        ImageUrl = Y.Key.ImageUrl,
                                        Score = Y.Sum(Z => Z.Score),
                                        MaxScore = Y.Sum(Z => Z.MaxScore)
                                    }).ToList()
                            }).ToList()
                    }).ToList();

                //foreach (var teams in dataEvaluatedTeams)
                //{
                //    foreach (var keyAction in teams.ListKeyAction)
                //    {
                //        foreach (var proficiency in keyAction.Proficiencies)
                //        {
                //            var blobId = proficiency.ImageUrl;
                //            string image = null;
                //            if (!string.IsNullOrEmpty(blobId))
                //            {
                //                var dataImage = await this.FileService.GetFileDetailAsync(blobId);
                //                image = dataImage.FileUrl;
                //            }
                //            proficiency.ImageUrl = image;
                //        }
                //    }
                //}

                var dataTeams = dataDemandedTeams;

                //untuk mengisi nilai score, max score, scoreSubTotal, maxScoreSubTotal, ScroreTotal dan MaxScoreTotal
                if (dataEvaluatedTeams != null)
                {
                    foreach (var item in dataEvaluatedTeams)
                    {
                        var getDataIndex = dataTeams.FindIndex(Q => Q.PrefixCode == item.PrefixCode);
                        dataTeams[getDataIndex].ScoreTotal = item.ScoreTotal;
                        dataTeams[getDataIndex].MaxScoreTotal = item.MaxScoreTotal;

                        foreach (var item1 in item.ListKeyAction)
                        {
                            var getDataIndexListKeyAction = dataTeams[getDataIndex].ListKeyAction.FindIndex(Q => Q.KeyActionCode == item1.KeyActionCode);
                            dataTeams[getDataIndex].ListKeyAction[getDataIndexListKeyAction].ScoreSubTotal = item1.ScoreSubTotal;
                            dataTeams[getDataIndex].ListKeyAction[getDataIndexListKeyAction].MaxScoreSubTotal = item1.MaxScoreSubTotal;

                            foreach (var item2 in item1.Proficiencies)
                            {
                                var getDataIndexProficiency = dataTeams[getDataIndex].ListKeyAction[getDataIndexListKeyAction].Proficiencies.FindIndex(Q => Q.EmployeeId == item2.EmployeeId);
                                dataTeams[getDataIndex].ListKeyAction[getDataIndexListKeyAction].Proficiencies[getDataIndexProficiency].Score = item2.Score;
                                dataTeams[getDataIndex].ListKeyAction[getDataIndexListKeyAction].Proficiencies[getDataIndexProficiency].MaxScore = item2.MaxScore;

                            }
                        }

                    }
                }

                return dataTeams;
            }
        }

        public async Task<UserSideTeamMappingModel> GetTeamMappingByPositionId(int teamId, int positionId)
        {
            var data = await this.GetTeamMappingById(teamId, positionId);

            var dataNew = new UserSideTeamMappingModel();
            if (data != null)
            {
                var dataSoft = data.Where(Q => Q.CompetencyTypeId == (int)Enums.CompetencyType.Soft).ToList();
                var dataHard = data.Where(Q => Q.CompetencyTypeId == (int)Enums.CompetencyType.Hard).ToList();

                if (dataSoft != null || dataSoft.Count() > 0)
                {
                    dataNew.ListSoftCompetencyModel = dataSoft;
                }
                if (dataHard != null || dataHard.Count() > 0)
                {
                    dataNew.ListHardCompetencyModel = dataHard;
                }
            }

            return dataNew;
        }

    }
}