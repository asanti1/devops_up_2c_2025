namespace Api.DAL.Models;
public class Note
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string? Contenido { get; set; }
    public DateTimeOffset Creada { get; set; } = DateTimeOffset.UtcNow;
    
}
