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
    public class SourceCountryController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _env;
        [Obsolete]
        public SourceCountryController(IHostingEnvironment env)
        {
            _env = env;
        }


        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new SourceCountryService();
            var model = await srv.GetSourceCountries();


            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new SourceCountryService();
            var modelOut = await srv.GetSourceCountryByID(_id);

            return View(modelOut);

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new SourceCountryService();
            var nRec = await ser.GetCount();
            var modelOut = new SourceCountryModel
            {
                SOURCE_COUNTRY_ID = nRec + 1,
                ACTIVE = "",
                CURRENCY_CODE = "",
                DATA_FORMAT = "",
                FIRST_DATE = DateTime.Now,
                FREQUENCY = "",
                GEO_CODE = "",
                GEO_CODE_DISCONTINUED_DATE = DateTime.Now,
                INDUSTRY_REPORTING_CENTRE_ID = 0,
                LATEST_DATE = DateTime.Now,
                LONG_LEGEND = "",
                NAME = "",
                NAME_OLD = "",
                OLD_GEO_CODE = "",
                REGION_NAME = "",
                SHORT_LEGEND = "",
                SIDE_OF_TRADE = ""                
            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(SourceCountryModel model)
        {
            var srv = new SourceCountryService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Source Country Details";
                var ser = new SourceCountryService();
                var nRec = await ser.GetCount();
                var modelOut = new SourceCountryModel
                {
                    SOURCE_COUNTRY_ID = nRec + 1,
                    ACTIVE = "",
                    CURRENCY_CODE = "",
                    DATA_FORMAT = "",
                    FIRST_DATE = DateTime.Now,
                    FREQUENCY = "",
                    GEO_CODE = "",
                    GEO_CODE_DISCONTINUED_DATE = DateTime.Now,
                    INDUSTRY_REPORTING_CENTRE_ID = 0,
                    LATEST_DATE = DateTime.Now,
                    LONG_LEGEND = "",
                    NAME = "",
                    NAME_OLD = "",
                    OLD_GEO_CODE = "",
                    REGION_NAME = "",
                    SHORT_LEGEND = "",
                    SIDE_OF_TRADE = ""

                };


                return View("Add", modelOut);
            }

            model.LATEST_DATE = DateTime.Now;
            model.GEO_CODE_DISCONTINUED_DATE = DateTime.Now;
            model.FIRST_DATE = DateTime.Now;

            await srv.Add(model);

            return RedirectToAction("Index", "SourceCountry");

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(SourceCountryModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new SourceCountryService();            
            model.LATEST_DATE = DateTime.Now;            

            await srv.SaveSourceCountry(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.SourceCountryModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Source Country Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "SourceCountry");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new SourceCountryService();
            var modelOut = await srv.GetSourceCountryByID(_id);            

            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.SourceCountryModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Source Country Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "SourceCountry");

        }

        //[GridDataSourceAction]
        //public async Task<IActionResult> GetSourceCountries()
        //{
        //    var srv = new SourceCountryService();
        //    var model = await srv.GetSourceCountries();
            
        //    IQueryable data = model.AsQueryable();
        //    return View(data);
        //}   

        //public async Task<ActionResult> SaveSourceCountry()
        //{
        //    GridModel gridModel = new GridModel();
        //    List<Transaction<SourceCountryModel>> transactions = gridModel.LoadTransactions<SourceCountryModel>(HttpContext.Request.Form["ig_transactions"]);

        //    var srv = new SourceCountryService();
        //    //var model = await srv.GetWorldCities();

        //    var countryList = new List<SourceCountryModel>();
        //    foreach (Transaction<SourceCountryModel> t in transactions)
        //    {
        //        if (t.type == "row")
        //        {
        //            var model = new SourceCountryModel();

        //            if (t.row.SOURCE_COUNTRY_ID != 0) model.SOURCE_COUNTRY_ID = t.row.SOURCE_COUNTRY_ID;

        //            if (t.row.LONG_LEGEND != null) model.LONG_LEGEND = t.row.LONG_LEGEND;

        //            if (t.row.NAME != null) model.NAME = t.row.NAME;
                    
        //            if (t.row.NAME_OLD != null) model.NAME_OLD = t.row.NAME_OLD;
                    
        //            if (t.row.OLD_GEO_CODE != null) model.OLD_GEO_CODE = t.row.OLD_GEO_CODE;
                    
        //            if (t.row.REGION_NAME != null) model.REGION_NAME = t.row.REGION_NAME;
                    
        //            if (t.row.SHORT_LEGEND != null) model.SHORT_LEGEND = t.row.SHORT_LEGEND;
                    
        //            if (t.row.SIDE_OF_TRADE != null) model.SIDE_OF_TRADE = t.row.SIDE_OF_TRADE;
                    

        //            model.INDUSTRY_REPORTING_CENTRE_ID = t.row.INDUSTRY_REPORTING_CENTRE_ID;
        //            model.LATEST_DATE = t.row.LATEST_DATE;


        //            countryList.Add(model);
        //        }

        //    }

        //    foreach (var Item in countryList)
        //    {
        //        await srv.SaveSourceCountry(Item);
        //    }

        //    return View(countryList.ToArray());

        //}

        public async Task<ActionResult> PrintPage()
        {
            var rs = new ReportingService("http://issb.nathan-software.com:8081","admin","Letmein2019");

            var report = await rs.RenderByNameAsync("source-countries-pdf", new
            {
                //Id = 123,
                //From = "Erich Gamma",
                //To = "Martin Fowler"
            });

            string fileName = "SourceCountries.pdf";
            using(FileStream fs = System.IO.File.Create(this.GetPath(fileName)))
            {
                report.Content.CopyTo(fs);
            }

            string myurl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            ViewBag.Url = myurl + "/reports/SourceCountries.pdf";

#if RELEASE
          var releaseUrl = myurl.Replace("http", "https");
          ViewBag.Url = releaseUrl + "/reports/SourceCountries.pdf";
#endif

            return View();

        }

        private string GetPath(string filename)
        {
            string path = _env.WebRootPath + "/reports/";

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path + filename;

        }
    }
}
