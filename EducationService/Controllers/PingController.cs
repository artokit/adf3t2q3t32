using Microsoft.AspNetCore.Mvc;

namespace EducationService.Controllers;

[ApiController]
[Route("[controller]")]
public class PingController : BaseController
{
    [HttpGet]
    public IActionResult Ping()
    {
        return Ok("Pong");
    }
}
