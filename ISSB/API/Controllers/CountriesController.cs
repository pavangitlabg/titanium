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
    public class CountriesController : ControllerBase
    {
        [HttpGet("GetMarketCountries")]
        [Authorize(Roles = "API_User")]
        [SwaggerOperation(Summary = "Gets All Market Countries", Description = "Gets the ISSB All Market Countries")]
        public async Task<IActionResult> GetMarket()
        {
            var marketCountryService = new MarketCountryService();
            var data = await marketCountryService.GetActiveMarketCountries();
            return Ok(data);
        }

        [HttpGet("GetSourceCountries")]
        [Authorize(Roles = "API_User")]
        [SwaggerOperation(Summary = "Gets User Source Countries", Description = "Gets the ISSB User Source Countries")]
        public async Task<IActionResult> GetSource()
        {
            var handler = new JwtSecurityTokenHandler();
            string authHeader = Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value;

            var authSrv = new UserServices();
            var user = await authSrv.GetUsersById(id);
            return Ok(user.AllowedSourceCountries);
        }
    }
}

