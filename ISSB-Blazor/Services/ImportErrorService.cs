using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class ImportErrorService
{
    public async Task<List<ImportErrorLogModel>> GetRecords()
    {
        var db = new DbContext();
        var cursor = await db.ImportErrorLogDb.FindAsync(new BsonDocument());

        IList<ImportErrorLogDB> results = cursor.ToList();
        var modelList = new List<ImportErrorLogModel>();

        foreach (var item in results)
        {
            var model = new ImportErrorLogModel
            {
                _id = item._id.ToString(),
                Type = item.Type,
                Code = item.Code,
                Class = item.Class,
                ErrorMessage = item.ErrorMessage,
                BatchNumber = item.BatchNumber
            };

            modelList.Add(model);
        }

        return modelList;
    }

    public async Task<bool> AddError(ImportErrorLogModel model)
    {
        var bReturn = false;
        var db = new DbContext();

        var modelDB = new ImportErrorLogDB
        {
            Type = model.Type,
            Code = model.Code,
            Class = model.Class,
            ErrorMessage = model.ErrorMessage,
            BatchNumber = model.BatchNumber
        };

        await db.ImportErrorLogDb.InsertOneAsync(modelDB);
        bReturn = true;

        return bReturn;
    }

    public async Task<ImportErrorLogModel> GetCode(string code)
    {
        var builder = Builders<ImportErrorLogDB>.Filter;
        var filter = builder.Eq("Code", code);

        var db = new DbContext();
        var cursor = await db.ImportErrorLogDb.FindAsync(filter);

        IList<ImportErrorLogDB> results = cursor.ToList();

        if (results.Count.Equals(0)) return new ImportErrorLogModel { Code = string.Empty };

        var Item = results[0];

        var model = new ImportErrorLogModel
        {
            _id = Item._id.ToString(),
            Type = Item.Type,
            BatchNumber = Item.BatchNumber,
            Class = Item.Class,
            Code = Item.Code,
            ErrorMessage = Item.ErrorMessage
        };

        return model;
    }

    public async Task<bool> DeleteRecord(string Id)
    {
        var RecordId = new BsonObjectId(new ObjectId(Id));
        var db = new DbContext();

        var builder = Builders<ImportErrorLogDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        await db.ImportErrorLogDb.DeleteOneAsync(filter);

        return true;
    }
}