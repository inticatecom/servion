using API.DTO;
using API.Exceptions;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/environments")]
public class EnvironmentController(EnvironmentService environments, ILogger<EnvironmentService> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List()
    {
        return Ok(await environments.ListEnvironments());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EnvironmentDto.Create request)
    {
        var environment = request.Data;

        try
        {
            await environments.CreateEnvironment(environment);
            return Created();
        }
        catch (Exception exception)
        {
            if (exception is EnvironmentException environmentException)
            {
                return StatusCode(environmentException.Code, environmentException.Message);
            }

            logger.LogError(exception, exception.Message);
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] EnvironmentDto.Delete request)
    {
        try
        {
            await environments.DeleteEnvironment(request.Id);
            return Ok();
        }
        catch (Exception exception)
        {
            if (exception is EnvironmentException environmentException)
            {
                return StatusCode(environmentException.Code, environmentException.Message);
            }

            logger.LogError(exception, exception.Message);
            return StatusCode(500, "Internal server error.");
        }
    }
}