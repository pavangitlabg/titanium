using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Models;
using Data.Models;
using Infragistics.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportDataController : ControllerBase
{
    [HttpPost("GetData")]
    [Authorize(Roles = "API_User")]
    [SwaggerOperation(Summary = "Get Query Data", Description = "Get Query Data")]
    public async Task<IActionResult> GetData(string QueryName)
    {
        var handler = new JwtSecurityTokenHandler();
        string authHeader = Request.Headers["Authorization"];
        authHeader = authHeader.Replace("Bearer ", "");
        var jsonToken = handler.ReadToken(authHeader);
        var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
        var id = tokenS.Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value;

        var srv = new QueryService();

        var reportID = await srv.GetReportIDFromName(QueryName);

        if (reportID.Equals("NotFound")) return Ok("Query Not Found");

        var model = await srv.GetReportByID(reportID);
        //ViewBag.Title = model.Name;
        //ViewBag.ReportId = Id;
        if (model.ProductGroupType.Equals("Long"))
        {
            var tariffSrv = new TariffService();
            var productList = new List<SaveQueryProductsModel>();
            foreach (var product in model.Products)
            {
                var pID = await tariffSrv.ConvertFromGuidToId(product.Product);
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

                foreach (var tCode in product.Items)
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
                            var pID = await tariffSrv.ConvertFromGuidToId(tModel._id);
                            var prodModel = new SaveQueryProductsModel { Product = pID.ToString() };
                            productList.Add(prodModel);
                        }
                    }
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
        if (model.IncludePorts)
            foreach (var p in model.Ports)
                portList.Add(p.PortID);

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

        var sz = JsonConvert.SerializeObject(searchModel);
        var json = JsonConvert.DeserializeObject<SearchModel>(sz);

        var res = new ReportEngineService();
        var factory = await res.GetReport(searchModel);
        var viewModel = new ReportEngineModel
            { Search = searchModel, Data = (IQueryable<ReportOutModel>)factory.Queryable };
        await viewModel.ConvertToCurrency(model.SelectedCurrency);
        //Force a Grid Width Calc here.

        var cB = new GridColumnBuilder<ReportOutModel>();
        viewModel.Columns(cB);

        return Ok(viewModel.Serialised);
    }
}