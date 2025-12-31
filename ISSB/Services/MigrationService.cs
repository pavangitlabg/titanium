using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
    public class MigrationService
    {

        public async Task<bool> UpdateTimeDimension(TimeDimensionModel model)
        {
            bool bReturn = false;
            // var filter = Builders<TradeFactDB>.Filter.Eq(x => x.ControlID, model.ControlID);
            var db = new DBContext();

            // var cursor = await db.TradeFactDB.FindAsync(filter);
            // IList<TradeFactDB> results = cursor.ToList();

            var modelDB = new TimeDimensionDB
            {
                CUMULATIVE_NUMBER_OF_CALENDAR_DAYS = model.CUMULATIVE_NUMBER_OF_CALENDAR_DAYS,
                CUMULATIVE_WEEKS = model.CUMULATIVE_WEEKS,
                FINANCIAL_YEAR = model.FINANCIAL_YEAR,
                MONTH = model.MONTH,
                MONTH_DATE = model.MONTH_DATE,
                MONTH_IN_FINANCIAL_YEAR = model.MONTH_IN_FINANCIAL_YEAR,
                NUMBER_OF_CALENDAR_DAYS = model.NUMBER_OF_CALENDAR_DAYS,
                NUMBER_OF_WEEKS_IN_PERIOD = model.NUMBER_OF_WEEKS_IN_PERIOD,
                ORDINAL_MONTH_OF_QUARTER = model.ORDINAL_MONTH_OF_QUARTER,
                ORDINAL_WEEK_OF_MONTH = model.ORDINAL_WEEK_OF_MONTH,
                PERIOD_END_DATE = model.PERIOD_END_DATE,
                PERIOD_GRANULARITY = model.PERIOD_GRANULARITY,
                PERIOD_LONG_LEGEND = model.PERIOD_LONG_LEGEND,
                PERIOD_NAME = model.PERIOD_NAME,
                PERIOD_NAME_OLD = model.PERIOD_NAME_OLD,
                PERIOD_SHORT_LEGEND = model.PERIOD_SHORT_LEGEND,
                QUARTER = model.QUARTER,
                QUARTER_IN_FINANCIAL_YEAR = model.QUARTER_IN_FINANCIAL_YEAR,
                TIME_ID = model.TIME_ID,
                WEEK = model.WEEK,
                WEEK_POSITION_IN_MONTH = model.WEEK_POSITION_IN_MONTH,
                YEAR = model.YEAR
            };

            await db.TimeDimensionDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }

        public async Task<bool> UpdatePort(PortsModel model)
        {
            bool bReturn = false;
            // var filter = Builders<TradeFactDB>.Filter.Eq(x => x.ControlID, model.ControlID);
            var db = new DBContext();

            // var cursor = await db.TradeFactDB.FindAsync(filter);
            // IList<TradeFactDB> results = cursor.ToList();

            var modelDB = new PortsDB
            {
                PortID = model.PortID,
                Name = model.Name
            };

            await db.PortsDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }

        public async Task<bool> UpdateTradeFact(TradeFactModel model)
        {
            bool bReturn = false;
            // var filter = Builders<TradeFactDB>.Filter.Eq(x => x.ControlID, model.ControlID);
            var db = new DBContext();

            // var cursor = await db.TradeFactDB.FindAsync(filter);
            // IList<TradeFactDB> results = cursor.ToList();
            //1988101000
            var modelDB = new TradeFactDB
            {
                COO_GEO_CODE = model.coo_geo_code ?? string.Empty,
                CWC_GEO_CODE = model.cwc_geo_code ?? string.Empty,
                ESTIMATED = model.estimated ?? string.Empty,
                MARKET_COUNTRY_ID = model.market_country_id,
                MONETARY_VALUE = model.monetary_value,
                PORT_ID = model.port_id,
                SIDE_OF_TRADE = model.side_of_trade ?? string.Empty,
                SOURCE_COUNTRY_ID = model.source_country_id,
                TARIFF_ID = model.tariff_id,
                TIME_ID = model.time_id,
                WEIGHT = model.weight,
                YTD_MONETARY_VALUE = model.ytd_monetary_value,
                YTD_WEIGHT = model.ytd_weight,
                MONTH = int.Parse(model.time_id.ToString().Substring(5,2)),
                YEAR = int.Parse(model.time_id.ToString().Substring(0, 4))

            };

            await db.TradeFactDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }

        public async Task<bool> UpdateTariffCodes(TariffModel model)
        {
            bool bReturn = false;
            // var filter = Builders<TradeFactDB>.Filter.Eq(x => x.ControlID, model.ControlID);
            var db = new DBContext();

            //var cursor = await db.TradeFactDB.FindAsync(filter);
            //IList<TradeFactDB> results = cursor.ToList();

            var modelDB = new TariffDB
            {
                TARIFF_ID = model.TARIFF_ID,
                DISCONTINUED_DATE = model.DISCONTINUED_DATE,
                HARMONISED_TARIFF_CODE = model.HARMONISED_TARIFF_CODE.TrimEnd(),
                HARMONISED_TARIFF_LONG_LEGEND = model.HARMONISED_TARIFF_LONG_LEGEND.TrimEnd(),
                HARMONISED_TARIFF_SHORT_LEGEND = model.HARMONISED_TARIFF_SHORT_LEGEND.TrimEnd(),
                HARMONISED_TARIFF_SHORT_LEGEND_BACKUP = model.HARMONISED_TARIFF_SHORT_LEGEND_BACKUP.TrimEnd(),
                ISSB_STORED_CODE = model.ISSB_STORED_CODE.TrimEnd(),
                ISSB_STORED_LEGEND = model.ISSB_STORED_LEGEND.TrimEnd(),
                LOWER_VPT = model.LOWER_VPT.TrimEnd(),
                SIDE_OF_TRADE = model.SIDE_OF_TRADE.TrimEnd(),
                SOURCE_COUNTRY_TARIFF_CODE = model.SOURCE_COUNTRY_TARIFF_CODE.TrimEnd(),
                SOURCE_COUNTRY_TARIFF_LONG_LEGEND = model.SOURCE_COUNTRY_TARIFF_LONG_LEGEND.TrimEnd(),
                SOURCE_COUNTRY_TARIFF_SHORT_LEGEND = model.SOURCE_COUNTRY_TARIFF_SHORT_LEGEND.TrimEnd(),
                START_DATE = model.START_DATE,
                TARIFF_CODE_TABLE_CODE = model.TARIFF_CODE_TABLE_CODE.TrimEnd(),
                UPPER_VPT = model.UPPER_VPT.TrimEnd(),
                WTO_ALLOY_CODE = model.WTO_ALLOY_CODE.TrimEnd(),
                WTO_ALLOY_LEGEND = model.WTO_ALLOY_LEGEND.TrimEnd(),
                WTO_CODE = model.WTO_CODE.TrimEnd(),
                WTO_CODE_LEGEND = model.WTO_CODE_LEGEND.TrimEnd()

            };

            await db.TariffDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }

        public async Task<bool> UpdateSourceCountry(SourceCountryModel model)
        {
            bool bReturn = false;
            // var filter = Builders<TradeFactDB>.Filter.Eq(x => x.ControlID, model.ControlID);
            var db = new DBContext();

            //var cursor = await db.TradeFactDB.FindAsync(filter);
            //IList<TradeFactDB> results = cursor.ToList();

            var modelDB = new SourceCountryDB
            {
                ACTIVE = model.ACTIVE,
                CURRENCY_CODE = model.CURRENCY_CODE,
                DATA_FORMAT = model.DATA_FORMAT,
                FIRST_DATE = model.FIRST_DATE,
                FREQUENCY = model.FREQUENCY,
                GEO_CODE = model.GEO_CODE,
                GEO_CODE_DISCONTINUED_DATE = model.GEO_CODE_DISCONTINUED_DATE,
                INDUSTRY_REPORTING_CENTRE_ID = model.INDUSTRY_REPORTING_CENTRE_ID,
                LATEST_DATE = model.LATEST_DATE,
                LONG_LEGEND = model.LONG_LEGEND,
                NAME = model.NAME,
                NAME_OLD = model.NAME_OLD,
                OLD_GEO_CODE = model.OLD_GEO_CODE,
                REGION_NAME = model.REGION_NAME,
                SHORT_LEGEND = model.SHORT_LEGEND,
                SIDE_OF_TRADE = model.SIDE_OF_TRADE,
                SOURCE_COUNTRY_ID = model.SOURCE_COUNTRY_ID

            };

            await db.SourceCountryDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }

        public async Task<bool> UpdateMarketCountry(MarketCountryModel model)
        {
            bool bReturn = false;
            // var filter = Builders<TradeFactDB>.Filter.Eq(x => x.ControlID, model.ControlID);
            var db = new DBContext();

            //var cursor = await db.TradeFactDB.FindAsync(filter);
            //IList<TradeFactDB> results = cursor.ToList();

            var modelDB = new MarketCountryDB
            {
                DISCONTINUED_DATE = model.DISCONTINUED_DATE,
                GEO_CODE = model.GEO_CODE,
                LONG_LEGEND = model.LONG_LEGEND,
                MARKET_COUNTRY_ID = model.MARKET_COUNTRY_ID,
                NAME = model.NAME,
                NAME_OLD = model.NAME_OLD,
                REGION_NAME = model.REGION_NAME,
                REPLACED_GEO_CODE = model.REPLACED_GEO_CODE,
                SHORT_LEGEND = model.SHORT_LEGEND,
                SOURCE_COUNTRY_INDICATOR = model.SOURCE_COUNTRY_INDICATOR,
                START_DATE = model.START_DATE,
                ACTIVE = true

            };

            await db.MarketCountryDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }

        public async Task<bool> InsertWorldCity(WorldCityModel model)
        {
            bool bReturn = false;
            // var filter = Builders<TradeFactDB>.Filter.Eq(x => x.ControlID, model.ControlID);
            var db = new DBContext();

            //var cursor = await db.TradeFactDB.FindAsync(filter);
            //IList<TradeFactDB> results = cursor.ToList();

            var modelDB = new WorldCityDB
            {
                Idx = model.Idx,
                GEO = model.GEO,
                City = model.City,
                CityAscii = model.CityAscii,
                LAT = model.LAT,
                LNG = model.LNG,
                Population = model.Population,
                Country = model.Country,
                ISO2 = model.ISO2,
                ISO3 = model.ISO3,
                Province = model.Province

            };

            await db.WorldCityDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }

        public async Task<bool> InsertBroadTrariffFilter(ProductBroadFilterModel model)
        {
            bool bReturn = false;
            // var filter = Builders<TradeFactDB>.Filter.Eq(x => x.ControlID, model.ControlID);
            var db = new DBContext();

            //var cursor = await db.TradeFactDB.FindAsync(filter);
            //IList<TradeFactDB> results = cursor.ToList();

            var dbItemsList = new List<ProductBroadFilterItemDB>();

            foreach (var Item in model.Items)
            {
                var iModel = new ProductBroadFilterItemDB { Sort = Item.Sort, TariffCode = Item.TariffCode, Name = Item.Name };

                dbItemsList.Add(iModel);
            }


            var modelDB = new ProductBroadFilterDB
            {
                Idx = model.Idx,
                Name = model.Name,
                SortOrder = model.SortOrder,
                Items = dbItemsList

            };

            await db.ProductBroadFilterDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }

        public async Task<bool> InsertMedumTrariffFilter(ProductMedumFilterModel model)
        {
            bool bReturn = false;
            // var filter = Builders<TradeFactDB>.Filter.Eq(x => x.ControlID, model.ControlID);
            var db = new DBContext();

            //var cursor = await db.TradeFactDB.FindAsync(filter);
            //IList<TradeFactDB> results = cursor.ToList();

            var dbItemsList = new List<ProductMedumFilterItemDB>();

            foreach (var Item in model.Items)
            {
                var iModel = new ProductMedumFilterItemDB { Sort = Item.Sort, TariffCode = Item.TariffCode, Name = Item.Name };

                dbItemsList.Add(iModel);
            }


            var modelDB = new ProductMedumFilterDB
            {
                Idx = model.Idx,
                Name = model.Name,
                SortOrder = model.SortOrder,
                Code = model.Code,
                Items = dbItemsList

            };

            await db.ProductMedumFilterDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }

        public async Task<bool> InsertAllTrariffFilter(ProductAllFilterModel model)
        {
            bool bReturn = false;
            // var filter = Builders<TradeFactDB>.Filter.Eq(x => x.ControlID, model.ControlID);
            var db = new DBContext();


            var modelDB = new ProductAllFilterDB
            {
                Idx = model.Idx,
                Name = model.Name,
                SortOrder = model.SortOrder,
                Cost = 0,
                TariffCode = model.TariffCode

            };

            await db.ProductAllFilterDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }

        public async Task<bool> MergeCoustomerToSystemUser()
        {
            var db = new DBContext();
            var salesSrv = new SalesProfileService();
            var usrSer = new UserServices();
            var cModel = await salesSrv.GetSalesProfiles();

            foreach(var Item in cModel)
            {
                if(!string.IsNullOrEmpty(Item.TES_EMAIL))
                {
                    string[] words = Item.NAME.Split(' ');
                    var userModel = new UserDB
                    {
                        AccountStatus = "Closed",
                        Address1 = Item.ADDRESS_1,
                        Address2 = Item.ADDRESS_2,
                        Address3 = Item.ADDRESS_3,
                        Address4 = Item.ADDRESS_4,
                        CompanyName = Item.NAME.Trim(),
                        Costs = 0,
                        Email = Item.TES_EMAIL,
                        IsTradeInquiry = true,
                        IsAdministrator = false,
                        Password = "fmLBlJ6Q7Otk9yelxABuiXU1DzeJ35DxlbjS6rKiKYQ=",
                        UserName = words[0],
                        FirstName = string.Empty,
                        LastName = string.Empty,
                        Mobile = Item.TELEPHONE_2,
                        Telephone = Item.TELEPHONE,
                        ExpiryDate = DateTime.Now,
                        StartDate = DateTime.Now,
                        IsSystemUser = false,
                        DateJoined = Item.DATE_ACCOUNT_OPENED,
                        PostalCode = Item.ADDRESS_5,
                        TwoFactorAuth = true

                    };

                    var bExist = await usrSer.DoesEmailExist(userModel.Email);
                    if(!bExist)
                       await db.UserDB.InsertOneAsync(userModel);
                }
            }

            return true;
        }

        public async Task<List<TariffImportModel>> GetTariffImports()
        {
            var db = new DBContext();
            var cursor = await db.TariffImportDB.FindAsync(new BsonDocument());

            IList<TariffImportDB> results = cursor.ToList();
            var modelList = new List<TariffImportModel>();

            foreach (var Item in results)
            {
                var model = new TariffImportModel
                {
                    _id = Item._id.ToString(),
                     
                    DESCRIPTION = Item.DESCRIPTION,
                    HS = Item.HS,
                    REGION_CODE = Item.REGION_CODE,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    TARIFF = Item.TARIFF


                };

                modelList.Add(model);
            }

            return modelList;
        }

    }
}
