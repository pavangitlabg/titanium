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
    public class TariffExceptionsController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _env;
        [Obsolete]
        public TariffExceptionsController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new TariffExceptionsService();
            var model = await srv.GetTariffs();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new TariffExceptionsService();
            var modelOut = await srv.GetTariffExceptionsByID(_id);

            return View(modelOut);

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new TariffExceptionsService();
            var nRec = await ser.GetCount();
            var modelOut = new TariffExceptionsModel
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
        public async Task<IActionResult> AddNew(TariffExceptionsModel model)
        {
            var srv = new TariffExceptionsService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Tariff Exceptions Details";
                var ser = new TariffExceptionsService();
                var nRec = await ser.GetCount();
                var modelOut = new TariffExceptionsModel
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

            await srv.Add(model);

            return RedirectToAction("Index", "TariffExceptions");

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(TariffExceptionsModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new TariffExceptionsService();
            model.START_DATE = DateTime.Now;
            model.DISCONTINUED_DATE = DateTime.Now;

            await srv.Save(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.TariffExceptionsModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Tariff Exceptions Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "TariffExceptions");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new TariffExceptionsService();
            var modelOut = await srv.GetTariffExceptionsByID(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.TariffExceptionsModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Tariff Exceptions Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "TariffExceptions");

        }
    }
}
