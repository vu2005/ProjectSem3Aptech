using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;


namespace CarInsuranceManage.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string toName, string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Car Insurance", "no-reply@carinsurance.com"));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;


            message.Body = new TextPart("plain")
            {
                Text = body
            };


            try
            {
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;


                    await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("insurancecarsore@gmail.com", "bfuv iniw uecz xgyl");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}

