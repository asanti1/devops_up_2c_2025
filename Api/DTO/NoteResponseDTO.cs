namespace Api.DTO
{
    public class NoteResponseDTO
    {
        public string Titulo { get; set; } = string.Empty;
        public string? Contenido { get; set; }
        public DateTimeOffset Creada { get; set; }
    }
}