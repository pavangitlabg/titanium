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
    public class ErrorController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new ErrorService();
            var model = await srv.GetErrors();


            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Detail(string _id)
        {
            var srv = new ErrorService();
            var model = await srv.GetErrorByID(_id);   
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new ErrorService();
            var modelOut = await srv.GetErrorByID(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.ErrorModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Error Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "Error");

        }
    }
}
