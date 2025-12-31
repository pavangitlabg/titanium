using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Models;
using ISSB.Models;
using Microsoft.Extensions.Hosting;
using Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace ISSB.Helpers
{
    public class ReportRunService : BackgroundService
    {
        private readonly SemaphoreSlim semaphoreReportRunning = new SemaphoreSlim(1, 1);
        private bool HasRun = false;

        public ReportRunService()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {

                var dt = DateTime.Now;
                var controlSrv = new SystemControlService();
                var shData = await controlSrv.GetSystemControl();

                if (dt.Hour == shData.ScheduleHour)
                {
                    if (semaphoreReportRunning.CurrentCount == 1)
                    {
                        await semaphoreReportRunning.WaitAsync();
                        try
                        {
                            if(!HasRun)
                               await RunReportAutomation();
                        }
                        finally
                        {
                            HasRun = true;
                            semaphoreReportRunning.Release();
                        }
                    }
                }
                else
                {
                    HasRun = false;
                }

                await Task.Delay(1000);
            }
        }


        private async Task RunReportAutomation()
        {
            var dt = DateTime.Now;
            var srv = new QueryService();
            var data = await srv.GetSheduledReoports(dt.Day);
          
            foreach (var Item in data)
            {
                await Download(Item.Id);
                //Send Email
                await SendMail(Item.Id);
            }

        }

        public async Task SendMail(string Id)
        {
            var srv = new QueryService();
            var model = await srv.GetReportByID(Id);

            var usrSrv = new UserServices();
            var user = await usrSrv.GetUser(model.User);

            if(user.EmailNotification)
            {
                var emailSrv = new EmailService();

                try
                {
                    var msgSrv = new EmailMessagesService();
                    var messageModel = await msgSrv.GetMessageByMessageId(2);
                    string subject = messageModel.Title;
                    string body = messageModel.HTML.Replace("[USER]", user.FirstName).Replace("[DATATYPE]", model.Name);
                    await emailSrv.SendEmailTo(user.Email, subject, body);

                }
                catch (Exception e)
                {
                    var eSrv = new ErrorService();
                    var eModel = new ErrorModel { Code = "91", Class = "ReportRunService Code 99", ErrorMessage = e.Message };
                    await eSrv.UpdateError(eModel);
                }
            }
        }


        public async Task Download(string Id)
        {
       
            var srv = new QueryService();
            var model = await srv.GetReportByID(Id);
     
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

            var usrSrv = new UserServices();

            var UserModel = await usrSrv.GetUser(model.User);

            // var UserId = User.FindFirst(ClaimTypes.Sid).Value;
            var UserId = UserModel._id;
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
           // return RedirectToAction("Index", "ReportsDownloads");
            // return new EmptyResult();

        }


        public async Task<ReportEngineModel> SearchResults(string Id)
        {

            var srv = new QueryService();
            var model = await srv.GetReportByID(Id);
            //ViewBag.Title = model.Name;
            //ViewBag.ReportId = Id;
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
            //Force a Grid Width Calc here.

            var cB = new Infragistics.Web.Mvc.GridColumnBuilder<ReportOutModel>();
            viewModel.Columns(cB);

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

            return viewModel;
        }

    }
}
