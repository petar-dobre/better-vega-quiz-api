using Microsoft.EntityFrameworkCore;
using QuizWebApp.Configuration;
using QuizWebApp.Data;
using QuizWebApp.Middleware;

namespace QuizWebApp.Extensions;

public static class AppBuilderExtensions
{
    async public static void ApplyPendingMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            var seeder = services.GetRequiredService<DataSeeder>();
            await seeder.SeedAsync();
        }
    }

    public static void UseSwaggerWithAuthorization(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    public static void UseExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }

    public static void UseConfigureApplicationServices(this IApplicationBuilder app)
    {

    }
}
