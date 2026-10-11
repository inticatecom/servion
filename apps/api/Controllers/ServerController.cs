using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/servers")]
public sealed class ServerController(ServerService serverService) : ControllerBase
{
    [HttpGet("list")]
    public IActionResult List()
    {
        return Ok(serverService.GetServer("abc"));
    }
}