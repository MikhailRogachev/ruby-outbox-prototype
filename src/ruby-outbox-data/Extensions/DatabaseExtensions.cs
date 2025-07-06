using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using ruby_outbox_core.Contracts.Options;
using ruby_outbox_data.Persistency;

namespace ruby_outbox_data.Extensions;

public static class DatabaseExtensions
{
    public static string NpgsqlConnectionStringBuilder(this DatabaseConfig databaseConfig)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseConfig.Host,
            Port = databaseConfig.Port,
            Username = databaseConfig.Username,
            Password = databaseConfig.Password,
            Database = databaseConfig.Database
        };

        return builder.ToString();
    }

    public static async Task MigrateDatabase(IServiceProvider serviceProvider)
    {
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            try
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                logger.LogInformation("Migrating database");
                await context.Database.MigrateAsync();
                logger.LogInformation("Migration of database is done");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred migrating the DB");
                throw;
            }
        }
    }
}
