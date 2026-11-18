using API.DTO;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
///     Routes relating to servers.
/// </summary>
[ApiController]
[Route("api/servers")]
public sealed class ServerController(ServerService servers) : ControllerBase
{
    /// <summary>
    ///     Lists all servers regardless of their owners.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.SuperAdmin)}")]
    public async Task<IActionResult> ListAll()
    {
        return Ok(await servers.ListServers());
    }

    /// <summary>
    ///     Creates a new server with the provided information.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ServerDto.Create request)
    {
        // Create the new server.
        var server = await servers.CreateServer(new ServerCreateParameters
        {
            Name = request.Name,
            OwnerId = request.OwnerId,
            EnvironmentId = request.EnvironmentId,
            ImageKey = request.Image,
            Variables = request.Variables
        });

        // If the request wants the server to startup, start it.
        if (request.ShouldStart) await servers.SetServerState(server.Id, ServerState.Online);

        return Created();
    }

    /// <summary>
    ///     Deletes a server and it's data.
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] ServerDto.Delete request)
    {
        await servers.DeleteServer(request.Id, false);
        return Ok();
    }

    /// <summary>
    ///     Gets a server by the provided ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOne(Guid id)
    {
        var server = await servers.GetServer(id);
        return server is null ? NotFound() : Ok(server);
    }

    /// <summary>
    ///     Updates the state for the provided server.
    /// </summary>
    [HttpPut("{id:guid}/action")]
    public async Task<IActionResult> SetState(Guid id, [FromBody] ServerDto.SetState request)
    {
        var state = request.State;

        await servers.SetServerState(id, state);
        return NoContent();
    }
}