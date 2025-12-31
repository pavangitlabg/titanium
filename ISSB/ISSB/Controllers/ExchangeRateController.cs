using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Linq;
using Infragistics.Web.Mvc;
using System.Collections.Generic;
using System;
using jsreport.Client;
using Microsoft.AspNetCore.Hosting;
using System.IO;


namespace ISSB.Controllers
{
    public class ExchangeRateController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = OpenExchangeRateService.Instance;
            var model = await srv.GetRates();
            IQueryable data = model.AsQueryable();
            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> RebuildRates()
        {

            await BuildExchnageRates();

            return RedirectToAction("Index", "ExchangeRate");
        }

        private async Task BuildExchnageRates()
        {
            var srv = new ExchangeRateServiceOld();
            await srv.DeleteCollection();

           // string date = DateTime.Now.Date.ToString();
            int Month = DateTime.Now.Month;
            int Year = DateTime.Now.Year;


            var ExchangeList = new List<ExchangeModel>();
            for (int y = 2000; y <= Year; y++)
            {
                int mCnt = 12;
                if(y == Year)
                {
                    mCnt = Month;
                }

                for (int i = 1; i <= mCnt; i++)
                {
                    var param = y.ToString() + "-" + i.ToString().PadLeft(2,'0') + "-1"; 
                    var model = await srv.GetRateByDay(param);
                    model.ReportDate = param;
                   
                    ExchangeList.Add(model);
                }
            }
            
            await srv.AddRates(ExchangeList);
        }
    }
}
