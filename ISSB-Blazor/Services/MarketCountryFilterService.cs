using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class MarketCountryFilterService
{
    public async Task<List<MarketCountryFilterModel>> GetFilters()
    {
        var db = new DbContext();
        var cursor = await db.MarketCountryFilterDb.FindAsync(new BsonDocument());

        IList<MarketCountryFilterDB> results = cursor.ToList();
        var modelList = new List<MarketCountryFilterModel>();

        foreach (var Item in results)
        {
            var Items = new List<MarketCountryFilterListModel>();

            foreach (var i in Item.Items)
            {
                var lModel = new MarketCountryFilterListModel
                {
                    SortOrder = i.SortOrder,
                    GeoCode = i.GeoCode,
                    CountryName = i.CountryName
                };
                Items.Add(lModel);
            }

            var model = new MarketCountryFilterModel
            {
                _id = Item._id.ToString(),

                Active = Item.Active,
                CanEdit = Item.CanEdit,
                Notes = Item.Notes,
                Name = Item.Name,
                SortOrder = Item.SortOrder
            };
            model.Items = Items;
            modelList.Add(model);
        }

        return modelList.OrderBy(x => x.Name).ToList();
    }

    public async Task<MarketCountryFilterModel> GetFilterById(string objId)
    {
        var RecordId = new BsonObjectId(new ObjectId(objId));
        var builder = Builders<MarketCountryFilterDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        var db = new DbContext();
        var cursor = await db.MarketCountryFilterDb.FindAsync(filter);
        IList<MarketCountryFilterDB> results = cursor.ToList();

        if (results.Count == 0) return new MarketCountryFilterModel();

        var Item = results[0];

        var model = new MarketCountryFilterModel
        {
            _id = Item._id.ToString(),

            Active = Item.Active,
            CanEdit = Item.CanEdit,
            Notes = Item.Notes,
            Name = Item.Name,
            SortOrder = Item.SortOrder
        };

        var Items = new List<MarketCountryFilterListModel>();

        foreach (var m in Item.Items)
        {
            var lModel = new MarketCountryFilterListModel
            {
                SortOrder = m.SortOrder,
                GeoCode = m.GeoCode,
                CountryName = m.CountryName
            };
            Items.Add(lModel);
        }

        model.Items = Items;
        return model;
    }


    public async Task<MarketCountryFilterModel> GetFilterByName(string Name)
    {
        var builder = Builders<MarketCountryFilterDB>.Filter;
        var filter = builder.Eq("Name", Name);

        var db = new DbContext();
        var cursor = await db.MarketCountryFilterDb.FindAsync(filter);
        IList<MarketCountryFilterDB> results = cursor.ToList();

        if (results.Count == 0) return new MarketCountryFilterModel();

        var Item = results[0];

        var model = new MarketCountryFilterModel
        {
            _id = Item._id.ToString(),

            Active = Item.Active,
            CanEdit = Item.CanEdit,
            Notes = Item.Notes,
            Name = Item.Name,
            SortOrder = Item.SortOrder
        };

        var Items = new List<MarketCountryFilterListModel>();

        foreach (var m in Item.Items)
        {
            var lModel = new MarketCountryFilterListModel
            {
                SortOrder = m.SortOrder,
                GeoCode = m.GeoCode,
                CountryName = m.CountryName
            };
            Items.Add(lModel);
        }

        model.Items = Items;
        return model;
    }

    public async Task AddFilter(MarketCountryFilterModel model)
    {
        var db = new DbContext();

        var newModel = new MarketCountryFilterDB
        {
            Active = model.Active,
            CanEdit = model.CanEdit,
            Name = model.Name,
            Notes = model.Notes,
            SortOrder = 1
        };

        var Items = new List<MarketCountryFilterListDB>();

        foreach (var Item in model.Items)
        {
            var lModel = new MarketCountryFilterListDB
                { GeoCode = Item.GeoCode, CountryName = Item.CountryName, SortOrder = Item.SortOrder };
            Items.Add(lModel);
        }

        newModel.Items = Items;

        await db.MarketCountryFilterDb.InsertOneAsync(newModel);
    }

    public async Task SaveFilter(MarketCountryFilterModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var db = new DbContext();

        var newModel = new MarketCountryFilterDB
        {
            _id = RecordId,

            Active = model.Active,
            CanEdit = model.CanEdit,
            Name = model.Name,
            Notes = model.Notes,
            SortOrder = 1
        };

        var Items = new List<MarketCountryFilterListDB>();

        foreach (var Item in model.Items)
        {
            var lModel = new MarketCountryFilterListDB
                { GeoCode = Item.GeoCode, CountryName = Item.CountryName, SortOrder = Item.SortOrder };
            Items.Add(lModel);
        }

        newModel.Items = Items;

        var builder = Builders<MarketCountryFilterDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        await db.MarketCountryFilterDb.FindOneAndReplaceAsync(filter, newModel);
    }

    public async Task<bool> Delete(string objId)
    {
        var RecordId = new BsonObjectId(new ObjectId(objId));
        var bReturn = false;
        var builder = Builders<MarketCountryFilterDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        var db = new DbContext();
        await db.MarketCountryFilterDb.DeleteOneAsync(filter);
        return bReturn;
    }
}