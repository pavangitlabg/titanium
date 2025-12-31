using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ISSB.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class JSReportsController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeDataModel>>> GetData()
        {
            var srv = new TradeDataService();
            var data = await srv.GetTradeData(2018,6,4,117);

            return data;
        }
    }
}
