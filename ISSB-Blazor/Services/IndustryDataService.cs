using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class IndustryDataService
{
    private static IndustryDataService _instance;

    public static IndustryDataService Instance
    {
        set { }
        get
        {
            if (_instance == null) _instance = new IndustryDataService();
            return _instance;
        }
    }

    public async Task<List<IndustryColumnModel>> GetColumns()
    {
        var db = DbContext.Instance;
        var cursor = await db.IndustryColumnDb.FindAsync(new BsonDocument());
        IList<IndustryColumnDB> results = cursor.ToList();
        var modelList = new List<IndustryColumnModel>();

        foreach (var Item in results)
        {
            var model = new IndustryColumnModel
            {
                _id = Item._id.ToString(),
                Name = Item.Name,
                Number = Item.Number
            };
            modelList.Add(model);
        }

        return modelList;
    }

    public async Task<bool> AddCols(IndustryColumnModel model)
    {
        var db = DbContext.Instance;

        var modelDB = new IndustryColumnDB
        {
            Name = model.Name,
            Number = model.Number
        };

        await db.IndustryColumnDb.InsertOneAsync(modelDB);
        return true;
    }

    public async Task<bool> SaveCol(IndustryColumnModel model)
    {
        var bReturn = false;

        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var filter = Builders<IndustryColumnDB>.Filter.Eq(x => x._id, RecordId);
        var db = DbContext.Instance;
        var cursor = await db.IndustryColumnDb.FindAsync(filter);
        IList<IndustryColumnDB> results = cursor.ToList();
        var modelDB = new IndustryColumnDB
        {
            _id = results[0]._id,

            Name = model.Name,
            Number = model.Number
        };
        await db.IndustryColumnDb.FindOneAndReplaceAsync(filter, modelDB);
        bReturn = true;
        return bReturn;
    }

    public async Task<IndustryColumnModel> GetCol(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));
        var db = DbContext.Instance;
        var builder = Builders<IndustryColumnDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var cursor = await db.IndustryColumnDb.FindAsync(filter);
        IList<IndustryColumnDB> results = cursor.ToList();

        var model = new IndustryColumnModel
        {
            Name = results[0].Name,
            Number = results[0].Number,
            _id = results[0]._id.ToString()
        };

        return model;
    }

    public async Task<FileUploadTypesModel> GetSource(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));
        var db = DbContext.Instance;
        var builder = Builders<FileUploadTypesDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var cursor = await db.FileUploadTypesDb.FindAsync(filter);
        IList<FileUploadTypesDB> results = cursor.ToList();

        var model = new FileUploadTypesModel
        {
            Description = results[0].Description,
            Number = results[0].Number,
            _id = results[0]._id.ToString()
        };

        return model;
    }

    public async Task<bool> DeleteCol(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));
        var db = DbContext.Instance;
        var builder = Builders<IndustryColumnDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var returnModel = await db.IndustryColumnDb.DeleteOneAsync(filter);
        return true;
    }

    public async Task<List<IndustryRowModel>> GetRows()
    {
        var db = DbContext.Instance;
        var cursor = await db.IndustryRowDb.FindAsync(new BsonDocument());
        IList<IndustryRowDB> results = cursor.ToList();
        var modelList = new List<IndustryRowModel>();

        foreach (var Item in results)
        {
            var model = new IndustryRowModel
            {
                _id = Item._id.ToString(),
                Description = Item.Description,
                Number = Item.Number
            };
            modelList.Add(model);
        }

        return modelList;
    }


    public async Task<bool> AddRows(IndustryRowModel model)
    {
        var db = DbContext.Instance;

        var modelDB = new IndustryRowDB
        {
            Description = model.Description,
            Number = model.Number
        };

        await db.IndustryRowDb.InsertOneAsync(modelDB);
        return true;
    }

    public async Task<bool> SaveRow(IndustryRowModel model)
    {
        var bReturn = false;

        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var filter = Builders<IndustryRowDB>.Filter.Eq(x => x._id, RecordId);
        var db = DbContext.Instance;
        var cursor = await db.IndustryRowDb.FindAsync(filter);
        IList<IndustryRowDB> results = cursor.ToList();
        var modelDB = new IndustryRowDB
        {
            _id = results[0]._id,

            Description = model.Description,
            Number = model.Number
        };
        await db.IndustryRowDb.FindOneAndReplaceAsync(filter, modelDB);
        bReturn = true;
        return bReturn;
    }

    public async Task<bool> SaveSource(FileUploadTypesModel model)
    {
        var bReturn = false;

        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var filter = Builders<FileUploadTypesDB>.Filter.Eq(x => x._id, RecordId);
        var db = DbContext.Instance;
        var cursor = await db.FileUploadTypesDb.FindAsync(filter);
        IList<FileUploadTypesDB> results = cursor.ToList();
        var modelDB = new FileUploadTypesDB
        {
            _id = results[0]._id,

            Description = model.Description,
            Number = model.Number
        };
        await db.FileUploadTypesDb.FindOneAndReplaceAsync(filter, modelDB);
        bReturn = true;
        return bReturn;
    }

    public async Task<IndustryRowModel> GetRow(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));
        var db = DbContext.Instance;
        var builder = Builders<IndustryRowDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var cursor = await db.IndustryRowDb.FindAsync(filter);
        IList<IndustryRowDB> results = cursor.ToList();

        var model = new IndustryRowModel
        {
            Description = results[0].Description,
            Number = results[0].Number,
            _id = results[0]._id.ToString()
        };

        return model;
    }

    public async Task<bool> DeleteRow(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));
        var db = DbContext.Instance;
        var builder = Builders<IndustryRowDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var returnModel = await db.IndustryRowDb.DeleteOneAsync(filter);
        return true;
    }

    public async Task<bool> DeleteSource(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));
        var db = DbContext.Instance;
        var builder = Builders<FileUploadTypesDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var returnModel = await db.FileUploadTypesDb.DeleteOneAsync(filter);
        return true;
    }
}