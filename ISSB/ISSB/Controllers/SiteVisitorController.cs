using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using Data.Models;
using Data.Enums;
using System;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class SiteVisitorController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new SiteVisitorService();
            var model = await srv.GetVisitorsGrouped();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Detail(string _id)
        {
            var srv = new SiteVisitorService();
            var model = await srv.GetVisitorById(_id);
            ViewBag.Lat = model.lat.ToString();
            ViewBag.Lng = model.lon.ToString();
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new SiteVisitorService();
            var modelOut = await srv.GetVisitorById(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.SiteVisitorModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Site Visitor Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "SiteVisitor");

        }
    }
}
