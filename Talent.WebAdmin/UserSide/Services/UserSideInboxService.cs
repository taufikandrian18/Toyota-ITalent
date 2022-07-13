using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.Services;
using Talent.WebAdmin.UserSide.Enums;
using Talent.WebAdmin.UserSide.Models;
using TAM.Talent.Commons.Core.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    /// <summary>
    /// Service class for providing various methods for interacting with inbox data.
    /// </summary>
    public class UserSideInboxService
    {
        private readonly TalentContext DB;
        private readonly IFileStorageService FileService;

        public UserSideInboxService(TalentContext talentContext, IFileStorageService fileService)
        {
            this.DB = talentContext;
            this.FileService = fileService;
        }

        /// <summary>
        /// Get all message for spesific employee.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<List<UserSideInboxModel>> GetInboxesByEmployeeId(string employeeId, int itemPerPage, int pageIndex, string search)
        {
            var inboxes = (from e in this.DB.Employees.AsNoTracking()
                           join i in this.DB.MobileInboxes.AsNoTracking() on e.EmployeeId equals i.EmployeeId
                           join it in this.DB.MobileInboxTypes.AsNoTracking() on i.MobileInboxTypeId equals it.MobileInboxTypeId
                           join es in this.DB.Employees.AsNoTracking() on i.CreatedBy equals es.EmployeeId into esi
                           from esit in esi.DefaultIfEmpty()
                           where e.EmployeeId == employeeId
                           orderby i.CreatedAt descending
                           select new
                           {
                               employeeId = e.EmployeeId,
                               employeeName = e.EmployeeName,
                               inboxId = i.MobileInboxId,
                               title = i.Title,
                               isRead = i.IsRead,
                               message = i.Message,
                               inboxTypeName = it.Name,
                               inboxTypeId = it.MobileInboxTypeId,
                               senderName = esit.EmployeeName,
                               createdBy = i.CreatedBy,
                               notes = i.Notes
                           });

            if (!string.IsNullOrEmpty(search))
            {
                inboxes = inboxes.Where(Q => (Q.title.ToLower().Contains(search.ToLower())
                            || Q.message.ToLower().Contains(search.ToLower())
                            || Q.senderName.ToLower().Contains(search.ToLower())
                            || Q.inboxTypeName.ToLower().Contains(search.ToLower())
                            || Q.notes.ToLower().Contains(search.ToLower())));
            }

            var result = await inboxes
                .Select(Q => new UserSideInboxModel
                {
                    EmployeeId = Q.employeeId,
                    EmployeeName = Q.employeeName,
                    InboxId = Q.inboxId,
                    Title = Q.title,
                    IsRead = Q.isRead,
                    Message = Q.message,
                    InboxTypeName = Q.inboxTypeName,
                    InboxTypeId = Q.inboxTypeId
                }).Skip((pageIndex - 1) * itemPerPage)
                .Take(itemPerPage)
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// Get detail message based on inbox id.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<UserSideInboxModel> GetDetailInbox(int inboxId)
        {
            // Get inbox data based on inbox id
            var inbox = await this.DB
                .MobileInboxes
                .FirstOrDefaultAsync(Q => Q.MobileInboxId == inboxId);

            if (!inbox.IsRead)
            {
                inbox.IsRead = true;
            }
            await DB.SaveChangesAsync();

            // Get employee Name
            var employeeName = await this.DB
                .Employees
                .Where(Q => Q.EmployeeId == inbox.EmployeeId)
                .Select(Q => Q.EmployeeName)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            // get inbox type name
            var typeName = await this.DB
                .MobileInboxTypes
                .Where(Q => Q.MobileInboxTypeId == inbox.MobileInboxTypeId)
                .Select(Q => Q.Name)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            var senderInformation = await (from e in this.DB.Employees.AsNoTracking()
                                           join b in this.DB.Blobs.AsNoTracking() on e.BlobId equals b.BlobId into ba
                                           from b in ba.DefaultIfEmpty()
                                           where e.EmployeeId == inbox.CreatedBy
                                           select new
                                           {
                                               name = e.EmployeeName,
                                               blobId = e.BlobId,
                                               mime = b.Mime
                                           }).FirstOrDefaultAsync();

            string senderImageUrl = null;
            if (senderInformation != null && senderInformation.blobId != null)
            {
                senderImageUrl = await FileService.GetFileAsync(senderInformation.blobId.ToString(), senderInformation.mime);
            }

            // Insert detail of inbox to a variable.
            var inboxDetail = new UserSideInboxModel
            {
                EmployeeId = inbox.EmployeeId,
                EmployeeName = employeeName,
                InboxId = inbox.MobileInboxId,
                InboxTypeId = inbox.MobileInboxTypeId,
                InboxTypeName = typeName,
                Notes = inbox.Notes,
                Message = inbox.Message,
                Title = inbox.Title,
                IsRead = inbox.IsRead,
                EmployeeResign = null,
                RotateTeamMemberRequest = null,
                FromName = senderInformation != null ? senderInformation.name : "SYSTEM",
                FromImageUrl = senderImageUrl,
                CreatedAt = inbox.CreatedAt
            };

            if (inbox.MobileInboxTypeId == (int)MobileInboxType.Resign && !string.IsNullOrEmpty(inbox.ResignEmployeeId))
            {
                inboxDetail.EmployeeResign = await GetEmployeeResign(inbox.ResignEmployeeId);
            }

            else if ((inbox.MobileInboxTypeId == (int)MobileInboxType.TeamMemberRotationRequest ||
                inbox.MobileInboxTypeId == (int)MobileInboxType.RotationAccepted ||
                inbox.MobileInboxTypeId == (int)MobileInboxType.RotationRejected) &&
                inbox.RotateTeamMemberId.HasValue == true)
            {
                inboxDetail.RotateTeamMemberRequest = await GetTeamMemberRotationRequest(inbox.RotateTeamMemberId.Value);
            }

            else if (inbox.MobileInboxTypeId == (int)MobileInboxType.TeamMemberRequest && inbox.TeamMemberRequestId.HasValue == true)
            {
                inboxDetail.TeamMemberRequest = await GetTeamMemberRequest(inbox.TeamMemberRequestId.Value);
            }

            return inboxDetail;
        }

        /// <summary>
        /// Get data team member request
        /// </summary>
        /// <param name="teamMemberRequestId"></param>
        /// <returns></returns>
        public async Task<UserSideTeamMemberRequestModel> GetTeamMemberRequest(int teamMemberRequestId)
        {
            var teamMemberRequestTemp = await (from rtm in this.DB.TeamMemberRequests.AsNoTracking()
                                               join e in this.DB.Employees.AsNoTracking() on rtm.EmployeeId equals e.EmployeeId
                                               join t in this.DB.Teams.AsNoTracking() on rtm.TeamId equals t.TeamId
                                               join a in this.DB.ApprovalStatus.AsNoTracking() on rtm.ApprovalStatusId equals a.ApprovalStatusId
                                               join b in this.DB.Blobs.AsNoTracking() on e.BlobId equals b.BlobId into ba
                                               from b in ba.DefaultIfEmpty()
                                               where rtm.TeamMemberRequestId == teamMemberRequestId
                                               select new
                                               {
                                                   rtm.TeamMemberRequestId,
                                                   rtm.EmployeeId,
                                                   rtm.TeamId,
                                                   rtm.ApprovalStatusId,
                                                   e.EmployeeName,
                                                   e.BlobId,
                                                   b.Mime,
                                                   a.ApprovalName,
                                                   t.TeamLeaderId,
                                                   rtm.CreatedBy
                                               }).FirstOrDefaultAsync();

            if (teamMemberRequestTemp == null)
            {
                return null;
            }

            string imageUrl = null;
            if (teamMemberRequestTemp.BlobId != null)
            {
                imageUrl = await FileService.GetFileAsync(teamMemberRequestTemp.BlobId.ToString(), teamMemberRequestTemp.Mime);
            }

            //TAM People doesn't have Outlet ID
            var HasOutlet = await (from o in DB.Outlets
                                   join e in DB.Employees on o.OutletId equals e.OutletId
                                   where e.EmployeeId == teamMemberRequestTemp.EmployeeId
                                   select o.OutletId).FirstOrDefaultAsync();

            
            if (HasOutlet == null) 
            {
                var getEmployeeInformation = await (from e in DB.Employees
                                                    join epm in DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                                                    join p in DB.Positions on epm.PositionId equals p.PositionId
                                                    where e.EmployeeId == teamMemberRequestTemp.CreatedBy
                                                    select new
                                                    {
                                                        name = e.EmployeeName,
                                                        positionId = epm.PositionId,
                                                        positionName = p.PositionName,
                                                        outletName = "Toyota Astra Motor"
                                                    }).OrderByDescending(Q => Q.positionId)
                                            .FirstOrDefaultAsync();
                var teamMemberRequest = new UserSideTeamMemberRequestModel
                {
                    TeamMemberRequestId = teamMemberRequestTemp.TeamMemberRequestId,
                    TeamId = teamMemberRequestTemp.TeamId,
                    EmployeeId = teamMemberRequestTemp.EmployeeId,
                    EmployeeName = teamMemberRequestTemp.EmployeeName,
                    ApprovalStatus = teamMemberRequestTemp.ApprovalName,
                    ImageUrl = imageUrl,
                    TeamLeaderId = teamMemberRequestTemp.TeamLeaderId,
                    TeamLeaderName = getEmployeeInformation.name,
                    PositionName = getEmployeeInformation.positionName,
                    OutletName = getEmployeeInformation.outletName
                };


                return teamMemberRequest;
            }
            else 
            { 
                var getEmployeeInformation = await (from e in DB.Employees
                                                join epm in DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                                                join p in DB.Positions on epm.PositionId equals p.PositionId
                                                join o in DB.Outlets on e.OutletId equals o.OutletId
                                                where e.EmployeeId == teamMemberRequestTemp.CreatedBy
                                                select new
                                                {
                                                    name = e.EmployeeName,
                                                    positionId = epm.PositionId,
                                                    positionName = p.PositionName,
                                                    outletName = o.Name
                                                }).OrderByDescending(Q => Q.positionId)
                                             .FirstOrDefaultAsync();
                var teamMemberRequest = new UserSideTeamMemberRequestModel
                {
                    TeamMemberRequestId = teamMemberRequestTemp.TeamMemberRequestId,
                    TeamId = teamMemberRequestTemp.TeamId,
                    EmployeeId = teamMemberRequestTemp.EmployeeId,
                    EmployeeName = teamMemberRequestTemp.EmployeeName,
                    ApprovalStatus = teamMemberRequestTemp.ApprovalName,
                    ImageUrl = imageUrl,
                    TeamLeaderId = teamMemberRequestTemp.TeamLeaderId,
                    TeamLeaderName = getEmployeeInformation.name,
                    PositionName = getEmployeeInformation.positionName,
                    OutletName = getEmployeeInformation.outletName
                };

                return teamMemberRequest;
            }
           
        }

        /// <summary>
        /// Get data request of team member rotation
        /// </summary>
        /// <param name="rotateTeamMemberId"></param>
        /// <returns></returns>
        public async Task<UserSideTeamMemberRotationRequestModel> GetTeamMemberRotationRequest(int rotateTeamMemberId)
        {
            var rotateTeamMemberRequestTemp = await (from rtm in this.DB.RotateTeamMembers.AsNoTracking()
                                                     join e in this.DB.Employees.AsNoTracking() on rtm.EmployeeId equals e.EmployeeId
                                                     join a in this.DB.ApprovalStatus.AsNoTracking() on rtm.ApprovalStatusId equals a.ApprovalStatusId
                                                     join b in this.DB.Blobs.AsNoTracking() on e.BlobId equals b.BlobId into ba
                                                     from b in ba.DefaultIfEmpty()
                                                     where rtm.RotateTeamMemberId == rotateTeamMemberId
                                                     select new
                                                     {
                                                         rtm.RotateTeamMemberId,
                                                         rtm.EmployeeId,
                                                         e.EmployeeName,
                                                         rtm.PreviousTeamId,
                                                         rtm.NextTeamId,
                                                         rtm.ApprovalStatusId,
                                                         a.ApprovalName,
                                                         e.BlobId,
                                                         b.Mime
                                                     }).FirstOrDefaultAsync();

            if (rotateTeamMemberRequestTemp == null)
            {
                return null;
            }

            string imageUrl = null;
            if (rotateTeamMemberRequestTemp.BlobId != null)
            {
                imageUrl = await FileService.GetFileAsync(rotateTeamMemberRequestTemp.BlobId.ToString(), rotateTeamMemberRequestTemp.Mime);
            }

            // Get previous team and previous team leader
            var previousTeam = await (from t in this.DB.Teams.AsNoTracking()
                                      join e in this.DB.Employees.AsNoTracking() on t.TeamLeaderId equals e.EmployeeId
                                      join o in this.DB.Outlets.AsNoTracking() on e.OutletId equals o.OutletId
                                      where t.TeamId == rotateTeamMemberRequestTemp.PreviousTeamId
                                      select new
                                      {
                                          t.TeamId,
                                          t.TeamLeaderId,
                                          teamLeaderName = e.EmployeeName,
                                          outletName = o.Name
                                      }).FirstOrDefaultAsync();

            // Get next team and next team leader
            var nextTeam = await (from t in this.DB.Teams.AsNoTracking()
                                  join e in this.DB.Employees.AsNoTracking() on t.TeamLeaderId equals e.EmployeeId
                                  where t.TeamId == rotateTeamMemberRequestTemp.NextTeamId
                                  select new
                                  {
                                      t.TeamId,
                                      t.TeamLeaderId,
                                      teamLeaderName = e.EmployeeName
                                  }).FirstOrDefaultAsync();

            var rotateTeamMemberRequest = new UserSideTeamMemberRotationRequestModel
            {
                RotateTeamMemberId = rotateTeamMemberRequestTemp.RotateTeamMemberId,
                EmployeeId = rotateTeamMemberRequestTemp.EmployeeId,
                EmployeeName = rotateTeamMemberRequestTemp.EmployeeName,
                PreviousTeamId = previousTeam.TeamId,
                PreviousTeamLeaderId = previousTeam.TeamLeaderId,
                PreviousTeamLeaderName = previousTeam.teamLeaderName,
                PreviousTeamLeaderOutletName = previousTeam.outletName,
                NextTeamId = nextTeam.TeamId,
                NextTeamLeaderId = nextTeam.TeamLeaderId,
                NextTeamLeaderName = nextTeam.teamLeaderName,
                ApprovalStatus = rotateTeamMemberRequestTemp.ApprovalName,
                ImageUrl = imageUrl
            };

            return rotateTeamMemberRequest;
        }

        /// <summary>
        /// Get data detail of employee resign.
        /// </summary>
        /// <param name="employeeIdResign"></param>
        /// <returns></returns>
        public async Task<UserSideEmployeeResignModel> GetEmployeeResign(string employeeIdResign)
        {
            var employeeResignTemp = await (from e in this.DB.Employees.AsNoTracking()
                                            join o in this.DB.Outlets.AsNoTracking() on e.OutletId equals o.OutletId into oa
                                            from oal in oa.DefaultIfEmpty()
                                            join epm in this.DB.EmployeePositionMappings.AsNoTracking() on e.EmployeeId equals epm.EmployeeId
                                            join p in this.DB.Positions.AsNoTracking() on epm.PositionId equals p.PositionId
                                            join td in this.DB.TeamDetails.AsNoTracking() on e.EmployeeId equals td.EmployeeId
                                            join t in this.DB.Teams.AsNoTracking() on td.TeamId equals t.TeamId
                                            where e.EmployeeId.ToLower() == employeeIdResign.ToLower()
                                            select new
                                            {
                                                e.EmployeeId,
                                                e.EmployeeName,
                                                e.BlobId,
                                                p.PositionName,
                                                outletName = oal.Name,
                                                teamLeaderId = t.TeamLeaderId
                                            })
                                       .FirstOrDefaultAsync();

            if (employeeResignTemp == null)
            {
                return null;
            }

            // Get team leader name.
            var teamLeaderName = await this.DB
                .Employees
                .Where(Q => Q.EmployeeId.ToLower() == employeeResignTemp.teamLeaderId.ToLower())
                .Select(Q => Q.EmployeeName)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            var employeeResign = new UserSideEmployeeResignModel
            {
                EmployeeId = employeeResignTemp.EmployeeId,
                Name = employeeResignTemp.EmployeeName,
                OutletName = employeeResignTemp.outletName,
                PositionName = employeeResignTemp.PositionName,
                TeamLeaderName = teamLeaderName,
                ImageUrl = null
            };

            // Get image url of employee resign
            if (employeeResignTemp.BlobId != null)
            {
                var blob = await this.DB
                    .Blobs
                    .Where(Q => Q.BlobId == employeeResignTemp.BlobId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                employeeResign.ImageUrl = await FileService.GetFileAsync(blob.BlobId.ToString(), blob.Mime);
            }

            return employeeResign;
        }

        /// <summary>
        /// Update status request rotation.(1 = Approved, 4 = Rejected)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateStatusRequestRotation(UserSideUpdateStatusRequestRotationModel model)
        {
            var rotateTeamMember = await this.DB
                .RotateTeamMembers
                .Where(Q => Q.RotateTeamMemberId == model.RotateTeamMemberId)
                .FirstOrDefaultAsync();

            var nextTeamLeaderId = await DB.Teams
                .Where(Q => Q.TeamId == rotateTeamMember.NextTeamId)
                .Select(Q => Q.TeamLeaderId)
                .FirstOrDefaultAsync();

            if (model.Status == ApprovalStatusesEnum.RejectedId)
            {
                // Update table rotate team member.
                rotateTeamMember.ApprovalStatusId = ApprovalStatusesEnum.RejectedId;
                rotateTeamMember.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();

                await InsertRotateRejectedInbox(rotateTeamMember.EmployeeId, nextTeamLeaderId, rotateTeamMember.CreatedBy, model.RotateTeamMemberId);

                await this.DB.SaveChangesAsync();
            }
            else if (model.Status == ApprovalStatusesEnum.ApproveId)
            {
                // Update table rotate team member
                rotateTeamMember.ApprovalStatusId = ApprovalStatusesEnum.ApproveId;
                rotateTeamMember.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();

                // Update table team detail
                var teamDetail = await this.DB
                    .TeamDetails
                    .Where(Q => Q.EmployeeId.ToLower() == rotateTeamMember.EmployeeId && Q.TeamId == rotateTeamMember.PreviousTeamId)
                    .FirstOrDefaultAsync();

                teamDetail.TeamId = rotateTeamMember.NextTeamId;
                await this.DB.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Update status request team member.(1 = Approved, 4 = Rejected)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateStatusRequestTeamMember(UserSideUpdateStatusRequestTeamMemberModel model)
        {
            var teamMemberRequest = await this.DB
                .TeamMemberRequests
                .Where(Q => Q.TeamMemberRequestId == model.RequestTeamMemberId)
                .FirstOrDefaultAsync();

            var newMobileInbox = new MobileInboxes
            {
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = teamMemberRequest.EmployeeId,
                EmployeeId = teamMemberRequest.CreatedBy,
                IsRead = false,
                MobileInboxTypeId = (int)MobileInboxType.TeamMemberRequestResult,
                Title = "Team Member Request Result"
            };

            var nameEmployee = await this.DB.Employees.Where(Q => Q.EmployeeId.ToLower() == teamMemberRequest.EmployeeId.ToLower()).Select(Q => Q.EmployeeName).FirstOrDefaultAsync();

            if (model.Status == ApprovalStatusesEnum.RejectedId)
            {
                // Update table request team member.
                teamMemberRequest.ApprovalStatusId = ApprovalStatusesEnum.RejectedId;
                teamMemberRequest.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();

                var messageContent = nameEmployee + " telah menolak permintaan penambahan team member request anda";

                newMobileInbox.Message = messageContent;

                this.DB.MobileInboxes.Add(newMobileInbox);

                await this.DB.SaveChangesAsync();
            }
            else if (model.Status == ApprovalStatusesEnum.ApproveId)
            {
                // Update table request team member
                teamMemberRequest.ApprovalStatusId = ApprovalStatusesEnum.ApproveId;
                teamMemberRequest.UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone();

                var messageContent = nameEmployee + " telah menyetujui permintaan penambahan team member request anda";

                newMobileInbox.Message = messageContent;

                this.DB.MobileInboxes.Add(newMobileInbox);

                // Update table team detail
                var teamDetail = await this.DB
                    .TeamDetails
                    .Where(Q => Q.EmployeeId.ToLower() == teamMemberRequest.EmployeeId)
                    .FirstOrDefaultAsync();

                if (teamDetail != null)
                {
                    teamDetail.TeamId = teamMemberRequest.TeamId;
                    await this.DB.SaveChangesAsync();
                }
                else
                {
                    this.DB
                        .TeamDetails
                        .Add(new TeamDetails
                        {
                            EmployeeId = teamMemberRequest.EmployeeId,
                            TeamId = teamMemberRequest.TeamId
                        });

                    await this.DB.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// insert mobile inbox member request
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="leaderEmployeeId"></param>
        /// <param name="teamMemberRequestId"></param>
        /// <returns></returns>
        public async Task InsertTeamMemberRequestInbox(string employeeId, string leaderEmployeeId, int teamMemberRequestId)
        {
            //get employee name
            var employeeNameReceiver = await this.DB.Employees.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.EmployeeName).FirstOrDefaultAsync();
            var employeeSender = await (from e in DB.Employees
                                        join epm in DB.EmployeePositionMappings on e.EmployeeId equals epm.EmployeeId
                                        join p in DB.Positions on epm.PositionId equals p.PositionId
                                        join o in DB.Outlets on e.OutletId equals o.OutletId into lo
                                        from o in lo.DefaultIfEmpty()
                                        where e.EmployeeId == leaderEmployeeId
                                        select new
                                        {
                                            name = e.EmployeeName,
                                            outletName = o != null ? o.Name : null,
                                            positionName = p.PositionName
                                        }).FirstOrDefaultAsync();
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var newTeamMemberRequest = new MobileInboxes
            {
                EmployeeId = employeeId,
                MobileInboxTypeId = Convert.ToInt32(MobileInboxType.TeamMemberRequest),
                Title = "Team Member Request",
                Message = "Hi " + employeeNameReceiver + ", Saya " + employeeSender.name + ", " + employeeSender.positionName + " dari " + (employeeSender.outletName != null ? employeeSender.outletName : "TAM") + ". Saya ingin menjadikan Anda sebagai team member saya.",
                TeamMemberRequestId = teamMemberRequestId,
                CreatedAt = thisDate,
                CreatedBy = leaderEmployeeId,
                IsRead = false
            };

            this.DB.MobileInboxes.Add(newTeamMemberRequest);
            await this.DB.SaveChangesAsync();
        }

        public async Task InsertRotateRequestInbox(string employeeId, string leaderEmployeeId, int rotateTeamMemberId)
        {
            //get employee name
            var employeeName = await this.DB.Employees.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.EmployeeName).FirstOrDefaultAsync();
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var newTeamMemberRequest = new MobileInboxes
            {
                EmployeeId = employeeId,
                MobileInboxTypeId = Convert.ToInt32(MobileInboxType.TeamMemberRotationRequest),
                Title = "Rotation Member Request",
                Message = "Hi " + employeeName + ", Saya ingin mengajukan rotasi team member.",
                CreatedAt = thisDate,
                CreatedBy = leaderEmployeeId,
                IsRead = false,
                RotateTeamMemberId = rotateTeamMemberId
            };

            this.DB.MobileInboxes.Add(newTeamMemberRequest);
            await this.DB.SaveChangesAsync();
        }

        public async Task InsertRotateRejectedInbox(string employeeId, string nextleaderEmployeeId, string prevleaderEmployeeId, int rotateTeamMemberId)
        {
            //get employee name
            var employeeName = await this.DB.Employees.Where(Q => Q.EmployeeId == employeeId).Select(Q => Q.EmployeeName).FirstOrDefaultAsync();
            var thisDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var getLeaderInformation = await (from e in DB.Employees
                                              join o in DB.Outlets on e.OutletId equals o.OutletId
                                              where e.EmployeeId == nextleaderEmployeeId
                                              select new
                                              {
                                                  name = e.EmployeeName,
                                                  outletName = o.Name
                                              }).FirstOrDefaultAsync();

            var teamMemberRejected = new MobileInboxes
            {
                EmployeeId = prevleaderEmployeeId,
                MobileInboxTypeId = Convert.ToInt32(MobileInboxType.RotationRejected),
                Title = "Rotation Member Request Rejected",
                Message = getLeaderInformation.name + " dari cabang " + getLeaderInformation.outletName + " menolak untuk menambahkan team member " + employeeName + " ke timnya.",
                CreatedAt = thisDate,
                CreatedBy = nextleaderEmployeeId,
                IsRead = false,
                RotateTeamMemberId = rotateTeamMemberId
            };

            this.DB.MobileInboxes.Add(teamMemberRejected);
            //await this.DB.SaveChangesAsync();
        }

        public async Task<bool> CheckUnreadInbox(string userId)
        {
            var getInboxes = await DB.MobileInboxes.Where(Q => Q.EmployeeId == userId && Q.IsRead == false).FirstOrDefaultAsync();

            if (getInboxes == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> InsertInboxForTrainingEdited(int trainingId)
        {
            //send inbox notification
            var getEmployeeIdList = await (from t in DB.Trainings
                                           join el in DB.EnrollLearnings on t.TrainingId equals el.TrainingId
                                           join c in DB.Courses on t.CourseId equals c.CourseId
                                           where el.IsJoined == true && t.TrainingId == trainingId
                                           select new
                                           {
                                               employeeId = el.EmployeeId,
                                               courseName = c.CourseName
                                           }).ToListAsync();

            if (getEmployeeIdList == null) return false;

            var todayDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var newNotification = getEmployeeIdList.Select(Q => new MobileInboxes
            {
                EmployeeId = Q.employeeId,
                MobileInboxTypeId = Convert.ToInt32(MobileInboxType.TrainingInvitation),
                Title = "Training Updated",
                Message = "Training " + Q.courseName + " mengalami beberapa perubahan. Silahkan cek detail di halaman Training Process. Terima Kasih.",
                CreatedAt = todayDate,
                CreatedBy = "SYSTEM",
                IsRead = false
            }).ToList();

            this.DB.MobileInboxes.AddRange(newNotification);

            return true;
        }

        public async Task<bool> InsertInboxForTrainingRemoved(int trainingId)
        {
            //send inbox notification
            var getEmployeeIdList = await (from t in DB.Trainings
                                           join el in DB.EnrollLearnings on t.TrainingId equals el.TrainingId
                                           join c in DB.Courses on t.CourseId equals c.CourseId
                                           where el.IsJoined == true && t.TrainingId == trainingId
                                           select new
                                           {
                                               employeeId = el.EmployeeId,
                                               courseName = c.CourseName
                                           }).ToListAsync();

            if (getEmployeeIdList == null) return false;

            var todayDate = DateTime.UtcNow.ToIndonesiaTimeZone();

            var newNotification = getEmployeeIdList.Select(Q => new MobileInboxes
            {
                EmployeeId = Q.employeeId,
                MobileInboxTypeId = Convert.ToInt32(MobileInboxType.TrainingInvitation),
                Title = "Training Cancelled",
                Message = "Mohon maaf, Training " + Q.courseName + " yang Anda enroll telah dibatalkan. Silahkan cek detail di halaman Training Process. Terima Kasih.",
                CreatedAt = todayDate,
                CreatedBy = "SYSTEM",
                IsRead = false
            }).ToList();

            this.DB.MobileInboxes.AddRange(newNotification);

            return true;
        }
    }
}
