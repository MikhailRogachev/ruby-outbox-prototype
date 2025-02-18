using Npgsql;

namespace ruby_outbox_data.Configuration;

public class DatabaseConfig
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Database { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string GetNpgsqlConnectionString()
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = Host,
            Port = Port,
            Username = Username,
            Password = Password,
            Database = Database
        };

        return builder.ToString();
    }
}
