using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Enums;
using Data.Models;
using ISSB.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ISSB.Controllers
{
    public class EmailSettingsController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new EmailService();
            var model = await srv.GetEmailCredentials();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(EmailCredentialsModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Index", model);
            }

            var srv = new EmailService();
            await srv.SaveEmailCredentials(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.EmailCredentialsModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Email Settings Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "Home");

        }

    }
}
