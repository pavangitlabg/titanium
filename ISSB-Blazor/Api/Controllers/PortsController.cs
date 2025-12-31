using Microsoft.AspNetCore.Mvc;
using Services;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortsController : ControllerBase
{
    [HttpGet("GetPorts")]
    [SwaggerOperation(Summary = "Gets Ports", Description = "Gets Ports")]
    public async Task<IActionResult> GetPorts()
    {
        var portSrv = new PortsService();
        var Ports = await portSrv.GetAllPorts();

        return Ok(Ports);
    }
}