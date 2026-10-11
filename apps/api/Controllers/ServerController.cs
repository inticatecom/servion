using API.DTO;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/servers")]
public sealed class ServerController(ServerService serverService) : ControllerBase
{
    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        return Ok(await serverService.ListServers());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] ServerDto.Create request)
    {
        await serverService.CreateServer(request.Name);
        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetServer(Guid id)
    {
        var server = await serverService.GetServer(id);
        return server is null ? NotFound() : Ok(server);
    }
}