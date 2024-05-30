using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
namespace DecorWeb.API.Helper
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {


            try
            {
                var client = new SmtpClient("live.smtp.mailtrap.io", 587)
                {
                    Credentials = new NetworkCredential("api", "0bfa0cc556f3675ba2381f28fe4f9cb6"),
                    EnableSsl = true
                };
                client.Send("mailtrap@demomailtrap.com", email, subject, htmlMessage);
                Console.WriteLine("Sent");
            }
            catch (Exception ex )
            {

                Console.WriteLine(ex.Message);
            }
           
        }
    }
}
