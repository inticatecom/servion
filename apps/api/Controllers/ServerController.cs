using API.DTO;
using API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/servers")]
public sealed class ServerController(ServerService servers) : ControllerBase
{
    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        return Ok(await servers.ListServers());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] ServerDto.Create request)
    {
        await servers.CreateServer(request.Name);
        return Created();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var server = await servers.GetServer(id);
        return server is null ? NotFound() : Ok(server);
    }

    [HttpPut("{id:guid}/action")]
    public async Task<IActionResult> SetState(Guid id, [FromBody] ServerDto.SetState request)
    {
        var state = (ServerState)request.State;

        try
        {
            await servers.SetServerState(id, state);
            return NoContent();
        }
        catch
        {
            return StatusCode(500, "Internal server error.");
        }
    }
}