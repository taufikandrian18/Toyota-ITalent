using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Models;

namespace Talent.WebAdmin.UserSide.Interfaces
{
    /// <summary>
    /// Interface definition for email service.
    /// </summary>
    public interface IEmailService
    {
        Task SendEmailAsync(UserSideSendMailModel mailContent);
    }
}
