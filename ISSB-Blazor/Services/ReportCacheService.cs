using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Services;

public class ReportCacheService
{
    private static readonly Lazy<ReportCacheService> lazy = new(() => new ReportCacheService());
    public static ReportCacheService Instance => lazy.Value;

    public async Task<List<object>> GetByReportID(string ReportID)
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(ReportWriterByMonthOutModel)))
            BsonClassMap.RegisterClassMap<ReportWriterByMonthOutModel>(cm => { cm.AutoMap(); });

        var db = DbContext.Instance;

        var builder = Builders<ReportCacheDB>.Filter;
        var filter = builder.Eq("ReportID", ReportID);

        var cursor = await db.ReportCacheDb.FindAsync(filter);

        var results = (IList<ReportCacheDB>)cursor.ToList().Take(1000);
        var modelList = new List<object>();

        foreach (var Item in results)
        {
            var model = new ReportCacheModel
            {
                _id = Item._id.ToString(),
                Key = Item.Key,
                ReportID = Item.ReportID,
                Data = Item.Data
            };

            modelList.Add(model.Data);
        }

        return modelList;
    }

    public async Task<List<object>> GetRange(string ReportID, int Skip, int Limit)
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(ReportWriterByMonthOutModel)))
            BsonClassMap.RegisterClassMap<ReportWriterByMonthOutModel>(cm => { cm.AutoMap(); });

        var db = DbContext.Instance;
        var collection = db._database.GetCollection<BsonDocument>("ReportCacheDB");
        var modelList = new List<object>();
        var dataList = new List<ReportCacheModel>();

        var options = new AggregateOptions
        {
            AllowDiskUse = true
        };

        PipelineDefinition<BsonDocument, BsonDocument> pipeline = new BsonDocument[]
        {
            new("$match", new BsonDocument()
                .Add("ReportID", ReportID)),
            // new BsonDocument("$skip", Skip),
            // new BsonDocument("$limit", Limit)
            new("$sort", new BsonDocument()
                .Add("_id", 1.0))
        };

        using (var cursor = await collection.AggregateAsync(pipeline, options))
        {
            while (await cursor.MoveNextAsync())
            {
                var batch = cursor.Current;
                foreach (var document in batch)
                {
                    var aItem = BsonSerializer.Deserialize<ReportCacheDB>(document.ToJson());

                    var bItem = new ReportCacheModel
                    {
                        _id = aItem._id.ToString(),
                        Key = aItem.Key,
                        ReportID = aItem.ReportID,
                        Data = aItem.Data
                    };

                    modelList.Add(bItem.Data);
                }
            }
        }

        return modelList;
    }


    public async Task<int> Count(string ReportID)
    {
        var NoOfRecords = new CountModel { RecordCount = 0 };
        var db = DbContext.Instance;

        var collection = db._database.GetCollection<BsonDocument>("ReportCacheDB");

        var options = new AggregateOptions
        {
            AllowDiskUse = false
        };

        PipelineDefinition<BsonDocument, BsonDocument> pipeline = new BsonDocument[]
        {
            new("$match", new BsonDocument()
                .Add("ReportID", ReportID)),
            new("$count", "RecordCount")
        };

        using (var cursor = await collection.AggregateAsync(pipeline, options))
        {
            while (await cursor.MoveNextAsync())
            {
                var batch = cursor.Current;
                foreach (var document in batch)
                {
                    Console.WriteLine(document.ToJson());
                    NoOfRecords = BsonSerializer.Deserialize<CountModel>(document.ToJson());
                }
            }
        }

        return NoOfRecords.RecordCount;
    }


    public async Task<bool> Add(ReportCacheModel model)
    {
        var db = DbContext.Instance;

        var builder = Builders<ReportCacheDB>.Filter;
        var filter = builder.Eq("ReportID", model.ReportID) & builder.Eq("Key", model.Key);
        await db.ReportCacheDb.DeleteManyAsync(filter);

        var modelDB = new ReportCacheDB
        {
            ReportID = model.ReportID,
            Data = model.Data,
            Key = model.Key
        };

        await db.ReportCacheDb.InsertOneAsync(modelDB);

        return true;
    }

    public async Task<bool> AddMany(List<ReportCacheModel> model)
    {
        var db = DbContext.Instance;

        //var builder = Builders<ReportCacheDB>.Filter;
        //var filter = builder.Eq("ReportID", model.ReportID) & builder.Eq("Key", model.Key);
        //await db.ReportCacheDB.DeleteManyAsync(filter);
        var modelDBList = new List<ReportCacheDB>();

        foreach (var Item in model)
        {
            var modelDB = new ReportCacheDB
            {
                ReportID = Item.ReportID,
                Data = Item.Data,
                Key = Item.Key
            };
            modelDBList.Add(modelDB);
        }

        await db.ReportCacheDb.InsertManyAsync(modelDBList);

        return true;
    }

    public async Task<bool> DeleteByReportID(string ReportID)
    {
        var db = DbContext.Instance;

        var builder = Builders<ReportCacheDB>.Filter;
        var filter = builder.Eq("ReportID", ReportID);

        await db.ReportCacheDb.DeleteManyAsync(filter);

        return true;
    }
}

public class CountModel
{
    public int RecordCount { get; set; }
}