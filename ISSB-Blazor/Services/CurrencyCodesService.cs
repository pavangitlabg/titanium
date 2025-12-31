using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class CurrencyCodesService
{
    public async Task<List<CurrencyCodesModel>> GetCurrencyCodes()
    {
        CurrencyCodesModel model;

        var db = new DbContext();
        var cursor = await db.CurrencyCodesDb.FindAsync(new BsonDocument());

        IList<CurrencyCodesDB> results = cursor.ToList();
        var modelList = new List<CurrencyCodesModel>();

        foreach (var item in results)
        {
            if (string.IsNullOrEmpty(item.CURRENCY_SYMBOL))
                item.CURRENCY_SYMBOL = string.Empty;

            if (item.CURRENCY_SYMBOL.Equals("NULL"))
                item.CURRENCY_SYMBOL = string.Empty;

            model = new CurrencyCodesModel
            {
                _id = item._id.ToString(),
                CURRENCY_CODE = item.CURRENCY_CODE,
                CURRENCY_NAME = item.CURRENCY_NAME,
                CURRENCY_SYMBOL = item.CURRENCY_SYMBOL
            };

            modelList.Add(model);
        }

        return modelList.OrderBy(x => x.CURRENCY_NAME).ToList();
    }


    public async Task<CurrencyCodesModel> GetCurrencyCodesByID(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));

        var builder = Builders<CurrencyCodesDB>.Filter;

        var filter = builder.Eq("_id", RecordId);

        var db = new DbContext();
        var cursor = await db.CurrencyCodesDb.FindAsync(filter);

        IList<CurrencyCodesDB> results = cursor.ToList();

        var item = results[0];
        if (string.IsNullOrEmpty(item.CURRENCY_SYMBOL))
            item.CURRENCY_SYMBOL = string.Empty;

        if (item.CURRENCY_SYMBOL.Equals("NULL"))
            item.CURRENCY_SYMBOL = string.Empty;

        var model = new CurrencyCodesModel
        {
            _id = item._id.ToString(),
            CURRENCY_CODE = item.CURRENCY_CODE,
            CURRENCY_NAME = item.CURRENCY_NAME,
            CURRENCY_SYMBOL = item.CURRENCY_SYMBOL
        };

        return model;
    }

    public async Task<bool> Add(CurrencyCodesModel model)
    {
        var db = new DbContext();

        var modelDB = new CurrencyCodesDB
        {
            CURRENCY_CODE = model.CURRENCY_CODE,
            CURRENCY_NAME = model.CURRENCY_NAME,
            CURRENCY_SYMBOL = model.CURRENCY_SYMBOL
        };

        await db.CurrencyCodesDb.InsertOneAsync(modelDB);

        return true;
    }

    public async Task<CurrencyCodesDB> Save(CurrencyCodesModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var builder = Builders<CurrencyCodesDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var db = new DbContext();

        var cursor = await db.CurrencyCodesDb.FindAsync(filter);
        IList<CurrencyCodesDB> results = cursor.ToList();

        var Item = results[0];

        var modelDB = new CurrencyCodesDB
        {
            _id = Item._id,
            CURRENCY_CODE = model.CURRENCY_CODE,
            CURRENCY_NAME = model.CURRENCY_NAME,
            CURRENCY_SYMBOL = model.CURRENCY_SYMBOL
        };

        var returnModel = await db.CurrencyCodesDb.FindOneAndReplaceAsync(filter, modelDB);

        return returnModel;
    }

    public async Task<bool> Delete(CurrencyCodesModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var db = new DbContext();

        var builder = Builders<CurrencyCodesDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        var returnModel = await db.CurrencyCodesDb.DeleteOneAsync(filter);

        return true;
    }

    public async Task<int> GetCount()
    {
        var db = new DbContext();
        var cursor = await db.CurrencyCodesDb.FindAsync(new BsonDocument());

        IList<CurrencyCodesDB> results = cursor.ToList();

        return results.Count;
    }
}