using ModelLayer.EmailModel;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class EmailService : IEmail
    {
        private readonly EmailSettingModel _emailSetting;

        public EmailService(EmailSettingModel emailSetting)
        {
            _emailSetting = emailSetting;
        }

        public async Task<bool> SendEmail(string to, string subject, string body)
        {
            var mailMessage = new MailMessage();
            var senderEmailID = _emailSetting.Username;
            if (senderEmailID != null)
            {
                mailMessage.From = new MailAddress(senderEmailID, "Fundoo!Notes");
            }
            mailMessage.To.Add(new MailAddress(to));
            mailMessage.Subject = subject;

            string message = body;
            mailMessage.Body = message;

            using (var smtpClient = new SmtpClient(_emailSetting.Server, _emailSetting.Port))
            {
                smtpClient.Credentials = new NetworkCredential(_emailSetting.Username, _emailSetting.Password);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);
            }
            return true;
        }

        public async Task<bool> SendOtpToEmail(SendOtpToEmailModel model, string subject, string body)
        {
            if (model == null) return false;
            if (model.Email == null) return false;
            if (model.Otp == null) return false;

            var mailMessage = new MailMessage();
            var senderEmailID = _emailSetting.Username;
            if (senderEmailID != null)
            {
                mailMessage.From = new MailAddress(senderEmailID, "Fundoo!Notes");
            }
            mailMessage.To.Add(new MailAddress(model.Email));
            mailMessage.Subject = subject; 

            string message = body + $"{model.Otp}"; 
            mailMessage.Body = message;

            using (var smtpClient = new SmtpClient(_emailSetting.Server, _emailSetting.Port))
            {
                smtpClient.Credentials = new NetworkCredential(_emailSetting.Username, _emailSetting.Password);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);
            }
            return true;
        }
    }
}
