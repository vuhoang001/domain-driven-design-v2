using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Configuration.Commands;
using Procurement.Application.Features.Order;

namespace ddd.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IProcurementModule procurementModule) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand request)
    {
        var result = await procurementModule.ExecuteCommandAsync(request);
        return Ok(new { message = result });
    }
}