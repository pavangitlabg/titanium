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
    public class MarketCountryService
    {
        public async Task<List<MarketCountryModel>> GetMarketAllCountries()
        {
       
            var db = new DBContext();
            var cursor = await db.MarketCountryDB.FindAsync(new BsonDocument());

            IList<MarketCountryDB> results = cursor.ToList();
            var modelList = new List<MarketCountryModel>();

            foreach (var Item in results)
            {

                var model = new MarketCountryModel
                {
                    _id = Item._id.ToString(),
                    ACTIVE = Item.ACTIVE,
                    DISCONTINUED_DATE = (DateTime)Item.DISCONTINUED_DATE,
                    GEO_CODE = Item.GEO_CODE,
                    REGION_NAME = Item.REGION_NAME,
                    LONG_LEGEND = Item.LONG_LEGEND,
                    MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                    NAME = Item.NAME,
                    NAME_OLD = Item.NAME_OLD,
                    REPLACED_GEO_CODE = Item.REPLACED_GEO_CODE,
                    SHORT_LEGEND = Item.SHORT_LEGEND,
                    SOURCE_COUNTRY_INDICATOR = Item.SOURCE_COUNTRY_INDICATOR,
                    START_DATE = (DateTime)Item.START_DATE
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }

        public async Task<List<MarketCountryModel>> GetActiveMarketCountries()
        {
            var builder = Builders<MarketCountryDB>.Filter;
            var filter = builder.Eq("ACTIVE", true);

            var db = new DBContext();
            var cursor = await db.MarketCountryDB.FindAsync(filter);

            IList<MarketCountryDB> results = cursor.ToList();
            var modelList = new List<MarketCountryModel>();

            foreach (var Item in results)
            {

                var model = new MarketCountryModel
                {
                    _id = Item._id.ToString(),
                    ACTIVE = Item.ACTIVE,
                    DISCONTINUED_DATE = (DateTime)Item.DISCONTINUED_DATE,
                    GEO_CODE = Item.GEO_CODE,
                    REGION_NAME = Item.REGION_NAME,
                    LONG_LEGEND = Item.LONG_LEGEND,
                    MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                    NAME = Item.NAME,
                    NAME_OLD = Item.NAME_OLD,
                    REPLACED_GEO_CODE = Item.REPLACED_GEO_CODE,
                    SHORT_LEGEND = Item.SHORT_LEGEND,
                    SOURCE_COUNTRY_INDICATOR = Item.SOURCE_COUNTRY_INDICATOR,
                    START_DATE = (DateTime)Item.START_DATE
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }


        public async Task<MarketCountryModel> GetMarketCountryByGeoCode(string geoCode)
        {
            //var filter = Builders<DBStockModel>.Filter.Eq(x => x.StockCode, stockCode);

            var builder = Builders<MarketCountryDB>.Filter;
            var filter = builder.Eq("GEO_CODE", geoCode);

            var db = new DBContext();
            var cursor = await db.MarketCountryDB.FindAsync(filter);

            IList<MarketCountryDB> results = cursor.ToList();

            if (results.Count.Equals(0))
            {
                return new MarketCountryModel { MARKET_COUNTRY_ID = 0 };
            }

            var Item = results[0];

            var model = new MarketCountryModel
            {
                _id = Item._id.ToString(),
                DISCONTINUED_DATE = (DateTime)Item.DISCONTINUED_DATE,
                GEO_CODE = Item.GEO_CODE,
                REGION_NAME = Item.REGION_NAME,
                LONG_LEGEND = Item.LONG_LEGEND,
                MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                NAME = Item.NAME,
                NAME_OLD = Item.NAME_OLD,
                REPLACED_GEO_CODE = Item.REPLACED_GEO_CODE,
                SHORT_LEGEND = Item.SHORT_LEGEND,
                SOURCE_COUNTRY_INDICATOR = Item.SOURCE_COUNTRY_INDICATOR,
                START_DATE = (DateTime)Item.START_DATE,
                ACTIVE = Item.ACTIVE
            };

            return model;
        }

        public async Task<MarketCountryExceptionModel> GetMarketCountryExceptionByGeoCode(string geoCode)
        {
            //var filter = Builders<DBStockModel>.Filter.Eq(x => x.StockCode, stockCode);

            var builder = Builders<MarketCountryExceptionDB>.Filter;
            var filter = builder.Eq("GEO_CODE", geoCode);

            var db = new DBContext();
            var cursor = await db.MarketCountryExceptionDB.FindAsync(filter);

            IList<MarketCountryExceptionDB> results = cursor.ToList();

            if (results.Count.Equals(0))
            {
                return new MarketCountryExceptionModel { MARKET_COUNTRY_ID = 0 };
            }

            var Item = results[0];

            var model = new MarketCountryExceptionModel
            {
                _id = Item._id.ToString(),
                DISCONTINUED_DATE = (DateTime)Item.DISCONTINUED_DATE,
                GEO_CODE = Item.GEO_CODE,
                REGION_NAME = Item.REGION_NAME,
                LONG_LEGEND = Item.LONG_LEGEND,
                MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                NAME = Item.NAME,
                NAME_OLD = Item.NAME_OLD,
                REPLACED_GEO_CODE = Item.REPLACED_GEO_CODE,
                SHORT_LEGEND = Item.SHORT_LEGEND,
                SOURCE_COUNTRY_INDICATOR = Item.SOURCE_COUNTRY_INDICATOR,
                START_DATE = (DateTime)Item.START_DATE,
                ACTIVE = Item.ACTIVE
            };

            return model;
        }

        public async Task<MarketCountryModel> GetMarketCountryByName(string countryName)
        {
            //var filter = Builders<DBStockModel>.Filter.Eq(x => x.StockCode, stockCode);


            BsonDocument filter = new BsonDocument();
            filter.Add("NAME", new BsonRegularExpression("^" + countryName + "$", "i"));

            var db = new DBContext();
            var cursor = await db.MarketCountryDB.FindAsync(filter);

            IList<MarketCountryDB> results = cursor.ToList();

            if (results.Count.Equals(0))
            {
                var rModel = new MarketCountryModel();
                return rModel;
            }

            var Item = results[0];

            var model = new MarketCountryModel
            {
                _id = Item._id.ToString(),
                DISCONTINUED_DATE = (DateTime)Item.DISCONTINUED_DATE,
                GEO_CODE = Item.GEO_CODE,
                REGION_NAME = Item.REGION_NAME,
                LONG_LEGEND = Item.LONG_LEGEND,
                MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                NAME = Item.NAME,
                NAME_OLD = Item.NAME_OLD,
                REPLACED_GEO_CODE = Item.REPLACED_GEO_CODE,
                SHORT_LEGEND = Item.SHORT_LEGEND,
                SOURCE_COUNTRY_INDICATOR = Item.SOURCE_COUNTRY_INDICATOR,
                START_DATE = (DateTime)Item.START_DATE,
                ACTIVE = Item.ACTIVE
            };

            return model;
        }

        public async Task<MarketCountryDB> SaveMarketCountry(MarketCountryModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<MarketCountryDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.MarketCountryDB.FindAsync(filter);
            IList<MarketCountryDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new MarketCountryDB
            {
                _id = Item._id,
                MARKET_COUNTRY_ID = model.MARKET_COUNTRY_ID,
                GEO_CODE = model.GEO_CODE,
                NAME_OLD = model.NAME_OLD,
                SHORT_LEGEND = model.SHORT_LEGEND,
                LONG_LEGEND = model.LONG_LEGEND,
                REGION_NAME = model.REGION_NAME,
                SOURCE_COUNTRY_INDICATOR = model.SOURCE_COUNTRY_INDICATOR,
                REPLACED_GEO_CODE = model.REPLACED_GEO_CODE,
                START_DATE = (DateTime) model.START_DATE,
                DISCONTINUED_DATE = (DateTime)model.DISCONTINUED_DATE,
                NAME = model.NAME,
                ACTIVE = model.ACTIVE
            };

            var returnModel = await db.MarketCountryDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;
        }

        public async Task<MarketCountryModel> GetMarketCountryByID(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var builder = Builders<MarketCountryDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.MarketCountryDB.FindAsync(filter);

            IList<MarketCountryDB> results = cursor.ToList();

            var Item = results[0];

            var model = new MarketCountryModel
            {
                _id = Item._id.ToString(),
                MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                GEO_CODE = Item.GEO_CODE,
                NAME_OLD = Item.NAME_OLD,
                SHORT_LEGEND = Item.SHORT_LEGEND,
                LONG_LEGEND = Item.LONG_LEGEND,
                REGION_NAME = Item.REGION_NAME,
                SOURCE_COUNTRY_INDICATOR = Item.SOURCE_COUNTRY_INDICATOR,
                REPLACED_GEO_CODE = Item.REPLACED_GEO_CODE,
                START_DATE = (DateTime)Item.START_DATE,
                DISCONTINUED_DATE = (DateTime)Item.DISCONTINUED_DATE,
                NAME = Item.NAME,
                ACTIVE = Item.ACTIVE

            };

            return model;

        }

        public async Task<bool> DoesExist(MarketCountryModel model)
        {
            bool bReturn = false;
            var builder = Builders<MarketCountryDB>.Filter;
            var filter = builder.Eq("_ID", model._id);

            var db = new DBContext();

            var cursor = await db.MarketCountryDB.FindAsync(filter);
            IList<MarketCountryDB> results = cursor.ToList();
            if (results.Count > 0)
                bReturn = true;

            return bReturn;
        }


        public async Task<bool> Add(MarketCountryModel model)
        {
            var db = new DBContext();

            var modelDB = new MarketCountryDB
            {
                
                MARKET_COUNTRY_ID = model.MARKET_COUNTRY_ID,
                GEO_CODE = model.GEO_CODE,
                NAME_OLD = model.NAME_OLD,
                SHORT_LEGEND = model.SHORT_LEGEND,
                LONG_LEGEND = model.LONG_LEGEND,
                REGION_NAME = model.REGION_NAME,
                SOURCE_COUNTRY_INDICATOR = model.SOURCE_COUNTRY_INDICATOR,
                REPLACED_GEO_CODE = model.REPLACED_GEO_CODE,
                START_DATE = model.START_DATE,
                DISCONTINUED_DATE = model.DISCONTINUED_DATE,
                NAME = model.NAME,
                ACTIVE = model.ACTIVE
            };

            await db.MarketCountryDB.InsertOneAsync(modelDB);

            return true;
        }

        public async Task<bool> AddException(MarketCountryExceptionModel model)
        {
            var db = new DBContext();

            var modelDB = new MarketCountryExceptionDB
            {

                MARKET_COUNTRY_ID = model.MARKET_COUNTRY_ID,
                GEO_CODE = model.GEO_CODE,
                NAME_OLD = model.NAME_OLD,
                SHORT_LEGEND = model.SHORT_LEGEND,
                LONG_LEGEND = model.LONG_LEGEND,
                REGION_NAME = model.REGION_NAME,
                SOURCE_COUNTRY_INDICATOR = model.SOURCE_COUNTRY_INDICATOR,
                REPLACED_GEO_CODE = model.REPLACED_GEO_CODE,
                START_DATE = model.START_DATE,
                DISCONTINUED_DATE = model.DISCONTINUED_DATE,
                NAME = model.NAME,
                ACTIVE = model.ACTIVE
            };

            await db.MarketCountryExceptionDB.InsertOneAsync(modelDB);

            return true;
        }


        public async Task<bool> Delete(MarketCountryModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<MarketCountryDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.MarketCountryDB.DeleteOneAsync(filter);

            return true;
        }

        public async Task<int> GetCount()
        {

            var db = new DBContext();
            var cursor = await db.MarketCountryDB.FindAsync(new BsonDocument());

            IList<MarketCountryDB> results = cursor.ToList();

            return results.Count;
        }

        public async Task<List<MarketCountryExceptionModel>> GetExceptionMarketCountries()
        {
           
            var db = new DBContext();
            var cursor = await db.MarketCountryExceptionDB.FindAsync(new BsonDocument());

            IList<MarketCountryExceptionDB> results = cursor.ToList();
            var modelList = new List<MarketCountryExceptionModel>();

            foreach (var Item in results)
            {

                var model = new MarketCountryExceptionModel
                {
                    _id = Item._id.ToString(),
                    ACTIVE = Item.ACTIVE,
                    DISCONTINUED_DATE = (DateTime)Item.DISCONTINUED_DATE,
                    GEO_CODE = Item.GEO_CODE,
                    REGION_NAME = Item.REGION_NAME,
                    LONG_LEGEND = Item.LONG_LEGEND,
                    MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                    NAME = Item.NAME,
                    NAME_OLD = Item.NAME_OLD,
                    REPLACED_GEO_CODE = Item.REPLACED_GEO_CODE,
                    SHORT_LEGEND = Item.SHORT_LEGEND,
                    SOURCE_COUNTRY_INDICATOR = Item.SOURCE_COUNTRY_INDICATOR,
                    START_DATE = (DateTime)Item.START_DATE
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }

    }
}
