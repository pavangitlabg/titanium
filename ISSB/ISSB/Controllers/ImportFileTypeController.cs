using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using Data.Models;
using System.Security.Claims;
using Data.Enums;
using System;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class ImportFileTypeController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {

            var srv = new ImportFileTypeService();
            var model = await srv.GetFileTypes();
            
            IQueryable data = model.AsQueryable();

            return View(data);

        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new ImportFileTypeService();
            var modelOut = await srv.GeTypetById(_id);

            return View(modelOut);

        }

        [Authorize]
        public IActionResult Add()
        {
           
            var modelOut = new ImportFileTypeModel
            {
                KEY =  string.Empty,
                NAME = string.Empty

            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(ImportFileTypeModel model)
        {
            var srv = new ImportFileTypeService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Port Details";
                var modelOut = new ImportFileTypeModel
                {
                    KEY = string.Empty,
                    NAME = string.Empty

                };
                return View("Add", modelOut);
            }

            await srv.Add(model);
            return RedirectToAction("Index", "ImportFileType");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(ImportFileTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new ImportFileTypeService();
            await srv.Save(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.ImportFileTypeModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Import File Type Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "ImportFileType");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new ImportFileTypeService();
            var modelOut = await srv.GeTypetById(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.ImportFileTypeModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Import File Type Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "ImportFileType");

        }

        private async Task InitData()
        {

            var srv = new ImportFileTypeService();

            var model = new ImportFileTypeModel
            {
                 KEY = "EUROFER",
                 NAME = "EUROFER"
            };
            await srv.Add(model);

            model = new ImportFileTypeModel
            {
                KEY = "EUROFER",
                NAME = "GERMAN"
            };
            await srv.Add(model);

            model = new ImportFileTypeModel
            {
                KEY = "EUROFER",
                NAME = "TURKEY"
            };
            await srv.Add(model);

            model = new ImportFileTypeModel
            {
                KEY = "EUROFER",
                NAME = "SWISS"
            };
            await srv.Add(model);

            model = new ImportFileTypeModel
            {
                KEY = "EUROFER",
                NAME = "THAILAND"
            };
            await srv.Add(model);

            model = new ImportFileTypeModel
            {
                KEY = "EUROFER",
                NAME = "SERBIA"
            };
            await srv.Add(model);

            model = new ImportFileTypeModel
            {
                KEY = "EUROFER",
                NAME = "RUSSIA"
            };
            await srv.Add(model);

            model = new ImportFileTypeModel
            {
                KEY = "EUROFER",
                NAME = "UKRAINE"
            };
            await srv.Add(model);

            model = new ImportFileTypeModel
            {
                KEY = "EUROFER",
                NAME = "COLOMBIA"
            };
            await srv.Add(model);

            model = new ImportFileTypeModel
            {
                KEY = "EUROFER",
                NAME = "PAKISTAN"
            };
            await srv.Add(model);

            model = new ImportFileTypeModel
            {
                KEY = "EUROFER",
                NAME = "WVSTAHL"
            };
            await srv.Add(model);

            model = new ImportFileTypeModel
            {
                KEY = "GTT",
                NAME = "INDIA"
            };
            await srv.Add(model);

            model = new ImportFileTypeModel
            {
                KEY = "GTT",
                NAME = "EGYPT"
            };
            await srv.Add(model);

            model = new ImportFileTypeModel
            {
                KEY = "GTT",
                NAME = "MEXICO"
            };
            await srv.Add(model);
        }
    }
}
