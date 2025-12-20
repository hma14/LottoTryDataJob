using Hangfire;
using Hangfire.SqlServer;
using LottoTryDataJob;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
    ContentRootPath = AppContext.BaseDirectory
});


// Run as Windows Service
builder.Host.UseWindowsService(); // 👈 Important!

// Configure logging (set to Warning level to reduce logs)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Warning); // 👈 Reduce log output


// Connection string for SQL Server
#if false

var dbPassword =
    Environment.GetEnvironmentVariable("LOTTO_DB_PASSWORD")
    ?? throw new InvalidOperationException("LOTTO_DB_PASSWORD is not set");

var connectionString = $"Server=webserver, 1433;Database=lottotry;User Id=sa;Password={dbPassword};MultipleActiveResultSets=True;TrustServerCertificate=True;Connection Timeout=30;";

#else
var baseConn = builder.Configuration.GetConnectionString("LottoDbContext");
var password = Environment.GetEnvironmentVariable("LOTTO_DB_PASSWORD");

if (string.IsNullOrWhiteSpace(password))
{
    throw new InvalidOperationException("LOTTO_DB_PASSWORD is not set");
}
var connectionString = $"{baseConn};Password={password}";

#endif


// Add DbContext
builder.Services.AddDbContext<LottoDb>(options =>
    options.UseSqlServer(connectionString)); // Ensure Microsoft.EntityFrameworkCore.SqlServer package is installed

// ✅ Add required services
builder.Services.AddScoped<SeleniumJob>(); // Ensure that your job is registered
builder.Services.AddLogging(); // Ensure logging is available



// Add Hangfire
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(connectionString, new SqlServerStorageOptions()));

builder.Services.AddHangfireServer();
builder.WebHost.UseUrls("http://localhost:5002");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5002);
});

var app = builder.Build();


// Enable Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");

//app.MapHangfireDashboard();

// ✅ Register Recurring Job inside the request pipeline
app.Lifetime.ApplicationStarted.Register(() =>
{
    RecurringJob.AddOrUpdate<SeleniumJob>(
        "selenium-job-2", // Unique job ID
        job => job.RunSeleniumScraper(),
        //"0 10 * * *", // Cron schedule for 10:00 AM daily
        Cron.Daily,  // Equivalent to "0 0 * * *"
                     //"*/2 * * * *", // every 2 mins
        new RecurringJobOptions { TimeZone = TimeZoneInfo.Local } // Use local timezone
    );

});
app.Run();
