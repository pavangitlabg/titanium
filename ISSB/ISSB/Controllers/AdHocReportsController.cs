using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Linq;
using System.Security.Claims;

namespace ISSB.Controllers
{
    public class AdHocReportsController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var srv = new QueryService();
            var model = await srv.GetAllReportHeadersByUser(UserId);
            IQueryable Data = model.AsQueryable();
            return View(Data);
        }

        [Authorize]
        public IActionResult SendTo(string Id)
        {
            var UserId = User.FindFirst(ClaimTypes.Email).Value;

            var model = new SendToModel
            {
                 FromEmail = UserId,
                 RecordQueryId = Id,
                 ReportId = string.Empty,
                 ToEmail = string.Empty
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Send(SendToModel model)
        {
            //if(model.ToEmail.ToLower().Equals(model.FromEmail.ToLower()))
            //{
            //    ViewData["Message"] = "Query can't be sent you your logon!";
            //    model.ToEmail = model.ToEmail.ToLower();
            //    return View("SendTo", model);
            //}


            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Please enter an email address";
                return View("SendTo", model);
            }

            var userSrv = new UserServices();
            var userModel = await userSrv.GetUser(model.ToEmail);

            if(userModel.Email == null)
            {
                ViewData["Message"] = "Email address not found!";
                return View("SendTo", model);
            }
           

            model.ToEmail = model.ToEmail.ToLower();
            var reportService = new QueryService();
            var reportModel = await reportService.GetReportByID(model.FromEmail, model.RecordQueryId);
            reportModel.User = model.ToEmail;
            reportModel.Id = string.Empty;
            reportModel.Name = reportModel.Name + " from: " + model.FromEmail;
            await reportService.InserNewQuery(reportModel);

            return RedirectToAction("Index", "AdhocReports");
        }
    }

  
}
