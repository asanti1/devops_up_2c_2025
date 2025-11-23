using Api.DTO;
using Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly ILogger<NoteController> _logger;

        public NoteController(INoteService noteService, ILogger<NoteController> logger)
        {
            _noteService = noteService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<NoteResponseDTO>>> GetNotes()
        {
            _logger.LogInformation("Request to GET all notes");
            List<NoteResponseDTO> noteResponses = await _noteService.GetAllNotes();
            _logger.LogInformation("Returning {Count} notes", noteResponses.Count);
            return Ok(noteResponses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteResponseDTO>> GetNoteById([FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Request to GET note {NoteId}", id);
                NoteResponseDTO note = await _noteService.GetNoteById(id);
                _logger.LogInformation("Returning note {NoteId}", id);
                return Ok(note);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Note {NoteId} not found", id);
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<NoteResponseDTO>> AddNote([FromBody] NoteAddRequestDTO noteAddDTO)
        {
            _logger.LogInformation("Request to CREATE note with title {Title}", noteAddDTO.Titulo);
            NoteResponseDTO n = await _noteService.AddNote(noteAddDTO);
            _logger.LogInformation("Note created");
            return Created(String.Empty, n);
        }

        [HttpPut]
        public async Task<ActionResult<NoteResponseDTO>> UpdateNote([FromBody] NoteUpdateRequestDTO updateRequestDTO)
        {
            try
            {
                _logger.LogInformation("Request to UPDATE note {NoteId}", updateRequestDTO.Id);
                NoteResponseDTO note = await _noteService.Update(updateRequestDTO);
                _logger.LogInformation("Note {NoteId} updated successfully", updateRequestDTO.Id);
                return Ok(note);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Note {NoteId} not found when trying to update", updateRequestDTO.Id);
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote([FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Request to DELETE note {NoteId}", id);
                await _noteService.BorrarNote(id);
                _logger.LogInformation("Note {NoteId} deleted successfully", id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Note {NoteId} not found when trying to delete", id);
                return NotFound();
            }

        }
    }
}