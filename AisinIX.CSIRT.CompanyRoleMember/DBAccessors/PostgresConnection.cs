using Npgsql;
using Microsoft.Extensions.Configuration;

public class PostgresConnection
{
    private readonly string _connectionString;

    public PostgresConnection(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Postgres");
    }

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}