using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Models;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortsController : ControllerBase
    {
        [HttpGet("GetPorts")]
        [Authorize(Roles = "API_User")]
        [SwaggerOperation(Summary = "Gets Ports", Description = "Gets Ports")]
        public async Task<IActionResult> GetPorts()
        {
            var portSrv = new PortsService();
            var Ports = await portSrv.GetAllPorts();

            return Ok(Ports);
        }
    }
}

