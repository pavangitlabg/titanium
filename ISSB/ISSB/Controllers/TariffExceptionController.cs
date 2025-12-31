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
    public class TariffController : Controller
    {
        private IHostingEnvironment _env;
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
            return RedirectToAction("Index", "Tariff");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new TariffService();
            var modelOut = await srv.GetTariffByID(_id);
            await srv.Delete(modelOut);

            return RedirectToAction("Index", "Tariff");

        }
    }
}
