using WebApplication10.Models;

namespace WebApplication10.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<User> AddUser(string username, string password, string AccessToken, string RefreshToken);
    public Task<User?> GetUserByUsername(string username);
    public Task<User> UpdateTokens(string username, string accessToken, string refreshToken);
}