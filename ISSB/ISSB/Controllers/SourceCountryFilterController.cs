using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Data.Enums;
using System;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class SourceCountryFilterController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new SourceCountryFilterService();
            var model = await srv.GetFilters();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var srv = new SourceCountryService();
            var model = await srv.GetSourceCountries();
            IQueryable data = model.AsQueryable();
            return View(data);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddFilter(string selectionData, string formData)
        {
            var srvSourceCountry = new SourceCountryService();
            string[] geos = selectionData.Split("|");
            string[] lines = formData.Split("|");
            bool bCanEdit = false;
            bool bActive = false;

            if (lines[2].Equals("true"))
            {
                bCanEdit = true;
            }

            if (lines[3].Equals("true"))
            {
                bActive = true;
            }
           // var filterSrv = new SourceCountryFilterService();


            var Model = new SourceCountryFilterModel
            {
                Name = lines[0],
                Notes = lines[1],
                CanEdit = bCanEdit,
                Active = bActive,

            };

            var Items = new List<SourceCountryFilterListModel>();

            int cnt = 1;
            foreach (var g in geos)
            {
                SourceCountryModel sModel = await srvSourceCountry.GetSourceCountryByGeoCode(g);

                var iModel = new SourceCountryFilterListModel { GeoCode = sModel.GEO_CODE, CountryName = sModel.NAME, SortOrder = cnt };
                Items.Add(iModel);
                cnt++;

            }
            Model.Items = Items;
            var srvPost = new SourceCountryFilterService();
            await srvPost.AddFilter(Model);


            return RedirectToAction("Index", "SourceCountryFilter");


        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateFilter(string selectionData, string formData)
        {
            var srvSourceCountry = new SourceCountryService();
            string[] geos = selectionData.Split("|");
            string[] lines = formData.Split("|");
            bool bCanEdit = false;
            bool bActive = false;

            if (lines[3].Equals("true"))
            {
                bCanEdit = true;
            }

            if (lines[4].Equals("true"))
            {
                bActive = true;
            }
            // var filterSrv = new MarketCountryFilterService();
            // var fModel = await filterSrv.GetFilterById(lines[0]);

            var Model = new SourceCountryFilterModel
            {
                _id = lines[0],
                Name = lines[1],
                Notes = lines[2],
                CanEdit = bCanEdit,
                Active = bActive

            };

            var Items = new List<SourceCountryFilterListModel>();

            int cnt = 1;
            foreach (var g in geos)
            {
                SourceCountryModel sModel = await srvSourceCountry.GetSourceCountryByGeoCode(g);

                var iModel = new SourceCountryFilterListModel { GeoCode = sModel.GEO_CODE, CountryName = sModel.NAME, SortOrder = cnt };
                Items.Add(iModel);
                cnt++;

            }
            Model.Items = Items;
            var srvPost = new SourceCountryFilterService();
            await srvPost.SaveFilter(Model);

            //return JavaScript("window.location='/Account/Login'");

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.SourceCountryFilterModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Source Country Filter Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model =  Model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "SourceCountryFilter");
            //return Json()

        }


        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var filterSrv = new SourceCountryFilterService();
            var fModel = await filterSrv.GetFilterById(_id);
            ViewBag.ObjId = fModel._id;

            var listItems = string.Empty;

            foreach (var item in fModel.Items)
            {
                listItems += item.GeoCode + "|";
            }

            ViewBag.Name = fModel.Name;
            ViewBag.Notes = fModel.Notes;
            ViewBag.Active = fModel.Active;
            ViewBag.Locked = fModel.CanEdit;
            ViewBag.SelectedItems = listItems;

            var srv = new SourceCountryService();
            var model = await srv.GetSourceCountries();
            IQueryable data = model.AsQueryable();
            return View(data);
        }

        //// [HttpPost]
        //[Authorize]
        //public async Task<IActionResult> Edit(string objId)
        //{
        //    var srv = new SourceCountryFilterService();
        //    var modelOut = await srv.GetFilterById(objId);

        //    return View(modelOut);

        //}

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var filterSrv = new SourceCountryFilterService();
            await filterSrv.Delete(_id);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.SourceCountryFilterModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Source Country Filter Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = _id
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "SourceCountryFilter");
        }

        [Authorize]
        public async Task<IActionResult> GetSourceCountries()
        {
          
            var srv = new SourceCountryService();
            var model = await srv.GetSourceCountries();
            IQueryable data = model.AsQueryable();
            return View(data);

        }
    }
}
