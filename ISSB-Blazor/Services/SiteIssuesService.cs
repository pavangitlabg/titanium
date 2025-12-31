using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class SiteIssuesService
{
    public async Task<List<SiteIssuesModel>> GetIssues()
    {
        SiteIssuesModel model;

        var db = new DbContext();
        var cursor = await db.SiteIssuesDb.FindAsync(new BsonDocument());

        IList<SiteIssuesDB> results = cursor.ToList();
        var modelList = new List<SiteIssuesModel>();

        foreach (var item in results)
        {
            model = new SiteIssuesModel
            {
                _id = item._id.ToString(),
                RequestID = item.RequestID,
                User = item.User,
                Subject = item.Subject,
                Description = item.Description,
                Comment = item.Comment,
                Status = item.Status,
                Severity = item.Severity,
                Email = item.Email,
                Timestamp = item.Timestamp
            };

            modelList.Add(model);
        }

        return modelList.OrderByDescending(x => x.RequestID).ToList();
    }

    public async Task<bool> Add(SiteIssuesModel model)
    {
        // BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
        var db = new DbContext();

        var modelDB = new SiteIssuesDB
        {
            RequestID = model.RequestID,
            User = model.User,
            Subject = model.Subject,
            Description = model.Description,
            Comment = model.Comment,
            Status = model.Status,
            Severity = model.Severity,
            Email = model.Email,
            Timestamp = model.Timestamp
        };

        await db.SiteIssuesDb.InsertOneAsync(modelDB);

        return true;
    }

    public async Task<SiteIssuesModel> GetIssuesById(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));

        var builder = Builders<SiteIssuesDB>.Filter;

        var filter = builder.Eq("_id", RecordId);

        var db = new DbContext();
        var cursor = await db.SiteIssuesDb.FindAsync(filter);

        IList<SiteIssuesDB> results = cursor.ToList();

        var Item = results[0];

        var model = new SiteIssuesModel
        {
            _id = Item._id.ToString(),
            RequestID = Item.RequestID,
            User = Item.User,
            Subject = Item.Subject,
            Description = Item.Description,
            Comment = Item.Comment,
            Status = Item.Status,
            Severity = Item.Severity,
            Email = Item.Email,
            Timestamp = Item.Timestamp
        };

        return model;
    }

    public async Task<bool> Delete(SiteIssuesModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var db = new DbContext();

        var builder = Builders<SiteIssuesDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        var returnModel = await db.SiteIssuesDb.DeleteOneAsync(filter);

        return true;
    }

    public async Task<int> GetCount()
    {
        var db = new DbContext();
        var cursor = await db.SiteIssuesDb.FindAsync(new BsonDocument());

        IList<SiteIssuesDB> results = cursor.ToList();

        return results.Count;
    }

    public async Task<SiteIssuesDB> SaveSiteIssues(SiteIssuesModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var builder = Builders<SiteIssuesDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var db = new DbContext();

        var cursor = await db.SiteIssuesDb.FindAsync(filter);
        IList<SiteIssuesDB> results = cursor.ToList();

        var Item = results[0];

        var modelDB = new SiteIssuesDB
        {
            _id = Item._id,
            RequestID = model.RequestID,
            User = model.User,
            Subject = model.Subject,
            Description = model.Description,
            Comment = model.Comment,
            Status = model.Status,
            Severity = model.Severity,
            Email = model.Email,
            Timestamp = model.Timestamp
        };

        var returnModel = await db.SiteIssuesDb.FindOneAndReplaceAsync(filter, modelDB);

        return returnModel;
    }

    public async Task<bool> DoesExist(SiteIssuesModel model)
    {
        var bReturn = false;
        var builder = Builders<SiteIssuesDB>.Filter;
        var filter = builder.Eq("_id", model._id);

        var db = new DbContext();

        var cursor = await db.SiteIssuesDb.FindAsync(filter);
        IList<SiteIssuesDB> results = cursor.ToList();
        if (results.Count > 0)
            bReturn = true;

        return bReturn;
    }
}