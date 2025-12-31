using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
    public class SiteVisitorService
    {
        public async Task<List<SiteVisitorModel>> GetVisitors()
        {
            SiteVisitorModel model;

            var db = new DBContext();
            var cursor = await db.SiteVisitorDB.FindAsync(new BsonDocument());

            IList<SiteVisitorDB> results = cursor.ToList();
            var modelList = new List<SiteVisitorModel>();

            foreach (var item in results)
            {
                model = new SiteVisitorModel
                {
                    _id = item._id.ToString(),
                    @as = item.@as,
                    city = item.city,
                    country = item.country,
                    countryCode = item.countryCode,
                    ipAddress = item.ipAddress,
                    isp = item.isp,
                    lat = item.lat,
                    lon = item.lon,
                    org = item.org,
                    query = item.query,
                    region = item.region,
                    regionName = item.regionName,
                    status = item.status,
                    timezone = item.timezone,
                    zip = item.zip,
                    Date = item.Date

                };

                modelList.Add(model);
            }

            return modelList.OrderByDescending(x => x._id).ToList();
        }

        public async Task<List<SiteVisitorModel>> GetVisitorsGrouped()
        {
            SiteVisitorModel model;

            var db = new DBContext();
            var cursor = await db.SiteVisitorDB.FindAsync(new BsonDocument());

            IList<SiteVisitorDB> results = cursor.ToList();
            var modelList = new List<SiteVisitorModel>();

            foreach (var item in results)
            {
                model = new SiteVisitorModel
                {
                    _id = item._id.ToString(),
                    @as = item.@as,
                    city = item.city,
                    country = item.country,
                    countryCode = item.countryCode,
                    ipAddress = item.ipAddress,
                    isp = item.isp,
                    lat = item.lat,
                    lon = item.lon,
                    org = item.org,
                    query = item.query,
                    region = item.region,
                    regionName = item.regionName,
                    status = item.status,
                    timezone = item.timezone,
                    zip = item.zip,
                    Date = item.Date

                };

                modelList.Add(model);
            }

            var groupedIPList = modelList.GroupBy(u => u.ipAddress).ToList();

            var groupedList = new List<SiteVisitorModel>();

            foreach(var Item in groupedIPList)
            {
                var SortedList = Item.OrderByDescending(x => x.Date).ToList();
                var modelItem = new SiteVisitorModel();
                int Visits = SortedList.Count;
                modelItem = SortedList.FirstOrDefault();
                modelItem.Count = Visits;
                groupedList.Add(modelItem);
            }

            return groupedList.OrderByDescending(x => x._id).ToList();
        }

        //public async Task<List<SiteVisitorModel>> GetDistinctVisitors()
        //{

        //    var modelList = new List<SiteVisitorModel>();
        //    try
        //    {
        //        var db = new DBContext();
        //        IMongoCollection<BsonDocument> collection = db._database.GetCollection<BsonDocument>("SiteVisitorDB");

        //        var options = new AggregateOptions()
        //        {
        //            AllowDiskUse = true
        //        };

        //        PipelineDefinition<BsonDocument, BsonDocument> pipeline = new BsonDocument[]
        //        {
        //        new BsonDocument("$sort", new BsonDocument()
        //                .Add("Date", -1.0)),
        //        new BsonDocument("$group", new BsonDocument()
        //                .Add("_id", new BsonDocument()
        //                        .Add("status", "$status")
        //                        .Add("country", "$country")
        //                        .Add("countryCode", "$countryCode")
        //                        .Add("region", "$region")
        //                        .Add("regionName", "$regionName")
        //                        .Add("city", "$city")
        //                        .Add("zip", "$zip")
        //                        .Add("lat", "$lat")
        //                        .Add("lon", "$lon")
        //                        .Add("timezone", "$timezone")
        //                        .Add("isp", "$isp")
        //                        .Add("org", "$org")
        //                        .Add("as", "$as")
        //                        .Add("query", "$query")
        //                        .Add("ipAddress", "$ipAddress")
        //                )),
        //        new BsonDocument("$project", new BsonDocument()
        //                .Add("country", -1.0)
        //                .Add("countryCode", -1.0)
        //                .Add("region", -1.0)
        //                .Add("regionName", -1.0)
        //                .Add("city", -1.0)
        //                .Add("zip", -1.0)
        //                .Add("lat", -1.0)
        //                .Add("lon", -1.0)
        //                .Add("timezone", -1.0)
        //                .Add("isp", -1.0)
        //                .Add("org", -1.0)
        //                .Add("as", -1.0)
        //                .Add("query", -1.0)
        //                .Add("ipAddress", -1.0))
        //        };

        //        using (var cursor = await collection.AggregateAsync(pipeline, options))
        //        {
        //            while (await cursor.MoveNextAsync())
        //            {
        //                var batch = cursor.Current;
        //                foreach (BsonDocument document in batch)
        //                {
        //                    SiteVisitorDB item; // collection item
        //                    SiteVisitorDB.Id id; // grouping identifier

        //                    item = BsonSerializer.Deserialize<SiteVisitorDB>(document);
        //                    id = BsonSerializer.Deserialize<SiteVisitorDB.Id>(item._id);
        //                    var model = new SiteVisitorModel
        //                    {
        //                        _id = item._id.ToString(),
        //                        @as = id.@as,
        //                        city = id.city,
        //                        country = id.country,
        //                        countryCode = id.countryCode,
        //                        ipAddress = id.ipAddress,
        //                        isp = id.isp,
        //                        lat = id.lat,
        //                        lon = id.lon,
        //                        org = id.org,
        //                        query = id.query,
        //                        region = id.region,
        //                        regionName = id.regionName,
        //                        status = id.status,
        //                        timezone = id.timezone,
        //                        zip = id.zip,
        //                        Date = DateTime.Now
        //                    };
        //                    modelList.Add(model);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        var eSrv = new ErrorService();
        //        var eModel = new ErrorModel { Code = "988", Class = "SiteVisitorService Line 124", ErrorMessage = e.Message };
        //        await eSrv.UpdateError(eModel);
        //    }

        //    return modelList;

        //}


        public async Task<bool> Add(SiteVisitorModel model)
        {
           
            var db = new DBContext();

            var modelDB = new SiteVisitorDB
            {
              //  _id = RecordId,
                @as = model.@as,
                city = model.city,
                country = model.country,
                countryCode = model.countryCode,
                ipAddress = model.ipAddress,
                isp = model.isp,
                lat = model.lat,
                lon = model.lon,
                org = model.org,
                query = model.query,
                region = model.region,
                regionName = model.regionName,
                status = model.status,
                timezone = model.timezone,
                zip = model.zip,
                Date = model.Date
            };

            await db.SiteVisitorDB.InsertOneAsync(modelDB);

            return true;
        }

        public async Task<SiteVisitorModel> GetVisitorById(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<SiteVisitorDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.SiteVisitorDB.FindAsync(filter);

            IList<SiteVisitorDB> results = cursor.ToList();

            var Item = results[0];

            var model = new SiteVisitorModel
            {
                _id = Item._id.ToString(),
                @as = Item.@as,
                city = Item.city,
                country = Item.country,
                countryCode = Item.countryCode,
                ipAddress = Item.ipAddress,
                isp = Item.isp,
                lat = Item.lat,
                lon = Item.lon,
                org = Item.org,
                query = Item.query,
                region = Item.region,
                regionName = Item.regionName,
                status = Item.status,
                timezone = Item.timezone,
                zip = Item.zip,
                Date = Item.Date

            };

            return model;
        }

        public async Task<bool> Delete(SiteVisitorModel model)
        {
            
            var db = new DBContext();

            var builder = Builders<SiteVisitorDB>.Filter;
            var filter = builder.Eq("ipAddress", model.ipAddress);

            await db.SiteVisitorDB.DeleteManyAsync(filter);

            return true;
        }
    }
}
