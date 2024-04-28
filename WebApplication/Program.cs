using HomeDev.DbEntities;
using HomeDev.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlite(configuration.GetConnectionString("WebApiDatabase")));

builder.Services.AddControllersWithViews();
builder.Services.AddHostedService<IotDevicePoller>();


  Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("logs/homedev.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();

 builder.Logging.ClearProviders();
 builder.Logging.AddSerilog(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();
