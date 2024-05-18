using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.RegisterModel;
using ModelLayer.ResponseModel;
using RepositoryLayer.Interfaces;

namespace FundooNotes.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterationBL _registeration;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(IRegisterationBL registeration, ILogger<RegisterController> logger)
        {
            _registeration = registeration;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserModel userModel)
        {
            try
            {
                await _registeration.UserRegister(userModel);
                _logger.LogInformation("User Registration Successful");

                var response = new ResponseModel
                {
                    Success = true,
                    Message = "User Registration Successful!",
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Registration unsuccessful");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"Null Data, {ex.Message}"
                };
                return Ok(response);
            }
        }
    }
}
