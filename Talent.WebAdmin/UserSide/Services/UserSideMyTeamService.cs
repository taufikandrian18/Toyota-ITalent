using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideMyTeamService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService fileMan;
        private readonly UserSideInboxService usersideInboxMan;

        public UserSideMyTeamService(TalentContext context, IFileStorageService fileService, UserSideInboxService userSideInbox)
        {
            this.DB = context;
            this.fileMan = fileService;
            this.usersideInboxMan = userSideInbox;
        }

        public async Task<bool> ValidateAddEmployee(string employeeId)
        {
            var isTeamMember = await this.DB.TeamDetails.AnyAsync(Q => Q.EmployeeId == employeeId);

            var isTeamLeader = await this.DB.Teams.AnyAsync(Q => Q.TeamLeaderId == employeeId);

            if (!isTeamLeader && isTeamMember)
            {
                return false;
            }

            return true;
        }

        public async Task<List<UserSideMyTeamEmployeeModel>> GetAllTeamEmployeeAsync(UserSideMyTeamEmployeeFilterModel filter, string currentEmployeeId)
        {

            var currentEmployeeDealerId = await (from e in this.DB.Employees
                                                 join o in this.DB.Outlets on e.OutletId equals o.OutletId into lo
                                                 from o in lo.DefaultIfEmpty()
                                                 join d in this.DB.Dealers on o.DealerId equals d.DealerId into ld
                                                 from d in ld.DefaultIfEmpty()
                                                 where e.EmployeeId == currentEmployeeId
                                                 select d.DealerId).FirstOrDefaultAsync();

            var queryNonTeamMember = from e in DB.Employees

                                     join b in DB.Blobs on e.BlobId equals b.BlobId into bNull
                                     from b in bNull.DefaultIfEmpty()

                                     join o in DB.Outlets on e.OutletId equals o.OutletId into lo
                                     from o in lo.DefaultIfEmpty()

                                     join d in DB.Dealers on o.DealerId equals d.DealerId into ld
                                     from d in ld.DefaultIfEmpty()

                                     join epm in DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                                     join p in DB.Positions on epm.PositionId equals p.PositionId

                                     join td in DB.TeamDetails on e.EmployeeId equals td.EmployeeId into tdNUll
                                     from td in tdNUll.DefaultIfEmpty()
                                     join tmr in DB.TeamMemberRequests on e.EmployeeId equals tmr.EmployeeId into tmrNull
                                     from tmr in tmrNull.DefaultIfEmpty()
                                     where td == null && e.IsTeamLeader == false && (tmr == null || tmr.ApprovalStatusId != ApprovalStatusesEnum.WaitingId) && e.IsDeleted == false
                                     select new
                                     {
                                         e.EmployeeId,
                                         e.EmployeeName,
                                         e.BlobId,
                                         b.Mime,
                                         o.OutletId,
                                         p.PositionId,
                                         p.PositionName,
                                         d.DealerId
                                     };

            var queryMemberRequest = from e in DB.Employees

                                     join b in DB.Blobs on e.BlobId equals b.BlobId into bNull
                                     from b in bNull.DefaultIfEmpty()

                                     join o in DB.Outlets on e.OutletId equals o.OutletId into lo
                                     from o in lo.DefaultIfEmpty()

                                     join d in DB.Dealers on o.DealerId equals d.DealerId into ld
                                     from d in ld.DefaultIfEmpty()

                                     join epm in DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                                     join p in DB.Positions on epm.PositionId equals p.PositionId

                                     join tmr in DB.TeamMemberRequests on e.EmployeeId equals tmr.EmployeeId
                                     join t in DB.Teams on tmr.TeamId equals t.TeamId
                                     where e.IsTeamLeader == false && t.TeamLeaderId == currentEmployeeId
                                     && e.IsDeleted == false && tmr.ApprovalStatusId == ApprovalStatusesEnum.WaitingId
                                     select new
                                     {
                                         e.EmployeeId,
                                         e.EmployeeName,
                                         e.BlobId,
                                         b.Mime,
                                         o.OutletId,
                                         p.PositionId,
                                         p.PositionName,
                                         tmr.ApprovalStatusId,
                                         d.DealerId
                                     };

            if (currentEmployeeDealerId != null)
            {
                queryNonTeamMember = queryNonTeamMember.Where(Q => Q.DealerId == currentEmployeeDealerId);
                queryMemberRequest = queryMemberRequest.Where(Q => Q.DealerId == currentEmployeeDealerId);
            }
            else
            {
                queryNonTeamMember = queryNonTeamMember.Where(Q => Q.OutletId == null);
                queryMemberRequest = queryMemberRequest.Where(Q => Q.OutletId == null);
            }

            if (filter.OutletId != null && filter.OutletId.FirstOrDefault() != null)
            {
                queryNonTeamMember = queryNonTeamMember.Where(Q => filter.OutletId.Contains(Q.OutletId));
                queryMemberRequest = queryMemberRequest.Where(Q => filter.OutletId.Contains(Q.OutletId));
            }

            if (filter.PositionId != null && filter.PositionId.FirstOrDefault() != null)
            {
                queryNonTeamMember = queryNonTeamMember.Where(Q => filter.PositionId.Contains(Q.PositionId.ToString()));
                queryMemberRequest = queryMemberRequest.Where(Q => filter.PositionId.Contains(Q.PositionId.ToString()));
            }

            if (!String.IsNullOrEmpty(filter.EmployeeName))
            {
                queryNonTeamMember = queryNonTeamMember.Where(Q => Q.EmployeeName.Contains(filter.EmployeeName));
                queryMemberRequest = queryMemberRequest.Where(Q => Q.EmployeeName.Contains(filter.EmployeeName));
            }

            var dataNonTeamMember = await queryNonTeamMember.Where(Q => Q.PositionName != "*").Select(Q => new UserSideMyTeamEmployeeModel
            {
                //BlobImageUrl = GenerateImageUrl(Q.BlobId, Q.Mime).Result,
                EmployeeId = Q.EmployeeId,
                EmployeeName = Q.EmployeeName,
                PositionName = Q.PositionName,
                IsWaitingApproval = false,
                BlobId = Q.BlobId != null ? Q.BlobId.ToString() : null,
                Mime = Q.Mime
            }).ToListAsync();

            var dataMemberRequest = await queryMemberRequest.Where(Q => Q.PositionName != "*").Select(Q => new UserSideMyTeamEmployeeModel
            {
                //BlobImageUrl = GenerateImageUrl(Q.BlobId, Q.Mime).Result,
                EmployeeId = Q.EmployeeId,
                EmployeeName = Q.EmployeeName,
                PositionName = Q.PositionName,
                IsWaitingApproval = Q.ApprovalStatusId == ApprovalStatusesEnum.WaitingId ? true : false,
                BlobId = Q.BlobId != null ? Q.BlobId.ToString() : null,
                Mime = Q.Mime
            }).ToListAsync();

            dataMemberRequest.AddRange(dataNonTeamMember);

            //var allData = dataMemberRequest.Where(Q => Q.PositionName != "*").ToList();
            var allData = dataMemberRequest;

            allData = allData
                .GroupBy(Q => Q.EmployeeId)
                .Select(Q => Q.First())
                .OrderBy(Q => Q.EmployeeName)
                .ToList();

            if (filter.PageIndex != null && filter.PageSize != null)
            {
                var skipCount = Math.Ceiling((decimal)(filter.PageIndex.GetValueOrDefault() - 1) * filter.PageSize.GetValueOrDefault());

                allData = allData.Skip((int)skipCount).Take(filter.PageSize.GetValueOrDefault()).ToList();


                foreach (var item in allData)
                {
                    if (item.BlobId != null)
                    {
                        item.BlobImageUrl = await this.fileMan.GetFileAsync(item.BlobId, item.Mime);
                    }
                }
            }

            return allData;
        }

        //private async Task<string> GenerateImageUrl(string BlobId, string BlobMime)
        //{
        //    if (BlobId == null)
        //    {
        //        return null;
        //    }

        //    return await this.fileMan.GetFileAsync(BlobId.ToString(), BlobMime);
        //}

        public async Task RequestNewMemberAsync(string leaderEmployeeId, string newEmployeeId)
        {
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();
            var isTeamExist = await this.DB.Teams.AsNoTracking().AnyAsync(Q => Q.TeamLeaderId == leaderEmployeeId);
            int teamId = 0;

            // Team is new
            if (!isTeamExist)
            {
                // Add leader of team to team
                var newTeam = new Teams
                {
                    TeamLeaderId = leaderEmployeeId
                };

                this.DB.Teams.Add(newTeam);

                var employee = await this.DB.Employees.Where(Q => Q.EmployeeId == leaderEmployeeId).FirstOrDefaultAsync();

                employee.IsTeamLeader = true;

                teamId = newTeam.TeamId;

                // Add leader of team to team detail
                var newTeamDetail = new TeamDetails
                {
                    TeamId = teamId,
                    EmployeeId = leaderEmployeeId,
                };

                this.DB.TeamDetails.Add(newTeamDetail);
            }

            else
            {
                teamId = await this.DB.Teams.Where(Q => Q.TeamLeaderId == leaderEmployeeId).Select(Q => Q.TeamId).FirstOrDefaultAsync();
            }

            var IsRequestAlreadyExisted = await DB.TeamMemberRequests
                .Where(Q => Q.TeamId == teamId && Q.EmployeeId == newEmployeeId && Q.ApprovalStatusId == ApprovalStatusesEnum.WaitingId)
                .AnyAsync();
            if (IsRequestAlreadyExisted)
            {
                return;
            }

            var newMemberRequest = new TeamMemberRequests
            {
                EmployeeId = newEmployeeId,
                TeamId = teamId,
                ApprovalStatusId = ApprovalStatusesEnum.WaitingId,
                CreatedAt = thisDate,
                UpdatedAt = thisDate,
                CreatedBy = leaderEmployeeId
            };

            this.DB.TeamMemberRequests.Add(newMemberRequest);

            await usersideInboxMan.InsertTeamMemberRequestInbox(newEmployeeId, leaderEmployeeId, newMemberRequest.TeamMemberRequestId);
        }
    }
}
