using Dapper;
using Microsoft.Win32;
using ModelLayer.EmailModel;
using ModelLayer.LoginModel;
using RepositoryLayer.Contexts;
using RepositoryLayer.Entities;
using RepositoryLayer.GlobalExceptions;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class LoginServiceRL : ILoginRL
    {
        private readonly DapperContext _context;
        private readonly IAuthorizationRL _authService;
        private readonly IEmail _email;

        public LoginServiceRL(DapperContext context, IAuthorizationRL authService, IEmail email)
        {
            _context = context;
            _authService = authService;
            _email = email;
        }

        public async Task<string> LoginUser(LoginUserModel model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Email", model.Email);

            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QueryFirstAsync<User>("spGetUserByEmail", parameters, commandType: CommandType.StoredProcedure);

                if (user == null)
                {
                    throw new UserNotFoundException($"User with email '{model.Email}' not found.");
                }

                if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    throw new InvalidPasswordFormateException($"Wrong Password");
                }

                var token = _authService.GenerateJwtToken(user);
                return token;
            }
        }

        public async Task<bool> UpdatePassword(UpdatePasswordModel model)
        {
            if (model == null) { throw new ArgumentNullException("Fields can't be null!"); }
            if (model.Email == null) new InvalidEmailFormateException("Email Field should not be empty");
            if (model.CurrentPassword == null) throw new InvalidPasswordException($"Current Password is Invalid");
            if (model.NewPassword == null || !IsPasswordValid(model.NewPassword)) throw new InvalidPasswordFormateException("Invalid Password formate!"); ;

            using (var connection = _context.CreateConnection())
            {
                var currentPasswordFromDatabase = connection.QueryFirstOrDefault<string>("spGetPasswordForEmail", new { model.Email }, commandType: CommandType.StoredProcedure);
                if (currentPasswordFromDatabase == null)
                {
                    throw new InvalidEmailException($"Entered email {model.Email} don't exists");
                }

                var isPasswordCurrentPasswordMatching = BCrypt.Net.BCrypt.Verify(model.CurrentPassword, currentPasswordFromDatabase);
                if (!isPasswordCurrentPasswordMatching)
                {
                    throw new InvalidPasswordException($"Current Password is Invalid");
                }

                var Parameter = new DynamicParameters();

                Parameter.Add("Email", model.Email, DbType.String);
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                Parameter.Add("NewPassword", hashPassword, DbType.String);
                await connection.ExecuteAsync("spUpdatePassword", Parameter, commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public async Task<bool> ForgotPassword(ForgetPasswordModel model)
        {
            if (model != null)
            {
                if (model.Email == null) { throw new InvalidEmailException("Email is required!"); }

                using var connection = _context.CreateConnection();
                var emailExists = connection.QueryFirstOrDefault<bool>("spToCheckIfEmailIsDuplicate", new { model.Email }, commandType: CommandType.StoredProcedure);
                if (!emailExists)
                {
                    throw new InvalidEmailException("Email Dosn't exists, Register first!");
                }

                string otp = GenerateOtp();
                int getIdOfUser = connection.QueryFirstOrDefault<int>("spGetUserIdForEmail", new { model.Email }, commandType: CommandType.StoredProcedure);

                connection.Execute("spUpdateResetPasswordOtp", new { UserId = getIdOfUser, Otp = otp, Expiry = DateTime.UtcNow.AddMinutes(15) }, commandType: CommandType.StoredProcedure);

                return await _email.SendOtpToEmail(new SendOtpToEmailModel { Email = model.Email, Otp = otp }, "Password Reset for Your Account", "OTP to reset your password: ");
            }
            return false;
        }

        public async Task<bool> ResetPassword(ResetPasswordModel model)
        {
            if (model == null) { throw new ArgumentNullException("Data required!"); }
            if (model.Email == null) { return false; }
            if (model.Password == null || !IsPasswordValid(model.Password)) { throw new InvalidPasswordFormateException("Invalid Password formate!"); }
            if (model.Otp == null) { throw new InvalidOtpException("Invalid OTP!"); }

            using var connection = _context.CreateConnection();
            var userInfo = connection.QueryFirstOrDefault("spGetUserInfoByEmail", new { model.Email }, commandType: CommandType.StoredProcedure);

            if (userInfo == null) throw new UserNotFoundException("Invalid email");

            var getOtp = userInfo.ResetPasswordOtp.Equals(model.Otp);

            if (!getOtp)
            {
                throw new InvalidOtpException("Invalid OTP!");
            }

            var expiryDate = userInfo.ResetPasswordOtpExpiry;
            if (expiryDate < DateTime.UtcNow)
            {
                throw new OtpExpiredException("Token has expired!");
            }
 
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            await connection.ExecuteAsync("spUpdatePasswordUsingEmail", new { model.Email, NewPassword = hashPassword }, commandType: CommandType.StoredProcedure);

            return true;
        }

        private static bool IsPasswordValid(string? password)
        {
            var pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%^&*])(?=.*[0-9]).{8,20}$";
            if (password == null) return false;
            return Regex.IsMatch(password, pattern);
        }

        // To geterate the Otp 
        private static string GenerateOtp()
        {
            const int otpLength = 6;
            var otpBuilder = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < otpLength; i++)
            {
                otpBuilder.Append(random.Next(0, 10));
            }

            return otpBuilder.ToString();
        }
    }
}
