using Api.DAL.Models;
using Api.DTO;
using Microsoft.EntityFrameworkCore;

namespace Api.DAL.Repository;

public class NoteRepository : Repository<Note>, INoteRepository
{
    public NoteRepository(NoteContext context) : base(context)
    {
    }

    public async Task<Note> AddNote(Note note)
    {
        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();
        return note;
    }

    public Task<List<Note>> GetAllNotes()
    {
        return _context.Notes.AsNoTracking().ToListAsync();
    }

    public Task<Note?> GetNoteById(int id)
    {
        return _context.Notes.FindAsync(id).AsTask();
    }

    public async Task<Note?> Update(NoteUpdateRequestDTO n)
    {
        var note = await _context.Notes.FindAsync(n.Id);
        if (note == null) return null;
        if (n.Titulo != null) note.Titulo = n.Titulo;
        if (n.Contenido != null) note.Contenido = n.Contenido;

        await _context.SaveChangesAsync();

        return note;
    }

    public async Task<bool> BorrarNote(int noteId)
    {
        var note = await _context.Notes.FindAsync(noteId);
        if (note == null) return false;

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
        return true;
    }
}