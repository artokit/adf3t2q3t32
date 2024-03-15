using EducationService.Dto;
using EducationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EducationService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private UserService _userService;
    
    public AuthController(UserService userService)
    {
        _userService = userService;
    }
    
    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login([FromForm] AuthModelDto authData)
    {
        var user = await _userService.GetUser(authData.Username);
        
        if (user is null || authData.Password != user?.HashedPassword)
        {
            return BadRequest("Неправильный логин/пароль");
        }
        
        return Ok(await _userService.UpdateUserTokens(authData.Username));
    }

    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register([FromForm] AuthModelDto authData)
    {
        if (await _userService.UserExist(authData.Username))
        {
            return BadRequest("Пользователь с данным логином уже зарегестрирован");
        }

        var tokens = await _userService.AddUser(authData.Username, authData.Password);
        return Ok(tokens);
    }
}