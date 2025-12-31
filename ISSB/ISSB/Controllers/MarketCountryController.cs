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
using Data.Enums;
using System.Security.Claims;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class MarketCountryController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _env;

        [Obsolete]
        public MarketCountryController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new MarketCountryService();
            var model = await srv.GetActiveMarketCountries();

            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [GridDataSourceAction]
        public async Task<IActionResult> GetMarketCountries()
        {
            var srv = new MarketCountryService();
            var model = await srv.GetMarketAllCountries();
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new MarketCountryService();
            var modelOut = await srv.GetMarketCountryByID(_id);

            return View(modelOut);

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new MarketCountryService();
            var nRec = await ser.GetCount();
            var modelOut = new MarketCountryModel
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
        public async Task<IActionResult> AddNew(MarketCountryModel model)
        {
            var srv = new MarketCountryService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Market Country Details";
                var ser = new MarketCountryService();
                var nRec = await ser.GetCount();
                var modelOut = new MarketCountryModel
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

            return RedirectToAction("Index", "MarketCountry");

        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(MarketCountryModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new MarketCountryService();
            model.START_DATE = DateTime.Now;
            model.DISCONTINUED_DATE = DateTime.Now;
            await srv.SaveMarketCountry(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.MarketCountryModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Market Country Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "MarketCountry");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new MarketCountryService();
            var modelOut = await srv.GetMarketCountryByID(_id);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.MarketCountryModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Market Country Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            await srv.Delete(modelOut);

            

            return RedirectToAction("Index", "MarketCountry");

        }

        //[Authorize]
        //public async Task<IActionResult> Delete(int Id)
        //{
        //    var srv = new MarketCountryService();
        //    var modelOut = await srv.GetMarketCountryByID(Id);
        //    await srv.Delete(modelOut);

        //    return RedirectToAction("Index", "MarketCountry");

        //}

        //public async Task<ActionResult> SaveMarketCountry()
        //{
        //    GridModel gridModel = new GridModel();
        //    List<Transaction<MarketCountryModel>> transactions = gridModel.LoadTransactions<MarketCountryModel>(HttpContext.Request.Form["ig_transactions"]);

        //    var srv = new MarketCountryService();
        //    //var model = await srv.GetWorldCities();

        //    var marketList = new List<MarketCountryModel>();
        //    foreach (Transaction<MarketCountryModel> t in transactions)
        //    {
        //        if (t.type == "row")
        //        {
        //            var model = new MarketCountryModel();

        //            if (t.row.MARKET_COUNTRY_ID != 0) model.MARKET_COUNTRY_ID = t.row.MARKET_COUNTRY_ID;

        //            if (t.row.LONG_LEGEND != null) model.LONG_LEGEND = t.row.LONG_LEGEND;

        //            if (t.row.NAME != null) model.NAME = t.row.NAME;

        //            if (t.row.NAME_OLD != null) model.NAME_OLD = t.row.NAME_OLD;

        //            if (t.row.REPLACED_GEO_CODE != null) model.REPLACED_GEO_CODE = t.row.REPLACED_GEO_CODE;

        //            if (t.row.REGION_NAME != null) model.REGION_NAME = t.row.REGION_NAME;

        //            if (t.row.SHORT_LEGEND != null) model.SHORT_LEGEND = t.row.SHORT_LEGEND;

        //            if (t.row.SOURCE_COUNTRY_INDICATOR != null) model.SOURCE_COUNTRY_INDICATOR = t.row.SOURCE_COUNTRY_INDICATOR;

        //            model.START_DATE = t.row.START_DATE;


        //            marketList.Add(model);
        //        }

        //    }

        //    foreach (var Item in marketList)
        //    {
        //        await srv.SaveMarketCountry(Item);
        //    }

        //    return View(marketList.ToArray());

        //}


        public async Task<ActionResult> PrintPage()
        {
            var rs = new ReportingService("http://issb.nathan-software.com:8081", "admin", "Letmein2019");

            var report = await rs.RenderByNameAsync("market-countries-pdf", new
            {
                //Id = 123,
                //From = "Erich Gamma",
                //To = "Martin Fowler"
            });

            string fileName = "MarketCountries.pdf";
            using (FileStream fs = System.IO.File.Create(this.GetPath(fileName)))
            {
                report.Content.CopyTo(fs);
            }

            string myurl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            ViewBag.Url = myurl + "/reports/MarketCountries.pdf";

#if RELEASE
          var releaseUrl = myurl.Replace("http", "https");
          ViewBag.Url = releaseUrl + "/reports/MarketCountries.pdf";
#endif

            return View();

        }

        private string GetPath(string filename)
        {
            string path = _env.WebRootPath + "/reports/";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path + filename;

        }
    }
}
