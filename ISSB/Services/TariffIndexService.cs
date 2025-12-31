using System.Collections.Generic ;
using System.Linq;
using System.Threading.Tasks ;
using Data ;
using Data.DBModels ;
using Data.Models ;
using MongoDB.Bson ;
using MongoDB.Driver ;

namespace Services
{
    public class TariffIndexService
    {
  
        public async Task<List<TariffIndexModel>> GetTariffIndex()
        {
            TariffIndexModel model ;       

            var db = new DBContext();
       
            var cursor = await db.TariffIndexDB.FindAsync(new BsonDocument());

            IList<TariffIndexDB> results = cursor.ToList();
            var modelList = new List<TariffIndexModel>();

            foreach (var item in results)
            {
                model = new TariffIndexModel
                {
                     _id = item._id.ToString(),
                     Name = item.Name,
                     Code = item.Code,
                     Description = item.Description,
                     Active = item.Active                   
                } ;

                modelList.Add( model ) ;
            }

            return modelList ;
        }      
       

        public async Task<TariffIndexModel> GetTariffIndexByID(string _id)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<TariffIndexDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.TariffIndexDB.FindAsync(filter);

            IList<TariffIndexDB> results = cursor.ToList();

            var item = results[0];

            var model = new TariffIndexModel
            {
                _id = item._id.ToString(),
                Name = item.Name,
                Code = item.Code,
                Description = item.Description,
                Active = item.Active

            };

            return model;
        }

        public async Task<bool> Add(TariffIndexModel model)
        {
            var db = new DBContext();

            var modelDB = new TariffIndexDB
            {
                Name = model.Name,
                Code = model.Code,
                Description = model.Description,
                Active = model.Active

            };

            await db.TariffIndexDB.InsertOneAsync(modelDB);

            return true;
        }

        public async Task<TariffIndexDB> Save(TariffIndexModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<TariffIndexDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.TariffIndexDB.FindAsync(filter);
            IList<TariffIndexDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new TariffIndexDB
            {
                _id = Item._id,
                Name = model.Name,
                Code = model.Code,
                Description = model.Description,
                Active = model.Active

            };

            var returnModel = await db.TariffIndexDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;
        }

        public async Task<bool> Delete(TariffIndexModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<TariffIndexDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.TariffIndexDB.DeleteOneAsync(filter);

            return true;
        }

        public async Task<int> GetCount()
        {

            var db = new DBContext();
            var cursor = await db.TariffIndexDB.FindAsync(new BsonDocument());

            IList<TariffIndexDB> results = cursor.ToList();

            return results.Count;
        }

    }
}