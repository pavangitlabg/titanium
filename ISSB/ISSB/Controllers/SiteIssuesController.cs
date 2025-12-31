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
	public class SiteIssuesController : Controller
	{
		[Authorize]
		public async Task<IActionResult> Index()
		{
			var srv = new SiteIssuesService();
			var model = await srv.GetIssues();


			IQueryable data = model.AsQueryable();

			return View(data);
		}

		[Authorize]
		public async Task<IActionResult> Edit(string _id)
		{
			var srv = new SiteIssuesService();
			var model = await srv.GetIssuesById(_id);
			return View(model);
		}

		[Authorize]
		public async Task<IActionResult> Delete(string _id)
		{
			var srv = new SiteIssuesService();
			var modelOut = await srv.GetIssuesById(_id);
			await srv.Delete(modelOut);

			var UserId = User.FindFirst(ClaimTypes.Email).Value;
			var EventModel = new SystemEventLogsModel
			{
				DataType = DataTypeEnums.SiteIssuesModel,
				TransactionType = TransactionTypeEnums.Delete,
				MESSAGE = "Site Issues Deleted, [" + UserId + "]",
				DATE = DateTime.Now,
				USER = UserId,
				Model = modelOut
			};
			new EventLog(EventModel);

			return RedirectToAction("Index", "SiteIssues");

		}

		[Authorize]
		public async Task<IActionResult> Add()
		{
			var ser = new SiteIssuesService();
			var nRec = await ser.GetCount();
			var modelOut = new SiteIssuesModel
			{
				RequestID = nRec + 1,
				User = "",
				Subject = "",
				Description = "",
				Comment = "",
				Status = "",
				Severity = "",
				Email = "",
				Timestamp = DateTime.Now
			};

			return View(modelOut);
		}

		[Authorize]
		public async Task<IActionResult> AddNew(SiteIssuesModel model)
		{
			var srv = new SiteIssuesService();

			if (!ModelState.IsValid)
			{
				ViewData["Message"] = "Invalid Site Issue Details";
				var ser = new SiteIssuesService();
				var nRec = await ser.GetCount();
				var modelOut = new SiteIssuesModel
				{
					RequestID = nRec + 1,
					User = "",
					Subject = "",
					Description = "",
					Comment = "",
					Status = "",
					Severity = "",
					Email = "",
					Timestamp = DateTime.Now

				};


				return View("Add", modelOut);
			}

			model.Timestamp = DateTime.Now;

			await srv.Add(model);

			return RedirectToAction("Index", "SiteIssues");

		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Save(SiteIssuesModel model)
		{
			if (!ModelState.IsValid)
			{
				ViewData["Message"] = "Invalid Details";
				return View("Edit", model);
			}

			var srv = new SiteIssuesService();
			model.Timestamp = DateTime.Now;

			await srv.SaveSiteIssues(model);

			var UserId = User.FindFirst(ClaimTypes.Email).Value;
			var EventModel = new SystemEventLogsModel
			{
				DataType = DataTypeEnums.SiteIssuesModel,
				TransactionType = TransactionTypeEnums.Edit,
				MESSAGE = "Site Issues Updated, [" + UserId + "]",
				DATE = DateTime.Now,
				USER = UserId,
				Model = model
			};
			new EventLog(EventModel);

			return RedirectToAction("Index", "SiteIssues");

		}
	}
}
