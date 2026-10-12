using API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/environments")]
public class EnvironmentController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EnvironmentDto.Create request)
    {
        var environment = request.Data;

        return Created();
    }
}