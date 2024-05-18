using Azure;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.LoginModel;
using ModelLayer.ResponseModel;
using System.ComponentModel.DataAnnotations;

namespace FundooNotes.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginBL _login;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILoginBL login, ILogger<LoginController> logger)
        {
            _login = login;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLogin([Required] LoginUserModel model)
        {
            try
            {
                var token = await _login.LoginUser(model);
                _logger.LogInformation("User login successful!");

                var response = new ResponseModel <string> 
                {
                    Success = true,
                    Message = "Login Successful!",
                    Data = token.ToString()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Login unsuccessful!");
                var resopnse = new ResponseModel 
                {
                    Success = false,
                    Message = $"Login unsuccessful. {ex.Message}"
                };
                return Ok(resopnse);
            }
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([Required] ForgetPasswordModel model)
        {
            try
            {
                await _login.ForgotPassword(model);
                _logger.LogInformation("Forgot password!");
                var response = new ResponseModel
                {
                    Success = true,
                    Message = "OTP sent successfully!"
                };
                return Ok(response);
            } catch (Exception ex)
            {
                _logger.LogError($"Some error occured!");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"OTP sending Failed. {ex.Message}"
                };
                return Ok(response);
            }
        }

        [HttpPatch("resetpassword")]
        public async Task<IActionResult> RestPassword([Required] ResetPasswordModel model)
        {
            try
            {
                await _login.ResetPassword(model);
                _logger.LogInformation("Password reste successful!");
                var response = new ResponseModel
                {
                    Success = true,
                    Message = "Password reset successful!"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Reset pssword unsuccessful");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
                return Ok(response);
            }
        }

        [HttpPatch("updatepassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordModel model)
        {
            try
            {
                await _login.UpdatePassword(model);
                _logger.LogInformation("Password update successful!");
                var response = new ResponseModel
                {
                    Success = true,
                    Message = "Password update successful!"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Update pssword unsuccessful");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"Update password unsuccessful. {ex.Message}"
                };
                return Ok(response);
            }
        }
    }
}
    

