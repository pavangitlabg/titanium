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
    public class AssetController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _env;

        [Obsolete]
        public AssetController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new AssetService();
            var model = await srv.GetAssets();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new AssetService();
            var modelOut = await srv.GetAssetByID(_id);

            return View(modelOut);

        }        

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new AssetService();
            var nRec = await ser.GetCount();
            var modelOut = new AssetModel
            {
                ID = nRec + 1,
                CHECKED_BY = "",
                DATE_CHECKED = DateTime.Now,
                DESCRIPTION = "",
                LAST_MODIFIED = DateTime.Now,
                LOCATION = "",
                MANUFACTURER = "",
                MEMO = "",
                MODEL_TYPE = "",
                PASSWORD = "",
                PURCHASE_DATE = DateTime.Now,
                SERIAL_NO = "",
                USERNAME = "",
                VALUE = 0
                
            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(AssetModel model)
        {
            var srv = new AssetService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Asset Details";
                var ser = new AssetService();
                var nRec = await ser.GetCount();
                var modelOut = new AssetModel
                {
                    ID = nRec + 1,
                    CHECKED_BY = "",
                    DATE_CHECKED = DateTime.Now,
                    DESCRIPTION = "",
                    LAST_MODIFIED = DateTime.Now,
                    LOCATION = "",
                    MANUFACTURER = "",
                    MEMO = "",
                    MODEL_TYPE = "",
                    PASSWORD = "",
                    PURCHASE_DATE = DateTime.Now,
                    SERIAL_NO = "",
                    USERNAME = "",
                    VALUE = 0

                };


                return View("Add", modelOut);
            }

         
            model.LAST_MODIFIED = DateTime.Now;
            
            await srv.Add(model);

            return RedirectToAction("Index", "Asset");

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(AssetModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new AssetService();
            model.LAST_MODIFIED = DateTime.Now;
            await srv.SaveAsset(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.AssetModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Asset Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "Asset");

        }


        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new AssetService();
            var modelOut = await srv.GetAssetByID(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.AssetModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Asset Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "Asset");

        }

        public async Task<ActionResult> PrintPage()
        {
            var rs = new ReportingService("http://issb.nathan-software.com:8081", "admin", "Letmein2019");

            var report = await rs.RenderByNameAsync("asset-list-pdf", new
            {
                //Id = 123,
                //From = "Erich Gamma",
                //To = "Martin Fowler"
            });

            string fileName = "AssetList.pdf";
            using (FileStream fs = System.IO.File.Create(this.GetPath(fileName)))
            {
                report.Content.CopyTo(fs);
            }

            string myurl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            ViewBag.Url = myurl + "/reports/AssetList.pdf";

#if RELEASE
          var releaseUrl = myurl.Replace("http", "https");
          ViewBag.Url = releaseUrl + "/reports/AssetList.pdf";
#endif

            return View();

        }

        private string GetPath(string filename)
        {
            string path = _env.WebRootPath + "/reports/";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path + filename;

        }

    }
}
