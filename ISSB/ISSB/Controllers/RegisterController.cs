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
    public class RegisterController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _env;
        [Obsolete]
        public RegisterController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            var srv = new RegisterService();
            var model = await srv.GetAllRegister();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new RegisterService();
            var modelOut = await srv.GetRegisterById(_id);

            return View(modelOut);

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new RegisterService();
            var nRec = await ser.GetCount();
            var modelOut = new RegisterModel
            {
                RegisterID = nRec + 1,
                FirstName = "",
                Lastname = "",
                Company = "",
                Info = "",
                Address1 = "",
                Address2 = "",
                City = "",
                State = "",
                Zip = "",
                Phone = "",
                Email = "",
                Comment = "",
                RegisDate = DateTime.Now,
                Status = false,
                Username = "",
                Password = "",
                ConfirmPassword = "",
                Country = ""

            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(RegisterModel model)
        {
            var srv = new RegisterService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Kindly answer all required [*] fields";
                var ser = new RegisterService();
                var nRec = await ser.GetCount();
                var modelOut = new RegisterModel
                {               
                    FirstName = "",
                    Lastname = "",
                    Company = "",
                    Info = "",
                    Address1 = "",
                    Address2 = "",
                    City = "",
                    State = "",
                    Zip = "",
                    Phone = "",
                    Email = "",
                    Comment = "",
                    RegisDate = DateTime.Now,
                    Status = false,
                    Username = "",
                    Password = "",
                    ConfirmPassword = "",
                    Country = ""

                };


                return View("Add", modelOut);
            }            
            await srv.Add(model);
            return RedirectToAction("Contact", "Home");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new RegisterService();            
            await srv.Save(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.RegisterModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Register Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "Register");

        }
        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new RegisterService();
            var modelOut = await srv.GetRegisterById(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.RegisterModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Register Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "Register");

        }        

    }
}
