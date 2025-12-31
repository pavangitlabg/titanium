using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using Infragistics.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace ISSB.Controllers
{
    public class ImportMonitorController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }


        [Authorize]
        [HttpGet]
        public async Task<string> GetData()
        {
            var srv = new ImportErrorService();
            var dList = await srv.GetRecords();
            var sList = dList.OrderByDescending(x => x._id).ToList();

            var rData = new ImportErrorLogJsonModel { data = sList };

            var routeOb = JsonConvert.SerializeObject(rData);

            return routeOb;

        }

        [Authorize]
        public async Task<IActionResult> UpdateGrid()
        {
            var srv = new ImportErrorService();
            var model = await srv.GetRecords();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteExceptions(string DataArray)
        {

            var ImpErrorSrv = new ImportErrorService();
            var deleteList = new List<string>();
            var data = DataArray.TrimEnd(DataArray[DataArray.Length - 1]);

            if (data.Contains("|"))
            {

                string[] Items = data.Split('|');

                foreach (string Item in Items)
                {
                    string[] Type = Item.Split(',');
                    deleteList.Add(Type[2]);
                }

                foreach (var delItem in deleteList)
                {
                    await ImpErrorSrv.DeleteRecord(delItem);
                }
            }
            else
            {
                string[] Item1 = data.Split(',');
                await ImpErrorSrv.DeleteRecord(Item1[2]);
            }


           
            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostExceptions(string DataArray)
        {
            var marketExpSrv = new MarketCountryService();
            var sourceExpSrv = new SourceCountryService();
            var tariffExpSrv = new TariffExceptionsService();
            var ImpErrorSrv = new ImportErrorService();
            var deleteList = new List<string>();
            var data = DataArray.TrimEnd(DataArray[DataArray.Length - 1]);

            if (data.Contains("|"))
            {
                
                string[] Items = data.Split('|');
               
                foreach (string Item in Items)
                {
                    string[] Type = Item.Split(',');
                    deleteList.Add(Type[2]);
                    if (Type[0].Equals("1"))
                    {
                        //Update Source Country Exception
                        var mModel = new SourceCountryExceptionModel
                        {
                            ACTIVE = false,
                            DISCONTINUED_DATE = DateTime.Now,
                            GEO_CODE = Type[1],
                            LONG_LEGEND = string.Empty,
                            SOURCE_COUNTRY_ID = 9999,
                            NAME = string.Empty,
                            NAME_OLD = string.Empty,
                            REGION_NAME = string.Empty,
                            REPLACED_GEO_CODE = Type[1],
                            SHORT_LEGEND = string.Empty,
                            SOURCE_COUNTRY_INDICATOR = string.Empty,
                            START_DATE = DateTime.Now

                        };

                        await sourceExpSrv.AddException(mModel);
                    }

                    if (Type[0].Equals("2"))
                    {
                        //Update Market Country Exception

                        var mModel = new MarketCountryExceptionModel
                        {
                            ACTIVE = false,
                            DISCONTINUED_DATE = DateTime.Now,
                            GEO_CODE = Type[1],
                            LONG_LEGEND = string.Empty,
                            MARKET_COUNTRY_ID = 9999,
                            NAME = string.Empty,
                            NAME_OLD = string.Empty,
                            REGION_NAME = string.Empty,
                            REPLACED_GEO_CODE = Type[1],
                            SHORT_LEGEND = string.Empty,
                            SOURCE_COUNTRY_INDICATOR = string.Empty,
                            START_DATE = DateTime.Now

                        };

                        await marketExpSrv.AddException(mModel);

                    }
                    if (Type[0].Equals("3"))
                    {
                        //Update Tariff Exception

                        var tModel = new TariffExceptionsModel
                        {
                            TARIFF_ID = 0,
                            SOURCE_COUNTRY_TARIFF_CODE = Type[1],
                            HARMONISED_TARIFF_CODE = Type[1].Substring(0, 6),
                            SIDE_OF_TRADE = "B",
                            DISCONTINUED_DATE = DateTime.Now,
                            HARMONISED_TARIFF_LONG_LEGEND = string.Empty,
                            HARMONISED_TARIFF_SHORT_LEGEND = string.Empty,
                            HARMONISED_TARIFF_SHORT_LEGEND_BACKUP = string.Empty,
                            ISSB_STORED_CODE = string.Empty,
                            ISSB_STORED_LEGEND = string.Empty,
                            LOWER_VPT = string.Empty,
                            SOURCE_COUNTRY_TARIFF_LONG_LEGEND = string.Empty,
                            SOURCE_COUNTRY_TARIFF_SHORT_LEGEND = string.Empty,
                            START_DATE = DateTime.Now,
                            TARIFF_CODE_TABLE_CODE = string.Empty,
                            UPPER_VPT = string.Empty,
                            WTO_ALLOY_CODE = string.Empty,
                            WTO_ALLOY_LEGEND = string.Empty,
                            WTO_CODE = string.Empty,
                            WTO_CODE_LEGEND = string.Empty

                        };

                        await tariffExpSrv.Add(tModel);

                    }

                }
            }
            else
            {
                string[] sType = data.Split(',');
                deleteList.Add(sType[2]);
                if (sType[0].Equals("1"))
                {
                    //Update Source Country Exception
                    //Update Source Country Exception
                    var mModel = new SourceCountryExceptionModel
                    {
                        ACTIVE = false,
                        DISCONTINUED_DATE = DateTime.Now,
                        GEO_CODE = sType[1],
                        LONG_LEGEND = string.Empty,
                        SOURCE_COUNTRY_ID = 9999,
                        NAME = string.Empty,
                        NAME_OLD = string.Empty,
                        REGION_NAME = string.Empty,
                        REPLACED_GEO_CODE = sType[1],
                        SHORT_LEGEND = string.Empty,
                        SOURCE_COUNTRY_INDICATOR = string.Empty,
                        START_DATE = DateTime.Now

                    };

                    await sourceExpSrv.AddException(mModel);
                }
                if (sType[0].Equals("2"))
                {
                    //Update Market Country Exception
                    var mModel = new MarketCountryExceptionModel
                    {
                        ACTIVE = false,
                        DISCONTINUED_DATE = DateTime.Now,
                        GEO_CODE = sType[1],
                        LONG_LEGEND = string.Empty,
                        MARKET_COUNTRY_ID = 0,
                        NAME = string.Empty,
                        NAME_OLD = string.Empty,
                        REGION_NAME = string.Empty,
                        REPLACED_GEO_CODE = sType[1],
                        SHORT_LEGEND = string.Empty,
                        SOURCE_COUNTRY_INDICATOR = string.Empty,
                        START_DATE = DateTime.Now

                    };

                    await marketExpSrv.AddException(mModel);
                }
                if (sType[0].Equals("3"))
                {
                    //Update Tariff Exception

                    var tModel = new TariffExceptionsModel
                    {
                        TARIFF_ID = 0,
                        SOURCE_COUNTRY_TARIFF_CODE = sType[1],
                        HARMONISED_TARIFF_CODE = sType[1].Substring(0, 6),
                        SIDE_OF_TRADE = "B",
                        DISCONTINUED_DATE = DateTime.Now,
                        HARMONISED_TARIFF_LONG_LEGEND = string.Empty,
                        HARMONISED_TARIFF_SHORT_LEGEND = string.Empty,
                        HARMONISED_TARIFF_SHORT_LEGEND_BACKUP = string.Empty,
                        ISSB_STORED_CODE = string.Empty,
                        ISSB_STORED_LEGEND = string.Empty,
                        LOWER_VPT = string.Empty,
                        SOURCE_COUNTRY_TARIFF_LONG_LEGEND = string.Empty,
                        SOURCE_COUNTRY_TARIFF_SHORT_LEGEND = string.Empty,
                        START_DATE = DateTime.Now,
                        TARIFF_CODE_TABLE_CODE = string.Empty,
                        UPPER_VPT = string.Empty,
                        WTO_ALLOY_CODE = string.Empty,
                        WTO_ALLOY_LEGEND = string.Empty,
                        WTO_CODE = string.Empty,
                        WTO_CODE_LEGEND = string.Empty

                    };

                    await tariffExpSrv.Add(tModel);

                }

            }

            foreach(var delItem in deleteList)
            {
                
                await ImpErrorSrv.DeleteRecord(delItem);

            }
            return Json(new { success = true });
        }
    }
}
