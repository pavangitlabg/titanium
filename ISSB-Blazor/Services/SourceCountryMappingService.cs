using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class SourceCountryMappingService
{
    public async Task<List<SourceCountryMappingModel>> GetSourceCountryMappings()
    {
        var db = new DbContext();
        var cursor = await db.SourceCountryMappingDb.FindAsync(new BsonDocument());

        IList<SourceCountryMappingDB> results = cursor.ToList();
        var modelList = new List<SourceCountryMappingModel>();

        foreach (var Item in results)
        {
            var model = new SourceCountryMappingModel
            {
                _id = Item._id.ToString(),
                COUNTRY_NAME = Item.COUNTRY_NAME,
                ISSB_Country_Geo_Code = Item.ISSB_Country_Geo_Code,
                Source_Country_ID = Item.Source_Country_ID,
                Source_Country_Value = Item.Source_Country_Value
            };

            modelList.Add(model);
        }

        return modelList.OrderBy(x => x._id).ToList();
    }

    public async Task<List<SourceCountryMappingDB>> GetSourceCountryMappingsRaw()
    {
        var db = new DbContext();
        var cursor = await db.SourceCountryMappingDb.FindAsync(new BsonDocument());

        IList<SourceCountryMappingDB> results = cursor.ToList();

        return results.ToList();
    }

    public async Task<bool> UpdateNames()
    {
        var list = await GetSourceCountryMappingsRaw();
        var mSrv = new MarketCountryService();


        var db = new DbContext();

        foreach (var Item in list)
        {
            var model = await mSrv.GetMarketCountryByGeoCode(Item.ISSB_Country_Geo_Code);
            Item.COUNTRY_NAME = model.NAME;

            var builder = Builders<SourceCountryMappingDB>.Filter;

            var filter = builder.Eq("_id", Item._id);

            await db.SourceCountryMappingDb.FindOneAndReplaceAsync(filter, Item);
        }


        //await db.SourceCountryMappingDB.UpdateManyAsync(filter,list);
        return true;
    }

    public async Task<SourceCountryMappingModel> GetSourceCountryByGeoCode(string geoCode)
    {
        //var filter = Builders<DBStockModel>.Filter.Eq(x => x.StockCode, stockCode);

        var builder = Builders<SourceCountryMappingDB>.Filter;
        //var filter = builder.Eq("GEO_CODE", geoCode);
        var filter = builder.Eq("Source_Country_Value", geoCode);

        var db = new DbContext();
        var cursor = await db.SourceCountryMappingDb.FindAsync(filter);

        IList<SourceCountryMappingDB> results = cursor.ToList();

        if (results.Count.Equals(0)) return new SourceCountryMappingModel { Source_Country_Value = string.Empty };

        var Item = results[0];
        var model = new SourceCountryMappingModel
        {
            _id = Item._id.ToString(),
            COUNTRY_NAME = Item.COUNTRY_NAME,
            ISSB_Country_Geo_Code = Item.ISSB_Country_Geo_Code,
            Source_Country_ID = Item.Source_Country_ID,
            Source_Country_Value = Item.Source_Country_Value
        };

        return model;
    }

    public async Task<SourceCountryMappingModel> GetSourceCountryMappingByID(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));

        var builder = Builders<SourceCountryMappingDB>.Filter;

        var filter = builder.Eq("_id", RecordId);

        var db = new DbContext();
        var cursor = await db.SourceCountryMappingDb.FindAsync(filter);

        IList<SourceCountryMappingDB> results = cursor.ToList();

        var item = results[0];

        var model = new SourceCountryMappingModel
        {
            _id = item._id.ToString(),
            COUNTRY_NAME = item.COUNTRY_NAME,
            ISSB_Country_Geo_Code = item.ISSB_Country_Geo_Code,
            Source_Country_ID = item.Source_Country_ID,
            Source_Country_Value = item.Source_Country_Value
        };

        return model;
    }

    public async Task<bool> Add(SourceCountryMappingModel model)
    {
        var db = new DbContext();

        var modelDB = new SourceCountryMappingDB
        {
            COUNTRY_NAME = model.COUNTRY_NAME,
            ISSB_Country_Geo_Code = model.ISSB_Country_Geo_Code,
            Source_Country_ID = model.Source_Country_ID,
            Source_Country_Value = model.Source_Country_Value
        };

        await db.SourceCountryMappingDb.InsertOneAsync(modelDB);

        return true;
    }

    public async Task<SourceCountryMappingDB> Save(SourceCountryMappingModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var builder = Builders<SourceCountryMappingDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var db = new DbContext();

        var cursor = await db.SourceCountryMappingDb.FindAsync(filter);
        IList<SourceCountryMappingDB> results = cursor.ToList();

        var Item = results[0];

        var modelDB = new SourceCountryMappingDB
        {
            _id = Item._id,
            COUNTRY_NAME = model.COUNTRY_NAME,
            ISSB_Country_Geo_Code = model.ISSB_Country_Geo_Code,
            Source_Country_ID = model.Source_Country_ID,
            Source_Country_Value = model.Source_Country_Value
        };

        var returnModel = await db.SourceCountryMappingDb.FindOneAndReplaceAsync(filter, modelDB);

        return returnModel;
    }

    public async Task<bool> Delete(SourceCountryMappingModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var db = new DbContext();

        var builder = Builders<SourceCountryMappingDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        var returnModel = await db.SourceCountryMappingDb.DeleteOneAsync(filter);

        return true;
    }

    public async Task<int> GetCount()
    {
        var db = new DbContext();
        var cursor = await db.SourceCountryMappingDb.FindAsync(new BsonDocument());

        IList<SourceCountryMappingDB> results = cursor.ToList();

        return results.Count;
    }
}