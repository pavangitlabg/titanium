using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using ISSB.Helpers;
using MongoDB.Bson;
using Data.Enums;

namespace ISSB.Controllers
{
    public class PipeLineBuilderController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index(string id, int iType, int itemType,int item,string RecordId)
        {
            var iRecordId = id;
            int Type = iType;
            int DashItemType = itemType;
            int DashItem = item;

            var model = new QueryModel();
            model.User = string.Empty;

            //var scSrv = new SourceCountryService();
            //model.SourceCountries = await scSrv.GetSourceCountries();
            var userSrv = new UserServices();
            var UserId = User.FindFirst(ClaimTypes.Sid).Value;
            var userModel = await userSrv.GetUsersById(UserId);
            model.SourceCountries = userModel.AllowedSourceCountries;

            model.CustomTariff = userModel.AllowedCustomFilters;

            // Get The latest Update Date from Master

            var scSrv = new SourceCountryService();

            foreach(var scItem in model.SourceCountries)
            {
                var scModel = await scSrv.GetSourceCountryByGeoCode(scItem.GEO_CODE);
                scItem.LATEST_DATE = scModel.LATEST_DATE;
            }

            var mcSrv = new MarketCountryService();
            model.MarketCountries = await mcSrv.GetActiveMarketCountries();

            var srSrv = new SourceCountryFilterService();
            model.SourceRegions = await srSrv.GetFilters();

            var mcfSrv = new MarketCountryFilterService();
            model.MarketRegions = await mcfSrv.GetFilters();

            var portSrv = new PortsService();
            model.Ports = await portSrv.GetAllPorts();

            var srv = new ProductService();
           
            if(userModel.IsAdministrator)
            {
                ViewBag.IsAdministrator = true;
            }

            if (userModel.IsAdvancedUser)
            {
                //Check for SuperUser

                var tariffsrv = new TariffService();
                var tariffIndexData = await tariffsrv.GetTariffIndex();

                IQueryable tariffIndex = tariffIndexData.AsQueryable();
                ViewBag.TariffIndex = tariffIndex;


                var tariff2DigitIndexData = await tariffsrv.GetTariff2DigitIndex();

                IQueryable tariff2DigitIndex = tariff2DigitIndexData.AsQueryable();
                ViewBag.Tariff2DigitIndex = tariff2DigitIndex;
                //Get the list of Tariff Codes based on 1st Index

               
                ViewBag.IsAdvancedUser = "True";

               
            }
            else
            {
                model.LongTariffCodes = new List<TariffShortModel>();
                userModel.IsAdvancedUser = false;
                ViewBag.IsAdvancedUser = "False";
            }

            var treeMedumModel = await srv.GetAllMedumProductsForTree();

            foreach (var Item in treeMedumModel)
            {
                var ItemItem = new List<ProductMedumFilterItemModel>();
                foreach (var ItemChild in Item.Items)
                {
                    var foundItem = userModel.AllowedTariffCodes.FirstOrDefault(x => x.TariffCode == ItemChild.TariffCode);
                    if (foundItem != null)
                    {

                        var pModel = new ProductMedumFilterItemModel
                        {
                            Name = ItemChild.Name,
                            TariffCode = ItemChild.TariffCode,
                            Description = ItemChild.Description,
                            Sort = ItemChild.Sort
                        };
                        ItemItem.Add(pModel);
                    }
                    Item.Items = ItemItem;
                }
            }

            IQueryable treeMedumData = treeMedumModel.AsQueryable();
            ViewBag.MedumTreeData = treeMedumData;


            var BroadItemItems = await srv.GetBroadProducts();
            var BroadItemItemAllowed = new List<ProductBroadFilterItemModel>();

            foreach (var Item in BroadItemItems)
            {
                var foundItem = userModel.AllowedTariffCodes.FirstOrDefault(x => x.TariffCode == Item.TariffCode);
                if (foundItem != null)
                {
                    var pModel = new ProductBroadFilterItemModel
                    {
                        Name = foundItem.Name,
                        TariffCode = foundItem.TariffCode,
                        Description = foundItem.Name,
                        Sort = foundItem.SortOrder
                    };
                    BroadItemItemAllowed.Add(pModel);
                }
                
            }
            model.HighTree = BroadItemItemAllowed;

