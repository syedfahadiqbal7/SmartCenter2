using AFFZ_API.Interfaces;
using AFFZ_API.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AFFZ_API
{
    public class EmailNotifications : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailNotifications(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        //public async Task SendEmail(string to, string subject, string body)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
        //    message.To.Add(new MailboxAddress("", to));  // Empty string for name if not available
        //    message.Subject = subject;

        //    var bodyBuilder = new BodyBuilder
        //    {
        //        HtmlBody = body
        //    };
        //    message.Body = bodyBuilder.ToMessageBody();

        //    using (var client = new SmtpClient())
        //    {
        //        try
        //        {
        //            await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, _smtpSettings.EnableSsl);
        //            await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
        //            await client.SendAsync(message);
        //        }
        //        finally
        //        {
        //            await client.DisconnectAsync(true);
        //            client.Dispose();
        //        }
        //    }
        //}
        public async Task<bool> SendEmail(string to, string subject, string body, string toname)
        {
            try
            {
                //MimeMessage - a class from Mimekit
                MimeMessage email_Message = new MimeMessage();
                MailboxAddress email_From = new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail);
                email_Message.From.Add(email_From);
                MailboxAddress email_To = new MailboxAddress(toname, to);
                email_Message.To.Add(email_To);
                email_Message.Subject = subject;
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = body;
                email_Message.Body = emailBodyBuilder.ToMessageBody();
                //this is the SmtpClient class from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                SmtpClient MailClient = new SmtpClient();
                MailClient.Connect(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);
                MailClient.Authenticate(_smtpSettings.SenderEmail, _smtpSettings.Password);
                MailClient.Send(email_Message);
                MailClient.Disconnect(true);
                MailClient.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }

    }

}
