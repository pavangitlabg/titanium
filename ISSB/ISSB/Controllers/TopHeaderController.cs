using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ISSB.Controllers
{
    public class TopHeaderController : Controller
    {

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new TopHeaderService();
            var model = await srv.GetHeaders();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new TopHeaderService();
            var modelOut = await srv.GetHeaderById(_id);

            return View(modelOut);

        }

        [Authorize]
        public IActionResult Add()
        { 
            return View();
        }

        [Authorize]
        public async Task<IActionResult> AddNew(TopHeaderModel model)
        {
            var srv = new TopHeaderService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Content Details";
              
                return View("Add", model);
            }

            await srv.AddHeader(model);

            return RedirectToAction("Index", "TopHeader");

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(TopHeaderModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }
            var srv = new TopHeaderService();
            await srv.UpdateHeader(model);
            return RedirectToAction("Index", "TopHeader");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new TopHeaderService();
            var modelOut = await srv.GetHeaderById(_id);
            await srv.Delete(modelOut);
            return RedirectToAction("Index", "TopHeader");

        }
    }
}
