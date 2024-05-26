using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.NoteModel;
using ModelLayer.ResponseModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Route("api/")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBL _note;
        private readonly ILogger<NoteController> _logger;

        public NoteController(INoteBL note, ILogger<NoteController> logger)
        {
            _note = note;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("createNote/{labelId}")]
        public async Task<IActionResult> CreateNotes([Required] CreateNoteModel model, int labelId)
        {
            try
            {
                var note = await _note.CreateNote(model, labelId);
                _logger.LogInformation("Note Added Successfully");
                var response = new ResponseModel<CreateNoteResponseModel>
                {
                    Success = true,
                    Message = "Notes Added Successfully",
                    Data = note
                };
                return Ok(response);
            } catch (Exception ex)
            {
                _logger.LogError("Some error has occured while creating note");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"Note creation unsuccesful {ex.Message}"
                };
                return Ok(response);
            }
        }
        [Authorize]
        [HttpPatch("updateNote/{noteId}")]
        public async Task<IActionResult> UpdateNote(UpdateNoteModel model, int noteId)
        {
            try
            {
                var note = await _note.UpdateNote(model, noteId);
                _logger.LogInformation("Note updated Successfully");
                var response = new ResponseModel<CreateNoteResponseModel>
                {
                    Success = true,
                    Message = "Notes updated Successfully",
                    Data = note
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Some error has occured while updating note");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"Note update unsuccesful {ex.Message}"
                };
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("getNotes/{labelId}")]
        public async Task<IActionResult> GetAllNotesInLabel([Required] int labelId)
        {
            try
            {
                var note = await _note.GetAllNotesInLabel(labelId);
                _logger.LogInformation("Notes fetched successfully");
                var response = new ResponseModel<IEnumerable<NoteResponseModel>>
                {
                    Success = true,
                    Message = "Notes fetched Successfully",
                    Data = note
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Some error has occured while retriving notes");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"Note fetch unsuccesful {ex.Message}"
                };
                return Ok(response);
            }
        }
        [Authorize]
        [HttpGet("getTrashNotes")]
        public async Task<IActionResult> GetAllTrashNotes()
        {
            try
            {
                var note = await _note.GetAllTrashNotes();
                _logger.LogInformation("Trash Notes fetched successfully");
                var response = new ResponseModel<IEnumerable<NoteResponseModel>>
                {
                    Success = true,
                    Message = "Trash Notes fetched Successfully",
                    Data = note
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Some error has occured while retriving Trash notes");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"Trash Note fetch unsuccesful {ex.Message}"
                };
                return Ok(response);
            }
        }
        [Authorize]
        [HttpGet("getArchivedNotes")]
        public async Task<IActionResult> GetAllArchivedNotes()
        {
            try
            {
                var note = await _note.GetAllArchivedNotes();
                _logger.LogInformation("Archived Notes fetched successfully");
                var response = new ResponseModel<IEnumerable<NoteResponseModel>>
                {
                    Success = true,
                    Message = "Archived Notes fetched Successfully",
                    Data = note
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Some error has occured while retriving Archived notes");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"Archived Note fetch unsuccesful {ex.Message}"
                };
                return Ok(response);
            }
        }

        [Authorize]
        [HttpPatch("moveToTrash/{noteId}")]
        public async Task<IActionResult> MoveToTrash([Required] int noteId)
        {
            try
            {
                await _note.MoveToTrash(noteId);
                _logger.LogInformation("Notes moved to trash successfully");
                var response = new ResponseModel
                {
                    Success = true,
                    Message = "Notes moved to trach successfully",
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Some error has occured while mopving notes to trash");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"Moving notes to trash unsuccesful {ex.Message}"
                };
                return Ok(response);
            }
        }

        [Authorize]
        [HttpPatch("retriveFromTrash/{noteId}")]
        public async Task<IActionResult> RetrieveFromTrash([Required] int noteId)
        {
            try
            {
                await _note.RetrieveFromTrash(noteId);
                _logger.LogInformation("Notes moved out of trash successfully");
                var response = new ResponseModel
                {
                    Success = true,
                    Message = "Notes moved out of trash successfully",
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Some error has occured while moving notes out of trash");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"Moving notes out of trash unsuccesful {ex.Message}"
                };
                return Ok(response);
            }
        }

        [Authorize]
        [HttpPatch("moveToArchive/{noteId}")]
        public async Task<IActionResult> MoveToArchive([Required] int noteId)
        {
            try
            {
                await _note.MoveToArchive(noteId);
                _logger.LogInformation("Notes moved to archived successfully");
                var response = new ResponseModel
                {
                    Success = true,
                    Message = "Notes moved to archived successfully",
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Some error has occured while moving notes to archive");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"Moving notes to archive unsuccesful {ex.Message}"
                };
                return Ok(response);
            }
        }

        [Authorize]
        [HttpPatch("removeFromArchive/{noteId}")]
        public async Task<IActionResult> RemoveFromArchive(int noteId)
        {
            try
            {
                await _note.RemoveFromArchive(noteId);
                _logger.LogInformation("Notes moved out of archive successfully");
                var response = new ResponseModel
                {
                    Success = true,
                    Message = "Notes moved out of archive successfully",
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Some error has occured while moving notes out of archive");
                var response = new ResponseModel
                {
                    Success = false,
                    Message = $"Moving notes out of archive unsuccesful {ex.Message}"
                };
                return Ok(response);
            }
        }
    }
}
