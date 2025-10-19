using Api.DAL.Models;
using Api.DTO;
public interface INoteRepository
{
    public Task<Note> AddNote(Note note);
    public Task<List<Note>> GetAllNotes();

    public Task<Note?> GetNoteById(int id);

    public Task<Note?> Update(NoteUpdateRequestDTO n);

    public Task<bool> BorrarNote(int noteId);
}