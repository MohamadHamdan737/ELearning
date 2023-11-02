using System.Net.Mail;
using System.Net;
using System;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace ELearning.UI.SendEmails
{
    public class SendEmail
    {
        public readonly IConfiguration _configuration;

        public SendEmail(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool EmailSend(string SenderEmail, string Subject, string Message, bool IsBodyHtml = false)
        {
            bool status = false;

            try
            {
                string HostAddress = _configuration["AppSettings:Host"];
                string FormEmailId = _configuration["AppSettings:MailFrom"];
                string Password = _configuration["AppSettings:Password"];
                string Port = _configuration["AppSettings:Port"];

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(FormEmailId),
                    Subject = Subject,
                    Body = Message,
                    IsBodyHtml = IsBodyHtml
                };

                mailMessage.To.Add(new MailAddress(SenderEmail));

                  NetworkCredential networkCredential = new NetworkCredential
                {
                    UserName = mailMessage.From.Address,
                    Password = Password
                };

                SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                {
                    Host = HostAddress,
                    EnableSsl = true,
                    Credentials=networkCredential,
                    Port= 587
                };

              

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = networkCredential;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Port = Convert.ToInt32(Port);

                smtp.Send(mailMessage);
                status = true;

                return status;
            }
            catch (Exception e)
            {
                return status;
            }
        }
    }
}

