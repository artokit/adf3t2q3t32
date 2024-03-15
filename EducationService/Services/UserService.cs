using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EducationService.Dto;
using EducationService.Models;
using EducationService.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace EducationService.Services;

public class UserService
{
    private IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<TokensDto> AddUser(string username, string password)
    {
        var tokens = new TokensDto{AccessToken = GenerateJwtToken(username), RefreshToken = GenerateJwtToken(username, AuthOptions.LifeTimeRefreshToken)};
        await _userRepository.AddUser(username, password, tokens.AccessToken, tokens.RefreshToken);
        return tokens;
    }
    
    public string GenerateJwtToken(string username, int timeExpire = AuthOptions.LifeTimeAccessToken)
    {
        var claims = new List<Claim>() { new Claim(ClaimTypes.Name, username) };
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            claims: claims, 
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(timeExpire)), 
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public async Task<bool> UserExist(string username)
    {
        return await _userRepository.GetUserByUsername(username) is not null;
    }

    public async Task<User?> GetUser(string username)
    {
        return await _userRepository.GetUserByUsername(username);
    }

    public async Task<TokensDto> UpdateUserTokens(string username)
    {
        var tokens = new TokensDto{AccessToken = GenerateJwtToken(username), RefreshToken = GenerateJwtToken(username, AuthOptions.LifeTimeRefreshToken)};
        await _userRepository.UpdateTokens(username, tokens.AccessToken, tokens.RefreshToken);
        return tokens;
    }
}