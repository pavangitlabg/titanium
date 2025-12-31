using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class AppointmentsService
{
    public async Task<List<AppointmentsModel>> GetAppointments()
    {
        var db = DbContext.Instance;
        var cursor = await db.AppointmentsDb.FindAsync(new BsonDocument());

        IList<AppointmentsDB> results = cursor.ToList();
        var modelList = new List<AppointmentsModel>();

        foreach (var Item in results)
        {
            var model = new AppointmentsModel
            {
                _id = Item._id.ToString(),
                Date = (DateTime)Item.Date,
                Title = Item.Title,
                Description = Item.Description,
                IsPublished = Item.IsPublished,
                Notes = Item.Notes
            };

            modelList.Add(model);
        }

        return modelList.OrderBy(x => x.Date).ToList();
    }

    public async Task<AppointmentsModel> GetAppointmentByID(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));
        var builder = Builders<AppointmentsDB>.Filter;

        var filter = builder.Eq("_id", RecordId);

        var db = DbContext.Instance;
        var cursor = await db.AppointmentsDb.FindAsync(filter);

        IList<AppointmentsDB> results = cursor.ToList();

        var Item = results[0];

        var model = new AppointmentsModel
        {
            _id = Item._id.ToString(),
            Date = (DateTime)Item.Date,
            Title = Item.Title,
            Description = Item.Description,
            IsPublished = Item.IsPublished,
            Notes = Item.Notes
        };

        return model;
    }

    public async Task<bool> Add(AppointmentsModel model)
    {
        var db = DbContext.Instance;

        var modelDB = new AppointmentsDB
        {
            Date = model.Date,
            Title = model.Title,
            Description = model.Description,
            IsPublished = model.IsPublished,
            Notes = model.Notes
        };

        await db.AppointmentsDb.InsertOneAsync(modelDB);

        return true;
    }

    public async Task<AppointmentsDB> Update(AppointmentsModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var builder = Builders<AppointmentsDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var db = DbContext.Instance;

        var cursor = await db.AppointmentsDb.FindAsync(filter);
        IList<AppointmentsDB> results = cursor.ToList();

        var Item = results[0];

        var modelDB = new AppointmentsDB
        {
            _id = Item._id,
            Date = model.Date,
            Title = model.Title,
            Description = model.Description,
            IsPublished = model.IsPublished,
            Notes = model.Notes
        };

        var returnModel = await db.AppointmentsDb.FindOneAndReplaceAsync(filter, modelDB);

        return returnModel;
    }

    public async Task<bool> Delete(AppointmentsModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var db = DbContext.Instance;

        var builder = Builders<AppointmentsDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        var returnModel = await db.AppointmentsDb.DeleteOneAsync(filter);

        return true;
    }
}