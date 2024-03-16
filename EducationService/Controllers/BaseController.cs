using Common;
using Microsoft.AspNetCore.Mvc;

namespace EducationService.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected int UserId => AuthHeader.GetUserId();
    protected string AuthHeader => HttpContext.Request.Headers["Authorization"].ToString();
}
