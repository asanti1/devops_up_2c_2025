using Api.DAL.Models;
using Api.DTO;
using Api.Mapping.Interface;

namespace Api.Mapping
{
    public class NoteMappers : INoteMapper
    {
        public Note ToEntity(NoteAddRequestDTO dto) => new Note
        {
            Titulo = dto.Titulo?.Trim() ?? string.Empty,
            Contenido = string.IsNullOrWhiteSpace(dto.Contenido) ? null : dto.Contenido.Trim()
        };

        public NoteResponseDTO ToResponse(Note note) => new NoteResponseDTO
        {
            Titulo = note.Titulo,
            Contenido = note.Contenido,
            Creada = note.Creada
        };
    }
}
