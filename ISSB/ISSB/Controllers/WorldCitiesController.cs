using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using Infragistics.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ISSB.Controllers
{
    public class WorldCitiesController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new WorldCityService();
            var model = await srv.GetWorldCities();

            IQueryable data = model.AsQueryable();


            return View(data);
        }

        [GridDataSourceAction]
        public async Task<IActionResult> GetCities()
        {
            var srv = new WorldCityService();
            var model = await srv.GetWorldCities();
            IQueryable data = model.AsQueryable();
            return View(data);
        }


        public async Task<ActionResult> SaveCities()
        {
            GridModel gridModel = new GridModel();
            List<Transaction<WorldCityModel>> transactions = gridModel.LoadTransactions<WorldCityModel>(HttpContext.Request.Form["ig_transactions"]);

            var srv = new WorldCityService();
            //var model = await srv.GetWorldCities();

            var cityList = new List<WorldCityModel>();
            foreach (Transaction<WorldCityModel> t in transactions)
            {
               if (t.type == "row")
                {
                    var model = new WorldCityModel();

                    if (t.row.Idx != 0)
                    {
                        model.Idx = t.row.Idx;
                    }
                    if (t.row.GEO != null)
                    {
                        model.GEO = t.row.GEO;
                    }
                    if (t.row.City != null)
                    {
                        model.City = t.row.City;
                    }
                    if (t.row.CityAscii != null)
                    {
                        model.CityAscii = t.row.CityAscii;
                    }
                    if (t.row.Country != null)
                    {
                        model.Country = t.row.Country;
                    }
                    if (t.row.ISO2 != null)
                    {
                        model.ISO2 = t.row.ISO2;
                    }
                    if (t.row.ISO3 != null)
                    {
                        model.ISO3 = t.row.ISO3;
                    }

                    if (t.row.Province != null)
                    {
                        model.Province = t.row.Province;
                    }

                    model.LAT = t.row.LAT;
                    model.LNG = t.row.LNG;
                    model.Population = t.row.Population;


                    cityList.Add(model);
                }
                
            }

            foreach(var Item in cityList)
            {
                await srv.SaveGEO(Item);
            }
           
            return View(cityList.ToArray());
          
        }
    }
}
