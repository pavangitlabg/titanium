using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using Infragistics.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Security.Claims;
using Data.Enums;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class SourceCountryMappingController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
           
            var srvCountries = new CountryCrossOverService();

            var model = await srvCountries.GetCountries();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new SourceCountryMappingService();
            var modelOut = await srv.GetSourceCountryMappingByID(_id);

            return View(modelOut);

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new SourceCountryMappingService();
            var nRec = await ser.GetCount();
            var modelOut = new SourceCountryMappingModel
            {
                Source_Country_ID = "",
                Source_Country_Value = "",
                COUNTRY_NAME = "",
                ISSB_Country_Geo_Code = ""               
                
            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(SourceCountryMappingModel model)
        {
            var srv = new SourceCountryMappingService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Source Country Mapping Details";
                var ser = new SourceCountryMappingService();
                var nRec = await ser.GetCount();
                var modelOut = new SourceCountryMappingModel
                {
                    Source_Country_ID = "",
                    Source_Country_Value = "",
                    COUNTRY_NAME = "",
                    ISSB_Country_Geo_Code = ""

                };


                return View("Add", modelOut);
            }
            await srv.Add(model);
            return RedirectToAction("Index", "SourceCountryMapping");

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(SourceCountryMappingModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new SourceCountryMappingService();       
            await srv.Save(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.SourceCountryMappingModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Source Country Mapping Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "SourceCountryMapping");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new SourceCountryMappingService();
            var modelOut = await srv.GetSourceCountryMappingByID(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.SourceCountryMappingModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Source Country Mapping Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "SourceCountryMapping");

        }
    }
}
