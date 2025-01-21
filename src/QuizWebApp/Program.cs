using Microsoft.OpenApi.Models;
using QuizWebApp.Configuration;
using QuizWebApp.Extensions;
using QuizWebApp.Filters;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

// Configure services
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.ConfigureJwtAuthentication(configuration);
builder.Services.ConfigureDbContext(configuration);
builder.Services.ConfigureServices();

// builder.WebHost.UseUrls("http://0.0.0.0:80");

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationErrorFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quiz API",
        Version = "v1"
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithAuthorization();
}

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.ApplyPendingMigrations();

app.Run();