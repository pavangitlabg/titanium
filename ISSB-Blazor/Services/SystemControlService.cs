using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class SystemControlService
{
    public async Task<SystemControlModel> GetSystemControl()
    {
        var db = new DbContext();
        var cursor = await db.SystemControlDb.FindAsync(new BsonDocument());

        IList<SystemControlDB> results = cursor.ToList();

        var model = new SystemControlModel
        {
            _id = results[0]._id.ToString(),
            LastFileNumber = results[0].LastFileNumber,
            ImportBatch = results[0].ImportBatch,
            HowlerWeight = results[0].HowlerWeight,
            HowlerValue = results[0].HowlerValue,
            SMSMessage = results[0].SMSMessage,
            SMSNumber = results[0].SMSNumber,
            ScheduleHour = results[0].ScheduleHour
        };
        return model;
    }

    public async Task<bool> UpdateControl(SystemControlModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var db = new DbContext();

        var dbModel = new SystemControlDB
        {
            _id = RecordId,
            LastFileNumber = model.LastFileNumber,
            ImportBatch = model.ImportBatch,
            HowlerValue = model.HowlerValue,
            HowlerWeight = model.HowlerWeight,
            SMSNumber = model.SMSNumber,
            SMSMessage = model.SMSMessage,
            ScheduleHour = model.ScheduleHour
        };


        var builder = Builders<SystemControlDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        await db.SystemControlDb.FindOneAndReplaceAsync(filter, dbModel);

        return true;
    }
}