using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class ReportKeysService
{
    public async Task<List<ReportKeysModel>> GetReportKeys()
    {
        ReportKeysModel model;

        var db = new DbContext();
        var cursor = await db.ReportKeysDb.FindAsync(new BsonDocument());

        IList<ReportKeysDB> results = cursor.ToList();
        var modelList = new List<ReportKeysModel>();

        foreach (var item in results)
        {
            model = new ReportKeysModel
            {
                _id = item._id.ToString(),
                Sort = item.Sort,
                Key = item.Key,
                Description = item.Description,
                Title = item.Title,
                IsSelected = item.IsSelected,
                Width = item.Width
            };

            modelList.Add(model);
        }

        return modelList.OrderBy(x => x.Sort).ToList();
    }


    //public async Task<bool> Save(ReportKeysModel model)
    //{
    //    bool bReturn;
    //    var db = new DBContext();

    //    var modelDB = new ReportKeysDB
    //    {
    //        Sort = model.Sort,
    //        Key = model.Key,
    //        Description = model.Description,
    //        Title = model.Title,
    //        IsSelected = model.IsSelected,
    //        Width = model.Width
    //    };

    //    await db.ReportKeysDB.InsertOneAsync(modelDB);
    //    bReturn = true;

    //    return bReturn;
    //}

    public async Task<ReportKeysModel> GetReportKeysByID(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));

        var builder = Builders<ReportKeysDB>.Filter;

        var filter = builder.Eq("_id", RecordId);

        var db = new DbContext();
        var cursor = await db.ReportKeysDb.FindAsync(filter);

        IList<ReportKeysDB> results = cursor.ToList();

        var item = results[0];

        var model = new ReportKeysModel
        {
            _id = item._id.ToString(),
            Sort = item.Sort,
            Key = item.Key,
            Description = item.Description,
            Title = item.Title,
            IsSelected = item.IsSelected,
            Width = item.Width
        };

        return model;
    }

    public async Task<bool> Add(ReportKeysModel model)
    {
        var db = new DbContext();

        var modelDB = new ReportKeysDB
        {
            Sort = model.Sort,
            Key = model.Key,
            Description = model.Description,
            Title = model.Title,
            IsSelected = model.IsSelected,
            Width = model.Width
        };

        await db.ReportKeysDb.InsertOneAsync(modelDB);

        return true;
    }

    public async Task<ReportKeysDB> Save(ReportKeysModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var builder = Builders<ReportKeysDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var db = new DbContext();

        var cursor = await db.ReportKeysDb.FindAsync(filter);
        IList<ReportKeysDB> results = cursor.ToList();

        var Item = results[0];

        var modelDB = new ReportKeysDB
        {
            _id = Item._id,
            Sort = model.Sort,
            Key = model.Key,
            Description = model.Description,
            Title = model.Title,
            IsSelected = model.IsSelected,
            Width = model.Width
        };

        var returnModel = await db.ReportKeysDb.FindOneAndReplaceAsync(filter, modelDB);

        return returnModel;
    }

    public async Task<bool> Delete(ReportKeysModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var db = new DbContext();

        var builder = Builders<ReportKeysDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        var returnModel = await db.ReportKeysDb.DeleteOneAsync(filter);

        return true;
    }

    public async Task<int> GetCount()
    {
        var db = new DbContext();
        var cursor = await db.ReportKeysDb.FindAsync(new BsonDocument());

        IList<ReportKeysDB> results = cursor.ToList();

        return results.Count;
    }
}