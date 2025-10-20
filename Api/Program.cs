using Api.DAL;
using Api.DAL.Repository;
using Api.Mapping;
using Api.Mapping.Interface;
using Api.Service;
using Api.Service.Interface;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}"); 
builder.Services.AddControllers();
builder.Services.AddOpenApi();

///builder.Services.AddDbContext<NoteContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddDbContext<NoteContext>(options =>
{
    var connString = builder.Configuration.GetConnectionString("DefaultConnection") ??
        $"Host={Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost"};" +
        $"Port={Environment.GetEnvironmentVariable("DB_PORT") ?? "5432"};" +
        $"Database={Environment.GetEnvironmentVariable("DB_NAME") ?? "notasdb"};" +
        $"Username={Environment.GetEnvironmentVariable("DB_USER") ?? "notasuser"};" +
        $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "notassecret"};";
    options.UseNpgsql(connString);
});

builder.Services.AddHealthChecks().AddDbContextCheck<NoteContext>("db"); ;


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


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NoteContext>();
    try
    {
        var can = await db.Database.CanConnectAsync();
        app.Logger.LogInformation("DB CanConnect: {can}", can);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "DB connection failed");
    }
}


app.UseAuthorization();

app.MapControllers();
app.MapOpenApi();

app.Run();
