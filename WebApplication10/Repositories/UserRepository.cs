using Dapper;
using WebApplication10.Domain;
using WebApplication10.Models;
using WebApplication10.Repositories.Interfaces;

namespace WebApplication10.Repositories;

public class UserRepository : IUserRepository
{
    private DatabaseConnection _connection;
    
    public UserRepository(DatabaseConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<User> AddUser(string username, string password, string accessToken, string refreshToken)
    {
        using (var connection = _connection.CreateConnection())
        {
            return await connection.QueryFirstAsync<User>(
                "INSERT INTO USERS(\"Username\", \"HashedPassword\", \"AccessToken\", \"RefreshToken\") VALUES(@username, @password, @accessToken, @refreshToken) RETURNING *",
                new {username, password, accessToken, refreshToken});
        }
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        using (var connection = _connection.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM USERS where \"Username\" = @username", new {username});
        }
    }

    public async Task<User> UpdateTokens(string username, string accessToken, string refreshToken)
    {
        using (var connection = _connection.CreateConnection())
        {
            return await connection.QueryFirstAsync<User>(
                "UPDATE USERS SET \"AccessToken\" = @accessToken, \"RefreshToken\" = @refreshToken where \"Username\" = @username RETURNING *",
                new {accessToken, refreshToken, username});
        }
    }
}