using Microsoft.EntityFrameworkCore;
using ruby_outbox_data.Persistency;

namespace ruby_outbox_api.Extensions;

public static class AppExtensions
{
    public static async Task DataBaseMigrateAsync(this WebApplication app, ILogger logger)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var services = scope.ServiceProvider;

        try
        {
            logger.LogInformation("Migration of database is started");
            var context = services.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();
            logger.LogInformation("Migration of database is completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
