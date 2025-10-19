using Api.Mapping;
using Api.DTO;
using Xunit;

namespace Api.Tests;

public class NoteMapperTests
{
    [Fact]
    public void ToEntity_Should_Trim_And_Nullify_Empty_Contenido()
    {
        var mapper = new NoteMappers();
        var dto = new NoteAddRequestDTO { Titulo = "  Hola  ", Contenido = "   " };

        var e = mapper.ToEntity(dto);

        Assert.Equal("Hola", e.Titulo);
        Assert.Null(e.Contenido);
    }
}
