using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using Data.Models;
using Infragistics.Web.Mvc;
using System.Collections.Generic;
using System;
using Data.Enums;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class CurrencyCodesController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new CurrencyCodesService();
            var model = await srv.GetCurrencyCodes();
            IQueryable Data = model.AsQueryable();
            return View(Data);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new CurrencyCodesService();
            var modelOut = await srv.GetCurrencyCodesByID(_id);

            return View(modelOut);

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new CurrencyCodesService();
            var nRec = await ser.GetCount();
            var modelOut = new CurrencyCodesModel
            {
                CURRENCY_CODE = "",
                CURRENCY_NAME = "",
                CURRENCY_SYMBOL = ""
             
            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(CurrencyCodesModel model)
        {
            var srv = new CurrencyCodesService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Currency Codes Details";
                var ser = new CurrencyCodesService();
                var nRec = await ser.GetCount();
                var modelOut = new CurrencyCodesModel
                {
                    CURRENCY_CODE = "",
                    CURRENCY_NAME = "",
                    CURRENCY_SYMBOL = ""

                };


                return View("Add", modelOut);
            }
            await srv.Add(model);
            return RedirectToAction("Index", "CurrencyCodes");

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(CurrencyCodesModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new CurrencyCodesService();
            await srv.Save(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.CurrencyCodesModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Currency Code Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "CurrencyCodes");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new CurrencyCodesService();
            var modelOut = await srv.GetCurrencyCodesByID(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.CurrencyCodesModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Currency Code Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "CurrencyCodes");

        }
    }
}
