using API.DTO;
using API.Exceptions;
using API.Models;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Routes relating to servers.
/// </summary>
/// <param name="servers">The server service for interacting with servers.</param>
[ApiController]
[Route("api/servers")]
public sealed class ServerController(ServerService servers, ExceptionUtility exceptions) : ControllerBase
{
    /// <summary>
    /// Lists all servers regardless of their owners.
    /// </summary>
    /// <returns>The list of servers.</returns>
    [HttpGet]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.SuperAdmin)}")]
    public async Task<IActionResult> ListAll()
    {
        return await exceptions.HandleHttpException<ServerException>(async () => Ok(await servers.ListServers()));
    }

    /// <summary>
    /// Creates a new server with the provided information.
    /// </summary>
    /// <param name="request">The request body.</param>
    /// <returns>The request result.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ServerDto.Create request)
    {
        return await exceptions.HandleHttpException<ServerException, ServerDto.Create>(request, async () =>
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
                if (request.ShouldStart)
                {
                    await servers.SetServerState(server.Id, ServerState.Online);
                }

                return Created();
            }
        );
    }

    /// <summary>
    /// Deletes a server and it's data.
    /// </summary>
    /// <param name="request">The data to include for the request.</param>
    /// <returns>The request result.</returns>
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] ServerDto.Delete request)
    {
        return await exceptions.HandleHttpException<ServerException>(async () =>
            {
                await servers.DeleteServer(request.Id, false);
                return Ok();
            }
        );
    }

    /// <summary>
    /// Gets a server by the provided ID.
    /// </summary>
    /// <param name="id">The ID of the server.</param>
    /// <returns>The request result.</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOne(Guid id)
    {
        return await exceptions.HandleHttpException<ServerException>(async () =>
        {
            var server = await servers.GetServer(id);
            return server is null ? NotFound() : Ok(server);
        });
    }

    /// <summary>
    /// Updates the state for the provided server.
    /// </summary>
    /// <param name="id">The ID of the server.</param>
    /// <param name="request">The request body.</param>
    /// <returns>The request result.</returns>
    [HttpPut("{id:guid}/action")]
    public async Task<IActionResult> SetState(Guid id, [FromBody] ServerDto.SetState request)
    {
        return await exceptions.HandleHttpException<ServerException>(async () =>
        {
            var state = (ServerState)request.State;

            await servers.SetServerState(id, state);
            return NoContent();
        });
    }
}