using ModelLayer.EmailModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IEmail
    {
        public Task<bool> SendEmail(string to, string subject, string body);
        public Task<bool> SendOtpToEmail(SendOtpToEmailModel model, string subject, string body);
    }
}
