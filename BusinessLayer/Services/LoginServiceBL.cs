using BusinessLayer.Interfaces;
using ModelLayer.LoginModel;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class LoginServiceBL : ILoginBL
    {
        private readonly ILoginRL _login;

        public LoginServiceBL(ILoginRL login)
        {
            _login = login;
        }

        public async Task<bool> ForgotPassword(ForgetPasswordModel model)
        {
            return await _login.ForgotPassword(model);
        }

        public async Task<string> LoginUser(LoginUserModel loginUserModel)
        {
            return await _login.LoginUser(loginUserModel);
        }

        public async Task<bool> ResetPassword(ResetPasswordModel model)
        {
            return await _login.ResetPassword(model);
        }

        public async Task<bool> UpdatePassword(UpdatePasswordModel model)
        {
            return await _login.UpdatePassword(model);
        }
    }
}
