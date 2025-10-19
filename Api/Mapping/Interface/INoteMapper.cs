using Api.DAL.Models;
using Api.DTO;

namespace Api.Mapping.Interface
{
    public interface INoteMapper
    {
        Note ToEntity(NoteAddRequestDTO dto);
        NoteResponseDTO ToResponse(Note note);
    }
}