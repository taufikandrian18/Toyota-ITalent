using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideTeamService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;
        private readonly UserSideInboxService UserSideInboxService;

        public UserSideTeamService(TalentContext talentContext, IFileStorageService fileService, UserSideInboxService userSideInboxService)
        {
            this.DB = talentContext;
            this.FileService = fileService;
            this.UserSideInboxService = userSideInboxService;
        }

        /// <summary>
        /// Get all team data by employee id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of team data in <seealso cref="List{UserSideTeamModel}"/> format.</returns>
        public async Task<UserSideTeamViewModel> GetTeamByEmployeeId(string id)
        {
            //// Get Team Id data by Employee Id
            //var queryTeam = await (from e in this.DB.Employees
            //                       join t in this.DB.TeamDetails
            //                       on e.EmployeeId equals t.EmployeeId into at
            //                       from atn in at.DefaultIfEmpty()
            //                       where e.EmployeeId.ToLower() == id.ToLower()
            //                       select new
            //                       {
            //                           e.EmployeeId,
            //                           e.IsTeamLeader,
            //                           TeamId = atn.TeamId > 0 ? atn.TeamId : 0
            //                       }).FirstOrDefaultAsync();

            //var dataTeam = new UserSideTeamViewModel
            //{
            //    EmployeeId = queryTeam.EmployeeId,
            //    IsTeamLeader = queryTeam.IsTeamLeader,
            //    TotalTeam = 0
            //};

            //if (queryTeam.TeamId > 0)
            //{
            //    // Get Teams data by Team Id
            //    var queryTeams = await (from td in DB.TeamDetails
            //                            join e in DB.Employees
            //                            on td.EmployeeId equals e.EmployeeId

            //                            join b in DB.Blobs
            //                            on e.BlobId equals b.BlobId into ab
            //                            from bn in ab.DefaultIfEmpty()

            //                            join epm in DB.EmployeePositionMappings
            //                            on e.EmployeeId equals epm.EmployeeId into aepm
            //                            from epmn in aepm.DefaultIfEmpty()

            //                            join p in DB.Positions
            //                            on epmn.PositionId equals p.PositionId into ap
            //                            from pn in ap.DefaultIfEmpty()

            //                            join al in DB.AssignedLearnings
            //                            on e.EmployeeId equals al.AssignedTo into aal
            //                            from al in aal.DefaultIfEmpty()

            //                            join rtm in this.DB.RotateTeamMembers
            //                            on e.EmployeeId equals rtm.EmployeeId into artm
            //                            from artmn in artm.DefaultIfEmpty()

            //                            where td.TeamId == queryTeam.TeamId && e.EmployeeId != id && e.ManpowerStatusType != null
            //                            select new
            //                            {
            //                                e.EmployeeId,
            //                                e.EmployeeName,
            //                                e.BlobId,
            //                                Mime = string.IsNullOrEmpty(bn.Mime) ? "" : bn.Mime,
            //                                AssignedTo = !string.IsNullOrEmpty(al.AssignedTo) ? true : false,
            //                                IsRotate = !string.IsNullOrEmpty(artmn.EmployeeId) && artmn.ApprovalStatusId == ApprovalStatusesEnum.WaitingId ? true : false,
            //                                e.IsTeamLeader,
            //                                PositionName = string.IsNullOrEmpty(pn.PositionName) ? "" : pn.PositionName,
            //                                e.ManpowerStatusType,
            //                                td.TeamId,
            //                            })
            //                            .ToListAsync();

            //    var totalTeam = await this
            //        .DB
            //        .TeamDetails
            //        .AsNoTracking()
            //        .Where(Q => Q.EmployeeId == queryTeam.EmployeeId)
            //        .CountAsync();

            //    //.Where(Q => Q.First().ManpowerStatusType.Contains(Constant.ManpowerStatusType.Contract))
            //    // Kenapa contains? karena data dari MDM ada spasinya
            //    var listContract = queryTeams.OrderByDescending(Q => Q.IsRotate).GroupBy(Q => Q.EmployeeId)
            //        .Where(Q => Q.First().ManpowerStatusType.Contains(Constant.ManpowerStatusType.Contract))
            //        .Select(async X => new UserSideTeamModel
            //        {
            //            EmployeeId = X.First().EmployeeId,
            //            EmployeeName = X.First().EmployeeName,
            //            ImageUrl = X.First().BlobId.HasValue ? await this.FileService.GetFileAsync(X.First().BlobId.Value.ToString(), X.First().Mime) : "",
            //            IsAssign = X.First().AssignedTo,
            //            IsRotated = X.First().IsRotate,
            //            IsTeamLeader = X.First().IsTeamLeader,
            //            PositionName = X.First().PositionName,
            //            StatusContract = X.First().ManpowerStatusType,
            //            TeamId = X.First().TeamId
            //        })
            //        .Select(Q => Q.Result)
            //        .ToList();

            //    // .Where(Q => Q.First().ManpowerStatusType.Contains(Constant.ManpowerStatusType.Permanent))
            //    // Kenapa contains? karena data dari MDM ada spasinya
            //    var listPermanent = queryTeams.GroupBy(Q => Q.EmployeeId)
            //        .Where(Q => Q.First().ManpowerStatusType.Contains(Constant.ManpowerStatusType.Permanent))
            //        .Select(async X => new UserSideTeamModel
            //        {
            //            EmployeeId = X.First().EmployeeId,
            //            EmployeeName = X.First().EmployeeName,
            //            ImageUrl = X.First().BlobId.HasValue ? await this.FileService.GetFileAsync(X.First().BlobId.Value.ToString(), X.First().Mime) : "",
            //            IsAssign = X.First().AssignedTo,
            //            IsRotated = X.First().IsRotate,
            //            IsTeamLeader = X.First().IsTeamLeader,
            //            PositionName = X.First().PositionName,
            //            StatusContract = X.First().ManpowerStatusType,
            //            TeamId = X.First().TeamId
            //        })
            //        .Select(Q => Q.Result)
            //        .ToList();

            //    dataTeam.ListTeamContract = listContract;
            //    dataTeam.ListTeamPermanent = listPermanent;
            //    dataTeam.TotalTeam = totalTeam;
            //}

            var dataTeam = new UserSideTeamViewModel();
            return dataTeam;
        }

        /// <summary>
        /// Get all employee data for show data employee before Rotate Team
        /// </summary>
        /// <returns></returns>
        public async Task<UserSideTeamResponseModel> GetAllLeader(UserSideTeamFilterModel filter, string employeeId)
        {
            var queryGetAllLeader = from e in this.DB.Employees
                                    join t in this.DB.Teams
                                    on e.EmployeeId equals t.TeamLeaderId

                                    join b in this.DB.Blobs
                                    on e.BlobId equals b.BlobId into ab
                                    from abn in ab.DefaultIfEmpty()

                                    join epm in this.DB.EmployeePositionMappings
                                    on e.EmployeeId equals epm.EmployeeId into aepm
                                    from epm in aepm.DefaultIfEmpty()

                                    join p in this.DB.Positions
                                    on epm.PositionId equals p.PositionId into ap
                                    from p in ap.DefaultIfEmpty()

                                    where e.EmployeeId != employeeId

                                    select new
                                    {
                                        e.EmployeeId,
                                        e.EmployeeName,
                                        t.TeamId,
                                        BlobId = e.BlobId.HasValue ? e.BlobId.Value : Guid.Empty,
                                        abn.Mime,
                                        p.PositionName
                                    };

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                queryGetAllLeader = queryGetAllLeader.Where(Q => Q.EmployeeName.ToLower().Contains(filter.Keyword.ToLower()) == true);
            }

            var dataLeaderList = await queryGetAllLeader.ToListAsync();
            var totalData = dataLeaderList.Count();

            if (totalData <= filter.PageSize)
            {
                filter.PageIndex = 1;
            };

            var skipCount = Math.Ceiling((decimal)(filter.PageIndex - 1) * filter.PageSize);

            var leaders = dataLeaderList
                .Select(async X => new UserSideTeamModel
                {
                    EmployeeId = X.EmployeeId,
                    EmployeeName = X.EmployeeName,
                    TeamId = X.TeamId,
                    ImageUrl = X.BlobId == Guid.Empty ? null : await this.FileService.GetFileAsync(X.BlobId.ToString(), X.Mime),
                    PositionName =X.PositionName
                })
                .Select(Q=>Q.Result)
                .Skip((int)skipCount)
                .Take(filter.PageSize)
                .ToList();

            var responseCoaches = new UserSideTeamResponseModel
            {
                ListTeam = leaders,
                TotalData = totalData
            };

            return responseCoaches;
        }

        /// <summary>
        /// Update team rotate / team details data based on requested team ID & teams.
        /// </summary>
        /// <param name="teamId">The requested team details data teamId</param>
        /// <param name="teams"></param>
        /// <returns></returns>
        public async Task RotateTeam(int teamId, List<UserSideTeamFormModel> teams)
        {
            // get all selected & rotated employeeIds
            var employeeIds = teams
                .Where(Q => Q.IsChecked == true || Q.IsRotated == true)
                .Select(Q => Q.EmployeeId)
                .ToList();

            var haveTeams = await this
                .DB
                .TeamDetails
                .AsNoTracking()
                .Where(Q => employeeIds.Contains(Q.EmployeeId))
                .ToListAsync();

            // if dont have team insert
            var notHaveTeam = employeeIds.Except(haveTeams.Select(Q => Q.EmployeeId));

            if (notHaveTeam.Count() > 0)
            {
                // insert
                var newTeam = new List<TeamDetails>();
                foreach (var employeeId in notHaveTeam)
                {
                    newTeam.Add(new TeamDetails
                    {
                        EmployeeId = employeeId,
                        TeamId = teamId
                    });
                }
                this.DB.TeamDetails.AddRange(newTeam);
            }

            // delete from team
            var deleteFormTeams = teams
                .Where(Q => Q.IsChecked == false && Q.IsRotated == true)
                .Select(Q => Q.EmployeeId)
                .ToList();

            if (deleteFormTeams.Count() > 0)
            {
                var deletedEmployees = haveTeams
                    .Where(Q => deleteFormTeams.Contains(Q.EmployeeId))
                    .ToList();

                haveTeams = haveTeams
                    .Where(Q => !deleteFormTeams.Contains(Q.EmployeeId))
                    .ToList();

                // delete
                this.DB.RemoveRange(deletedEmployees);
            }

            // if have teams && not deleted will updated
            if (haveTeams.Count() > 0)
            {
                // update
                foreach (var employeeTeam in haveTeams)
                {
                    if (teamId != employeeTeam.TeamId)
                    {
                        employeeTeam.TeamId = teamId;
                    }
                }

                this.DB.TeamDetails.UpdateRange(haveTeams);
            }

            await this.DB.SaveChangesAsync();
        }

        /// <summary>
        /// Delete my team data.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task DeleteMyTeam(string employeeId)
        {
            var myTeamData = await this
                .DB
                .TeamDetails
                .Where(Q => Q.EmployeeId.Contains(employeeId))
                .FirstOrDefaultAsync();

            this.DB.Remove(myTeamData);
            await this.DB.SaveChangesAsync();
        }

        /// <summary>
        /// Submit rotate new member.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="nextTeamId"></param>
        /// <param name="newRotate"></param>
        /// <returns></returns>
        public async Task CreateRotateAsync(string employeeId, int nextTeamId, MobileTalentJWTModel responseToken)
        {
            // Set the fixed date & time value for this current method.
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var oldDataTeam = await this
                .DB
                .TeamDetails
                .Where(Q => Q.EmployeeId.Contains(employeeId))
                .FirstOrDefaultAsync();

            var dataRotate = new RotateTeamMembers
            {
                EmployeeId = oldDataTeam.EmployeeId,
                PreviousTeamId = oldDataTeam.TeamId,
                NextTeamId = nextTeamId,
                ApprovalStatusId = Constant.ApprovalStatus.WaitingForApproval,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                CreatedBy = responseToken.Unique_name
            };


            this.DB.Add(dataRotate);
            await this.DB.SaveChangesAsync();

            var nextTeamLeaderId = await this.DB.Teams.Where(Q => Q.TeamId == dataRotate.NextTeamId).Select(Q => Q.TeamLeaderId).FirstOrDefaultAsync();
            await this.UserSideInboxService.InsertRotateRequestInbox(nextTeamLeaderId, responseToken.EmployeeId, dataRotate.RotateTeamMemberId);
        }
    }
}