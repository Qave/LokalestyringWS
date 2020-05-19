using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using LokalestyringUWP.Handler;

namespace LokalestyringUWP.Service
{
    public class MailService
    {
        public static async void MailSender(string userMail, string subject, string body, bool isBodyHtml)
        {
            await SendMail(userMail, subject, body, isBodyHtml);
        }

        private static async Task SendMail(string userMail, string subject, string body, bool isBodyHtml)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("derp43434@gmail.com");
                mail.To.Add(userMail);
                mail.IsBodyHtml = isBodyHtml;
                mail.Headers.Add("Importance", "high");
                mail.Subject = subject;
                mail.Body = body;

                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential("derp43434", "Derp-1234");
                smtpServer.EnableSsl = true;

                await smtpServer.SendMailAsync(mail);
            }
            catch (Exception)
            {
            }
        }
    }
}
