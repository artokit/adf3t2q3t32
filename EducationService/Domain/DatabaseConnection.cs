using Npgsql;

namespace EducationService.Domain;

public class DatabaseConnection
{
    private string? _connectionString;

    public DatabaseConnection(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DatabaseConnection");
    }

    public NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}