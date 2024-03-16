using Database;
using EducationService.Models;

namespace EducationService.Repositories;

public class UserRepository 
{
    private Connection connection;

    public UserRepository(Connection connection)
    {
        this.connection = connection;
    }

    public async Task<User?> AddUser(User user)
    {
        var queryObject = new QueryObject(
            $"INSERT INTO USERS(Username, Password, Email) VALUES(@username, @password, @email) RETURNING Id, Username, Password, Email",
            new { username = user.Username, password = user.Password, email = user.Email });
        return await connection.CommandWithResponse<User>(queryObject);
    }

    public async Task<User?> GetByUsername(string username)
    {
        var queryObject = new QueryObject(
            "SELECT Id, Username, Password, Email FROM USERS WHERE Username = @username",
            new { username });
        return await connection.FirstOrDefault<User>(queryObject);
    }

    public async Task<User?> GetById(int id)
    {
        var queryObject = new QueryObject(
            "SELECT Id, Username, Password, Email FROM USERS WHERE Id = @id",
            new { id });
        return await connection.FirstOrDefault<User>(queryObject);
    }
}
