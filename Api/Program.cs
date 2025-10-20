using Api.DAL;
using Api.DAL.Repository;
using Api.Mapping;
using Api.Mapping.Interface;
using Api.Service;
using Api.Service.Interface;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<NoteContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddHealthChecks().AddDbContextCheck<NoteContext>("db"); ;

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddDbContextCheck<NoteContext>("db");



builder.Services.AddScoped<INoteMapper, NoteMappers>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(
    x => new UnitOfWork(
        x.GetRequiredService<NoteContext>(),
        x.GetRequiredService<INoteRepository>()
        ));


var app = builder.Build();

app.MapHealthChecks("/health-db");

app.MapHealthChecks("/health", new HealthCheckOptions {
    Predicate = r => r.Name == "self"
});


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
