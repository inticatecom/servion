using API.DTO;
using API.Exceptions;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/environments")]
public sealed class EnvironmentController(EnvironmentService environments)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List()
    {
        return Ok(await environments.ListEnvironments());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EnvironmentDto.Create request)
    {
        await environments.CreateEnvironment(request.Data);
        return Created();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] EnvironmentDto.Delete request)
    {
        await environments.DeleteEnvironment(request.Id);
        return Ok();
    }
}