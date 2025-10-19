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

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<NoteResponseDTO>>> GetNotes()
        {
            List<NoteResponseDTO> noteResponses = await _noteService.GetAllNotes();
            return Ok(noteResponses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteResponseDTO>> GetNoteById([FromRoute] int id)
        {
            try
            {
                NoteResponseDTO note = await _noteService.GetNoteById(id);
                return Ok(note);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<NoteResponseDTO>> AddNote([FromBody] NoteAddRequestDTO noteAddDTO)
        {
            NoteResponseDTO n = await _noteService.AddNote(noteAddDTO);
            return Created(String.Empty, n);
        }

        [HttpPut]
        public async Task<ActionResult<NoteResponseDTO>> UpdateNote([FromBody] NoteUpdateRequestDTO updateRequestDTO)
        {
            try
            {
                NoteResponseDTO note = await _noteService.Update(updateRequestDTO);
                return Ok(note);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote([FromRoute] int id)
        {
            try
            {
                await _noteService.BorrarNote(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

        }
    }
}