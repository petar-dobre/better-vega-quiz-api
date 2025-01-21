using Microsoft.EntityFrameworkCore;
using QuizWebApp.Configuration;
using QuizWebApp.Middleware;

namespace QuizWebApp.Extensions;

public static class AppBuilderExtensions
{
    public static void UseSwaggerWithAuthorization(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    public static void ApplyPendingMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }

    public static void UseExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}