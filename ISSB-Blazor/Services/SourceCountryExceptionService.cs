using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class SourceCountryExceptionService
{
    public async Task<List<SourceCountryExceptionModel>> GetSourceCountryExceptions()
    {
        var db = new DbContext();
        var cursor = await db.SourceCountryExceptionDb.FindAsync(new BsonDocument());

        IList<SourceCountryExceptionDB> results = cursor.ToList();
        var modelList = new List<SourceCountryExceptionModel>();

        foreach (var Item in results)
        {
            var model = new SourceCountryExceptionModel
            {
                _id = Item._id.ToString(),
                SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                ACTIVE = Item.ACTIVE,
                GEO_CODE = Item.GEO_CODE,
                NAME_OLD = Item.NAME_OLD,
                SHORT_LEGEND = Item.SHORT_LEGEND,
                LONG_LEGEND = Item.LONG_LEGEND,
                REGION_NAME = Item.REGION_NAME,
                SOURCE_COUNTRY_INDICATOR = Item.SOURCE_COUNTRY_INDICATOR,
                REPLACED_GEO_CODE = Item.REPLACED_GEO_CODE,
                START_DATE = (DateTime)Item.START_DATE,
                DISCONTINUED_DATE = (DateTime)Item.DISCONTINUED_DATE,
                NAME = Item.NAME
            };

            modelList.Add(model);
        }

        return modelList.OrderBy(x => x._id).ToList();
    }

    public async Task<SourceCountryExceptionModel> GetSourceCountryExceptionByID(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));

        var builder = Builders<SourceCountryExceptionDB>.Filter;

        var filter = builder.Eq("_id", RecordId);

        var db = new DbContext();

        var cursor = await db.SourceCountryExceptionDb.FindAsync(filter);

        IList<SourceCountryExceptionDB> results = cursor.ToList();

        var Item = results[0];

        var model = new SourceCountryExceptionModel
        {
            _id = Item._id.ToString(),
            SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
            ACTIVE = Item.ACTIVE,
            GEO_CODE = Item.GEO_CODE,
            NAME_OLD = Item.NAME_OLD,
            SHORT_LEGEND = Item.SHORT_LEGEND,
            LONG_LEGEND = Item.LONG_LEGEND,
            REGION_NAME = Item.REGION_NAME,
            SOURCE_COUNTRY_INDICATOR = Item.SOURCE_COUNTRY_INDICATOR,
            REPLACED_GEO_CODE = Item.REPLACED_GEO_CODE,
            START_DATE = (DateTime)Item.START_DATE,
            DISCONTINUED_DATE = (DateTime)Item.DISCONTINUED_DATE,
            NAME = Item.NAME
        };

        return model;
    }

    public static explicit operator SourceCountryExceptionService(SourceCountryExceptionModel v)
    {
        throw new NotImplementedException();
    }


    public async Task<bool> DoesExist(SourceCountryExceptionModel model)
    {
        var bReturn = false;
        var builder = Builders<SourceCountryExceptionDB>.Filter;
        var filter = builder.Eq("_id", model._id);

        var db = new DbContext();

        var cursor = await db.SourceCountryExceptionDb.FindAsync(filter);
        IList<SourceCountryExceptionDB> results = cursor.ToList();
        if (results.Count > 0)
            bReturn = true;

        return bReturn;
    }

    public async Task<SourceCountryExceptionDB> SaveSourceCountryException(SourceCountryExceptionModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var builder = Builders<SourceCountryExceptionDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var db = new DbContext();

        var cursor = await db.SourceCountryExceptionDb.FindAsync(filter);
        IList<SourceCountryExceptionDB> results = cursor.ToList();

        var Item = results[0];

        var modelDB = new SourceCountryExceptionDB
        {
            _id = Item._id,
            SOURCE_COUNTRY_ID = model.SOURCE_COUNTRY_ID,
            ACTIVE = model.ACTIVE,
            GEO_CODE = model.GEO_CODE,
            NAME_OLD = model.NAME_OLD,
            SHORT_LEGEND = model.SHORT_LEGEND,
            LONG_LEGEND = model.LONG_LEGEND,
            REGION_NAME = model.REGION_NAME,
            SOURCE_COUNTRY_INDICATOR = model.SOURCE_COUNTRY_INDICATOR,
            REPLACED_GEO_CODE = model.REPLACED_GEO_CODE,
            START_DATE = model.START_DATE,
            DISCONTINUED_DATE = model.DISCONTINUED_DATE,
            NAME = model.NAME
        };

        var returnModel = await db.SourceCountryExceptionDb.FindOneAndReplaceAsync(filter, modelDB);

        return returnModel;
    }

    public async Task<bool> Add(SourceCountryExceptionModel model)
    {
        var db = new DbContext();

        var modelDB = new SourceCountryExceptionDB
        {
            SOURCE_COUNTRY_ID = model.SOURCE_COUNTRY_ID,
            ACTIVE = model.ACTIVE,
            GEO_CODE = model.GEO_CODE,
            NAME_OLD = model.NAME_OLD,
            SHORT_LEGEND = model.SHORT_LEGEND,
            LONG_LEGEND = model.LONG_LEGEND,
            REGION_NAME = model.REGION_NAME,
            SOURCE_COUNTRY_INDICATOR = model.SOURCE_COUNTRY_INDICATOR,
            REPLACED_GEO_CODE = model.REPLACED_GEO_CODE,
            START_DATE = model.START_DATE,
            DISCONTINUED_DATE = model.DISCONTINUED_DATE,
            NAME = model.NAME
        };

        await db.SourceCountryExceptionDb.InsertOneAsync(modelDB);

        return true;
    }


    public async Task<SourceCountryExceptionModel> GetSourceCountryExceptionByGeoCode(string geoCode)
    {
        //var filter = Builders<DBStockModel>.Filter.Eq(x => x.StockCode, stockCode);

        var builder = Builders<SourceCountryExceptionDB>.Filter;
        var filter = builder.Eq("GEO_CODE", geoCode);

        var db = new DbContext();
        var cursor = await db.SourceCountryExceptionDb.FindAsync(filter);

        IList<SourceCountryExceptionDB> results = cursor.ToList();

        if (results.Count.Equals(0)) return new SourceCountryExceptionModel { SOURCE_COUNTRY_ID = 0 };

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

    public async Task<bool> Delete(SourceCountryExceptionModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var db = new DbContext();

        var builder = Builders<SourceCountryExceptionDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        var returnModel = await db.SourceCountryExceptionDb.DeleteOneAsync(filter);

        return true;
    }

    public async Task<int> GetCount()
    {
        var db = new DbContext();
        var cursor = await db.SourceCountryExceptionDb.FindAsync(new BsonDocument());

        IList<SourceCountryExceptionDB> results = cursor.ToList();

        return results.Count;
    }
}