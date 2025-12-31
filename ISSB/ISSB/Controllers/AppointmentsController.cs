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
    public class AppointmentsController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new AppointmentsService();
            var model = await srv.GetAppointments();
            IQueryable data = model.AsQueryable();
            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new AppointmentsService();
            var modelOut = await srv.GetAppointmentByID(_id);
            return View(modelOut);
        }

        [Authorize]
        public IActionResult Add()
        {

            var modelOut = new AppointmentsModel
            {
                Date = DateTime.Now,
                Title = string.Empty,
                Description = string.Empty,
                Notes = string.Empty,
                IsPublished = false
            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(AppointmentsModel model)
        {
            var srv = new AppointmentsService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Appointment Details";
                var ser = new AssetService();
                var nRec = await ser.GetCount();
                var modelOut = new AppointmentsModel
                {
                    Date = DateTime.Now,
                    Title = string.Empty,
                    Description = string.Empty,
                    Notes = string.Empty,
                    IsPublished = false
                };


                return View("Add", modelOut);
            }
            await srv.Add(model);

            return RedirectToAction("Index", "Appointments");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(AppointmentsModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new AppointmentsService();
            await srv.Update(model);
            return RedirectToAction("Index", "Appointments");
        }


        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new AppointmentsService();
            var modelOut = await srv.GetAppointmentByID(_id);
            await srv.Delete(modelOut);

            return RedirectToAction("Index", "Appointments");

        }
    }
}
