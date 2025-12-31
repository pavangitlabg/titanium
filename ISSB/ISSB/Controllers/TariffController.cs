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
    public class TariffController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _env;
        [Obsolete]
        public TariffController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new TariffService();
            var model = await srv.GetTariffs();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new TariffService();
            var modelOut = await srv.GetTariffByID(_id);

            return View(modelOut);

        }

        [Authorize]
        public async Task<IActionResult> GetTop1000(string _id)
        {

            var srv = new TariffService();
            var model = await srv.GetTariffByTariffID(_id);

            ViewBag.Message = model.SOURCE_COUNTRY_TARIFF_CODE + " " + model.SOURCE_COUNTRY_TARIFF_LONG_LEGEND;

            var modelOut = await srv.GetTop1000Transactions(_id);
            IQueryable data = modelOut.AsQueryable();
            return View(data);

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new TariffService();
            var nRec = await ser.GetCount();
            var modelOut = new TariffModel
            {
                TARIFF_ID = nRec + 1,
                WTO_CODE = "",
                WTO_CODE_LEGEND = "",
                WTO_ALLOY_CODE = "",
                WTO_ALLOY_LEGEND = "",
                ISSB_STORED_CODE = "",
                ISSB_STORED_LEGEND = "",
                HARMONISED_TARIFF_CODE = "",
                HARMONISED_TARIFF_SHORT_LEGEND_BACKUP = "",
                HARMONISED_TARIFF_LONG_LEGEND = "",
                SOURCE_COUNTRY_TARIFF_CODE = "",
                SOURCE_COUNTRY_TARIFF_SHORT_LEGEND = "",
                SOURCE_COUNTRY_TARIFF_LONG_LEGEND = "",
                LOWER_VPT = "",
                UPPER_VPT = "",
                START_DATE = DateTime.Now,
                DISCONTINUED_DATE = DateTime.Now,
                SIDE_OF_TRADE = "",
                TARIFF_CODE_TABLE_CODE = "",
                HARMONISED_TARIFF_SHORT_LEGEND = ""
            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(TariffModel model)
        {
            var srv = new TariffService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Tariff Details";
                var ser = new TariffService();
                var nRec = await ser.GetCount();
                var modelOut = new TariffModel
                {
                    TARIFF_ID = nRec + 1,
                    WTO_CODE = "",
                    WTO_CODE_LEGEND = "",
                    WTO_ALLOY_CODE = "",
                    WTO_ALLOY_LEGEND = "",
                    ISSB_STORED_CODE = "",
                    ISSB_STORED_LEGEND = "",
                    HARMONISED_TARIFF_CODE = "",
                    HARMONISED_TARIFF_SHORT_LEGEND_BACKUP = "",
                    HARMONISED_TARIFF_LONG_LEGEND = "",
                    SOURCE_COUNTRY_TARIFF_CODE = "",
                    SOURCE_COUNTRY_TARIFF_SHORT_LEGEND = "",
                    SOURCE_COUNTRY_TARIFF_LONG_LEGEND = "",
                    LOWER_VPT = "",
                    UPPER_VPT = "",
                    START_DATE = DateTime.Now,
                    DISCONTINUED_DATE = DateTime.Now,
                    SIDE_OF_TRADE = "",
                    TARIFF_CODE_TABLE_CODE = "",
                    HARMONISED_TARIFF_SHORT_LEGEND = ""

                };


                return View("Add", modelOut);
            }

            model.START_DATE = DateTime.Now;
            model.DISCONTINUED_DATE = DateTime.Now;
            int foundIndex = await srv.GetLastIndex();

            if(foundIndex != 0)
                model.TARIFF_ID = foundIndex;

            await srv.Add(model);

            return RedirectToAction("Index", "Tariff");

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(TariffModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new TariffService();
            model.START_DATE = DateTime.Now;
            model.DISCONTINUED_DATE = DateTime.Now;

            await srv.Save(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.TariffModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Tariff Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "Tariff");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new TariffService();

            var modelOut = await srv.GetTariffByID(_id);

            int foundIndex = await srv.GetOnSystemIndex(modelOut);

            if (foundIndex == 0)
            {
                await srv.Delete(modelOut);

                var UserId = User.FindFirst(ClaimTypes.Email).Value;
                var EventModel = new SystemEventLogsModel
                {
                    DataType = DataTypeEnums.TariffModel,
                    TransactionType = TransactionTypeEnums.Delete,
                    MESSAGE = "Tariff Deleted, [" + UserId + "]",
                    DATE = DateTime.Now,
                    USER = UserId,
                    Model = modelOut
                };
                new EventLog(EventModel);
            }
            return RedirectToAction("Index", "Tariff");

        }
    }
}
