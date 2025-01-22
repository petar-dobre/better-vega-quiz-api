using Microsoft.OpenApi.Models;
using QuizWebApp.Configuration;
using QuizWebApp.Extensions;
using QuizWebApp.Filters;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
var configuration = builder.Configuration;

// Configure services
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.ConfigureJwtAuthentication(configuration);
builder.Services.ConfigureDbContext(configuration);
builder.Services.ConfigureIdentity(configuration);
builder.Services.RegisterServices();


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
