using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Services;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TariffCodesController : ControllerBase
    {
        [HttpGet("GetHighLevelTariffCodes")]
        [SwaggerOperation(Summary = "Gets High Level Tariff Codes", Description = "Gets High Level Tariff Codes")]
        public async Task<IActionResult> GetHighLevelTariffCodes()
        {
            var handler = new JwtSecurityTokenHandler();
            string authHeader = Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value;

            var authSrv = new UserServices();
            var user = await authSrv.GetUsersById(id);

            var tariffList = new List<TariffFilterModel>();

            foreach (var Item in user.AllowedTariffCodes)
            {
                var model = new TariffFilterModel
                {
                    Idx = Item.Idx,
                    Name = Item.Name,
                    TariffCode = Item.TariffCode
                };
                tariffList.Add(model);
            }

            return Ok(tariffList);
        }

        [HttpGet("GetCustomFilters")]
        [SwaggerOperation(Summary = "Get Custom Filters", Description = "Get Custom Filters")]
        public async Task<IActionResult> GetCustomFilters()
        {
            var handler = new JwtSecurityTokenHandler();
            string authHeader = Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value;

            var authSrv = new UserServices();
            var user = await authSrv.GetUsersById(id);
            var filterList = new List<TariffCustomModel>();

            foreach (var Item in user.AllowedCustomFilters)
            {
                var model = new TariffCustomModel
                {
                    Name = Item.Name,
                    _id = Item._id
                };
                filterList.Add(model);
            }

            return Ok(filterList);
        }

        [HttpGet("Get2DigitCodes")]
        [SwaggerOperation(Summary = "Get Two Digit Codes", Description = "Get Two Digit Codes")]
        public async Task<IActionResult> Get2DigitCodes()
        {
            var handler = new JwtSecurityTokenHandler();
            string authHeader = Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value;

            var authSrv = new UserServices();
            var user = await authSrv.GetUsersById(id);
            var srv = new ProductService();
            var treeMedumModel = await srv.GetAllMedumProductsForTree();

            return Ok(treeMedumModel);
        }

        [HttpGet("GetLongTariffCodes")]
        [SwaggerOperation(Summary = "Get Long Tariff Codes, example CN,HS,400 - USA,720 - CHINA ...",
            Description = "Get Long Tariff Codes, example CN,HS,400 - USA,720 - CHINA ...")]
        public async Task<IActionResult> GetLongTariffCodes(string Code)
        {
            var handler = new JwtSecurityTokenHandler();
            string authHeader = Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value;

            var authSrv = new UserServices();
            var user = await authSrv.GetUsersById(id);
            var srv = new ProductService();
            var LongTariffModel = await srv.GetLongTarrifByCode(Code);

            return Ok(LongTariffModel);
        }
    }
}

public class TariffCustomModel
{
    public string _id { get; set; }
    public string Name { get; set; }
}

public class TariffFilterModel
{
    public int Idx { get; set; }
    public string TariffCode { get; set; }
    public string Name { get; set; }
}