using Api.DAL;
using Api.DAL.Repository;
using Api.Service;
using Api.DTO;
using Api.Tests.Helpers;
using Api.Mapping;
using Xunit;

namespace Api.Tests;

public class NoteServiceTests
{
    [Fact]
    public async Task AddNote_Should_Create_Note()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();
        var repo = new NoteRepository(context);
        var uow = new UnitOfWork(context, repo);
        var mapper = new NoteMappers();
        var service = new NoteService(uow, mapper);

        var dto = new NoteAddRequestDTO { Titulo = "Nueva nota", Contenido = "Contenido test" };
        var result = await service.AddNote(dto);

        Assert.Equal("Nueva nota", result.Titulo);
    }

    [Fact]
    public async Task BorrarNote_Should_Throw_When_Id_Not_Exists()
    {
        using var ctx = TestDbContextFactory.CreateInMemoryContext();
        var uow = new UnitOfWork(ctx, new NoteRepository(ctx));
        var svc = new NoteService(uow, new NoteMappers());

        await Assert.ThrowsAsync<KeyNotFoundException>(() => svc.BorrarNote(12345));
    }
}
