using Api.DAL;
using Microsoft.EntityFrameworkCore;

namespace Api.Tests.Helpers;

public static class TestDbContextFactory
{
    public static NoteContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<NoteContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new NoteContext(options);
    }
}