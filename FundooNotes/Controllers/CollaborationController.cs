using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.CollaborationModel;
using ModelLayer.ResponseModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CollaborationController : ControllerBase
    {
        private readonly ICollaborationBL _collaboration;
        private readonly ILogger<CollaborationController> _logger;

        public CollaborationController(ICollaborationBL collaboration, ILogger<CollaborationController> logger)
        {
            _collaboration = collaboration;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("addCollaboration")]
        public async Task<IActionResult> AddCollaboration([Required] CreateCollaborationModel model)
        {
            try
            {
                var userIdClaim = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(userIdClaim);
                await _collaboration.AddCollaboration(userId, model);
                _logger.LogInformation("Collaboration successful");
                var response = new ResponseModel
                {
                    Success = true,
                    Message = "Collaboration successful"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unsuccessful collaboration");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"Collaboration unsuccessful {ex.Message}"
                };
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("getCollaboration")]
        public async Task<IActionResult> GetCollaborations()
        {
            try
            {
                var collaborations = await _collaboration.GetAllCollaborations();
                _logger.LogInformation("Getting all collaborations");
                var response = new ResponseModel<IEnumerable<CollaborationResponseModel>>
                {
                    Success = true,
                    Message = "Collaborations fetched successfully",
                    Data = collaborations
                };
                return Ok(response);
            } catch (Exception ex)
            {
                _logger.LogError("Unexpeced error while getting colleboration");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"Unexpeced error while getting colleboration. {ex.Message}"
                };
                return Ok(response);
            }
        }

        [Authorize]
        [HttpDelete("deleteCollaboration/{collaborationId}")]
        public async Task<IActionResult> DeleteCollaboration([Required] int collaborationId)
        {
            try
            {
                await _collaboration.RemoveCollaboration(collaborationId);
                _logger.LogInformation("Collaboration deleted successfully");
                var response = new ResponseModel
                {
                    Success = true,
                    Message = "Collaboration deleted successfully"
                };
                return Ok(response);
            } catch (Exception ex)
            {
                _logger.LogError("Collaboration deleted unsuccessfully");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"Collaboration deleted unsuccessfully. {ex.Message}"
                };
                return Ok(response);
            }
        }
    }
}
