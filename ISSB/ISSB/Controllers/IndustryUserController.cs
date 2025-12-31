using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using System.Collections.Generic;
using Newtonsoft.Json;
using Data.Enums;
using ISSB.Helpers;
using ISSB.Models;
using System.IO;

namespace ISSB.Controllers
{
    public class IndustryUserController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {

            var srv = new IndustryService();
            var model = await srv.GetUsers();
            IQueryable data = model.AsQueryable();
            return View(data);
        }

        //[HttpGet("{rowId}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string rowId)
        {
            var srv = new IndustryService();
            var model = await srv.GetUsersById(rowId);
            model.Password = EncryptionHelper.Decrypt(model.Password);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(IndustryUserModel model)
        {
            var srv = new IndustryService();
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid User Details";
                return View("Edit", model);
            }

            model.Password = EncryptionHelper.Encrypt(model.Password);
            await srv.UpdateUser(model);
            return RedirectToAction("Index", "IndustryUser");

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new IndustryService();
            var nRec = await ser.GetCount();
            var modelOut = new IndustryUserModel
            {
                DateJoined = DateTime.Now,
                Email = "",
                IsAdministrator = false,
                IsSystemUser = false,
                //Permissions = true,
                Password = "",
                UserName = "",
                IsTradeInquiry = false,
                StartDate = DateTime.Now,
                ExpiryDate = new DateTime(2021, 1, 1),
                FirstName = "",
                LastName = "",
                CompanyName = "",
                Address1 = "",
                Address2 = "",
                Address3 = "",
                Address4 = "",
                PostalCode = "",
                Telephone = "",
                Mobile = "",
                Costs = 0,
                AccountStatus = "",
                TwoFactorAuth = false
            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(IndustryUserModel model)
        {
            var srv = new IndustryService();
            var nRec = await srv.GetCount();
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid User Details";
                var ser = new IndustryService();

                var modelOut = new IndustryUserModel
                {
                    DateJoined = DateTime.Now,
                    Email = "",
                    IsAdministrator = false,
                    IsSystemUser = false,
                    //Permissions = true,
                    Password = "",
                    UserName = "",
                    IsTradeInquiry = false,
                    StartDate = DateTime.Now,
                    ExpiryDate = new DateTime(2021, 1, 1),
                    FirstName = "",
                    LastName = "",
                    CompanyName = "",
                    Address1 = "",
                    Address2 = "",
                    Address3 = "",
                    Address4 = "",
                    PostalCode = "",
                    Telephone = "",
                    Mobile = "",
                    Costs = 0,
                    AccountStatus = "",
                    TwoFactorAuth = false

                };

                return View("Add", modelOut);
            }

            bool bResult = await srv.DoesEmailExist(model.Email);

            if (bResult)
            {

                ViewData["Message"] = "User Email already exists";
                return View("Add", model);

            }

            await srv.AddUser(model);

            return RedirectToAction("Index", "IndustryUser");
        }

        [Authorize]
        public async Task<IActionResult> Delete(string rowId)
        {
            var srv = new IndustryService();
            var modelOut = await srv.GetUsersById(rowId);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            
            return RedirectToAction("Index", "IndustryUser");

        }
    }
}
