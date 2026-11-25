using API.DTO;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
///     Routes for interacting with environments.
/// </summary>
[ApiController]
[Route("api/environments")]
public sealed class EnvironmentController(EnvironmentService environments)
    : ControllerBase
{
    /// <summary>
    ///     Lists all the environments that exist within the system.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> List()
    {
        return Ok(await environments.ListEnvironmentsAsync());
    }

    /// <summary>
    ///     Creates a new environment in the system.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EnvironmentDto.Create request)
    {
        await environments.CreateEnvironmentAsync(request.Data);
        return Created();
    }

    /// <summary>
    ///     Deletes the provided environment if it exists.
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] EnvironmentDto.Delete request)
    {
        await environments.DeleteEnvironmentAsync(request.Id);
        return Ok();
    }
}