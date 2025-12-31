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
    public class PortsController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _env;
        [Obsolete]
        public PortsController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            var srv = new PortsService();
            var model = await srv.GetAllPorts();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new PortsService();
            var modelOut = await srv.GetPortById(_id);

            return View(modelOut);

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new PortsService();
            var nRec = await ser.GetCount();
            var modelOut = new PortsModel
            {
                PortID = nRec + 1,
                Name = ""

        };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(PortsModel model)
        {
            var srv = new PortsService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Port Details";
                var ser = new PortsService();
                var nRec = await ser.GetCount();
                var modelOut = new PortsModel
                {
                    PortID = nRec + 1,
                    Name = ""

                };


                return View("Add", modelOut);
            }

            await srv.Add(model);
            return RedirectToAction("Index", "Ports");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(PortsModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new PortsService();            
            await srv.Save(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.PortsModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Ports Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "Ports");

        }
        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new PortsService();
            var modelOut = await srv.GetPortById(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.PortsModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Ports Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "Ports");

        }

        //public async Task<ActionResult> PrintPage()
        //{
        //    var rs = new ReportingService("http://issb.nathan-software.com:8081", "admin", "Letmein2019");

        //    var report = await rs.RenderByNameAsync("port-list-pdf", new
        //    {
        //        //Id = 123,
        //        //From = "Erich Gamma",
        //        //To = "Martin Fowler"
        //    });

        //    string fileName = "PortList.pdf";
        //    using (FileStream fs = System.IO.File.Create(this.GetPath(fileName)))
        //    {
        //        report.Content.CopyTo(fs);
        //    }

        //    string myurl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
        //    ViewBag.Url = myurl + "/reports/PortList.pdf";

        //#if RELEASE
        //  var releaseUrl = myurl.Replace("http", "https");
        //  ViewBag.Url = releaseUrl + "/reports/PortList.pdf";
        //#endif

        //    return View();

        //}

        //private string GetPath(string filename)
        //{
        //    string path = _env.WebRootPath + "/reports/";

        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    return path + filename;

        //}

    }
}
