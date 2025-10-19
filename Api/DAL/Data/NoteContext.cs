using Api.DAL.DataSeed;
using Api.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.DAL;

public class NoteContext : DbContext
{

    public NoteContext(DbContextOptions<NoteContext> options) : base(options)
    {
    }

    public DbSet<Note> Notes => Set<Note>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NoteSeed());
    }
}