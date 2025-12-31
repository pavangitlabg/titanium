using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class TariffIndexService
{
    public async Task<List<TariffIndexModel>> GetTariffIndex()
    {
        TariffIndexModel model;

        var db = new DbContext();

        var cursor = await db.TariffIndexDb.FindAsync(new BsonDocument());

        IList<TariffIndexDB> results = cursor.ToList();
        var modelList = new List<TariffIndexModel>();

        foreach (var item in results)
        {
            model = new TariffIndexModel
            {
                _id = item._id.ToString(),
                Name = item.Name,
                Code = item.Code,
                Description = item.Description,
                Active = item.Active
            };

            modelList.Add(model);
        }

        return modelList;
    }


    public async Task<TariffIndexModel> GetTariffIndexByID(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));

        var builder = Builders<TariffIndexDB>.Filter;

        var filter = builder.Eq("_id", RecordId);

        var db = new DbContext();
        var cursor = await db.TariffIndexDb.FindAsync(filter);

        IList<TariffIndexDB> results = cursor.ToList();

        var item = results[0];

        var model = new TariffIndexModel
        {
            _id = item._id.ToString(),
            Name = item.Name,
            Code = item.Code,
            Description = item.Description,
            Active = item.Active
        };

        return model;
    }

    public async Task<bool> Add(TariffIndexModel model)
    {
        var db = new DbContext();

        var modelDB = new TariffIndexDB
        {
            Name = model.Name,
            Code = model.Code,
            Description = model.Description,
            Active = model.Active
        };

        await db.TariffIndexDb.InsertOneAsync(modelDB);

        return true;
    }

    public async Task<TariffIndexDB> Save(TariffIndexModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var builder = Builders<TariffIndexDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var db = new DbContext();

        var cursor = await db.TariffIndexDb.FindAsync(filter);
        IList<TariffIndexDB> results = cursor.ToList();

        var Item = results[0];

        var modelDB = new TariffIndexDB
        {
            _id = Item._id,
            Name = model.Name,
            Code = model.Code,
            Description = model.Description,
            Active = model.Active
        };

        var returnModel = await db.TariffIndexDb.FindOneAndReplaceAsync(filter, modelDB);

        return returnModel;
    }

    public async Task<bool> Delete(TariffIndexModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var db = new DbContext();

        var builder = Builders<TariffIndexDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        var returnModel = await db.TariffIndexDb.DeleteOneAsync(filter);

        return true;
    }

    public async Task<int> GetCount()
    {
        var db = new DbContext();
        var cursor = await db.TariffIndexDb.FindAsync(new BsonDocument());

        IList<TariffIndexDB> results = cursor.ToList();

        return results.Count;
    }
}