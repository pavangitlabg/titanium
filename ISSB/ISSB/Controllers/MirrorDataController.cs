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
using System.Collections;

namespace ISSB.Controllers
{
    public class MirrorDataController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
           
            string mn = DateTime.Now.Month.ToString();
            string yy = DateTime.Now.Year.ToString();

            var model = new MirrorDataModel
            {
                TariffCodes = "260111,260112",
                Year = int.Parse(yy),
                Month = int.Parse(mn),
                GeoCode = "038"
            };
            
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Execute(MirrorDataModel model)
        {
            ViewBag.Year = model.Year;
            ViewBag.Month = model.Month;
            ViewBag.GeoCode = model.GeoCode;
            ViewBag.TariffCodes = model.TariffCodes;

            var srv = new TradeDataService();
            var dataRecs = await srv.GetDataToMirror(model);
            IQueryable data = dataRecs.AsQueryable();
            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> PostExecute(string GeoCode, int Year, int Month, string TariffCodes)
        {
            
            var sysCont = new SystemControlService();
            var sysControlModel = await sysCont.GetSystemControl();
            var BatchNumber = sysControlModel.ImportBatch;
            BatchNumber++;
            sysControlModel.ImportBatch = BatchNumber;
            await sysCont.UpdateControl(sysControlModel);

            var pendingList = new List<PendingImportModel>();
            var model = new MirrorDataModel { Year = Year, Month = Month, GeoCode = GeoCode, TariffCodes = TariffCodes };
            var srv = new TradeDataService();
            
            var dataRecs = await srv.GetDataToMirror(model);
            //Needs further investigation

            foreach (var Item in dataRecs)
            {
                var importModel = new PendingImportModel
                {
                    CURRENCY_CODE = "EUR",
                    BATCH_NO = (int)BatchNumber,
                    YEAR = Item.YEAR,
                    MONTH = Item.MONTH,
                    QUARTER = Item.QUARTER,
                    SC_GEO = Item.MC_GEO,
                    CWC_GEO_CODE = Item.MC_GEO,
                    ESTIMATED = Item.ESTIMATED,
                    H_TARIFF = Item.H_TARIFF,
                    IMPORT_TARIFF = Item.IMPORT_TARIFF,
                    IMP_UNIT = Item.IMP_UNIT,
                    ID = Item.ID,
                    MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                    MC_GEO = Item.SC_GEO,
                    MONETARY_VALUE = Item.MONETARY_VALUE,
                    PORT_ALPHA = string.Empty,
                    PORT_ID = Item.PORT_ID,
  
                    COO_GEO_CODE = Item.MC_GEO,
                    SIDE_OF_TRADE = "I",
                    SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                    TARIFF_ID = Item.TARIFF_ID,
                    TIME_ID = Item.TIME_ID,
                    VALUE_PER_TONNE = 0,
                    WEIGHT = Item.WEIGHT,
                    YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                    YTD_WEIGHT = Item.YTD_WEIGHT

                };
                pendingList.Add(importModel);
            }

            //Post Data
            //Add Header
            var varSrv = new ValidationEngineService();
            string status = "Batch: " + BatchNumber + " Data For: " + GeoCode + " AUSTRIA " + Month + " - " + Year;

            var headModel = new PendingImportHeaderModel
            {
                BATCH_NO = (int)BatchNumber,
                DATE = DateTime.Now,
                END_DATE = DateTime.Now,
                FAIL = false,
                IMPORT_TYPE = "AUSTRIA",
                POSTED = false,
                STATUS = status,
                TIME_TAKEN = "",

            };

            await varSrv.AddHeader(headModel);
          
            await varSrv.AddForValidation(pendingList);

            return RedirectToAction("Index", "Upsert");
        }
    }
}
