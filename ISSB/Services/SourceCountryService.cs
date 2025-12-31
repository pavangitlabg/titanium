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
    public class SourceCountryService
    {
        public async Task<List<SourceCountryModel>> GetSourceCountries()
        {
            var db = new DBContext();
            var cursor = await db.SourceCountryDB.FindAsync(new BsonDocument());                          

            IList<SourceCountryDB> results = cursor.ToList();
            var modelList = new List<SourceCountryModel>();

            foreach (var Item in results)
            {

                var model = new SourceCountryModel
                {
                    _id = Item._id.ToString(),
                    ACTIVE = Item.ACTIVE,
                    CURRENCY_CODE = Item.CURRENCY_CODE,
                    DATA_FORMAT = Item.DATA_FORMAT,
                    FIRST_DATE = (DateTime)Item.FIRST_DATE,
                    FREQUENCY = Item.FREQUENCY,
                    GEO_CODE = Item.GEO_CODE,
                    GEO_CODE_DISCONTINUED_DATE = (DateTime)Item.GEO_CODE_DISCONTINUED_DATE,
                    INDUSTRY_REPORTING_CENTRE_ID = Item.INDUSTRY_REPORTING_CENTRE_ID,
                    LATEST_DATE = (DateTime)Item.LATEST_DATE,
                    LONG_LEGEND = Item.LONG_LEGEND,
                    NAME = Item.NAME,
                    NAME_OLD = Item.NAME_OLD,
                    OLD_GEO_CODE = Item.OLD_GEO_CODE,
                    REGION_NAME = Item.REGION_NAME,
                    SHORT_LEGEND = Item.SHORT_LEGEND,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }

        public async Task<List<SourceCountryModel>> GetActiveSourceCountries()
        {
            var builder = Builders<SourceCountryDB>.Filter;
            var filter = builder.Eq("ACTIVE", "Y");

            var db = new DBContext();
            var cursor = await db.SourceCountryDB.FindAsync(filter);

            IList<SourceCountryDB> results = cursor.ToList();
            var modelList = new List<SourceCountryModel>();

            foreach (var Item in results)
            {

                var model = new SourceCountryModel
                {
                    _id = Item._id.ToString(),
                    ACTIVE = Item.ACTIVE,
                    CURRENCY_CODE = Item.CURRENCY_CODE,
                    DATA_FORMAT = Item.DATA_FORMAT,
                    FIRST_DATE = (DateTime)Item.FIRST_DATE,
                    FREQUENCY = Item.FREQUENCY,
                    GEO_CODE = Item.GEO_CODE,
                    GEO_CODE_DISCONTINUED_DATE = (DateTime)Item.GEO_CODE_DISCONTINUED_DATE,
                    INDUSTRY_REPORTING_CENTRE_ID = Item.INDUSTRY_REPORTING_CENTRE_ID,
                    LATEST_DATE = (DateTime)Item.LATEST_DATE,
                    LONG_LEGEND = Item.LONG_LEGEND,
                    NAME = Item.NAME,
                    NAME_OLD = Item.NAME_OLD,
                    OLD_GEO_CODE = Item.OLD_GEO_CODE,
                    REGION_NAME = Item.REGION_NAME,
                    SHORT_LEGEND = Item.SHORT_LEGEND,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x.GEO_CODE).ToList();
        }

        public async Task<SourceCountryModel> GetSourceCountryByID(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<SourceCountryDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();

            var cursor = await db.SourceCountryDB.FindAsync(filter);

            IList<SourceCountryDB> results = cursor.ToList();

            var Item = results[0];

            var model = new SourceCountryModel
            {
                _id = Item._id.ToString(),
                ACTIVE = Item.ACTIVE,
                CURRENCY_CODE = Item.CURRENCY_CODE,
                DATA_FORMAT = Item.DATA_FORMAT,
                FIRST_DATE = (DateTime)Item.FIRST_DATE,
                FREQUENCY = Item.FREQUENCY,
                GEO_CODE = Item.GEO_CODE,
                GEO_CODE_DISCONTINUED_DATE = (DateTime)Item.GEO_CODE_DISCONTINUED_DATE,
                INDUSTRY_REPORTING_CENTRE_ID = Item.INDUSTRY_REPORTING_CENTRE_ID,
                LATEST_DATE = (DateTime)Item.LATEST_DATE,
                LONG_LEGEND = Item.LONG_LEGEND,
                NAME = Item.NAME,
                NAME_OLD = Item.NAME_OLD,
                OLD_GEO_CODE = Item.OLD_GEO_CODE,
                REGION_NAME = Item.REGION_NAME,
                SHORT_LEGEND = Item.SHORT_LEGEND,
                SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID

            };

            return model;
        }

        public async Task<SourceCountryModel> GetSourceCountryByCurrency(string currency)
        {
           
            var builder = Builders<SourceCountryDB>.Filter;

            var filter = builder.Eq("CURRENCY_CODE", currency) & builder.Eq("ACTIVE", "Y");

            var db = new DBContext();

            var cursor = await db.SourceCountryDB.FindAsync(filter);

            IList<SourceCountryDB> results = cursor.ToList();

            if(results.Count == 0)
            {
                return new SourceCountryModel();
            }

            var Item = results[0];

            var model = new SourceCountryModel
            {
                _id = Item._id.ToString(),
                ACTIVE = Item.ACTIVE,
                CURRENCY_CODE = Item.CURRENCY_CODE,
                DATA_FORMAT = Item.DATA_FORMAT,
                FIRST_DATE = (DateTime)Item.FIRST_DATE,
                FREQUENCY = Item.FREQUENCY,
                GEO_CODE = Item.GEO_CODE,
                GEO_CODE_DISCONTINUED_DATE = (DateTime)Item.GEO_CODE_DISCONTINUED_DATE,
                INDUSTRY_REPORTING_CENTRE_ID = Item.INDUSTRY_REPORTING_CENTRE_ID,
                LATEST_DATE = (DateTime)Item.LATEST_DATE,
                LONG_LEGEND = Item.LONG_LEGEND,
                NAME = Item.NAME,
                NAME_OLD = Item.NAME_OLD,
                OLD_GEO_CODE = Item.OLD_GEO_CODE,
                REGION_NAME = Item.REGION_NAME,
                SHORT_LEGEND = Item.SHORT_LEGEND,
                SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID

            };

            return model;
        }

        public async Task<SourceCountryModel> GetSourceCountryByGeoCode(string geoCode)
        {
            //var filter = Builders<DBStockModel>.Filter.Eq(x => x.StockCode, stockCode);

            var builder = Builders<SourceCountryDB>.Filter;
            //var filter = builder.Eq("GEO_CODE", geoCode);
            var filter = builder.Eq("GEO_CODE", geoCode) & builder.Eq("ACTIVE", "Y");

            var db = new DBContext();
            var cursor = await db.SourceCountryDB.FindAsync(filter);

            IList<SourceCountryDB> results = cursor.ToList();

            if(results.Count.Equals(0))
            {
                return new SourceCountryModel { SOURCE_COUNTRY_ID = 0 };
            }

            var Item = results[0];
            var model = new SourceCountryModel
            {
                _id = Item._id.ToString(),
                ACTIVE = Item.ACTIVE,
                CURRENCY_CODE = Item.CURRENCY_CODE,
                DATA_FORMAT = Item.DATA_FORMAT,
                FIRST_DATE = (DateTime)Item.FIRST_DATE,
                FREQUENCY = Item.FREQUENCY,
                GEO_CODE = Item.GEO_CODE,
                GEO_CODE_DISCONTINUED_DATE = (DateTime)Item.GEO_CODE_DISCONTINUED_DATE,
                INDUSTRY_REPORTING_CENTRE_ID = Item.INDUSTRY_REPORTING_CENTRE_ID,
                LATEST_DATE = (DateTime)Item.LATEST_DATE,
                LONG_LEGEND = Item.LONG_LEGEND,
                NAME = Item.NAME,
                NAME_OLD = Item.NAME_OLD,
                OLD_GEO_CODE = Item.OLD_GEO_CODE,
                REGION_NAME = Item.REGION_NAME,
                SHORT_LEGEND = Item.SHORT_LEGEND,
                SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID
            };

            return model;
        }

        public static explicit operator SourceCountryService(SourceCountryModel v)
        {
            throw new NotImplementedException();
        }


        public async Task<bool> DoesExist(SourceCountryModel model)
        {
            bool bReturn = false;
            var builder = Builders<SourceCountryDB>.Filter;
            var filter = builder.Eq("_id", model._id);

            var db = new DBContext();

            var cursor = await db.SourceCountryDB.FindAsync(filter);
            IList<SourceCountryDB> results = cursor.ToList();
            if (results.Count > 0)
                bReturn = true;

            return bReturn;
        }
                     
        public async Task<SourceCountryDB> SaveSourceCountry(SourceCountryModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<SourceCountryDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.SourceCountryDB.FindAsync(filter);
            IList<SourceCountryDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new SourceCountryDB
            {
                _id = Item._id,
                ACTIVE = model.ACTIVE,
                CURRENCY_CODE = model.CURRENCY_CODE,
                DATA_FORMAT = model.DATA_FORMAT,
                FIRST_DATE = (DateTime)model.FIRST_DATE,
                FREQUENCY = model.FREQUENCY,
                GEO_CODE = model.GEO_CODE,
                GEO_CODE_DISCONTINUED_DATE = (DateTime)model.GEO_CODE_DISCONTINUED_DATE,
                INDUSTRY_REPORTING_CENTRE_ID = model.INDUSTRY_REPORTING_CENTRE_ID,
                LATEST_DATE = (DateTime)model.LATEST_DATE,
                LONG_LEGEND = model.LONG_LEGEND,
                NAME = model.NAME,
                NAME_OLD = model.NAME_OLD,
                OLD_GEO_CODE = model.OLD_GEO_CODE,
                REGION_NAME = model.REGION_NAME,
                SHORT_LEGEND = model.SHORT_LEGEND,
                SIDE_OF_TRADE = model.SIDE_OF_TRADE,
                SOURCE_COUNTRY_ID = model.SOURCE_COUNTRY_ID
            };

            var returnModel = await db.SourceCountryDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;
        }

        public async Task<bool> Add(SourceCountryModel model)
        {
            var db = new DBContext();

            var modelDB = new SourceCountryDB
            {
                ACTIVE = model.ACTIVE,
                CURRENCY_CODE = model.CURRENCY_CODE,
                DATA_FORMAT = model.DATA_FORMAT,
                FIRST_DATE = (DateTime)model.FIRST_DATE,
                FREQUENCY = model.FREQUENCY,
                GEO_CODE = model.GEO_CODE,
                GEO_CODE_DISCONTINUED_DATE = (DateTime)model.GEO_CODE_DISCONTINUED_DATE,
                INDUSTRY_REPORTING_CENTRE_ID = model.INDUSTRY_REPORTING_CENTRE_ID,
                LATEST_DATE = (DateTime)model.LATEST_DATE,
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

            return true;
        }

        public async Task<bool> AddException(SourceCountryExceptionModel model)
        {
            var db = new DBContext();

            var modelDB = new SourceCountryExceptionDB
            {

                SOURCE_COUNTRY_ID = model.SOURCE_COUNTRY_ID,
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

            await db.SourceCountryExceptionDB.InsertOneAsync(modelDB);

            return true;
        }

        public async Task<SourceCountryExceptionModel> GetSourceCountryExceptionByGeoCode(string geoCode)
        {
            //var filter = Builders<DBStockModel>.Filter.Eq(x => x.StockCode, stockCode);

            var builder = Builders<SourceCountryExceptionDB>.Filter;
            var filter = builder.Eq("GEO_CODE", geoCode);

            var db = new DBContext();
            var cursor = await db.SourceCountryExceptionDB.FindAsync(filter);

            IList<SourceCountryExceptionDB> results = cursor.ToList();

            if (results.Count.Equals(0))
            {
                return new SourceCountryExceptionModel { SOURCE_COUNTRY_ID = 0 };
            }

            var Item = results[0];

            var model = new SourceCountryExceptionModel
            {
                _id = Item._id.ToString(),
                DISCONTINUED_DATE = (DateTime)Item.DISCONTINUED_DATE,
                GEO_CODE = Item.GEO_CODE,
                REGION_NAME = Item.REGION_NAME,
                LONG_LEGEND = Item.LONG_LEGEND,
                SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
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


        //public async Task<bool> AddSourceCountry(SourceCountryModel model)
        //{
        //    var db = new DBContext();

        //    if (await DoesExist(model)) return false;

        //    var modelDB = new SourceCountryDB
        //    {

        //        ACTIVE = model.ACTIVE,
        //        CURRENCY_CODE = model.CURRENCY_CODE,
        //        DATA_FORMAT = model.DATA_FORMAT,
        //        FIRST_DATE = (DateTime)model.FIRST_DATE,
        //        FREQUENCY = model.FREQUENCY,
        //        GEO_CODE = model.GEO_CODE,
        //        GEO_CODE_DISCONTINUED_DATE = (DateTime)model.GEO_CODE_DISCONTINUED_DATE,
        //        INDUSTRY_REPORTING_CENTRE_ID = model.INDUSTRY_REPORTING_CENTRE_ID,
        //        LATEST_DATE = (DateTime)model.LATEST_DATE,
        //        LONG_LEGEND = model.LONG_LEGEND,
        //        NAME = model.NAME,
        //        NAME_OLD = model.NAME_OLD,
        //        OLD_GEO_CODE = model.OLD_GEO_CODE,
        //        REGION_NAME = model.REGION_NAME,
        //        SHORT_LEGEND = model.SHORT_LEGEND,
        //        SIDE_OF_TRADE = model.SIDE_OF_TRADE,
        //        SOURCE_COUNTRY_ID = model.SOURCE_COUNTRY_ID
        //    };

        //    await db.SourceCountryDB.InsertOneAsync(modelDB);
        //    return true;
        //}


        //public async Task<bool> Add(SourceCountryModel model)
        //{
        //    var db = new DBContext();

        //    var modelDB = new SourceCountryDB
        //    {

        //        ACTIVE = model.ACTIVE,
        //        CURRENCY_CODE = model.CURRENCY_CODE,
        //        DATA_FORMAT = model.DATA_FORMAT,
        //        FIRST_DATE = (DateTime)model.FIRST_DATE,
        //        FREQUENCY = model.FREQUENCY,
        //        GEO_CODE = model.GEO_CODE,
        //        GEO_CODE_DISCONTINUED_DATE = (DateTime)model.GEO_CODE_DISCONTINUED_DATE,
        //        INDUSTRY_REPORTING_CENTRE_ID = model.INDUSTRY_REPORTING_CENTRE_ID,
        //        LATEST_DATE = (DateTime)model.LATEST_DATE,
        //        LONG_LEGEND = model.LONG_LEGEND,
        //        NAME = model.NAME,
        //        NAME_OLD = model.NAME_OLD,
        //        OLD_GEO_CODE = model.OLD_GEO_CODE,
        //        REGION_NAME = model.REGION_NAME,
        //        SHORT_LEGEND = model.SHORT_LEGEND,
        //        SIDE_OF_TRADE = model.SIDE_OF_TRADE,
        //        SOURCE_COUNTRY_ID = model.SOURCE_COUNTRY_ID
        //    };

        //    await db.SourceCountryDB.InsertOneAsync(modelDB);

        //    return true;
        //}


        public async Task<bool> Delete(SourceCountryModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<SourceCountryDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.SourceCountryDB.DeleteOneAsync(filter);

            return true;
        }

        public async Task<int> GetCount()
        {

            var db = new DBContext();
            var cursor = await db.SourceCountryDB.FindAsync(new BsonDocument());

            IList<SourceCountryDB> results = cursor.ToList();

            return results.Count;
        }

    }
}
