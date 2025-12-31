/*
    ISSB/Controllers/DashboardController.cs

    dashboard controller
*/

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Collections.Generic;
using System.Linq;
using ISSB.Models;

namespace ISSB.Controllers
{
    class Dept { public int Budget; public string Department; }

    public class DashboardController : Controller
    {
        /// <summary>Return search results.</summary>
        /// <param name="Id">saved query ID</param>
        /// <returns>search results</returns>

        private async Task<ReportEngineModel> SearchResults(string Id)
        {
            var srv = new QueryService();
            var model = await srv.GetReportByID(Id);

            var dateList = new List<int>();
            foreach (var d in model.TimeIDs) dateList.Add(d.TimeID);

            var scList = new List<string>();
            foreach (var sc in model.SourceCountryGEOs) scList.Add(sc.GeoCode);

            var mcList = new List<string>();
            foreach (var mc in model.MarketCountryGEOs) mcList.Add(mc.GeoCode);

            var prodList = new List<string>();
            foreach (var p in model.Products) prodList.Add(p.Product);

            var portList = new List<int>();
            if (model.IncludePorts) foreach (var p in model.Ports) portList.Add(p.PortID);

            var searchModel = new SearchModel
            {
                Id = model.Id,
                Date = model.Date,
                User = model.User,
                Name = model.Name,
                Description = model.Description,
                Comments = model.Comments,

                SourceCountryGeoCodes = scList,
                MarketCountryGeoCodes = mcList,
                IncludePorts = model.IncludePorts,
                Ports = portList,
                Products = prodList,
                GroupByMonth = model.GroupByMonth,
                GroupByQuarter = model.GroupByQuarter,
                GroupByYear = model.GroupByYear,
                Dates = dateList,

                SourceCountryGroup = model.SourceCountryGroup,
                TradeFlowType = model.TradeFlowType,
                MarketCountryGroup = model.MarketCountryGroup,
                PortGroup = model.PortGroup,
                ProductGroupType = model.ProductGroupType,
                ProductGroup = model.ProductGroup,
                TonnesValuesGroup = model.TonnesValuesGroup
            };

            var res = new ReportEngineService();
            var factory = await res.GetReport(searchModel);
            var viewModel = new ReportEngineModel { Search = searchModel, Data = (IQueryable<ReportOutModel>)factory.Queryable };
            await viewModel.ConvertToCurrency(model.SelectedCurrency);
            return viewModel;
        }

        # region endpoints

        [Authorize]
        public IActionResult Index() { return View(); }

        /// <summary>Display a chart.</summary>
        /// <param name="type">type parameter</param>
        /// <returns>view</returns>

        [Authorize]
        public async Task<IActionResult> Chart(int type, string query)
        {
            var model = new ChartModel { Type = type, Query = query, EngineModel = await SearchResults(query) };

            return View(model);
        }

        #endregion
    }
}