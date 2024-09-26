using MimeKit;
using MailKit.Net.Smtp; // Make sure you're using MailKit's SmtpClient, not System.Net.Mail
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Threading.Tasks;

namespace RazorPagesPizza.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender() { }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return SendEmailImplementationAsync(email, subject, htmlMessage);
        }

        private async Task SendEmailImplementationAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Your Name", "youremail@gmail.com"));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;

                // Create the body of the email
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlMessage
                };
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    // Connect to the SMTP server
                    await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                    // Authenticate with the SMTP server
                    await client.AuthenticateAsync("youremail@gmail.com", "password");

                    // Send the email
                    await client.SendAsync(message);

                    // Disconnect from the SMTP server
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw; // Optionally rethrow the exception or handle it appropriately
            }
        }
    }
}
