using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiYearsController : ControllerBase
    {

        private IConfiguration _config;

        public ApiYearsController(IConfiguration config)
        {
            _config = config;
        }


        [HttpGet("GetYears")]
        [Authorize(Roles = "API_User")]
        [SwaggerOperation(Summary = "Gets User Years, Quarters, Months", Description = "Gets User Years, Quarters, Months")]
        public async Task<IActionResult> GetYears()
        {
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var handler = new JwtSecurityTokenHandler();
            string authHeader = Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value;

           // var currentUser = GetCurrentUser();
            
            var authSrv = new UserServices();
            var user = await authSrv.GetUsersById(id);

            var Years = new List<YearMonthQuarterModel>();
            string[] YearList = user.AllowedYears.Split(",");

            foreach (string y in YearList)
            {
                for (int i = 1; i <= 12; i++)
                {
                    var date = new DateTime(int.Parse(y),i,1);
                    var yModel = new YearMonthQuarterModel
                    {
                        Month = i,
                        Quarter = (date.Month + 2) / 3,
                        Year = int.Parse(y)
                    };

                    Years.Add(yModel);
                }
            }
            //return Years;
            return Ok(Years);
        }

        private ApiUserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new ApiUserModel
                {
                    Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    GivenName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                    _id = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.PrimarySid)?.Value
                };
            }
            return null;
        }
    }


}


