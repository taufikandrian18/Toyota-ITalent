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

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideTrainingProcessService
    {

        private readonly TalentContext DB;
        private readonly FileService FileMan;

        public UserSideTrainingProcessService(TalentContext db, FileService fl)
        {
            this.DB = db;
            this.FileMan = fl;
        }

        /// <summary>
        /// Get list training inivitation for current user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<UserSideTrainingProcessViewModel>> GetTrainingNotifListAsync(string user, UserSideTrainingProcessFilterModel filter)
        {

            if (string.IsNullOrEmpty(filter.Name))
            {
                filter.Name = "";
            }
            else
            {
                filter.Name = filter.Name.ToLower();
            }

            if (string.IsNullOrEmpty(filter.Type))
            {
                filter.Type = "all";
            }

            var invitationQuery = (from invite in DB.TrainingInvitations

                                       // join tmmTab in DB.TrainingModuleMappings
                                       //on invite.TrainingId equals tmmTab.TrainingId
                                       //into tmmJoin
                                       // from tmm in tmmJoin

                                   join pro in DB.TrainingProcesses on invite.TrainingInvitationId equals pro.TrainingInvitationId into proj
                                   from process in proj.DefaultIfEmpty()

                                       //join eltimes in DB.EnrollLearningTimes
                                       //on invite.Training.TrainingId equals eltimes.EnrollLearning.TrainingId
                                       //into eltj
                                       //from eltt in eltj.DefaultIfEmpty()

                                   join eltab in DB.EnrollLearnings on new { TrainingId = invite.TrainingId, EmployeeId = invite.EmployeeId.ToLower() } equals new { TrainingId = eltab.TrainingId.Value, EmployeeId = eltab.EmployeeId.ToLower() } into eljoin
                                   from el in eljoin.DefaultIfEmpty()

                                   where
                                   invite.EmployeeId == user
                                   select new
                                   {
                                       invite.TrainingId,
                                       TrainingInvId = invite.TrainingInvitationId,
                                       TrainingName = invite.Training.Course.CourseName,
                                       ProgramType = invite.Training.Course.ProgramType.ProgramTypeName,
                                       invite.Training.Batch,
                                       InvitationApprovalStatus = invite.ApprovalStatusId,
                                       //TrainingStartDate = tmm.TrainingStart != null? tmm.TrainingStart : null,
                                       //TrainingEndDate = tmm.TrainingEnd != null ? tmm.TrainingEnd : null,
                                       TrainingStartDate = invite.Training.StartDate != null ? invite.Training.StartDate : null,
                                       TrainingEndDate = invite.Training.EndDate != null ? invite.Training.EndDate : null,
                                       //EnrollStartDate = eltt.StartTime,
                                       //EnrollEndDate = eltt.EndTime,
                                       EnrollDate = (el != null ? (DateTime?)el.CreatedAt : null),
                                       IsDelete = invite.Training.IsDeleted,
                                       IsRescheluded = invite.Training.RescheduledAt != null,
                                       IsJoined = el != null ? el.IsJoined : false,
                                       Process = process == null ? null : process,
                                       Blob = invite.Training.Course.Blob != null ? invite.Training.Course.Blob : null,
                                       BlobId = invite.Training.Course.BlobId != null ? invite.Training.Course.BlobId.ToString() : "",
                                       IsAccommodate = invite.Training.IsAccommodate
                                       //Status = invite.ApprovalStatus == null ? null : invite.ApprovalStatus
                                   });

            if (string.IsNullOrEmpty(filter.Name) == false)
            {
                invitationQuery = invitationQuery.Where(Q => Q.TrainingName.ToLower().Contains(filter.Name));
            }

            var data = await invitationQuery
                .Select(Q => new UserSideTrainingProcessViewModel
                {
                    ImageUrl = Q.BlobId,
                    TrainingInvId = Q.TrainingInvId,
                    TrainingName = Q.TrainingName,
                    ProgramType = Q.ProgramType,
                    Batch = Q.Batch,
                    TrainingStartDate = Q.TrainingStartDate.HasValue ? Q.TrainingStartDate : null,
                    TrainingEndDate = Q.TrainingEndDate.HasValue ? Q.TrainingEndDate : null,
                    //EnrollStartDate = Q.EnrollStartDate.HasValue ? Q.EnrollStartDate.ToString() : "-",
                    //EnrollEndDate = Q.EnrollEndDate.HasValue ? Q.EnrollEndDate.ToString() : "-",
                    EnrollDate = Q.EnrollDate,
                    TrainingStatus = GenerateTrainingStatus(Q.IsDelete, Q.TrainingStartDate, Q.TrainingEndDate, Q.Process, Q.IsJoined, Q.InvitationApprovalStatus),
                    IsRescheluded = Q.IsRescheluded,
                    IsConfirmed = GetIsConfirmed(Q.IsDelete, Q.Process, Q.IsJoined, "confirm"),
                    IsRejected = GetIsConfirmed(Q.IsDelete, Q.Process, Q.IsJoined, "reject"),
                    IsAccommodate = Q.IsAccommodate.GetValueOrDefault()
                })
                .AsNoTracking()
                .ToListAsync();

            if (filter.Type.ToLower() == TrainingInvitationNotifTypeEnum.Announcement)
            {
                //data = data.Where(Q => Q.TrainingStatus.ToLower().Contains("waiting")).ToList();
                data = data.Where(Q => Q.TrainingStatus == TrainingStatusEnum.WatingConfirmation).ToList();
            }
            else
            if (filter.Type.ToLower() == TrainingInvitationNotifTypeEnum.Training)
            {
                //data = data.Where(Q => Q.TrainingStatus.ToLower().Contains("closed") || Q.TrainingStatus.ToLower().Contains("confirmed")).ToList();
                data = data.Where(Q => Q.TrainingStatus == TrainingStatusEnum.Close || Q.TrainingStatus == TrainingStatusEnum.Confirm).ToList();
            }

            var isExist = data.Count();
            if (isExist > 0)
            {
                if (filter.IsOrderedByDateAscending == true)
                {
                    data = data.OrderByDescending(Q => Q.TrainingStatus).ThenBy(Q => Q.TrainingStartDate).ToList();
                }
                else
                {
                    data = data.OrderByDescending(Q => Q.TrainingStatus).ThenByDescending(Q => Q.TrainingStartDate).ToList();
                }
            }

            var dataGroupList = data
                            .GroupBy(Q => Q.TrainingInvId)
                            .Select(Q => Q.First())
                            .ToList();


            var dataList = dataGroupList
                           .Skip((filter.Page - 1) * filter.PageSize)
                           .Take(filter.PageSize)
                           .ToList();
            //play safe
            foreach (var notifData in dataList)
            {
                if (string.IsNullOrEmpty(notifData.ImageUrl) == false)
                {
                    var file = await FileMan.GetFileDetailAsync(notifData.ImageUrl);
                    notifData.ImageUrl = file.FileUrl;
                }
            }

            return dataList;
        }

        /// <summary>
        /// Generate training status sesuai dengan ketentuan yang diminta.
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="endDate"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        private string GenerateTrainingStatus(bool isDeleted, DateTime? startDate, DateTime? endDate, TrainingProcesses process, bool isJoined, int inviteApprovalStatusId)
        {
            if (isDeleted)
            {
                return TrainingStatusEnum.Close;
            }

            var timeNow = DateTime.UtcNow.ToIndonesiaTimeZone();
            var threeDaysAgo = timeNow.AddDays(-3);

            //if (startDate != null)
            //{
            //    if (threeDaysAgo > startDate)
            //    {
            //        return TrainingStatusEnum.Close;
            //    }
            //}

            if (endDate != null)
            {
                if (endDate == null)
                {
                    return TrainingStatusEnum.Close;
                }
                else if (timeNow > endDate)
                {
                    return TrainingStatusEnum.Close;
                }
            }

            if (inviteApprovalStatusId == 4)
            {
                return TrainingStatusEnum.Close;
            }

            if (process == null)
            {
                return TrainingStatusEnum.WatingConfirmation;
            }

            if (isJoined == false)
            {
                return TrainingStatusEnum.WaitingApproval;
            }

            return TrainingStatusEnum.Confirm;
        }

        /// <summary>
        /// Get time for training.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        private async Task<(TimeSpan? start, TimeSpan? end)> GetTimeTrainingAsync(int trainingId)
        {
            if (trainingId == 0)
            {
                return (null, null);
            }
            var trainingQuery = await DB.TrainingModuleMappings
                .Where(Q => Q.TrainingId == trainingId)
                .Select(Q => new
                {
                    startTime = Q.TrainingStart != null ? (TimeSpan?)Q.TrainingStart.Value.TimeOfDay : null,
                    endTime = Q.TrainingEnd != null ? (TimeSpan?)Q.TrainingEnd.Value.TimeOfDay : null,

                })
                .AsNoTracking()
                .ToListAsync();

            if (trainingQuery.Count < 0)
            {
                return (null, null);
            }
            else
            {
                var getStartTime = trainingQuery.Where(Q => Q.startTime != null).Min(Q => Q.startTime);
                var getEndTime = trainingQuery.Where(Q => Q.endTime != null).Max(Q => Q.endTime);
                return (getStartTime, getEndTime);
            }

        }

        /// <summary>
        /// Generate bool for isConfirm dan isReject buat front end (muncul sign di UI) by it's type.
        /// </summary>
        /// <param name="process">data training process</param>
        /// <param name="type">for isConfirm or isReject</param>
        /// <returns></returns>
        private bool GetIsConfirmed(bool isDeleted, TrainingProcesses process, bool isJoined, string type)
        {
            if (process == null)
            {
                return false;
            }

            if (type.ToLower() == "confirm")
            {
                if (isDeleted)
                {
                    return false;
                }

                if (isJoined)
                {
                    return true;
                }
                return false;

                // return process.IsConfirmed;
            }
            else
            {
                if (isDeleted)
                {
                    return true;
                }
                return false;
                // return !process.IsConfirmed;
            }
        }

        /// <summary>
        /// Get detail training invitation by id.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="trainingInvId"></param>
        /// <returns></returns>
        public async Task<UserSideTrainingProcessDetailModel> GetDetailTrainingProcessAsync(string user, int trainingInvId)
        {
            if (trainingInvId <= 0)
            {
                return null;
            }

            var query = (from invite in DB.TrainingInvitations
                         join pro in DB.TrainingProcesses on invite.TrainingInvitationId equals pro.TrainingInvitationId into proj
                         from process in proj.DefaultIfEmpty()
                         join el in DB.EnrollLearnings on new { TrainingId = invite.TrainingId, EmployeeId = invite.EmployeeId.ToLower() } equals new { TrainingId = el.TrainingId.Value, EmployeeId = el.EmployeeId.ToLower() } into lel
                         from el in lel.DefaultIfEmpty()

                             //join tmmTab in DB.TrainingModuleMappings
                             //on invite.TrainingId equals tmmTab.TrainingId
                             //into tmmJoin
                             //from tmm in tmmJoin

                         where
                         invite.EmployeeId == user &&
                         invite.TrainingInvitationId == trainingInvId

                         select new
                         {
                             TrainingId = invite.TrainingId,
                             TrainingInvId = invite.TrainingInvitationId,
                             TrainingName = invite.Training.Course.CourseName,
                             Batch = invite.Training.Batch,
                             InvitationApprovalStatusId = invite.ApprovalStatusId,

                             TrainingStartDate = invite.Training.StartDate,
                             TrainingEndDate = invite.Training.EndDate,
                             //TrainingStartDate = tmm.TrainingStart != null ? tmm.TrainingStart : null,
                             //TrainingEndDate = tmm.TrainingEnd != null ? tmm.TrainingEnd : null,

                             Location = invite.Training.Location,
                             Price = invite.Training.Course.CoursePrice,

                             IsDelete = invite.Training.IsDeleted,
                             IsRescheluded = invite.Training.RescheduledAt != null,
                             Process = process == null ? null : process,

                             IsJoined = el != null ? el.IsJoined : false

                         });
            // cara v.1.0
            //var queryAccomodation = await DB.Accommodations.Select(Q => new UserSideAcomodationListModel{
            //    Id = Q.AccommodationId,
            //    Name = Q.Name,
            //    Price = Q.Price,
            //    Each = 1,
            //})
            //.AsNoTracking()
            //.ToListAsync();

            //cara v.1.1
            var queryAccomodation = await GetAccomodationListAsync();

            var getUserData = await DB.EmployeePositionMappings.Where(Q => Q.EmployeeId == user).Select(Q => new
            {
                UserName = Q.Employee.EmployeeName,
                Position = Q.Position.PositionName
            }).FirstOrDefaultAsync();

            if (getUserData == null)
            {
                return null;
            }

            var data = await query.Select(Q => new UserSideTrainingProcessDetailModel
            {
                TrainingInvId = Q.TrainingInvId,
                TrainingId = Q.TrainingId,
                TrainingName = Q.TrainingName,
                Batch = Q.Batch,

                TrainingStartDate = Q.TrainingStartDate.HasValue ? Q.TrainingStartDate : null,
                TrainingEndDate = Q.TrainingEndDate.HasValue ? Q.TrainingEndDate : null,
                //TrainingStartTime = Q.TrainingStartDate.HasValue ? Q.TrainingStartDate.Value.TimeOfDay : TimeSpan.Zero,
                //TrainingEndTime = Q.TrainingEndDate.HasValue ? Q.TrainingEndDate.Value.TimeOfDay : TimeSpan.Zero,
                TrainingStatus = GenerateTrainingStatus(Q.IsDelete, Q.TrainingStartDate, Q.TrainingEndDate, Q.Process, Q.IsJoined, Q.InvitationApprovalStatusId),

                EmployeeName = getUserData.UserName,
                EmployeePosition = getUserData.Position,

                Location = Q.Location,
                Price = (decimal)(Q.Price != null ? Q.Price : 0),
                AccomodationList = queryAccomodation,

                IsRescheluded = Q.IsRescheluded,
                IsConfirmed = GetIsConfirmed(Q.IsDelete, Q.Process, Q.IsJoined, "confirm"),
                IsRejected = GetIsConfirmed(Q.IsDelete, Q.Process, Q.IsJoined, "reject"),

            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

            var getTrainingTime = await GetTimeTrainingAsync(data.TrainingId);

            if (data.TrainingStartDate != null)
            {
                if (getTrainingTime.start != null)
                {
                    var date = data.TrainingStartDate.Value.Date + getTrainingTime.start;
                    data.TrainingStartDate = date;
                }
            }

            if (data.TrainingEndDate != null)
            {
                if (getTrainingTime.end != null)
                {
                    var date = data.TrainingEndDate.Value.Date + getTrainingTime.end;
                    data.TrainingEndDate = date;
                }
            }

            if (data.TrainingStatus.ToLower().Equals(TrainingStatusEnum.Confirm.ToLower()))
            {
                var trainingProses = await query.Select(Q => Q.Process).FirstOrDefaultAsync();

                if (trainingProses != null)
                {
                    data.NeedAccomodation = trainingProses.AccomodationId != null ? true : false;
                    data.SelectedAccomodation = trainingProses.AccomodationId != null ? trainingProses.AccomodationId.Value : 0;
                    data.StayStartDate = trainingProses.DateofStayStart != null ? trainingProses.DateofStayStart : null;
                    data.StayEndDate = trainingProses.DateofStayEnd != null ? trainingProses.DateofStayEnd : null;
                }
                else
                {
                    data.NeedAccomodation = false;
                    data.SelectedAccomodation = 0;
                    data.StayStartDate = null;
                    data.StayEndDate = null;
                }
            }

            return data;
        }

        /// <summary>
        /// Get List Accomodation dari table accomodation.
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserSideAcomodationListModel>> GetAccomodationListAsync()
        {
            var getAccomodation = await DB.Accommodations.Select(Q => new UserSideAcomodationListModel
            {
                Id = Q.AccommodationId,
                Name = Q.Name,
                Price = Q.Price,
                Each = 1,
            })
            .AsNoTracking()
            .ToListAsync();

            return getAccomodation;
        }

        /// <summary>
        /// Set confirmation buat training invitation lalu lanjut ke training process.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public async Task<string> SetTrainingConfirmationAsync(string user, UserSideTrainingProcessConfirmModel answer)
        {
            var isExist = await DB.TrainingProcesses.Where(Q => Q.TrainingInvitationId == answer.TrainingInvId).FirstOrDefaultAsync();

            if (isExist != null)
            {
                return ResponseMessageEnum.AlreadyExist;
            }

            var invData = await DB.TrainingInvitations.Where(Q => Q.TrainingInvitationId == answer.TrainingInvId).FirstOrDefaultAsync();
            var approvalStatusList = await DB.ApprovalStatus.ToListAsync();

            if (invData == null)
            {
                return ResponseMessageEnum.FailedSave;
            }

            var data = new TrainingProcesses
            {
                TrainingInvitationId = answer.TrainingInvId,

                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                UpdatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                CreatedBy = user,
                UpdatedBy = user,
            };


            if (answer.Answer.ToLower() == "yes")
            {
                var statusId = approvalStatusList.Where(Q => Q.ApprovalName == ApprovalStatusesEnum.Approve).Select(Q => Q.ApprovalStatusId).FirstOrDefault();
                invData.ApprovalStatusId = statusId;
                data.IsConfirmed = true;
            }
            else if (answer.Answer.ToLower() == "no")
            {
                var enrollLearnings = await this.DB.EnrollLearnings.Where(Q => Q.EmployeeId.ToLower() == user.ToLower() && Q.TrainingId == invData.TrainingId).FirstOrDefaultAsync();

                var statusId = approvalStatusList.Where(Q => Q.ApprovalName == ApprovalStatusesEnum.Rejected).Select(Q => Q.ApprovalStatusId).FirstOrDefault();
                invData.ApprovalStatusId = statusId;
                data.IsConfirmed = false;
                enrollLearnings.IsDrafted = false;
            }

            if (answer.NeedAccomodation == true)
            {
                if (answer.Accomodation != null)
                {
                    data.AccomodationId = answer.Accomodation.Id;
                    data.DateofStayStart = answer.Accomodation.StartTime;
                    data.DateofStayEnd = answer.Accomodation.EndTime;
                }
            }

            DB.TrainingInvitations.Update(invData);
            DB.TrainingProcesses.Add(data);

            await DB.SaveChangesAsync();
            try
            {
            }
            catch (Exception e)
            {
                return ResponseMessageEnum.FailedBaseString + "Join to Training";
            }

            if (answer.Answer.ToLower() == "yes")
            {
                return ResponseMessageEnum.SuccessBaseString + "Save your Confirmation to join this Training";
            }
            else
            {
                return ResponseMessageEnum.SuccessBaseString + "Save your Confirmation to not join this Training";
            }
        }

        /// <summary>
        /// Change all training process into training invitation again.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        public async Task ReChangeTrainingInviteAsync(int trainingId)
        {
            var approvalStatusList = await DB.ApprovalStatus.ToListAsync();
            var listData = await DB.TrainingProcesses.Where(Q => Q.TrainingInvitation.TrainingId == trainingId).Select(Q => new
            {
                invite = Q.TrainingInvitation,
                process = Q
            }).ToListAsync();

            var listProcessData = listData.Select(Q => Q.process).ToList();
            var listInvData = listData.Select(Q => Q.invite).ToList();

            DB.TrainingProcesses.RemoveRange(listProcessData);

            List<TrainingInvitations> list = new List<TrainingInvitations>();

            foreach (var data in listInvData)
            {
                var statusID = approvalStatusList.Where(Q => Q.ApprovalName == ApprovalStatusesEnum.Waiting).Select(Q => Q.ApprovalStatusId).FirstOrDefault();
                data.ApprovalStatusId = statusID;
                list.Add(data);
            }

            DB.TrainingInvitations.UpdateRange(list);

            await DB.SaveChangesAsync();
        }

    }
}
