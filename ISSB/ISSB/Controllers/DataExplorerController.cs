using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data.Enums;
using Data.Models;
using ISSB.Helpers;
using ISSB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ISSB.Controllers
{
    public class DataExplorerController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {

            var model = new TradeDataQueryModel { BatchNumber = 0 };

            return View(model);

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ExportEurofer(int ID)
        {
            if (ID <= 0)
            {
                return RedirectToAction("Index", "DataExplorer");
            }

            var model = new TradeDataQueryModel { BatchNumber = ID };
            var srv = new TradeDataService();
            var dataRecs = await srv.GetDataByBatchNumber(model.BatchNumber);


            if (dataRecs.Count == 0)
            {
                return RedirectToAction("Index", "DataExplorer");

            }

            var scSrv = new SourceCountryService();
            var scModel = await scSrv.GetSourceCountryByGeoCode(dataRecs[0].SC_GEO);
            var FileName = "EUROFER-" + scModel.NAME + "-" + dataRecs[0].MONTH.ToString().PadLeft(2,'0') + "-" + dataRecs[0].YEAR + ".txt";

            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            var YearNow = DateTime.Now.Year;
            var MonthNow = DateTime.Now.Month.ToString().PadLeft(2, '0');

            string sHead = string.Empty;
            int cntExports;
            int cntImports;
            var dataImports = dataRecs.Where(x => x.SIDE_OF_TRADE.Equals("I"));
            cntImports = dataImports.Count();
            if (cntImports > 0)
            {
                sHead = "1" + scModel.GEO_CODE + "T" + "1" + dataRecs[0].YEAR + dataRecs[0].MONTH.ToString().PadLeft(2, '0') + YearNow + MonthNow + "GBP" + "0" + "1";
                tw.WriteLine(sHead);
            }

            foreach (var Item in dataImports)
            {

                string sLine = "2" +
                    Item.IMPORT_TARIFF.PadRight(11, ' ') +
                    Item.MC_GEO + Item.MC_GEO +
                    Math.Ceiling(Item.WEIGHT).ToString().PadLeft(12, '0') +
                    Math.Ceiling(Item.MONETARY_VALUE).ToString().PadLeft(12, '0') +
                    Math.Ceiling(Item.YTD_WEIGHT).ToString().PadLeft(12, '0') +
                    Math.Ceiling(Item.YTD_MONETARY_VALUE).ToString().PadLeft(12, '0');
                tw.WriteLine(sLine);

            }

            if (cntImports > 0)
                tw.WriteLine("3" + cntImports.ToString());

            var dataExports = dataRecs.Where(x => x.SIDE_OF_TRADE.Equals("E"));
            cntExports = dataExports.Count();
            if (cntExports > 1)
            {
                sHead = "1" + scModel.GEO_CODE + "T" + "2" + dataRecs[0].YEAR + dataRecs[0].MONTH.ToString().PadLeft(2, '0') + YearNow + MonthNow + "GBP" + "0" + "1";
                tw.WriteLine(sHead);
            }

            foreach (var Item in dataExports)
            {
                string sLine = "2" +
                    Item.IMPORT_TARIFF.PadRight(11, ' ') +
                    Item.MC_GEO + Item.MC_GEO +
                    Math.Ceiling(Item.WEIGHT).ToString().PadLeft(12, '0') +
                     Math.Ceiling(Item.MONETARY_VALUE).ToString().PadLeft(12, '0') +
                     Math.Ceiling(Item.YTD_WEIGHT).ToString().PadLeft(12, '0') +
                     Math.Ceiling(Item.YTD_MONETARY_VALUE).ToString().PadLeft(12, '0');
                tw.WriteLine(sLine);

            }

            if (cntExports > 1)
                tw.WriteLine("3" + cntExports.ToString());

            int Total = cntExports + cntImports;

            tw.WriteLine("Total item(s) : " + Total.ToString());

            tw.Flush();


            var length = ms.Length;
            tw.Close();
            var toWrite = new byte[length];
            Array.Copy(ms.GetBuffer(), 0, toWrite, 0, length);
            ms.Close();
            return File(toWrite, "text/plain", FileName);

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ExportGTT(int ID)
        {

            if (ID <= 0)
            {
                return RedirectToAction("Index", "DataExplorer");
            }

            var model = new TradeDataQueryModel { BatchNumber = ID };
            var srv = new TradeDataService();
            var dataRecs = await srv.GetDataByBatchNumber(model.BatchNumber);

            if (dataRecs.Count == 0)
            {
                return RedirectToAction("Index", "DataExplorer");
            }

            var scSrv = new SourceCountryService();
            var scModel = await scSrv.GetSourceCountryByGeoCode(dataRecs[0].SC_GEO);
            var DateString = new DateTime(dataRecs[0].YEAR, dataRecs[0].MONTH, 1).ToString("MMMM yyy");
            var FileName = "GTT-" + scModel.NAME + "-" + dataRecs[0].MONTH + "-" + dataRecs[0].YEAR + ".csv";

           // ViewBag.Title = "Trade Data for: [" + scModel.GEO_CODE + "] " + scModel.NAME + " " + DateString + " Batch No: " + model.BatchNumber;

            var mcSrv = new MarketCountryService();
            var mcList = await mcSrv.GetActiveMarketCountries();


            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            int cnt = 0, Total = 0;
            tw.WriteLine("YEAR,MONTH,GEO,COUNTRY NAME,FLOW,HS CODE,GEO,PARTNER COUNTRY,QUANTITY,UNIT,VALUE");
            Total++;
            foreach (var Item in dataRecs)
            {
                var FLOW = string.Empty;
                if (Item.SIDE_OF_TRADE.Equals("I"))
                    FLOW = "Import";
                else
                    FLOW = "Export";
                var MC_NAME = mcList.Find(x => x.GEO_CODE.Equals(Item.MC_GEO)).NAME;
                string sLine = Item.YEAR + "," + Item.MONTH + "," + Item.SC_GEO.PadLeft(3,'0') + "," +
                               scModel.NAME + "," + FLOW + "," + Item.IMPORT_TARIFF + "," + Item.MC_GEO +
                               "," + MC_NAME + "," + Item.WEIGHT.ToString() + ",KG," + Item.MONETARY_VALUE.ToString();


                tw.WriteLine(sLine);
                cnt++;
                Total++;
            }

            //tw.WriteLine(cnt.ToString());
            //Total++;
            //tw.WriteLine("Total item(s) : " + Total.ToString());

            tw.Flush();



            var length = ms.Length;
            tw.Close();
            var toWrite = new byte[length];
            Array.Copy(ms.GetBuffer(), 0, toWrite, 0, length);
            ms.Close();
            return File(toWrite, "text/plain", FileName);
        }

        [Authorize]
        public async Task<IActionResult> Execute(TradeDataQueryModel model)
        {

            if (model.BatchNumber <= 0)
            {
                ViewBag.Title = "Invalid Batch Number";

                return RedirectToAction("Index", "DataExplorer");

            }

            var srv = new TradeDataService();
            var dataRecs = await srv.GetDataByBatchNumber(model.BatchNumber);

            if(dataRecs.Count == 0)
            {
                ViewBag.Title = "No Data found for this batch : " + model.BatchNumber;
                IQueryable rdata = dataRecs.AsQueryable();
                return View(rdata);
               
            }

            var scSrv = new SourceCountryService();
            var scModel = await scSrv.GetSourceCountryByGeoCode(dataRecs[0].SC_GEO);
            var DateString = new DateTime(dataRecs[0].YEAR, dataRecs[0].MONTH, 1).ToString("MMMM yyy");
            ViewBag.Title = "Trade Data for: [" + scModel.GEO_CODE + "] " + scModel.NAME + " " + DateString + " Batch No: " + model.BatchNumber;

            IQueryable data = dataRecs.AsQueryable();
            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new TradeDataService();
            var model = await srv.GetTradeDataRecord(_id);
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Save(TradeDataModel model)
        {
            var srv = new TradeDataService();
            var resultModel = await srv.GetTradeDataRecord(model._id);
            resultModel.MONETARY_VALUE = model.MONETARY_VALUE;
            resultModel.WEIGHT = model.WEIGHT;
            resultModel.YTD_MONETARY_VALUE = model.YTD_MONETARY_VALUE;
            resultModel.YTD_WEIGHT = model.YTD_WEIGHT;


            await srv.Save(resultModel);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.TradeDataModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Data Explorer Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "DataExplorer");
        }

     
    }
}
