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
    public class TariffIndexController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _env;
        [Obsolete]
        public TariffIndexController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new TariffIndexService();
            var model = await srv.GetTariffIndex();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new TariffIndexService();
            var modelOut = await srv.GetTariffIndexByID(_id);

            return View(modelOut);

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new TariffIndexService();
            var nRec = await ser.GetCount();
            var modelOut = new TariffIndexModel
            {
                Name = "",
                Code = "",
                Description = "",
                Active = true 
            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(TariffIndexModel model)
        {
            var srv = new TariffIndexService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Tariff Index Details";
                var ser = new TariffIndexService();
                var nRec = await ser.GetCount();
                var modelOut = new TariffIndexModel
                {
                    Name = "",
                    Code = "",
                    Description = "",
                    Active = true
                };


                return View("Add", modelOut);
            }           

            await srv.Add(model);

            return RedirectToAction("Index", "TariffIndex");

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(TariffIndexModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new TariffIndexService();            

            await srv.Save(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.TariffIndexModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Tariff Index Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "TariffIndex");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new TariffIndexService();
            var modelOut = await srv.GetTariffIndexByID(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.TariffIndexModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Tariff Index Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "TariffIndex");

        }
    }
}
