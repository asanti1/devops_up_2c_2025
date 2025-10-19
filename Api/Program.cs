using Api.DAL;
using Api.DAL.Repository;
using Api.Mapping;
using Api.Mapping.Interface;
using Api.Service;
using Api.Service.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<NoteContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddHealthChecks().AddDbContextCheck<NoteContext>("db");;

builder.Services.AddScoped<INoteMapper, NoteMappers>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(
    x => new UnitOfWork(
        x.GetRequiredService<NoteContext>(),
        x.GetRequiredService<INoteRepository>()
        ));


var app = builder.Build();

app.MapHealthChecks("/health");

if (args.Contains("--migrate-only"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<NoteContext>();
    await db.Database.MigrateAsync();
    return;
}


app.UseAuthorization();

app.MapControllers();
app.MapOpenApi();

app.Run();
