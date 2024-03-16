using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Common;
using EducationService.Dto;
using EducationService.Models;
using Microsoft.IdentityModel.Tokens;

namespace EducationService.Services;

public class AuthorizationService
{
    private readonly UserService userService;

    public AuthorizationService(UserService userService)
    {
        this.userService = userService;
    }

    public async Task<AuthResponseDto?> AuthByLoginPassword(string username, string password)
    {
        var user = await userService.GetByUsername(username);
        if (user is null || user.Password != Common.Common.PasswordHash(password))
        {
            return null;
        }

        return new AuthResponseDto { AccessToken = GenerateAccessToken(user), RefreshToken = GenerateRefreshToken(user) };
    }

    public async Task<AuthResponseDto?> Register(User user)
    {
        if (await userService.GetByUsername(user.Username) != null)
        {
            return null;
        }
        var userAdded = await userService.AddUser(user);
        return userAdded is null ? null : new AuthResponseDto { AccessToken = GenerateAccessToken(userAdded), RefreshToken = GenerateRefreshToken(userAdded) };
    }

    public async Task<AuthResponseDto?> AuthByRefreshToken(string refreshToken)
    {
        var id = refreshToken.GetUserId();
        if (id == default)
        {
            return null;
        }

        var user = await userService.GetById(id);
        return user is null ? null : new AuthResponseDto { AccessToken = GenerateAccessToken(user), RefreshToken = GenerateRefreshToken(user) };
    }


    private static string GenerateAccessToken(User user)
    {
        var claims = GetClaims(user.Id);
        return GenerateJwtToken(claims, AuthOptions.LifeTimeAccessToken);
    }

    private static string GenerateRefreshToken(User user)
    {
        var claims = GetClaims(user.Id);
        return GenerateJwtToken(claims, AuthOptions.LifeTimeRefreshToken);
    }

    private static string GenerateJwtToken(List<Claim> claims, int timeExpire)
    {
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(timeExpire)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private static List<Claim> GetClaims(int userId)
    {
        var claims = new List<Claim> { new(ClaimType.Id.ToString(), userId.ToString()) };
        return claims;
    }
}
