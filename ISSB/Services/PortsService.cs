/*
    Services/PortsService.cs

    port service
*/

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
    public class PortsService
    {
        public async Task<List<PortsModel>> GetAllPorts()
        {
            var db = new DBContext();
            var cursor = await db.PortsDB.FindAsync(new BsonDocument());

            IList<PortsDB> results = cursor.ToList();
            var modelList = new List<PortsModel>();

            foreach (var Item in results)
            {
                var model = new PortsModel
                {
                    _id = Item._id.ToString(),
                    PortID = Item.PortID,
                    AlphaCode = Item.AlphaCode,
                    Name = Item.Name,
                    GeoCode = Item.GeoCode
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }      
        
        /// <summary>Return ports matching IDs.</summary>
        /// <param name="ports">port IDs</param>
        /// <returns>matching ports</returns>
        
        public async Task<List<PortsModel>> GetPorts( List<int> ports )
        {
            IList<PortsDB> results ;

            var db = new DBContext() ;

            var array = new BsonArray() ;
            foreach ( int i in ports ) array = array.Add( i ) ;

            var filter = new BsonDocument() ;
            filter.Add( "PortID" , new BsonDocument().Add( "$in" , array ) ) ;

            var modelList = new List<PortsModel>() ;

            using ( var cursor = await db.PortsDB.FindAsync( filter ) )
            {
                results = cursor.ToList() ;

                foreach ( var item in results ) modelList.Add( new PortsModel { PortID = item.PortID , Name = item.Name, GeoCode = item.GeoCode } ) ;
            }

            return modelList ;
        }

        public async Task<PortsModel> GetPortById(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<PortsDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.PortsDB.FindAsync(filter);

            IList<PortsDB> results = cursor.ToList();

            var Item = results[0];

            var model = new PortsModel
            {
                _id = Item._id.ToString(),
                PortID = Item.PortID,
                AlphaCode = Item.AlphaCode,
                Name = Item.Name,
                GeoCode = Item.GeoCode

            };

            return model;
        }

        public async Task<bool> Add(PortsModel model)
        {
            var db = new DBContext();

            var modelDB = new PortsDB
            {
                PortID = model.PortID,
                Name = model.Name,
                AlphaCode = model.AlphaCode,
                GeoCode = model.GeoCode
                 

            };

            await db.PortsDB.InsertOneAsync(modelDB);

            return true;
        }

        public async Task<PortsDB> Save(PortsModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<PortsDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.PortsDB.FindAsync(filter);
            IList<PortsDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new PortsDB
            {
                _id = Item._id,
                PortID = model.PortID,
                AlphaCode = model.AlphaCode,
                Name = model.Name,
                GeoCode = model.GeoCode
            };

            var returnModel = await db.PortsDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;

        }

        public async Task<bool> Delete(PortsModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<PortsDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.PortsDB.DeleteOneAsync(filter);

            return true;
        }

        public async Task<int> GetCount()
        {

            var db = new DBContext();
            var cursor = await db.PortsDB.FindAsync(new BsonDocument());

            IList<PortsDB> results = cursor.ToList();

            return results.Count;
        }

        public async Task<bool> DoesExist(PortsModel model)
        {
            bool bReturn = false;
            var builder = Builders<PortsDB>.Filter;
            var filter = builder.Eq("_id", model.PortID);

            var db = new DBContext();

            var cursor = await db.PortsDB.FindAsync(filter);
            IList<PortsDB> results = cursor.ToList();
            if (results.Count > 0)
                bReturn = true;

            return bReturn;
        }

        public async Task<PortsModel> GetPortByAlphaCode(string Code)
        {
            
            var builder = Builders<PortsDB>.Filter;

            var filter = builder.Eq("AlphaCode", Code);

            var db = new DBContext();
            var cursor = await db.PortsDB.FindAsync(filter);

            IList<PortsDB> results = cursor.ToList();

            if(results.Count.Equals(0))
            {
                var rModel = new PortsModel { PortID = 0 };
                return rModel;
            }


            var Item = results[0];

            var model = new PortsModel
            {
                _id = Item._id.ToString(),
                PortID = Item.PortID,
                AlphaCode = Item.AlphaCode,
                Name = Item.Name,
                GeoCode = Item.GeoCode

            };

            return model;
        }

        public async Task<List<PortsModel>> GetPortsByGeoCode(string GeoCode)
        {

            var builder = Builders<PortsDB>.Filter;

            var filter = builder.Eq("GeoCode", GeoCode);

            var db = new DBContext();
            var cursor = await db.PortsDB.FindAsync(filter);

            IList<PortsDB> results = cursor.ToList();
            var modelList = new List<PortsModel>();

            if (results.Count.Equals(0))
            {
                var rModel = new PortsModel { PortID = 0 };
                modelList.Add(rModel);
                return modelList;
            }
           
            foreach (var Item in results)
            {
                var model = new PortsModel
                {
                    _id = Item._id.ToString(),
                    AlphaCode = Item.AlphaCode,
                    GeoCode = Item.GeoCode,
                    Name = Item.Name,
                    PortID = Item.PortID
                };
                modelList.Add(model);
            }

            return modelList;
        }


        public async Task<bool> ConvertData()
        {
            var db = new DBContext();
            var Ports = await GetAllPorts();
            var PortsNewList = new List<PortsDB>();
            foreach(var Item in Ports)
            {
                BsonObjectId RecordId = new BsonObjectId(new ObjectId(Item._id));
                var newModel = new PortsDB
                {
                     _id = RecordId,
                     AlphaCode = string.Empty,
                     Name = Item.Name,
                     PortID = Item.PortID,
                     GeoCode = Item.GeoCode
                };
                PortsNewList.Add(newModel);
            }

            db._database.RenameCollection("PortsDB", "PortsOldDB");
            await db.PortsDB.InsertManyAsync(PortsNewList);
            return true;
        }
     }
}
