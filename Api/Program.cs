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

// LOGGING
var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console();


Log.Logger = loggerConfig.CreateLogger();
builder.Host.UseSerilog();


//  SENTRY 
var sentryDsn = builder.Configuration["SENTRY_DSN"];
if (!string.IsNullOrWhiteSpace(sentryDsn))
{
    builder.WebHost.UseSentry(o =>
    {
        o.Dsn = sentryDsn;
        o.Debug = true;
        o.TracesSampleRate = 0.5;
    });
}

// MISC
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// DB
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

// DI

builder.Services.AddScoped<INoteMapper, NoteMappers>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(
    x => new UnitOfWork(
        x.GetRequiredService<NoteContext>(),
        x.GetRequiredService<INoteRepository>()
        ));

var app = builder.Build();

// HEALTH
app.MapHealthChecks("/health");

// SENTRY TEST
if (app.Environment.IsDevelopment())
{
    SentrySdk.CaptureMessage("Hello Sentry from NotasApi (dev startup)");

    app.MapGet("/sentry-test", () =>
    {
        throw new Exception("Test exception for Sentry (dev only)");
    });
}

// MIGRATION
if (args.Contains("--migrate-only"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<NoteContext>();
    await db.Database.MigrateAsync();
    return;
}


// DB CHECK
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


// MIDDLEWARES 
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

app.UseSentryTracing();



// ENDPOINTS
app.MapControllers();
app.MapOpenApi();


// RUN
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
