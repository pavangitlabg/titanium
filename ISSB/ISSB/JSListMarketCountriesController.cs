using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ISSB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JSListMarketCountriesController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<ActionResult<MarketCountryJsonModel>> GetSourceCountries()
        {
            var srv = new MarketCountryService();
            var data = await srv.GetMarketAllCountries();

            var CountryList = new List<MarketCountryModel>();

            foreach (var d in data)
            {
                CountryList.Add(d);
            }

            var cList = CountryList.OrderBy(s => s.MARKET_COUNTRY_ID).ToList();

            var dataOut = new MarketCountryJsonModel { Countries = cList };


            return dataOut;
        }
    }
}
