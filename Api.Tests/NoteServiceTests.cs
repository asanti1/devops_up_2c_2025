using Api.DAL;
using Api.DAL.Repository;
using Api.Service;
using Api.DTO;
using Api.Tests.Helpers;
using Api.Mapping;
using Xunit;
using Moq;
using Api.DAL.Models;

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

    [Fact]
    public async Task GetAll_ShouldReturnAllNotes()
    {
        var mockRepo = new Mock<INoteRepository>();
        mockRepo
            .Setup(r => r.GetAllNotes())
            .ReturnsAsync(new List<Note>
            {
                new Note { Id = 1, Titulo = "Primera nota", Contenido = "Contenido 1" },
                new Note { Id = 2, Titulo = "Segunda nota", Contenido = "Contenido 2" }
            });

        var mockUow = new Mock<IUnitOfWork>();

        mockUow.Setup(u => u.NoteRepository).Returns(mockRepo.Object);

        var mapper = new NoteMappers();
        var service = new NoteService(mockUow.Object, mapper);

        var result = await service.GetAllNotes();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, n => n.Titulo == "Primera nota");

        mockRepo.Verify(r => r.GetAllNotes(), Times.Once);
    }
}
