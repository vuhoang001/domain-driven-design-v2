using MasterData.Application.Features.SampleFeature;
using MasterData.Infrastructure.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace ddd.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateSampleRequest request)
    {
        var command = new CreateItemCommand(request.Name);
        var result  = await CommandsExecutor.Execute(command);
        return Ok(new { message = result });
    }
}

public class CreateSampleRequest
{
    public string Name { get; set; } = string.Empty;
}