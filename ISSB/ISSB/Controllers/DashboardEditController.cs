using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using System.Web;
using System;
using System.Security.Claims;
using Data.Enums;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class DashboardEditController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var dSrv = new UserDashboardService();
            var model = await dSrv.GetUserDashboards(GetUserEmail());

            IQueryable Data = model.AsQueryable();
            return View(Data);
   
        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var dSrv = new UserDashboardService();
            await dSrv.Delete(_id);           

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.UserDashboardModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Dasboard Edit Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = _id
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "DashboardEdit");

        }

        private string GetUserEmail()
        {
            var UserEmail = User.FindFirst(ClaimTypes.Email).Value;
            return UserEmail;
        }
    }
}
