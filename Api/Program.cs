using Api.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<NotesContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NotesContext>();
    db.Database.Migrate();
}


app.MapGet("/notas", async (NotesContext db) => await db.Notes.ToListAsync());
app.MapPost("/notas", async (NotesContext db, Note n) =>
{
    db.Notes.Add(n);
    await db.SaveChangesAsync();
    return Results.Created($"/notas/{n.Id}", n);
});

app.UseAuthorization();

app.MapControllers();

app.Run();
