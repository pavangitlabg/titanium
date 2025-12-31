using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ISSB.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Data.Models;
using Services;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Collections.Generic;
using Data.Enums;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Home Page";
            
            //Get the MapChart Data
            var chartsHelper = ChartsPageHelper.Instance;

            var viewModel = await chartsHelper.SetupHomePageCharts();
            ViewBag.Title = chartsHelper.PageTitle;

            var GroupMapList = chartsHelper.GroupList;

            if (GroupMapList.Count != 0)
            {
                //foreach(var Item in GroupMapList)
                //{
                //    Item.MONEY = Item.MONEY / 1000;
                //}

                ViewBag.SourceMapStatus = "False";
                ViewBag.SourceMapData = chartsHelper.SourceMapData;
                ViewBag.DestinationMapData = JsonConvert.SerializeObject(GroupMapList);
            }
            else
            {
                ViewBag.SourceMapStatus = "True";
            }

            //Inject Top Content
            var headerSrv = new TopHeaderService();
            var hList = await headerSrv.GetHeaders();
            var hTopContent = string.Empty;
            var hFooterContent = string.Empty;

            foreach (var hItem in hList)
            {
                if(hItem.IsActive && !hItem.IsFooter)
                   hTopContent += hItem.HTML;
                if (hItem.IsActive && hItem.IsFooter)
                    hFooterContent += hItem.HTML;
            }

            ViewBag.TopContent = hTopContent;
            ViewBag.FooterContent = hFooterContent;

            return View(viewModel);
        }     


        [HttpPost]
        public async Task<IActionResult> SetIP(string ipAddress)
        {
            //var ipAddress = this.httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if(ipAddress == null)
            {
                return View();
            }

            using (var httpClient = new HttpClient())
            {
                var seekIP = "http://ip-api.com/json/" + ipAddress;
                var response = await httpClient.GetAsync(seekIP);

                string apiResponse = await response.Content.ReadAsStringAsync();
                var visitor = JsonConvert.DeserializeObject<SiteVisitorModel>(apiResponse);
                if (!visitor.status.Equals("fail"))
                {
                    visitor.ipAddress = ipAddress;
                    visitor.Date = DateTime.Now;
                    var srv = new SiteVisitorService();
                    await srv.Add(visitor);
                }
            }

            return Json(new { success = true });
        }

        public IActionResult Indextest()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetUserId()
        {
            var UserId = User.FindFirst(ClaimTypes.Sid).Value;
            return UserId;
        }

        [Authorize]
        public async Task<IActionResult> ContactUsIndex()
        {

            var srv = new HomeService();
            var model = await srv.GetAllContactForm();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> ContactUsEdit(string _id)
        {
            var srv = new HomeService();
            var modelOut = await srv.GetContactFormById(_id);

            return View(modelOut);

        }

        [Authorize]
        public async Task<IActionResult> ContactUsAdd()
        {
            var ser = new HomeService();
            var nRec = await ser.GetCount();
            var modelOut = new HomeModel
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
                ContactDate = DateTime.Now,
                Status = false
            
            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> ContactUsAddNew(HomeModel model)
        {
            var srv = new HomeService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Kindly answer all required [*] fields";
                var ser = new HomeService();
                var nRec = await ser.GetCount();
                var modelOut = new HomeModel
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
                    ContactDate = DateTime.Now,
                    Status = false                  

                };

                return View("ContactUsAdd", modelOut);
            }
            await srv.ContactUsAdd(model);
            return RedirectToAction("Contact", "Home");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactUsSave(HomeModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new HomeService();
            await srv.ContactUsSave(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.HomeModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Contact Us Index Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new Helpers.EventLog(EventModel);

            return RedirectToAction("ContactUsIndex", "Home");

        }
        [Authorize]
        public async Task<IActionResult> ContactUsDelete(string _id)
        {
            var srv = new HomeService();
            var modelOut = await srv.GetContactFormById(_id);
            await srv.ContactUsDelete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.HomeModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Contact Us Index Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new Helpers.EventLog(EventModel);

            return RedirectToAction("ContactUsIndex", "Home");

        }
    }
}
