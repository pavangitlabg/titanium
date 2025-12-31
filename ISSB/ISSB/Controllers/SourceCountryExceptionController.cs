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
	public class SourceCountryExceptionController : Controller
	{
		[Obsolete]
		private IHostingEnvironment _env;
		[Obsolete]
		public SourceCountryExceptionController(IHostingEnvironment env)
		{
			_env = env;
		}


		[Authorize]
		public async Task<IActionResult> Index()
		{
			var srv = new SourceCountryExceptionService();
			var model = await srv.GetSourceCountryExceptions();


			IQueryable data = model.AsQueryable();

			return View(data);
		}

		[Authorize]
		public async Task<IActionResult> Edit(string _id)
		{
			var srv = new SourceCountryExceptionService();
			var modelOut = await srv.GetSourceCountryExceptionByID(_id);

			return View(modelOut);

		}

		[Authorize]
		public async Task<IActionResult> Add()
		{
			var ser = new SourceCountryExceptionService();
			var nRec = await ser.GetCount();
			var modelOut = new SourceCountryExceptionModel
			{
				SOURCE_COUNTRY_ID = nRec + 1,
				ACTIVE = true,				
				GEO_CODE = "",
                NAME_OLD = "",
                SHORT_LEGEND = "",
                LONG_LEGEND = "",
                REGION_NAME = "",
                SOURCE_COUNTRY_INDICATOR = "",
                REPLACED_GEO_CODE = "",
                START_DATE = DateTime.Now,
                DISCONTINUED_DATE = DateTime.Now,              				                
                NAME = ""													
				
			};

			return View(modelOut);
		}

		[Authorize]
		public async Task<IActionResult> AddNew(SourceCountryExceptionModel model)
		{
			var srv = new SourceCountryExceptionService();

			if (!ModelState.IsValid)
			{
				ViewData["Message"] = "Invalid Source Country Exception Details";
				var ser = new SourceCountryExceptionService();
				var nRec = await ser.GetCount();
				var modelOut = new SourceCountryExceptionModel
				{
                    SOURCE_COUNTRY_ID = nRec + 1,
                    ACTIVE = true,
                    GEO_CODE = "",
                    NAME_OLD = "",
                    SHORT_LEGEND = "",
                    LONG_LEGEND = "",
                    REGION_NAME = "",
                    SOURCE_COUNTRY_INDICATOR = "",
                    REPLACED_GEO_CODE = "",
                    START_DATE = DateTime.Now,
                    DISCONTINUED_DATE = DateTime.Now,
                    NAME = ""

                };


				return View("Add", modelOut);
			}

			model.START_DATE = DateTime.Now;
			model.DISCONTINUED_DATE = DateTime.Now;			

			await srv.Add(model);

			return RedirectToAction("Index", "SourceCountryException");

		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Save(SourceCountryExceptionModel model)
		{
			if (!ModelState.IsValid)
			{
				ViewData["Message"] = "Invalid Details";
				return View("Edit", model);
			}

			var srv = new SourceCountryExceptionService();
			model.START_DATE = DateTime.Now;

			await srv.SaveSourceCountryException(model);

			var UserId = User.FindFirst(ClaimTypes.Email).Value;
			var EventModel = new SystemEventLogsModel
			{
				DataType = DataTypeEnums.SourceCountryExceptionModel,
				TransactionType = TransactionTypeEnums.Edit,
				MESSAGE = "Source Country Exception Updated, [" + UserId + "]",
				DATE = DateTime.Now,
				USER = UserId,
				Model = model
			};
			new EventLog(EventModel);

			return RedirectToAction("Index", "SourceCountryException");

		}

		[Authorize]
		public async Task<IActionResult> Delete(string _id)
		{
			var srv = new SourceCountryExceptionService();
			var modelOut = await srv.GetSourceCountryExceptionByID(_id);
			await srv.Delete(modelOut);

			var UserId = User.FindFirst(ClaimTypes.Email).Value;
			var EventModel = new SystemEventLogsModel
			{
				DataType = DataTypeEnums.SourceCountryExceptionModel,
				TransactionType = TransactionTypeEnums.Delete,
				MESSAGE = "Source Country Exception Deleted, [" + UserId + "]",
				DATE = DateTime.Now,
				USER = UserId,
				Model = modelOut
			};
			new EventLog(EventModel);

			return RedirectToAction("Index", "SourceCountryException");

		}
		
		public async Task<ActionResult> PrintPage()
		{
			var rs = new ReportingService("http://issb.nathan-software.com:8081", "admin", "Letmein2019");

			var report = await rs.RenderByNameAsync("source-countries-pdf", new
			{
				//Id = 123,
				//From = "Erich Gamma",
				//To = "Martin Fowler"
			});

			string fileName = "SourceCountries.pdf";
			using (FileStream fs = System.IO.File.Create(this.GetPath(fileName)))
			{
				report.Content.CopyTo(fs);
			}

			string myurl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
			ViewBag.Url = myurl + "/reports/SourceCountries.pdf";

#if RELEASE
          var releaseUrl = myurl.Replace("http", "https");
          ViewBag.Url = releaseUrl + "/reports/SourceCountries.pdf";
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