            var treeBroadModel = await srv.GetAllBroadProducts();
            IQueryable treeBroadData = treeBroadModel.AsQueryable();
            ViewBag.BroadTreeData = treeBroadData;
            

            //All Tariff H Codes
            var treeAllModel = await srv.GetAllProductsForTree();
            var treeAllModelAllowed = new List<ProductAllFilterModel>();
            foreach (var TreeAll in treeAllModel)
            {
                var foundItem = userModel.AllowedTariffCodes.FirstOrDefault(x => x.TariffCode == TreeAll.TariffCode);
                if (foundItem != null)
                {
                    foundItem.Name = TreeAll.Name;
                    treeAllModelAllowed.Add(foundItem);
                }

            }

            IQueryable dataAll = treeAllModelAllowed.AsQueryable();
            ViewBag.AllTreeData = dataAll;

            var yearSrv = new LookupsService();
            var modelYear = await yearSrv.GetYearsDropdown();
            //IQueryable dataYears = modelYear.AsQueryable();
            //ViewBag.YearTreeData = dataYears;

            var tableYears = new List<QueryDateMonthModel>();
            int cnt = 0;
            foreach (var year in modelYear)
            {
                var mYear = new QueryDateMonthModel
                {
                    Id = cnt++,
                    Year = int.Parse(year.Text),
                    January = false,
                    February = false,
                    March = false,
                    April = false,
                    May = false,
                    June = false,
                    July = false,
                    August = false,
                    September = false,
                    October = false,
                    November = false,
                    December = false
                };
                tableYears.Add(mYear);
            }

            var AllowedYears = userModel.AllowedYears;
            string[] years = AllowedYears.Split(',');

            var tableAllowedYears = new List<QueryDateMonthModel>();

            foreach (var y in years)
            {
                var yModel = tableYears.FirstOrDefault(s => s.Year.Equals(int.Parse(y)));
                tableAllowedYears.Add(yModel);
            }
            tableYears = tableAllowedYears;
          

