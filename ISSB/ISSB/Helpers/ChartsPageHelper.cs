using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using ISSB.Models;
using Newtonsoft.Json;
using Services;

namespace ISSB.Helpers
{
    public class ChartsPageHelper
    {
        private static ChartsPageHelper _instance;
        public object SourceMapData;
        public string PageTitle;
        public List<MapChartModel> GroupList;

        public static ChartsPageHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ChartsPageHelper();
                }
                return _instance;
            }
        }

        public async Task<ReportEngineModel> SetupHomePageCharts()
        {
            var srv = new QueryService();
            var Homemodel = await srv.GeDefaultHomePage();

            var model = await srv.GetReportByID(Homemodel.Id);
            PageTitle = model.Name;

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

            var cachedData = await srv.GetMainIndexPage(true);
            IQueryable<ReportOutModel> dataList = cachedData.Model as IQueryable<ReportOutModel>;

            // var res = new ReportEngineService();
            // var factory = await res.GetReport(searchModel);
            var viewModel = new ReportEngineModel { Search = searchModel, Data = dataList };
            await viewModel.ConvertToCurrency(model.SelectedCurrency);
            //Force a Grid Width Calc here.

            var cB = new Infragistics.Web.Mvc.GridColumnBuilder<ReportOutModel>();
            viewModel.Columns(cB);

            GroupList = await SetMapParams(dataList, searchModel);
            return viewModel;
        }

        public async Task<List<MapChartModel>> SetMapParams(IQueryable<ReportOutModel> dataList, SearchModel searchModel)
        {
            var GroupMapList = new List<MapChartModel>();
            var MapData = new List<ReportOutHelperModel>();

            foreach (var Item in dataList)
            {
                var model = new ReportOutHelperModel()
                {
                    ID = Item.ID,
                    MC_GEO = Item.MC_GEO,
                    MC_NAME = Item.MC_NAME,
                    MONETARY_VALUE = Item.MONETARY_VALUE,
                    MONTH = Item.MONTH,
                    PORT_ID = Item.PORT_ID,
                    PORT_NAME = Item.PORT_NAME,
                    QUARTER = Item.QUARTER,
                    SC_GEO = Item.SC_GEO,
                    SC_NAME = Item.SC_NAME,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    TARIFF_CODE = Item.TARIFF_CODE,
                    TARIFF_LEGEND = Item.TARIFF_LEGEND,
                    TIME_ID = Item.TIME_ID,
                    WEIGHT = Item.WEIGHT,
                    YEAR = Item.YEAR,
                    YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                    YTD_WEIGHT = Item.YTD_WEIGHT
                     
                };
                MapData.Add(model);
            }
        

            if (searchModel.SourceCountryGrouping == GroupingOption.Together)
            {
                //GetMapChart Data
                var srvCities = new WorldCityService();
                var CityList = await srvCities.GetWorldCapitalCities();
                var mapChartList = new List<MapChartModel>();
                var SourceMapChartList = new List<MapChartModel>();
                var SourceCity = CityList.FirstOrDefault(x => x.GEO.Equals(searchModel.SourceCountryGeoCodes[0]));

                //Refactor this code
                var sumTotalWeight = (from x in MapData select x.WEIGHT).Sum();
                var sumTotalValue = (from x in MapData select x.MONETARY_VALUE).Sum();

                var totalGEOWeightList = MapData.ToList().GroupBy(x => x.MC_GEO).
                          Select(x =>
                          {
                              var ret = x.First();
                              ret.WEIGHT = x.Sum(xt => xt.WEIGHT);
                              return ret;
                          }).ToList();
                var totalGEOValueList = MapData.ToList().GroupBy(x => x.MC_GEO).
                           Select(x =>
                           {
                               var ret = x.First();
                               ret.MONETARY_VALUE = x.Sum(xt => xt.MONETARY_VALUE);
                               return ret;
                           }).ToList();

                var SourceMapModel = new MapChartModel
                {
                    GEO = SourceCity.GEO,
                    CITY = SourceCity.City,
                    COUNTRY = SourceCity.Country,
                    LATITUDE = SourceCity.LAT,
                    LONGITUDE = SourceCity.LNG,
                    MONEY = sumTotalValue,
                    WEIGHT = sumTotalWeight
                 

                };
                mapChartList.Add(SourceMapModel);
                SourceMapData = JsonConvert.SerializeObject(SourceMapModel);


                foreach (var MapVar in MapData)
                {
                    var DesinationMapModel = new MapChartModel();

                    var DestinationCity = CityList.FirstOrDefault(x => x.GEO.Equals(MapVar.MC_GEO));
                    if (DestinationCity != null)
                    {
                        DesinationMapModel.GEO = DestinationCity.GEO;
                        DesinationMapModel.CITY = DestinationCity.City;
                        DesinationMapModel.COUNTRY = DestinationCity.Country;
                        DesinationMapModel.LATITUDE = DestinationCity.LAT;
                        DesinationMapModel.LONGITUDE = DestinationCity.LNG;
                        DesinationMapModel.MONEY = totalGEOValueList.FirstOrDefault(x => x.MC_GEO.Equals(MapVar.MC_GEO)).MONETARY_VALUE;
                        DesinationMapModel.WEIGHT = totalGEOWeightList.FirstOrDefault(x => x.MC_GEO.Equals(MapVar.MC_GEO)).WEIGHT;
                        mapChartList.Add(DesinationMapModel);
                    }
                }

                 GroupMapList = mapChartList.GroupBy(x => x.GEO)
                                   .Select(grp => grp.First())
                                   .ToList();

                
            }

            if (searchModel.MarketCountryGrouping == GroupingOption.Together)
            {
                //GetMapChart Data
                var srvCities = new WorldCityService();
                var CityList = await srvCities.GetWorldCapitalCities();
                var mapChartList = new List<MapChartModel>();
                var SourceMapChartList = new List<MapChartModel>();
                var SourceCity = CityList.FirstOrDefault(x => x.GEO.Equals(searchModel.MarketCountryGeoCodes[0]));

                //Refactor this code
                var sumTotalWeight = (from x in MapData select x.WEIGHT).Sum();
                var sumTotalValue = (from x in MapData select x.MONETARY_VALUE).Sum();

                var totalGEOWeightList = MapData.ToList().GroupBy(x => x.SC_GEO).
                          Select(x =>
                          {
                              var ret = x.First();
                              ret.WEIGHT = x.Sum(xt => xt.WEIGHT);
                              return ret;
                          }).ToList();
                var totalGEOValueList = MapData.ToList().GroupBy(x => x.SC_GEO).
                           Select(x =>
                           {
                               var ret = x.First();
                               ret.MONETARY_VALUE = x.Sum(xt => xt.MONETARY_VALUE);
                               return ret;
                           }).ToList();

                var SourceMapModel = new MapChartModel
                {
                    GEO = SourceCity.GEO,
                    CITY = SourceCity.City,
                    COUNTRY = SourceCity.Country,
                    LATITUDE = SourceCity.LAT,
                    LONGITUDE = SourceCity.LNG,
                    MONEY = sumTotalValue,
                    WEIGHT = sumTotalWeight
                };
                mapChartList.Add(SourceMapModel);
                SourceMapData = JsonConvert.SerializeObject(SourceMapModel);


                foreach (var MapVar in MapData)
                {
                    var DesinationMapModel = new MapChartModel();

                    var DestinationCity = CityList.FirstOrDefault(x => x.GEO.Equals(MapVar.SC_GEO));
                    if (DestinationCity != null)
                    {
                        DesinationMapModel.GEO = DestinationCity.GEO;
                        DesinationMapModel.CITY = DestinationCity.City;
                        DesinationMapModel.COUNTRY = DestinationCity.Country;
                        DesinationMapModel.LATITUDE = DestinationCity.LAT;
                        DesinationMapModel.LONGITUDE = DestinationCity.LNG;
                        DesinationMapModel.MONEY = totalGEOValueList.FirstOrDefault(x => x.SC_GEO.Equals(MapVar.SC_GEO)).MONETARY_VALUE;
                        DesinationMapModel.WEIGHT = totalGEOWeightList.FirstOrDefault(x => x.SC_GEO.Equals(MapVar.SC_GEO)).WEIGHT;
                        mapChartList.Add(DesinationMapModel);
                    }
                }

                GroupMapList = mapChartList.GroupBy(x => x.GEO)
                                  .Select(grp => grp.First())
                                  .ToList();


            }

            return GroupMapList;
        }
    }
}
