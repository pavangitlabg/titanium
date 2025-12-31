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
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Data.Enums;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class ClientDataImportController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new ClientDataImportService();
            var model = await srv.GetImportClients();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var lookupSrv = new TariffService();
            ViewBag.TariffIndex = await lookupSrv.GetTariffIndexLookup();

            var srvType = new ImportFileTypeService();
            var TypeList = await srvType.GetFileTypes();

            List<SelectListItem> FileTypeitems = new List<SelectListItem>();

            var TypeListDistinct = TypeList.Select(e => e.KEY).Distinct().ToList();

            foreach (var Item in TypeListDistinct)
            {
                var sModel = new SelectListItem
                {
                    Text = Item,
                    Value = Item
                };
                FileTypeitems.Add(sModel);
            }

            ViewBag.FileTypes = FileTypeitems;

            var modelOut = new ClientDataImportModel
            {
                Address1 = string.Empty,
                Address2 = string.Empty,
                Address3 = string.Empty,
                Address4 = string.Empty,
                Address5 = string.Empty,
                Email = string.Empty,
                Fax = string.Empty,
                WebSite = string.Empty,
                Name = string.Empty,
                Notes = string.Empty,
                PostCode = string.Empty,
                SearchString = string.Empty,
                Telephone = string.Empty,
                WeightType = string.Empty,
                CountryMapping = false,
                SourceGEO = string.Empty

            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(ClientDataImportModel model)
        {
            
            var srv = new ClientDataImportService();

            if (!ModelState.IsValid)
            {
                var lookupSrv = new TariffService();
                ViewBag.TariffIndex = await lookupSrv.GetTariffIndexLookup();

                var srvType = new ImportFileTypeService();
                var TypeList = await srvType.GetFileTypes();

                List<SelectListItem> FileTypeitems = new List<SelectListItem>();

                var TypeListDistinct = TypeList.Select(e => e.KEY).Distinct().ToList();

                foreach (var Item in TypeListDistinct)
                {
                    var sModel = new SelectListItem
                    {
                        Text = Item,
                        Value = Item
                    };
                    FileTypeitems.Add(sModel);
                }

                ViewBag.FileTypes = FileTypeitems;

                ViewData["Message"] = "Invalid Client Details";
                var modelOut = new ClientDataImportModel
                {
                    Address1 = string.Empty,
                    Address2 = string.Empty,
                    Address3 = string.Empty,
                    Address4 = string.Empty,
                    Address5 = string.Empty,
                    Email = string.Empty,
                    Fax = string.Empty,
                    WebSite = string.Empty,
                    Name = string.Empty,
                    Notes = string.Empty,
                    PostCode = string.Empty,
                    SearchString = string.Empty,
                    Telephone = string.Empty,
                    WeightType = string.Empty,
                    CountryMapping = false

                };
                return View("Add", modelOut);
            }


            var cList = new List<ClientDataImportTransactionsModel>();
            model.Transactions = cList;
            model.LastModified = DateTime.Now;
            await srv.Add(model);
            return RedirectToAction("Index", "ClientDataImport");


        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srvType = new ImportFileTypeService();
            var TypeList = await srvType.GetFileTypes();

            List<SelectListItem> FileTypeitems = new List<SelectListItem>();

            var TypeListDistinct = TypeList.Select(e => e.KEY).Distinct().ToList();

            foreach(var Item in TypeListDistinct)
            {
                var sModel = new SelectListItem
                {
                     Text = Item,
                     Value = Item
                };
                FileTypeitems.Add(sModel);
            }

            ViewBag.FileTypes = FileTypeitems;


            var lookupSrv = new TariffService();
            ViewBag.TariffIndex = await lookupSrv.GetTariffIndexLookup();
            
            var srv = new ClientDataImportService();
            var modelOut = await srv.GetClientByID(_id);

            if(string.IsNullOrEmpty(modelOut.ImportType))
            {
                modelOut.ImportType = string.Empty;
            }

            return View(modelOut);

        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(ClientDataImportModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var cList = new List<ClientDataImportTransactionsModel>();
            model.Transactions = cList;
            var srv = new ClientDataImportService();
            model.LastModified = DateTime.Now;
            await srv.SaveClient(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.ClientDataImportModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Client Data Import Transaction Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "ClientDataImport");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new ClientDataImportService();
            var modelOut = await srv.GetClientByID(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.ClientDataImportModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Client Data Import Transaction Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "ClientDataImport");

        }
    }
}

