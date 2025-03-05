using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SisAt.Settings;

namespace SisAt.Repository.Persistence
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<bool> SendMailAsync(string name, string mail, string subject, string body)
        {
            try
            {
                using MimeMessage mailMessage = new();

                MailboxAddress mailFrom = new(_mailSettings.SenderName, _mailSettings.SenderEmail);
                mailMessage.From.Add(mailFrom);
                MailboxAddress mailTo = new(name, mail);
                mailMessage.To.Add(mailTo);

                mailMessage.Subject = subject;

                BodyBuilder mailBodyBuilder = new();
                mailBodyBuilder.HtmlBody = body;

                mailMessage.Body = mailBodyBuilder.ToMessageBody();

                using SmtpClient mailClient = new();
                await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                await mailClient.SendAsync(mailMessage);
                await mailClient.DisconnectAsync(true);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
