using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Entities.Entities;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Helpers;
using Talent.WebAdmin.UserSide.Constant;
using Talent.WebAdmin.UserSide.Interfaces;
using Talent.WebAdmin.UserSide.Models;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideScheduleBookingService
    {
        private readonly TalentContext DB;
        private readonly IEmailService EmailService;
        private readonly ITemplatingService TemplatingService;

        public UserSideScheduleBookingService(
            TalentContext talentContext,
            IEmailService emailService,
            ITemplatingService templatingService)
        {
            this.DB = talentContext;
            this.EmailService = emailService;
            this.TemplatingService = templatingService;
        }

        public async Task SendEmailAsync(string subject, List<string> to, UserSideScheduleBookingModel scheduleBooking, string template)
        {
            var message = TemplatingService.Render(template, scheduleBooking);

            var mailData = new UserSideSendMailModel
            {
                Tos = to,
                Subject = subject,
                Message = message
            };

            await this.EmailService.SendEmailAsync(mailData);
        }

        public async Task<bool> BookingScheduleCoach(int id, string message, string employeeId)
        {
            var querySchedule = await (from cs in DB.CoachSchedules
                                       join c in DB.Coaches
                                       on cs.CoachId equals c.CoachId

                                       join e in DB.Employees
                                       on c.EmployeeId equals e.EmployeeId

                                       where cs.CoachScheduleId == id
                                       select new
                                       {
                                           EmployeeId = e.EmployeeId,
                                           CoachEmail = e.EmployeeEmail,
                                           CoachName = e.EmployeeName,
                                           cs.StartDateTime,
                                           cs.EndDateTime,
                                           cs.CoachScheduleId
                                       })
                                       .FirstOrDefaultAsync();

            var alreadyBooked = await this.DB.CoachBookingSchedules.Where(Q => Q.CoachScheduleId == querySchedule.CoachScheduleId && Q.EmployeeId.ToLower() == employeeId.ToLower()).FirstOrDefaultAsync();

            if (querySchedule.EmployeeId.ToLower() == employeeId.ToLower() || alreadyBooked != null)
            {
                return false;
            }

            var queryTemplate = await this
                .DB
                .EmailTemplates
                .Where(Q => Q.EmailTemplateId == Constant.Templates.ScheduleBookingTemplate)
                .Select(Q => new
                {
                    Q.Title,
                    Q.Template
                })
                .FirstOrDefaultAsync();

            var queryParticipant = await this
                .DB
                .Employees
                .Where(Q => Q.EmployeeId == employeeId)
                .Select(Q => new
                {
                    ParticipantName = Q.EmployeeName
                })
                .FirstOrDefaultAsync();

            var data = new UserSideScheduleBookingModel
            {
                CoachName = querySchedule.CoachName,
                RequesterName = queryParticipant.ParticipantName,
                ScheduleDate = querySchedule.StartDateTime.ToString("dd'-'MM'-'yyyy"),
                ScheduleTime = querySchedule.StartDateTime.ToString("HH:mm") + " - " + querySchedule.EndDateTime.ToString("HH:mm"),
                Message = message
            };

            var template = queryTemplate.Template;
            var title = queryTemplate.Title;
            var mailCoach = querySchedule.CoachEmail;

            var mail = new List<string>();
            mail.Add(mailCoach);

            var coachBooked = new CoachBookingSchedules
            {
                CoachScheduleId = id,
                CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone(),
                EmployeeId = employeeId,
            };

            this.DB.CoachBookingSchedules.Add(coachBooked);

            var hasCoachEverBooked = await DB.CoachBookingSchedules.Where(Q => Q.CoachScheduleId == id).FirstOrDefaultAsync();

            if (hasCoachEverBooked == null)
            {
                var getCoachInformation = await (from cs in DB.CoachSchedules
                                                 join c in DB.Coaches on cs.CoachId equals c.CoachId
                                                 join e in DB.Employees on c.EmployeeId equals e.EmployeeId
                                                 where cs.CoachScheduleId == id
                                                 select new
                                                 {
                                                     employee = e,
                                                     coachSchedule = cs
                                                 }).FirstOrDefaultAsync();

                var getScheduleTime = getCoachInformation.coachSchedule.EndDateTime.Subtract(getCoachInformation.coachSchedule.StartDateTime).TotalMinutes;

                var isTimePointExist = await this.DB.TimePoints.Where(Q => Q.PointTypeId == 3 && Q.Time == (int)getScheduleTime).FirstOrDefaultAsync();

                if (isTimePointExist != null)
                {
                    getCoachInformation.employee.TeachingPoint += isTimePointExist.Points;

                    var newPointHistory = new EmployeePointHistories
                    {
                        EmployeeId = getCoachInformation.employee.EmployeeId,
                        RewardPointTypeId = RewardPointTypeEnum.TeachingPointType,
                        PointTransactionTypeId = (int)PointTransactionTypeEnum.Income,
                        Point = isTimePointExist.Points,
                        CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone()
                    };

                    this.DB.EmployeePointHistories.Add(newPointHistory);
                }
                else
                {
                    var newPointHistory = new EmployeePointHistories
                    {
                        EmployeeId = getCoachInformation.employee.EmployeeId,
                        RewardPointTypeId = RewardPointTypeEnum.TeachingPointType,
                        PointTransactionTypeId = (int)PointTransactionTypeEnum.Income,
                        Point = 0,
                        CreatedAt = DateTime.UtcNow.ToIndonesiaTimeZone()
                    };

                    this.DB.EmployeePointHistories.Add(newPointHistory);
                }
            }

            try
            {
                await SendEmailAsync(title, mail, data, template);
            }
            catch
            {
                return false;
            }

            await this.DB.SaveChangesAsync();
            return true;

        }
    }
}
