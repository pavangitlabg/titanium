using System.Net;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiVersionController : ControllerBase
{
    [HttpGet("GetVersion")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [SwaggerOperation(Summary = "Gets the ISSB Api Version", Description = "Gets the ISSB Api Version from the server")]
    public IActionResult GetApi()
    {
        
        return Ok(new AppVersionModel { Version = "1.0", _id = "1" });
    }
}