using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using ISSB.Models;

namespace ISSB.Controllers
{
    public class ReportsDownloadsController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var UserId = User.FindFirst(ClaimTypes.Sid).Value;
            var srv = DownloadsService.Instance;
            var model = await srv.GetDownloads(UserId);
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            var srv = new DownloadsService();
            var modelOut = await srv.GetById(Id);
            await srv.Delete(modelOut);

            //Remove File
            srv.DeleteFile(modelOut.FilePath);

            return RedirectToAction("Index", "ReportsDownloads");

        }

        [Authorize]
        public async Task<IActionResult> Download(string Id)
        {
//Task.Run(async () =>
  //          {
                var srv = new QueryService();
                var model = await srv.GetReportByID(Id);
                ViewBag.Title = model.Name;
                ViewBag.ReportId = Id;
                if (model.ProductGroupType.Equals("Long"))
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

                var data = new ReportEngineModel { Search = searchModel, Data = (IQueryable<ReportOutModel>)factory.Queryable };
                await data.ConvertToCurrency(model.SelectedCurrency);

                var column = new Infragistics.Web.Mvc.GridColumnBuilder<ReportOutModel>();
                var cols = data.Columns(column);

                var HearderList = new List<CSVHeaderFieldsModel>();
                foreach (var colItem in cols)
                {
                    var hModel = new CSVHeaderFieldsModel { FieldName = colItem.Key, Name = colItem.HeaderText };
                    HearderList.Add(hModel);
                }

                var UserId = User.FindFirst(ClaimTypes.Sid).Value;
                var downloadSrv = DownloadsService.Instance;
                var dataList = data.Data.ToList();

                var DataCSVList = new List<ReportCSVModel>();

                foreach (var d in dataList)
                {
                    var newTARIFF_LEGEND = string.Empty;
                    if (!string.IsNullOrEmpty(d.TARIFF_LEGEND))
                        newTARIFF_LEGEND = d.TARIFF_LEGEND.Replace(",", "-");

                    //ToDo Fix the GEO Formating
                    var m = new ReportCSVModel
                    {
                        ID = d.ID,
                        MC_GEO = d.MC_GEO,
                        MC_NAME = d.MC_NAME,
                        MONETARY_VALUE = d.MONETARY_VALUE,
                        MONTH = d.MONTH,
                        PORT_ID = d.PORT_ID,
                        PORT_NAME = d.PORT_NAME,
                        QUARTER = d.QUARTER,
                        SC_GEO = d.SC_GEO,
                        SC_NAME = d.SC_NAME,
                        SIDE_OF_TRADE = d.SIDE_OF_TRADE,
                        TARIFF_CODE = d.TARIFF_CODE,
                        TARIFF_LEGEND = newTARIFF_LEGEND,
                        TIME_ID = d.TIME_ID,
                        WEIGHT = d.WEIGHT,
                        YEAR = d.YEAR,
                        YTD_MONETARY_VALUE = d.YTD_MONETARY_VALUE,
                        YTD_WEIGHT = d.YTD_WEIGHT

                    };
                    DataCSVList.Add(m);
                }

                await downloadSrv.SaveFile(Id, UserId, HearderList, DataCSVList, model.Name);
           // });
            return RedirectToAction("Index", "ReportsDownloads");
           // return new EmptyResult();

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DownloadFile(string ID)
        {
            var srv = DownloadsService.Instance;

            var model = await srv.GetById(ID);

            var newFileName = model.ReportName.TrimEnd();

            var net = new System.Net.WebClient();
            var data = net.DownloadData(model.FilePath);
            var content = new System.IO.MemoryStream(data);
            var contentType = "APPLICATION/octet-stream";
            var fileName = newFileName.Replace(",","") + ".csv.zip";
            return File(content, contentType, fileName);

        }
    }
}
