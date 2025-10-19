using Api.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.DAL.DataSeed;

public class NoteSeed : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasData(new Note
        {
            Id = 1,
            Titulo = "Primer apunte",
            Contenido = "Configuración inicial del proyecto .NET con Docker y Postgres.",
            Creada = new DateTimeOffset(2024, 10, 01, 0, 0, 0, TimeSpan.Zero)
        },
            new Note
            {
                Id = 2,
                Titulo = "Recordatorio",
                Contenido = "Agregar validaciones de modelo y respuestas 400/404 en los endpoints.",
                Creada = new DateTimeOffset(2024, 10, 02, 0, 0, 0, TimeSpan.Zero)
            },
            new Note
            {
                Id = 3,
                Titulo = "Pendiente",
                Contenido = "Probar el deploy en Render y verificar las migraciones automáticas.",
                Creada = new DateTimeOffset(2024, 10, 03, 0, 0, 0, TimeSpan.Zero)
            },
            new Note
            {
                Id = 4,
                Titulo = "Idea futura",
                Contenido = "Implementar JWT para autenticar las operaciones sobre notas.",
                Creada = new DateTimeOffset(2024, 10, 04, 0, 0, 0, TimeSpan.Zero)
            },
            new Note
            {
                Id = 5,
                Titulo = "Machete DevOps",
                Contenido = "Recordar CAMS: Culture, Automation, Measurement, Sharing.",
                Creada = new DateTimeOffset(2024, 10, 05, 0, 0, 0, TimeSpan.Zero)
            });
    }
}