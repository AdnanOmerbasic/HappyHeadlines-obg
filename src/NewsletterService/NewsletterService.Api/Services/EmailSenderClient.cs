using MailKit.Net.Smtp;
using MimeKit;
using NewsletterService.Application.Interfaces;

namespace NewsletterService.Api.Services
{
    public class EmailSenderClient : IEmailSenderClient
    {
        private readonly IConfiguration _config;
        public EmailSenderClient(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendWelcomeEmailAsync(string email, CancellationToken ct)
        {
            var message = new MimeMessage();
            var messageFrom = new MailboxAddress(_config["Smtp:FromName"], _config["Smtp:FromEmail"]);
            message.From.Add(messageFrom);
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Welcome to happyheadlines newsletter!";
            message.Body = new TextPart("plain")
            {
                Text = "Thank you for subscribing to happyheadlines newsletter!"
            };

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_config["Smtp:Host"]!, int.Parse(_config["Smtp:Port"]!), false, ct);
            await smtpClient.SendAsync(message, ct);
            await smtpClient.DisconnectAsync(true, ct);
        }

    }
}