            if (Type == 0)
            {
                var reportService = new QueryService();
                model.CurrencyUsed = await reportService.CurencyUsed();

            }
            if (Type == 1)
            {
                ViewBag.Type = Type;
                var uEmail = GetUserEmail();

                var reportService = new QueryService();
                var reportModel = await reportService.GetReportByID(uEmail, iRecordId);
                // var reportModel = await reportService.GetReportByID(RecordId);
                model.CurrencyUsed = await reportService.CurencyUsed();
       

                ViewBag.SourceCountryGroup = reportModel.SourceCountryGroup;
                ViewBag.MarketCountryGroup = reportModel.MarketCountryGroup;
                ViewBag.TradeFlowType = reportModel.TradeFlowType;
                ViewBag.ProductGroupType = reportModel.ProductGroupType;
                ViewBag.ProductGroup = reportModel.ProductGroup;
                ViewBag.TonnesValuesGroup = reportModel.TonnesValuesGroup;
                ViewBag.GroupByMonth = reportModel.GroupByMonth.ToString();
                ViewBag.GroupByQuarter = reportModel.GroupByQuarter.ToString();
                ViewBag.GroupByYear = reportModel.GroupByYear.ToString();
                ViewBag.SaveToVisitorPageIndex = reportModel.SaveToVisitorPageIndex.ToString();
                ViewBag.SaveToDashboard = reportModel.SaveToDashboard.ToString();
                ViewBag.SourceCountryGEOs = JsonConvert.SerializeObject(reportModel.SourceCountryGEOs);
                ViewBag.MarketCountryGEOs = JsonConvert.SerializeObject(reportModel.MarketCountryGEOs);
                ViewBag.Products = JsonConvert.SerializeObject(reportModel.Products);
                ViewBag.Ports = JsonConvert.SerializeObject(reportModel.Ports);
                ViewBag.UsePorts = reportModel.IncludePorts.ToString();
                ViewBag.PortsGrouping = reportModel.PortGroup;
                ViewBag.SelectedCurency = reportModel.SelectedCurrency;
                ViewBag.SelectedDay = reportModel.SelectedSheduleDay;
                ViewBag.IsSchedule = reportModel.IsShedule.ToString();
                ViewBag.ReportName = reportModel.Name;
                ViewBag.ReportDescription = reportModel.Description;
                ViewBag.ReportComments = reportModel.Comments;
                ViewBag.Id = reportModel.Id;
               // ViewBag.Name = reportModel.Name;
                ViewBag.Message = " Name: " + reportModel.Name;
                ViewBag.TariffRegion = reportModel.ProductRegionCode;
                ViewBag.Tariff2Digit = reportModel.Product2DigitCode;

                var selectedMonths = reportModel.TimeIDs;


                foreach(var sMonth in selectedMonths)
                {
                    //tableYears 202010200

                    var year = int.Parse(sMonth.TimeID.ToString().Substring(0, 4));
                    var Mth = int.Parse(sMonth.TimeID.ToString().Substring(5, 2));
                    var tblItem = tableYears.Find(x => x.Year == year);

                    if (Mth == 1)
                    {
                        tblItem.January = true;
                    }

                    if (Mth == 2)
                    {
                        tblItem.February = true;
                    }

                    if (Mth == 3)
                    {
                        tblItem.March = true;
                    }

                    if (Mth == 4)
                    {
                        tblItem.April = true;
                    }

                    if (Mth == 5)
                    {
                        tblItem.May = true;
                    }

                    if (Mth == 6)
                    {
                        tblItem.June = true;
                    }

                    if (Mth == 7)
                    {
                        tblItem.July = true;
                    }

                    if (Mth == 8)
                    {
                        tblItem.August = true;
                    }

                    if (Mth == 9)
                    {
                        tblItem.September = true;
                    }

                    if (Mth == 10)
                    {
                        tblItem.October = true;
                    }

                    if (Mth == 11)
                    {
                        tblItem.November = true;
                    }

                    if (Mth == 12)
                    {
                        tblItem.December = true;
                    }
                }


            }
            //from dashboard builder
            if(Type == 2)
            {
                
                ViewBag.DashItemType = DashItemType;
                ViewBag.DashItem = DashItem;
                ViewBag.DashRecordID = RecordId;

                ViewBag.Type = Type;
                var uEmail = GetUserEmail();

                // get the query
                var reportService = new UserDashboardService();

               // if(RecordId != null)

                    //var dashModel = await reportService.GetCurrentDesignDashboard("CHAS", true);
                var dashModel = await reportService.GetCurrentDashboardByGUID(RecordId);

                var reportModel = dashModel.Items[int.Parse(iRecordId) - 1];
                // var reportModel = dashModel.Items[DashItem - 1];
                ViewBag.Id = iRecordId;
                if (DashItem == 1)
                {
                    if (reportModel.Query1.SourceCountryGEOs == null)
                    {
                        ViewBag.Type = 3;

                    }
                    else
                    {

                        ViewBag.SourceCountryGroup = reportModel.Query1.SourceCountryGroup;
                        ViewBag.MarketCountryGroup = reportModel.Query1.MarketCountryGroup;
                        ViewBag.TradeFlowType = reportModel.Query1.TradeFlowType;
                        ViewBag.ProductGroupType = reportModel.Query1.ProductGroupType;
                        ViewBag.ProductGroup = reportModel.Query1.ProductGroup;
                        ViewBag.TonnesValuesGroup = reportModel.Query1.TonnesValuesGroup;
                        ViewBag.GroupByMonth = reportModel.Query1.GroupByMonth.ToString();
                        ViewBag.GroupByQuarter = reportModel.Query1.GroupByQuarter.ToString();
                        ViewBag.GroupByYear = reportModel.Query1.GroupByYear.ToString();

                        ViewBag.SourceCountryGEOs = JsonConvert.SerializeObject(reportModel.Query1.SourceCountryGEOs);
                        ViewBag.MarketCountryGEOs = JsonConvert.SerializeObject(reportModel.Query1.MarketCountryGEOs);
                        ViewBag.Products = JsonConvert.SerializeObject(reportModel.Query1.Products);

                        if (reportModel.Query1.IncludePorts)
                        {
                            ViewBag.Ports = JsonConvert.SerializeObject(reportModel.Query1.Ports);
                        }
                        else
                        {
                            reportModel.Query1.Ports = new List<DashBoardQueryPortModel>();
                            ViewBag.Ports = JsonConvert.SerializeObject(reportModel.Query1.Ports);
                        }

                        ViewBag.UsePorts = reportModel.Query1.IncludePorts.ToString();
                        ViewBag.PortsGrouping = reportModel.Query1.PortGroup;

                        //get the years here

                        ViewBag.ReportName = reportModel.Query1.Name;
                        ViewBag.ReportDescription = reportModel.Query1.Description;
                        ViewBag.ReportComments = reportModel.Query1.Comments;
                       // ViewBag.Id = reportModel.Query1.Id;
                        // ViewBag.Name = reportModel.Name;
                        ViewBag.Message = " Name: " + reportModel.Query1.Name;

                        var selectedMonths = reportModel.Query1.TimeIDs;

                        foreach (var sMonth in selectedMonths)
                        {
                            var year = int.Parse(sMonth.TimeID.ToString().Substring(0, 4));
                            var Mth = int.Parse(sMonth.TimeID.ToString().Substring(5, 2));
                            var tblItem = tableYears.Find(x => x.Year == year);

                            if (Mth == 1)
                            {
                                tblItem.January = true;
                            }

                            if (Mth == 2)
                            {
                                tblItem.February = true;
                            }

                            if (Mth == 3)
                            {
                                tblItem.March = true;
                            }

                            if (Mth == 4)
                            {
                                tblItem.April = true;
                            }

                            if (Mth == 5)
                            {
                                tblItem.May = true;
                            }

                            if (Mth == 6)
                            {
                                tblItem.June = true;
                            }

                            if (Mth == 7)
                            {
                                tblItem.July = true;
                            }

                            if (Mth == 8)
                            {
                                tblItem.August = true;
                            }

                            if (Mth == 9)
                            {
                                tblItem.September = true;
                            }

                            if (Mth == 10)
                            {
                                tblItem.October = true;
                            }

                            if (Mth == 11)
                            {
                                tblItem.November = true;
                            }

                            if (Mth == 12)
                            {
                                tblItem.December = true;
                            }
                        }

                }
                }

                if (DashItem == 2)
                {
                    if (reportModel.Query2.SourceCountryGEOs == null)
                    {
                        ViewBag.Type = 3;

                    }
                    else
                    {

                        ViewBag.SourceCountryGroup = reportModel.Query2.SourceCountryGroup;
                        ViewBag.MarketCountryGroup = reportModel.Query2.MarketCountryGroup;
                        ViewBag.TradeFlowType = reportModel.Query2.TradeFlowType;
                        ViewBag.ProductGroupType = reportModel.Query2.ProductGroupType;
                        ViewBag.ProductGroup = reportModel.Query2.ProductGroup;
                        ViewBag.TonnesValuesGroup = reportModel.Query2.TonnesValuesGroup;
                        ViewBag.GroupByMonth = reportModel.Query2.GroupByMonth.ToString();
                        ViewBag.GroupByQuarter = reportModel.Query2.GroupByQuarter.ToString();
                        ViewBag.GroupByYear = reportModel.Query2.GroupByYear.ToString();

                        ViewBag.SourceCountryGEOs = JsonConvert.SerializeObject(reportModel.Query2.SourceCountryGEOs);
                        ViewBag.MarketCountryGEOs = JsonConvert.SerializeObject(reportModel.Query2.MarketCountryGEOs);
                        ViewBag.Products = JsonConvert.SerializeObject(reportModel.Query2.Products);

                        if (reportModel.Query2.IncludePorts)
                        {
                            ViewBag.Ports = JsonConvert.SerializeObject(reportModel.Query2.Ports);
                        }
                        else
                        {
                            reportModel.Query2.Ports = new List<DashBoardQueryPortModel>();
                            ViewBag.Ports = JsonConvert.SerializeObject(reportModel.Query2.Ports);
                        }


                        ViewBag.UsePorts = reportModel.Query2.IncludePorts.ToString();
                        ViewBag.PortsGrouping = reportModel.Query2.PortGroup;

                    

                        ViewBag.ReportName = reportModel.Query2.Name;
                        ViewBag.ReportDescription = reportModel.Query2.Description;
                        ViewBag.ReportComments = reportModel.Query2.Comments;
                        //ViewBag.Id = reportModel.Query2.Id;
                       // ViewBag.Name = reportModel.Name;
                        ViewBag.Message = " Name: " + reportModel.Query2.Name;

                        var selectedMonths = reportModel.Query2.TimeIDs;

                        foreach (var sMonth in selectedMonths)
                        {
                            var year = int.Parse(sMonth.TimeID.ToString().Substring(0, 4));
                            var Mth = int.Parse(sMonth.TimeID.ToString().Substring(5, 2));
                            var tblItem = tableYears.Find(x => x.Year == year);

                            if (Mth == 1)
                            {
                                tblItem.January = true;
                            }

                            if (Mth == 2)
                            {
                                tblItem.February = true;
                            }

                            if (Mth == 3)
                            {
                                tblItem.March = true;
                            }

                            if (Mth == 4)
                            {
                                tblItem.April = true;
                            }

                            if (Mth == 5)
                            {
                                tblItem.May = true;
                            }

                            if (Mth == 6)
                            {
                                tblItem.June = true;
                            }

                            if (Mth == 7)
                            {
                                tblItem.July = true;
                            }

                            if (Mth == 8)
                            {
                                tblItem.August = true;
                            }

                            if (Mth == 9)
                            {
                                tblItem.September = true;
                            }

                            if (Mth == 10)
                            {
                                tblItem.October = true;
                            }

                            if (Mth == 11)
                            {
                                tblItem.November = true;
                            }

                            if (Mth == 12)
                            {
                                tblItem.December = true;
                            }
                        }
                    }
                }

                if (DashItem == 3)
                {
                    if (reportModel.Query3.SourceCountryGEOs == null)
                    {
                        ViewBag.Type = 3;

                    }
                    else
                    {

                        ViewBag.SourceCountryGroup = reportModel.Query3.SourceCountryGroup;
                        ViewBag.MarketCountryGroup = reportModel.Query3.MarketCountryGroup;
                        ViewBag.TradeFlowType = reportModel.Query3.TradeFlowType;
                        ViewBag.ProductGroupType = reportModel.Query3.ProductGroupType;
                        ViewBag.ProductGroup = reportModel.Query3.ProductGroup;
                        ViewBag.TonnesValuesGroup = reportModel.Query3.TonnesValuesGroup;
                        ViewBag.GroupByMonth = reportModel.Query3.GroupByMonth.ToString();
                        ViewBag.GroupByQuarter = reportModel.Query3.GroupByQuarter.ToString();
                        ViewBag.GroupByYear = reportModel.Query3.GroupByYear.ToString();

                        ViewBag.SourceCountryGEOs = JsonConvert.SerializeObject(reportModel.Query3.SourceCountryGEOs);
                        ViewBag.MarketCountryGEOs = JsonConvert.SerializeObject(reportModel.Query3.MarketCountryGEOs);
                        ViewBag.Products = JsonConvert.SerializeObject(reportModel.Query3.Products);

                        if (reportModel.Query3.IncludePorts)
                        {
                            ViewBag.Ports = JsonConvert.SerializeObject(reportModel.Query3.Ports);
                        }
                        else
                        {
                            reportModel.Query3.Ports = new List<DashBoardQueryPortModel>();
                            ViewBag.Ports = JsonConvert.SerializeObject(reportModel.Query3.Ports);
                        }
                       
                        ViewBag.UsePorts = reportModel.Query3.IncludePorts.ToString();
                        ViewBag.PortsGrouping = reportModel.Query3.PortGroup;

                    

                        ViewBag.ReportName = reportModel.Query3.Name;
                        ViewBag.ReportDescription = reportModel.Query3.Description;
                        ViewBag.ReportComments = reportModel.Query3.Comments;
                       // ViewBag.Id = reportModel.Query3.Id;
                        // ViewBag.Name = reportModel.Name;
                        ViewBag.Message = " Name: " + reportModel.Query3.Name;
                        var selectedMonths = reportModel.Query3.TimeIDs;

                        foreach (var sMonth in selectedMonths)
                        {
                            var year = int.Parse(sMonth.TimeID.ToString().Substring(0, 4));
                            var Mth = int.Parse(sMonth.TimeID.ToString().Substring(5, 2));
                            var tblItem = tableYears.Find(x => x.Year == year);

                            if (Mth == 1)
                            {
                                tblItem.January = true;
                            }

                            if (Mth == 2)
                            {
                                tblItem.February = true;
                            }

                            if (Mth == 3)
                            {
                                tblItem.March = true;
                            }

                            if (Mth == 4)
                            {
                                tblItem.April = true;
                            }

                            if (Mth == 5)
                            {
                                tblItem.May = true;
                            }

                            if (Mth == 6)
                            {
                                tblItem.June = true;
                            }

                            if (Mth == 7)
                            {
                                tblItem.July = true;
                            }

                            if (Mth == 8)
                            {
                                tblItem.August = true;
                            }

                            if (Mth == 9)
                            {
                                tblItem.September = true;
                            }

                            if (Mth == 10)
                            {
                                tblItem.October = true;
                            }

                            if (Mth == 11)
                            {
                                tblItem.November = true;
                            }

                            if (Mth == 12)
                            {
                                tblItem.December = true;
                            }
                        }
                    }
                }

                if (DashItem == 4)
                {
                    if (reportModel.Query4.SourceCountryGEOs == null)
                    {
                        ViewBag.Type = 3;

                    }
                    else
                    {

                        ViewBag.SourceCountryGroup = reportModel.Query4.SourceCountryGroup;
                        ViewBag.MarketCountryGroup = reportModel.Query4.MarketCountryGroup;
                        ViewBag.TradeFlowType = reportModel.Query4.TradeFlowType;
                        ViewBag.ProductGroupType = reportModel.Query4.ProductGroupType;
                        ViewBag.ProductGroup = reportModel.Query4.ProductGroup;
                        ViewBag.TonnesValuesGroup = reportModel.Query4.TonnesValuesGroup;
                        ViewBag.GroupByMonth = reportModel.Query4.GroupByMonth.ToString();
                        ViewBag.GroupByQuarter = reportModel.Query4.GroupByQuarter.ToString();
                        ViewBag.GroupByYear = reportModel.Query4.GroupByYear.ToString();

                        ViewBag.SourceCountryGEOs = JsonConvert.SerializeObject(reportModel.Query4.SourceCountryGEOs);
                        ViewBag.MarketCountryGEOs = JsonConvert.SerializeObject(reportModel.Query4.MarketCountryGEOs);
                        ViewBag.Products = JsonConvert.SerializeObject(reportModel.Query4.Products);

                        if (reportModel.Query4.IncludePorts)
                        {
                            ViewBag.Ports = JsonConvert.SerializeObject(reportModel.Query4.Ports);
                        }
                        else
                        {
                            reportModel.Query4.Ports = new List<DashBoardQueryPortModel>();
                            ViewBag.Ports = JsonConvert.SerializeObject(reportModel.Query4.Ports);
                        }

                       
                        ViewBag.UsePorts = reportModel.Query4.IncludePorts.ToString();
                        ViewBag.PortsGrouping = reportModel.Query4.PortGroup;


                        ViewBag.ReportName = reportModel.Query4.Name;
                        ViewBag.ReportDescription = reportModel.Query4.Description;
                        ViewBag.ReportComments = reportModel.Query4.Comments;
                        //ViewBag.Id = reportModel.Query4.Id;
                        // ViewBag.Name = reportModel.Name;
                        ViewBag.Message = " Name: " + reportModel.Query4.Name;

                        var selectedMonths = reportModel.Query4.TimeIDs;
                        if (selectedMonths != null)
                        {
                            foreach (var sMonth in selectedMonths)
                            {
                                var year = int.Parse(sMonth.TimeID.ToString().Substring(0, 4));
                                var Mth = int.Parse(sMonth.TimeID.ToString().Substring(5, 2));
                                var tblItem = tableYears.Find(x => x.Year == year);

                                if (Mth == 1)
                                {
                                    tblItem.January = true;
                                }

                                if (Mth == 2)
                                {
                                    tblItem.February = true;
                                }

                                if (Mth == 3)
                                {
                                    tblItem.March = true;
                                }

                                if (Mth == 4)
                                {
                                    tblItem.April = true;
                                }

                                if (Mth == 5)
                                {
                                    tblItem.May = true;
                                }

                                if (Mth == 6)
                                {
                                    tblItem.June = true;
                                }

                                if (Mth == 7)
                                {
                                    tblItem.July = true;
                                }

                                if (Mth == 8)
                                {
                                    tblItem.August = true;
                                }

                                if (Mth == 9)
                                {
                                    tblItem.September = true;
                                }

                                if (Mth == 10)
                                {
                                    tblItem.October = true;
                                }

                                if (Mth == 11)
                                {
                                    tblItem.November = true;
                                }

                                if (Mth == 12)
                                {
                                    tblItem.December = true;
                                }
                            }
                        }
                    }
                }




            }
            int yCnt = 0;
            foreach(var yNo in tableYears)
            {
                yNo.Id = yCnt;
                yCnt++;
            }
            model.YearTable = tableYears;
            model.SheduleDays = new List<SheduleDayModel>();

