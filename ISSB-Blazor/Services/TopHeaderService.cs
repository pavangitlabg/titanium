using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class TopHeaderService
{
    public async Task<List<TopHeaderModel>> GetHeaders()
    {
        var db = DbContext.Instance;
        var cursor = await db.TopHeaderDb.FindAsync(new BsonDocument());
        IList<TopHeaderDB> results = cursor.ToList();
        var modelList = new List<TopHeaderModel>();
        foreach (var Item in results)
        {
            var model = new TopHeaderModel
            {
                _id = Item._id.ToString(),
                IsActive = Item.IsActive,
                IsFooter = Item.IsFooter,
                Title = Item.Title,
                Description = Item.Description,
                HTML = Item.HTML
            };
            modelList.Add(model);
        }

        return modelList.OrderBy(x => x._id).ToList();
    }

    public async Task<TopHeaderModel> GetHeaderById(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));
        var builder = Builders<TopHeaderDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var db = DbContext.Instance;
        var cursor = await db.TopHeaderDb.FindAsync(filter);
        IList<TopHeaderDB> results = cursor.ToList();
        var Item = results[0];

        var model = new TopHeaderModel
        {
            _id = Item._id.ToString(),
            IsActive = Item.IsActive,
            IsFooter = Item.IsFooter,
            Title = Item.Title,
            Description = Item.Description,
            HTML = Item.HTML
        };
        return model;
    }

    public async Task<bool> AddHeader(TopHeaderModel model)
    {
        //Do String replace here

        var bReturn = false;
        var db = DbContext.Instance;
        var modelDB = new TopHeaderDB
        {
            IsActive = model.IsActive,
            IsFooter = model.IsFooter,
            Title = model.Title,
            Description = model.Description,
            HTML = model.HTML
        };
        await db.TopHeaderDb.InsertOneAsync(modelDB);
        bReturn = true;
        return bReturn;
    }

    public async Task<bool> UpdateHeader(TopHeaderModel model)
    {
        var bReturn = false;

        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var filter = Builders<TopHeaderDB>.Filter.Eq(x => x._id, RecordId);
        var db = DbContext.Instance;
        var cursor = await db.TopHeaderDb.FindAsync(filter);
        IList<TopHeaderDB> results = cursor.ToList();
        var modelDB = new TopHeaderDB
        {
            _id = results[0]._id,
            IsActive = model.IsActive,
            IsFooter = model.IsFooter,
            Title = model.Title,
            Description = model.Description,
            HTML = model.HTML
        };

        await db.TopHeaderDb.FindOneAndReplaceAsync(filter, modelDB);
        bReturn = true;
        return bReturn;
    }

    public async Task<bool> Delete(TopHeaderModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var db = DbContext.Instance;


        var builder = Builders<TopHeaderDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        await db.TopHeaderDb.DeleteOneAsync(filter);

        return true;
    }
}