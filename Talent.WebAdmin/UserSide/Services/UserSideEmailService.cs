using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Serilog;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Interfaces;
using Talent.WebAdmin.UserSide.Models;
using Talent.WebAdmin.UserSide.Models.Configurations;

namespace Talent.WebAdmin.UserSide.Services
{
    public class UserSideEmailService : IEmailService
    {
        private readonly SmtpConfiguration SmtpConfiguration;

        public UserSideEmailService(IOptions<SmtpConfiguration> options)
        {
            this.SmtpConfiguration = options.Value;
        }

        /// <summary>
        /// Send email using MailKit APIs.
        /// </summary>
        /// <param name="mailContent"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(UserSideSendMailModel mailContent)
        {
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(this.SmtpConfiguration.SenderName, this.SmtpConfiguration.Username));

            foreach (var to in mailContent.Tos)
            {
                mimeMessage.To.Add(new MailboxAddress(to));
            }

            foreach (var cc in mailContent.Ccs)
            {
                mimeMessage.Cc.Add(new MailboxAddress(cc));
            }

            foreach (var bcc in mailContent.Bccs)
            {
                mimeMessage.Bcc.Add(new MailboxAddress(bcc));
            }

            mimeMessage.Subject = mailContent.Subject;

            mimeMessage.Body = new TextPart("html")
            {
                Text = mailContent.Message
            };

            using (var client = new SmtpClient())
            {
                // TODO: Testing purpose only, remove it on production.
                // Or just go along with it....
                Log.Warning("Ignoring certificates....");

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                Log.Information($"Sending mail with these following SMTP configurations: Host={this.SmtpConfiguration.Host}, Port={this.SmtpConfiguration.Port}, IsSsl={this.SmtpConfiguration.IsSsl}, Username={this.SmtpConfiguration.Username}");

                await client.ConnectAsync(this.SmtpConfiguration.Host, this.SmtpConfiguration.Port, this.SmtpConfiguration.IsSsl);

                if (string.IsNullOrEmpty(this.SmtpConfiguration.Password) == false && SmtpConfiguration.Password != "null")
                {
                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(this.SmtpConfiguration.Username, this.SmtpConfiguration.Password);
                }
                foreach (var to in mimeMessage.To)
                {
                    Log.Information($"Sending mail to: {to.ToString()}");
                }

                await client.SendAsync(mimeMessage);

                foreach (var to in mimeMessage.To)
                {
                    Log.Information($"Success sending mail to: {to.ToString()}");
                }

                await client.DisconnectAsync(true);
            }
        }
    }
}