using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.LabelModel;
using ModelLayer.ResponseModel;
using RepositoryLayer.Entities;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL _label;
        private readonly ILogger<LabelController> _logger;

        public LabelController(ILabelBL label, ILogger<LabelController> logger)
        {
            _label = label;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("createLabel")]
        public async Task<IActionResult> CreateLabel([Required] LabelCreateModel model)
        {
            try
            {
                var userIdClaim = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(userIdClaim);
                var labelId = await _label.CreateLabel(model, userId);
                _logger.LogInformation("Label Created Successfully!");
                var response = new ResponseModel
                {
                    Success = true,
                    Message = "Label Created Successfully!"
                };
                return Ok(response);
            } catch (Exception ex)
            {
                _logger.LogError("Label Creation unsuccessful!");
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = "Label Creation unsuccessful!",
                    Data = ex.Message
                };
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("getLabels")]
        public async Task<IActionResult> GetAllLabels()
        {
            try
            {
                var userIdClaim = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(userIdClaim);
                var labels = await _label.GetAllLabels(userId);
                _logger.LogInformation("Labels fetched successfully!");
                var response = new ResponseModel<IEnumerable<Label>>
                {
                    Success = true,
                    Message = "Labels fetched successfully!",
                    Data = labels
                };
                return Ok(response);
            } catch (Exception ex)
            {
                _logger.LogError($"Some error occured!. {ex.Message}");
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = "Some error occured!",
                    Data = ex.Message
                };
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("getLabel/{labelId}")]
        public async Task<IActionResult> GetAllLabelById([Required] int labelId)
        {
            try
            {
                var userIdClaim = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(userIdClaim);
                var labels = await _label.GetLabel(userId, labelId);
                _logger.LogInformation("Label fetched successfully!");
                var response = new ResponseModel<Label>
                {
                    Success = true,
                    Message = "Label fetched successfully!",
                    Data = labels
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Some error occured!. {ex.Message}");
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = "Some error occured!",
                    Data = ex.Message
                };
                return Ok(response);
            }
        }

        [Authorize]
        [HttpDelete("deleteLabel/{labelId}")]
        public async Task<IActionResult> DeleteLabel([Required] int labelId)
        {
            try
            {
                var labels = await _label.DeleteLabel(labelId);
                _logger.LogInformation("Label deleted successfully!");
                var response = new ResponseModel
                {
                    Success = true,
                    Message = "Label deleted successfully!",
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Some error occured!. {ex.Message}");
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = "Some error occured!",
                    Data = ex.Message
                };
                return Ok(response);
            }
        }
    }
}
