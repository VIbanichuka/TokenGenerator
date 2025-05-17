using MailKit.Net.Smtp;
using MimeKit;
using TokenGenerator.Application.Dtos;
using TokenGenerator.Application.Interfaces.IServices;

namespace TokenGenerator.Infrastructure.Services
{
    public class MailKitEmailService : IEmailService
    {
        public async Task SendEmailAsync(EmailMessage message)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(message.From ?? "INSERT YOUR EMAIL"));
            email.To.Add(MailboxAddress.Parse(message.To));
            email.Subject = message.Subject;

            email.Body = new TextPart("html")
            {
                Text = message.Body
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("INSERT YOUR EMAIL", "INSERT YOUR APP PASSWORD");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
