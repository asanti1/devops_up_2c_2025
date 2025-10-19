using Api.DAL;
using Api.DAL.Models;
using Api.DTO;
using Api.Mapping;

namespace Api.Service.Interface;
public interface INoteService
{
    public Task<NoteResponseDTO> AddNote(NoteAddRequestDTO noteAddRequest);
    public Task<List<NoteResponseDTO>> GetAllNotes();
    public Task<NoteResponseDTO> GetNoteById(int id);
    public Task<NoteResponseDTO> Update(NoteUpdateRequestDTO n);
    public Task BorrarNote(int noteId);
}