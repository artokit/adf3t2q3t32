using EducationService.Dto;
using EducationService.Models;
using EducationService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EducationService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorizationController : BaseController
{
    private readonly AuthorizationService authorizationService;

    public AuthorizationController(AuthorizationService authorizationService)
    {
        this.authorizationService = authorizationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthModelDto authData)
    {
        if (authData.Login.IsNullOrEmpty() || authData.Password.IsNullOrEmpty())
        {
            return BadRequest("Логин и пароль не могут быть пустыми");
        }

        var res = await authorizationService.AuthByLoginPassword(authData.Login, authData.Password);

        if (res == null)
        {
            return BadRequest("Неправильный логин/пароль");
        }

        return Ok(res);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (user.Login.IsNullOrEmpty() || user.Password.IsNullOrEmpty())
        {
            return BadRequest("Логин и пароль не могут быть пустыми");
        }

        var res = await authorizationService.Register(user);

        if (res == null)
        {
            return BadRequest("Пользователь с таким логином уже существует");
        }

        return Ok(res);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        if (refreshToken.IsNullOrEmpty())
        {
            return BadRequest("Refresh token не может быть пустым");
        }

        var res = await authorizationService.AuthByRefreshToken(refreshToken);

        if (res == null)
        {
            return BadRequest("Refresh token недействителен");
        }

        return Ok(res);
    }
}
