namespace Api.DTO
{
    public class NoteUpdateRequestDTO
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
    }
}