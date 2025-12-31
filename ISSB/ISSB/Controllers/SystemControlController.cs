using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using Data.Enums;
using System;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class SystemControlController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new SystemControlService();
            var model = await srv.GetSystemControl();
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Save(SystemControlModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Index", model);
            }

            var srv = new SystemControlService();
            await srv.UpdateControl(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.SystemControlModel,
                MESSAGE = "System Control Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "SystemControl");

        }
    }
}
