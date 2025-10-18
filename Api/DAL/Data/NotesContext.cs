using Api.DAL.DataSeed;
using Microsoft.EntityFrameworkCore;

namespace Api.DAL;

public class NotesContext : DbContext
{

    public NotesContext(DbContextOptions<NotesContext> options) : base(options)
    {
    }

    public DbSet<Note> Notes => Set<Note>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NotesSeed());
    }
}