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
using System.Security.Claims;
using Data.Enums;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class MarketCountryExceptionController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _env;
        [Obsolete]
        public MarketCountryExceptionController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new MarketCountryExceptionService();
            var model = await srv.GetAllMarketExceptionCountries();

            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [GridDataSourceAction]
        public async Task<IActionResult> GetMarketExceptionCountries()
        {
            var srv = new MarketCountryService();
            var model = await srv.GetExceptionMarketCountries();
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new MarketCountryExceptionService();
            var modelOut = await srv.GetMarketCountryExceptionByID(_id);

            return View(modelOut);

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new MarketCountryExceptionService();
            var nRec = await ser.GetCount();
            var modelOut = new MarketCountryExceptionModel
            {
                MARKET_COUNTRY_ID = nRec + 1,
                ACTIVE = true,
                GEO_CODE = "",
                NAME_OLD = "",
                SHORT_LEGEND = "",
                LONG_LEGEND = "",
                REGION_NAME = "",
                SOURCE_COUNTRY_INDICATOR = "",
                REPLACED_GEO_CODE = "",
                START_DATE = DateTime.Now,
                DISCONTINUED_DATE = DateTime.Now,                
                NAME = ""                 
                
            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(MarketCountryExceptionModel model)
        {
            var srv = new MarketCountryExceptionService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Market Country Exception Details";
                var ser = new MarketCountryExceptionService();
                var nRec = await ser.GetCount();
                var modelOut = new MarketCountryExceptionModel
                {
                    MARKET_COUNTRY_ID = nRec + 1,
                    ACTIVE = true,
                    GEO_CODE = "",
                    NAME_OLD = "",
                    SHORT_LEGEND = "",
                    LONG_LEGEND = "",
                    REGION_NAME = "",
                    SOURCE_COUNTRY_INDICATOR = "",
                    REPLACED_GEO_CODE = "",
                    START_DATE = DateTime.Now,
                    DISCONTINUED_DATE = DateTime.Now,
                    NAME = ""

                };


                return View("Add", modelOut);
            }


            model.START_DATE = DateTime.Now;
            model.DISCONTINUED_DATE = DateTime.Now;          
            await srv.Add(model);

            return RedirectToAction("Index", "MarketCountryException");

        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(MarketCountryExceptionModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new MarketCountryExceptionService();
            model.START_DATE = DateTime.Now;
            model.DISCONTINUED_DATE = DateTime.Now;

            await srv.SaveMarketCountryException(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.MarketCountryExceptionModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Market Country Exception Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "MarketCountryException");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new MarketCountryExceptionService();
            var modelOut = await srv.GetMarketCountryExceptionByID(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.MarketCountryExceptionModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Market Country Exception Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "MarketCountryException");

        }


//        public async Task<ActionResult> PrintPage()
//        {
//            var rs = new ReportingService("http://issb.nathan-software.com:8081", "admin", "Letmein2019");

//            var report = await rs.RenderByNameAsync("market-countries-pdf", new
//            {
//                //Id = 123,
//                //From = "Erich Gamma",
//                //To = "Martin Fowler"
//            });

//            string fileName = "MarketCountries.pdf";
//            using (FileStream fs = System.IO.File.Create(this.GetPath(fileName)))
//            {
//                report.Content.CopyTo(fs);
//            }

//            string myurl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
//            ViewBag.Url = myurl + "/reports/MarketCountries.pdf";

//#if RELEASE
//          var releaseUrl = myurl.Replace("http", "https");
//          ViewBag.Url = releaseUrl + "/reports/MarketCountries.pdf";
//#endif

//            return View();

//        }

//        private string GetPath(string filename)
//        {
//            string path = _env.WebRootPath + "/reports/";

//            if (!Directory.Exists(path))
//            {
//                Directory.CreateDirectory(path);
//            }

//            return path + filename;

//        }
    }
}
