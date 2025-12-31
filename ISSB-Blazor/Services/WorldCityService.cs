using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class WorldCityService
{
    public async Task<List<WorldCityModel>> GetWorldCities()
    {
        var db = new DbContext();
        var cursor = await db.WorldCityDb.FindAsync(new BsonDocument());

        IList<WorldCityDB> results = cursor.ToList();
        var modelList = new List<WorldCityModel>();

        foreach (var Item in results)
        {
            var model = new WorldCityModel
            {
                Idx = Item.Idx,
                City = Item.City,
                CityAscii = Item.CityAscii,
                Country = Item.Country,
                GEO = Item.GEO,
                ISO2 = Item.ISO2,
                ISO3 = Item.ISO3,
                LAT = Item.LAT,
                LNG = Item.LNG,
                Population = Item.Population,
                Province = Item.Province
            };

            modelList.Add(model);
        }

        return modelList;
    }

    public async Task<List<WorldCityModel>> GetWorldCapitalCities()
    {
        var db = new DbContext();
        var cursor = await db.WorldCityDb.FindAsync(new BsonDocument());

        IList<WorldCityDB> results = cursor.ToList();
        var modelList = new List<WorldCityModel>();

        foreach (var Item in results)
        {
            var model = new WorldCityModel
            {
                Idx = Item.Idx,
                City = Item.City,
                CityAscii = Item.CityAscii,
                Country = Item.Country,
                GEO = Item.GEO,
                ISO2 = Item.ISO2,
                ISO3 = Item.ISO3,
                LAT = Item.LAT,
                LNG = Item.LNG,
                Population = Item.Population,
                Province = Item.Province
            };

            modelList.Add(model);
        }

        var GroupMapList = from element in modelList
            group element by element.GEO
            into groups
            select groups.OrderByDescending(p => p.Population).First();

        //var GroupMapList = modelList.GroupBy(x => x.GEO)
        //                  .Select(group => new { GeoCode = group.Key, Population = group.Max(prod => prod.Population) })
        //                  .ToList();

        //var modelCapitals = new List<WorldCityModel>();
        //foreach (var Geo in GroupMapList)
        //{
        //    var mCapital = modelList.FirstOrDefault(x => x.GEO.Equals(Geo.GeoCode));
        //    modelCapitals.Add(mCapital);
        //}
        return GroupMapList.ToList();
    }

    public async Task<bool> SaveGEO(WorldCityModel model)
    {
        var bReturn = false;
        var builder = Builders<WorldCityDB>.Filter;
        var filter = builder.Eq("Idx", model.Idx);
        var db = new DbContext();

        var cursor = await db.WorldCityDb.FindAsync(filter);
        IList<WorldCityDB> results = cursor.ToList();


        var modelDB = new WorldCityDB
        {
            _id = results[0]._id,
            Idx = model.Idx,
            City = model.City,
            Province = model.Province,
            Population = model.Population,
            LAT = model.LAT,
            LNG = model.LNG,
            CityAscii = model.CityAscii,
            Country = model.Country,
            GEO = model.GEO,
            ISO2 = model.ISO2,
            ISO3 = model.ISO3
        };

        await db.WorldCityDb.FindOneAndReplaceAsync(filter, modelDB);
        bReturn = true;


        return bReturn;
    }

    public async Task<WorldCityModel> GetWorldCityByGeo(string geoCode)
    {
        var builder = Builders<WorldCityDB>.Filter;
        var filter = builder.Eq("GEO", geoCode);

        var db = new DbContext();
        var cursor = await db.WorldCityDb.FindAsync(filter);

        IList<WorldCityDB> results = cursor.ToList();

        var Item = results[0];
        var model = new WorldCityModel
        {
            Idx = Item.Idx,
            City = Item.City,
            Province = Item.Province,
            Population = Item.Population,
            LAT = Item.LAT,
            LNG = Item.LNG,
            CityAscii = Item.CityAscii,
            Country = Item.Country,
            GEO = Item.GEO,
            ISO2 = Item.ISO2,
            ISO3 = Item.ISO3
        };

        return model;
    }
}