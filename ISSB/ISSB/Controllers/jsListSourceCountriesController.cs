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
    public class JSListSourceCountriesController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<ActionResult<SourceCountryJsonModel>> GetSourceCountries()
        {
            var srv = new SourceCountryService();
            var data = await srv.GetSourceCountries();

            var CountryList = new List<SourceCountryModel>();

            foreach (var d in data)
            {
                CountryList.Add(d);
            }

            var cList = CountryList.OrderBy(s => s.SOURCE_COUNTRY_ID).ToList();

            var dataOut = new SourceCountryJsonModel { Countries = cList };

        
            return dataOut;
        }
    }
}
