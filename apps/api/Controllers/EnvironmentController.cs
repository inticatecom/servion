using API.DTO;
using API.Exceptions;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/environments")]
public sealed class EnvironmentController(EnvironmentService environments, ExceptionUtility exceptions)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List()
    {
        return await exceptions.HandleHttpException<EnvironmentException>(async () =>
            Ok(await environments.ListEnvironments())
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EnvironmentDto.Create request)
    {
        return await exceptions.HandleHttpException<EnvironmentException>(async () =>
        {
            await environments.CreateEnvironment(request.Data);
            return Created();
        });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] EnvironmentDto.Delete request)
    {
        return await exceptions.HandleHttpException<EnvironmentException>(async () =>
        {
            await environments.DeleteEnvironment(request.Id);
            return Ok();
        });
    }
}