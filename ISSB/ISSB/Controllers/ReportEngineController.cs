/*
    ISSB/Controllers/ReportEngineController.cs

    reporting engine controller
*/

using System.Collections.Generic ;
using System.Linq ;
using System.Threading.Tasks ;
using Data.Models ;
using Microsoft.AspNetCore.Authorization ;
using Microsoft.AspNetCore.Mvc ;
using Services ;
using ISSB.Models ;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ISSB.Controllers
{
    public class ReportEngineController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }

        /// <summary>search results</summary>
        /// <param name="Id">report ID</param>
        /// <returns>action result</returns>

        [HttpGet] [Authorize] public async Task<ActionResult> SearchResults( string Id )
        {
            
            var srv = new QueryService() ;
            var model = await srv.GetReportByID( Id ) ;
            ViewBag.Title = model.Name;
            ViewBag.ReportId = Id;
            if(model.ProductGroupType.Equals("Long"))
            {
                var tariffSrv = new TariffService();
                var productList = new List<SaveQueryProductsModel>();
                foreach (var product in model.Products)
                {
                    int pID = await tariffSrv.ConvertFromGuidToId(product.Product);
                    var prodModel = new SaveQueryProductsModel { Product = pID.ToString() };
                    productList.Add(prodModel);
                }
                model.Products = productList;
            }

            if (model.ProductGroupType.Equals("Custom"))
            {
                model.ProductGroupType = "Long";
                var tariffSrv = new TariffService();
                var filterSrv = new ProductCustomFilterService();
                var productList = new List<SaveQueryProductsModel>();

                foreach (var item in model.Products)
                {
                    var product = await filterSrv.GetByID(item.Product);

                    foreach(var tCode in product.Items)
                    {
                        if (tCode.TariffCode.Length == 6)
                        {
                            model.ProductGroupType = "Broad";
                            var tModel = await tariffSrv.GetHSTariffByCode(tCode.TariffCode);
                            if (tModel != null)
                            {
                                //var pID = await tariffSrv.GetHSTariffByCode(tCode.TariffCode);
                                var prodModel = new SaveQueryProductsModel { Product = tCode.TariffCode };
                                productList.Add(prodModel);

                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(product.SearchType))
                                product.SearchType = "CN";

                            var tModel = await tariffSrv.GetTariffByCodeAndRegion(tCode.TariffCode, product.SearchType);
                            if (tModel._id != null)
                            {
                                int pID = await tariffSrv.ConvertFromGuidToId(tModel._id);
                                var prodModel = new SaveQueryProductsModel { Product = pID.ToString() };
                                productList.Add(prodModel);

                            }
                        }
                    }
                }
                model.Products = productList;
            }

            var dateList = new List<int>() ;
            foreach( var d in model.TimeIDs ) dateList.Add( d.TimeID ) ;

            var scList = new List<string>() ;
            foreach( var sc in model.SourceCountryGEOs ) scList.Add( sc.GeoCode ) ;

            var mcList = new List<string>() ;
            foreach( var mc in model.MarketCountryGEOs ) mcList.Add( mc.GeoCode ) ;

            var prodList = new List<string>() ;
            foreach ( var p in model.Products ) prodList.Add( p.Product ) ;

            var portList = new List<int>() ;
            if ( model.IncludePorts ) foreach( var p in model.Ports ) portList.Add( p.PortID ) ;

            var searchModel = new SearchModel
            {
                Id = model.Id ,
                Date = model.Date ,
                User = model.User ,
                Name = model.Name ,
                Description = model.Description ,
                Comments = model.Comments ,

                SourceCountryGeoCodes = scList ,
                MarketCountryGeoCodes = mcList ,
                IncludePorts = model.IncludePorts ,
                Ports = portList,
                Products = prodList ,
                GroupByMonth = model.GroupByMonth ,
                GroupByQuarter = model.GroupByQuarter ,
                GroupByYear = model.GroupByYear ,
                Dates = dateList ,

                SourceCountryGroup = model.SourceCountryGroup ,
                TradeFlowType = model.TradeFlowType ,
                MarketCountryGroup = model.MarketCountryGroup ,
                PortGroup = model.PortGroup ,
                ProductGroupType = model.ProductGroupType ,
                ProductGroup = model.ProductGroup ,
                TonnesValuesGroup = model.TonnesValuesGroup
            } ;

            var sz = JsonConvert.SerializeObject(searchModel);
            var json = JsonConvert.DeserializeObject<SearchModel>(sz);

            var res = new ReportEngineService() ;
            var factory = await res.GetReport( searchModel ) ;
            var viewModel = new ReportEngineModel { Search = searchModel , Data = (IQueryable<ReportOutModel>)factory.Queryable } ;
            await viewModel.ConvertToCurrency(model.SelectedCurrency);
            //Force a Grid Width Calc here.

            var cB =  new Infragistics.Web.Mvc.GridColumnBuilder<ReportOutModel>() ;
            viewModel.Columns( cB );

            //if(model.SaveToVisitorPageIndex)
            //{
            //    await srv.DeleteCache();
            //    var UserEmail = User.FindFirst(ClaimTypes.Email).Value;
            //    //var DashModel = await srv.GetReportByIsVisitorDashboard(UserEmail);
            //    var cModel = new CachedReportsModel
            //    {
            //         IsVisitorPage = model.SaveToVisitorPageIndex,
            //         User = UserEmail,
            //         Model = viewModel.Data
            //    };
            //    await srv.AddCacheReport(cModel);
            //}

            return View( viewModel ) ;
        }
    }
}