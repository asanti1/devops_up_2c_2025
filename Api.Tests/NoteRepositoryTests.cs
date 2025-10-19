using System.Net.NetworkInformation;
using Api.DAL;
using Api.DAL.Models;
using Api.DAL.Repository;
using Api.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.Tests;

public class NoteRepositoryTests
{
    [Fact]
    public async Task AddNote_Should_Add_Entity()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();
        var repo = new NoteRepository(context);

        var note = new Note { Titulo = "Test", Contenido = "Hola mundo" };
        await repo.AddNote(note);

        var saved = await context.Notes.FirstOrDefaultAsync();
        Assert.NotNull(saved);
        Assert.Equal("Test", saved!.Titulo);
    }

    [Fact]
    public async Task GetNoteById_Should_Return_Note()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();
        var repo = new NoteRepository(context);

        var note = new Note { Titulo = "Test", Contenido = "Hola" };
        note.Id = 1;
        await repo.AddNote(note);

        var result = await repo.GetNoteById(note.Id);
        Assert.NotNull(result);
        Assert.Equal("Hola", result!.Contenido);
    }
}
