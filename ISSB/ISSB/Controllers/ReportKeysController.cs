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
    public class ReportKeysController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new ReportKeysService();
            var model = await srv.GetReportKeys();
            IQueryable Data = model.AsQueryable();
            return View(Data);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new ReportKeysService();
            var modelOut = await srv.GetReportKeysByID(_id);

            return View(modelOut);

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new ReportKeysService();
            var nRec = await ser.GetCount();
            var modelOut = new ReportKeysModel
            {
                Description = "",
                IsSelected = false,
                Key = "",
                Sort = 0,
                Title = "",
                Width = ""
            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(ReportKeysModel model)
        {
            var srv = new ReportKeysService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Report Keys Details";
                var ser = new ReportKeysService();
                var nRec = await ser.GetCount();
                var modelOut = new ReportKeysModel
                {
                    Description = "",
                    IsSelected = false,
                    Key = "",
                    Sort = 0,
                    Title = "",
                    Width = ""

                };


                return View("Add", modelOut);
            }
            await srv.Add(model);
            return RedirectToAction("Index", "ReportKeys");

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(ReportKeysModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new ReportKeysService();
            await srv.Save(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.ReportKeysModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Report Keys Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "ReportKeys");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new ReportKeysService();
            var modelOut = await srv.GetReportKeysByID(_id);
            await srv.Delete(modelOut);

            //var srv = new ReportKeysService();
            //await srv.Save(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.ReportKeysModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Report Keys Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "ReportKeys");

        }
    }
}