            for (int i = 1; i <= 28; i++)
            {
                var dayModel = new SheduleDayModel
                {
                    Code = i,
                    Day = i
                };
                model.SheduleDays.Add(dayModel);
            }
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetSourceGeo(string geoName)
        {
            var srSrv = new SourceCountryFilterService();
            var data = await srSrv.GetFilterByName(geoName);
            var GeoItems = new List<string>();

            foreach (var geo in data.Items)
            {
                var geoCode = geo.GeoCode;
                GeoItems.Add(geoCode);
            }

            return Json(new { success = true, data = GeoItems });
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetMarketGeo(string geoName)
        {
            var srSrv = new MarketCountryFilterService();
            var data = await srSrv.GetFilterByName(geoName);
            var GeoItems = new List<string>();

            foreach (var geo in data.Items)
            {
                var geoCode = geo.GeoCode;
                GeoItems.Add(geoCode);
            }

            return Json(new { success = true, data = GeoItems });
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetTariffRegions(string geoName)
        {
            var code = "CN";
           
            if (!string.IsNullOrEmpty(geoName))
            {
                string[] words = geoName.Split('-');
                code = words[0].Trim();
            }
            
           
            var srv = new ProductService(); 
            var Items = await srv.GetLongTarrifByCode(code);

            int RecCount = Items.Count();

            var tList = Items.OrderBy(x => x.SOURCE_COUNTRY_TARIFF_CODE).ToList();
            var jString = JsonConvert.SerializeObject(tList);
            var rRecords = JsonConvert.SerializeObject(RecCount);
            // return jString;
            return Json(new { success = true, data = jString , cnt  = rRecords });
        }

        

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetTariff2DigitRegions(string geoName, string Code2)
        {
            var code = "CN";
            var code2 = "";

            if (!string.IsNullOrEmpty(geoName))
            {
                string[] words = geoName.Split('-');
                code = words[0].Trim();
            }

            if (!string.IsNullOrEmpty(Code2))
            {
                string[] word1s = Code2.Split('-');
                code2 = word1s[0].Trim();
            }


            var srv = new ProductService();
            var Items = await srv.GetLongTarrifBy2DigitCode(code, code2);

            int RecCount = Items.Count();

            var tList = Items.OrderBy(x => x.SOURCE_COUNTRY_TARIFF_CODE).ToList();
            var jString = JsonConvert.SerializeObject(tList);
            var rRecords = JsonConvert.SerializeObject(RecCount);
            return Json(new { success = true, data = jString, cnt = rRecords });
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            var qSrv = new QueryService();
            await qSrv.Delete(id);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.SavedQueryModel,
                MESSAGE = "Pipeline Builder Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = id
            };
            new EventLog(EventModel);

            return Redirect("/AdHocReports/Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> UpdateSaveQuery(string SourceCountryGrouping,
                                                        string TradeFlowGrouping,
                                                        string MarketCountryGrouping,
                                                        string ProductType,
                                                        string ProductGrouping,
                                                        string ValuesGrouping,
                                                        bool GroupByMonth,
                                                        bool GroupByQuarter,
                                                        bool GroupByYear,
                                                        string SourceCountries,
                                                        string MarketCountries,
                                                        string Products,
                                                        string YearsMonths,
                                                        string Name,
                                                        string Description,
                                                        string Comments,
                                                        string Id,
                                                        bool UsePorts,
                                                        string PortGrouping,
                                                        string Ports,
                                                        string ComboTariffRegionSelector,
                                                        string ComboTariff2DigitSelector,
                                                        bool SaveToVisitorPageIndex,
                                                        bool SaveToDashboard,
                                                        string SelectedCurrency,
                                                        string SelectedSheduleDay,
                                                        bool SelectedShedule)
        {

            if (string.IsNullOrEmpty(SelectedSheduleDay))
                SelectedSheduleDay = "1";

            if (string.IsNullOrEmpty(SelectedCurrency))
                SelectedCurrency = "GBP";

            var sGeos = new List<SaveQuerySourceCountriesModel>();
            string[] GeoCodes = SourceCountries.Split('|');
            foreach (string geo in GeoCodes)
            {
                var sGeo = new SaveQuerySourceCountriesModel { GeoCode = geo };
                sGeos.Add(sGeo);
            }

            var mGeos = new List<SaveQueryMarketCountriesModel>();
            string[] mGeoCodes = MarketCountries.Split('|');
            foreach (string geo in mGeoCodes)
            {
                var mGeo = new SaveQueryMarketCountriesModel { GeoCode = geo };
                mGeos.Add(mGeo);
            }

            var listProducts = new List<SaveQueryProductsModel>();
            string[] pList = Products.Split('|');
            foreach (string prod in pList)
            {
                var pROD = new SaveQueryProductsModel { Product = prod };
                listProducts.Add(pROD);
            }

            var listPorts = new List<SaveQueryPortModel>();
            if (UsePorts)
            {
                string[] portList = Ports.Split('|');
                foreach (string port in portList)
                {
                    if (!port.Equals(string.Empty))
                    {
                        var pt = new SaveQueryPortModel { PortID = int.Parse(port) };
                        listPorts.Add(pt);
                    }
                }
            }

            var utils = new HelpersService();

            var TimeIds = utils.ConvertToTimeDimension(YearsMonths);

            if (Comments == null)
                Comments = string.Empty;
            if (Description == null)
                Description = string.Empty;

            var qSrv = new QueryService();
            
            if(string.IsNullOrEmpty(Id))
            {
                Id = string.Empty;
            }

            var postModel = new SavedQueryModel
            {
                Id = Id,
                Date = DateTime.Now,
                User = GetUserEmail(),
                Name = Name,
                Comments = Comments,
                Description = Description,
                SourceCountryGroup = SourceCountryGrouping,
                TradeFlowType = TradeFlowGrouping,
                MarketCountryGroup = MarketCountryGrouping,
                ProductGroupType = ProductType,
                ProductGroup = ProductGrouping,
                TonnesValuesGroup = ValuesGrouping,
                GroupByMonth = GroupByMonth,
                GroupByQuarter = GroupByQuarter,
                GroupByYear = GroupByYear,
                SourceCountryGEOs = sGeos,
                MarketCountryGEOs = mGeos,
                Products = listProducts,
                TimeIDs = TimeIds,
                Ports = listPorts,
                PortGroup = PortGrouping,
                IncludePorts = UsePorts,
                ProductRegionCode = ComboTariffRegionSelector,
                Product2DigitCode = ComboTariff2DigitSelector,
                SaveToVisitorPageIndex = SaveToVisitorPageIndex,
                SaveToDashboard = SaveToDashboard,
                SelectedCurrency = SelectedCurrency,
                SelectedSheduleDay = int.Parse(SelectedSheduleDay),
                IsShedule = SelectedShedule
            };


            //Set the already existing Model to false
            var DashModel = await qSrv.GetReportByIsVisitorDashboard(GetUserEmail());
            if (DashModel.User != null)
            {
                DashModel.SaveToVisitorPageIndex = false;
                await qSrv.InserNewQuery(DashModel);
            }

            await qSrv.InserNewQuery(postModel);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.SavedQueryModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Pipeline Builder Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = postModel
            };
            new EventLog(EventModel);

            return Json(new { success = true });
        }

        private string GetUserEmail()
        {
            var UserEmail = User.FindFirst(ClaimTypes.Email).Value;
            return UserEmail;
        }

        [HttpGet]
        [Authorize]
        public ActionResult RunReport(string Id)
        {
            var URL = "/ReportEngine/SearchResults?Id=" + Id ;
            //return RedirectToAction("SearchResults", "ReportEngine", new { Id = Id }) ;
            return Redirect(URL) ;
        }
    }
}

