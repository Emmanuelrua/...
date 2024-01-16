
using API.FINANCE.API.Configuration;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace API.FINANCE.API.Services
{
    public class EmailService : IEmailSender
    {
        private readonly SmtpSettings _smtpsettings;
        public EmailService(IOptions<SmtpSettings> smptsettings)
        {
            _smtpsettings = smptsettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(_smtpsettings.SenderName, _smtpsettings.SenderEmail));
                message.To.Add(new MailboxAddress("",email));
                message.Subject=subject;
                message.Body=new TextPart("html") { Text = htmlMessage};

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpsettings.Server);
                    await client.AuthenticateAsync(_smtpsettings.UserName, _smtpsettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
