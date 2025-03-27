using System.Security.Authentication;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using PizzaShop.Service.Interface;

namespace PizzaShop.Service.Implementaion;

public class EmailSender : IEmailSender
{
    public async Task<bool> SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            Console.WriteLine($"Starting to send email to {email} using System.Net.Mail");
            
            using (var client = new System.Net.Mail.SmtpClient("mail.etatvasoft.com", 587))
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("test.dotnet@etatvasoft.com", "P}N^{z-]7Ilp");
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.Timeout = 60000; // 60 seconds
                
                var mailMessage = new System.Net.Mail.MailMessage
                {
                    From = new System.Net.Mail.MailAddress("test.dotnet@etatvasoft.com"),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };
                
                mailMessage.To.Add(email);
                
                Console.WriteLine("Mail message created, sending...");
                await client.SendMailAsync(mailMessage);
                Console.WriteLine("Email sent successfully");
                
                
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return false;
        }
    }
}



    