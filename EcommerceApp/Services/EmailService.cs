using System.Net;
using System.Net.Mail;

namespace EcommerceApp.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmailAsync(string ToEmail, string Subject, string Body, bool IsBodyHtml = false)
        {
            // Retrieve the mail server (SMTP host) from the configuration.
            string? MailServer = _configuration["EmailSettings:MailServer"];
            // Retrieve the sender email address from the configuration.
            string? FromEmail = _configuration["EmailSettings:FromEmail"];
            // Retrieve the sender email password from the configuration.
            string? Password = _configuration["EmailSettings:Password"];
            // Retrieve the sender's display name from the configuration.
            string? SenderName = _configuration["EmailSettings:SenderName"];
            // Retrieve the SMTP port number from the configuration and convert it to an integer.
            int Port = Convert.ToInt32(_configuration["EmailSettings:MailPort"]);
            // Create a new instance of SmtpClient using the mail server and port number.
            var client = new SmtpClient(MailServer, Port)
            {
                Credentials = new NetworkCredential(FromEmail, Password),
                // Enable SSL for secure email communication.
                EnableSsl = true,
            };
            MailAddress fromAddress = new MailAddress(FromEmail, SenderName);
            MailMessage mailMessage = new MailMessage
            {
                From = fromAddress,
                Subject = Subject, 
                Body = Body, 
                IsBodyHtml = IsBodyHtml 
            };
            mailMessage.To.Add(ToEmail);
            return client.SendMailAsync(mailMessage);
        }
    }
}
