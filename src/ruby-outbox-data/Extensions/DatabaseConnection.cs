using Npgsql;
using ruby_outbox_core.Contracts.Options;

namespace ruby_outbox_data.Extensions;

public static class DatabaseConnection
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
}
