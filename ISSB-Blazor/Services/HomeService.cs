using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class HomeService
{
    public async Task<List<HomeModel>> GetAllContactForm()
    {
        var db = new DbContext();
        var cursor = await db.HomeDb.FindAsync(new BsonDocument());

        IList<HomeDB> results = cursor.ToList();
        var modelList = new List<HomeModel>();

        foreach (var Item in results)
        {
            var model = new HomeModel
            {
                _id = Item._id.ToString(),
                FirstName = Item.FirstName,
                Lastname = Item.Lastname,
                Company = Item.Company,
                Info = Item.Info,
                Address1 = Item.Address1,
                Address2 = Item.Address2,
                City = Item.City,
                State = Item.State,
                Zip = Item.Zip,
                Phone = Item.Phone,
                Email = Item.Email,
                Comment = Item.Comment,
                ContactDate = (DateTime)Item.ContactDate,
                Status = Item.Status
            };

            modelList.Add(model);
        }

        return modelList.OrderBy(x => x._id).ToList();
    }

    public async Task<HomeModel> GetContactFormById(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));

        var builder = Builders<HomeDB>.Filter;

        var filter = builder.Eq("_id", RecordId);

        var db = new DbContext();
        var cursor = await db.HomeDb.FindAsync(filter);

        IList<HomeDB> results = cursor.ToList();

        var Item = results[0];

        var model = new HomeModel
        {
            _id = Item._id.ToString(),
            FirstName = Item.FirstName,
            Lastname = Item.Lastname,
            Company = Item.Company,
            Info = Item.Info,
            Address1 = Item.Address1,
            Address2 = Item.Address2,
            City = Item.City,
            State = Item.State,
            Zip = Item.Zip,
            Phone = Item.Phone,
            Email = Item.Email,
            Comment = Item.Comment,
            ContactDate = (DateTime)Item.ContactDate,
            Status = Item.Status
        };

        return model;
    }

    public async Task<bool> ContactUsAdd(HomeModel model)
    {
        var db = new DbContext();

        var modelDB = new HomeDB
        {
            FirstName = model.FirstName,
            Lastname = model.Lastname,
            Company = model.Company,
            Info = model.Info,
            Address1 = model.Address1,
            Address2 = model.Address2,
            City = model.City,
            State = model.State,
            Zip = model.Zip,
            Phone = model.Phone,
            Email = model.Email,
            Comment = model.Comment,
            ContactDate = model.ContactDate,
            Status = model.Status
        };

        await db.HomeDb.InsertOneAsync(modelDB);

        return true;
    }

    public async Task<HomeDB> ContactUsSave(HomeModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var builder = Builders<HomeDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var db = new DbContext();

        var cursor = await db.HomeDb.FindAsync(filter);
        IList<HomeDB> results = cursor.ToList();

        var Item = results[0];

        var modelDB = new HomeDB
        {
            _id = Item._id,
            FirstName = model.FirstName,
            Lastname = model.Lastname,
            Company = model.Company,
            Info = model.Info,
            Address1 = model.Address1,
            Address2 = model.Address2,
            City = model.City,
            State = model.State,
            Zip = model.Zip,
            Phone = model.Phone,
            Email = model.Email,
            Comment = model.Comment,
            ContactDate = model.ContactDate,
            Status = model.Status
        };

        var returnModel = await db.HomeDb.FindOneAndReplaceAsync(filter, modelDB);

        return returnModel;
    }

    public async Task<bool> ContactUsDelete(HomeModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var db = new DbContext();

        var builder = Builders<HomeDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        var returnModel = await db.HomeDb.DeleteOneAsync(filter);

        return true;
    }

    public async Task<int> GetCount()
    {
        var db = new DbContext();
        var cursor = await db.HomeDb.FindAsync(new BsonDocument());

        IList<HomeDB> results = cursor.ToList();

        return results.Count;
    }

    public async Task<bool> DoesExist(HomeModel model)
    {
        var bReturn = false;
        var builder = Builders<HomeDB>.Filter;
        var filter = builder.Eq("_id", model._id);

        var db = new DbContext();

        var cursor = await db.HomeDb.FindAsync(filter);
        IList<HomeDB> results = cursor.ToList();
        if (results.Count > 0)
            bReturn = true;

        return bReturn;
    }
}