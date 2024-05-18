using ModelLayer.LoginModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ILoginBL
    {
        public Task<string> LoginUser(LoginUserModel model);
        public Task<bool> UpdatePassword(UpdatePasswordModel model);
        public Task<bool> ForgotPassword(ForgetPasswordModel model);
        public Task<bool> ResetPassword(ResetPasswordModel model);
    }
}
