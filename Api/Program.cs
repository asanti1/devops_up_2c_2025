using Api.DAL;
using Api.DAL.Repository;
using Api.Mapping;
using Api.Mapping.Interface;
using Api.Service;
using Api.Service.Interface;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sentry;
using Sentry.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
builder.Services.AddControllers();
builder.Services.AddOpenApi();

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

// --- LOGGING CONFIG ---

var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console(); // Siempre logueamos a consola


var sentryDsn = builder.Configuration["SENTRY_DSN"];

// Creamos el logger global de Serilog
Log.Logger = loggerConfig.CreateLogger();

builder.Host.UseSerilog();


if (!string.IsNullOrWhiteSpace(sentryDsn))
{
    builder.WebHost.UseSentry(o =>
    {
        o.Dsn = sentryDsn;
        o.Debug = true;
        o.TracesSampleRate = 0.5;
    });
}


var app = builder.Build();

app.UseSentryTracing();

app.MapHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
    SentrySdk.CaptureMessage("Hello Sentry from NotasApi (dev startup)");

    app.MapGet("/sentry-test", () =>
    {
        throw new Exception("Test exception for Sentry (dev only)");
    });
}

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


app.MapControllers();
app.MapOpenApi();

try
{
    Log.Information("Starting up");
    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
