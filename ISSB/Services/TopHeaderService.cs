using System;
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
    public class TopHeaderService
    {
        public async Task<List<TopHeaderModel>> GetHeaders()
        {
            var db = DBContext.Instance;
            var cursor = await db.TopHeaderDB.FindAsync(new BsonDocument());
            IList<TopHeaderDB> results = cursor.ToList();
            var modelList = new List<TopHeaderModel>();
            foreach (var Item in results)
            {
                var model = new TopHeaderModel
                {
                    _id = Item._id.ToString(),
                    IsActive = Item.IsActive,
                    IsFooter = Item.IsFooter,
                    Title = Item.Title,
                    Description = Item.Description,
                    HTML = Item.HTML
                };
                modelList.Add(model);
            }
            return modelList.OrderBy(x => x._id).ToList();
        }

        public async Task<TopHeaderModel> GetHeaderById(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var builder = Builders<TopHeaderDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = DBContext.Instance;
            var cursor = await db.TopHeaderDB.FindAsync(filter);
            IList<TopHeaderDB> results = cursor.ToList();
            var Item = results[0];

            var model = new TopHeaderModel
            {
                _id = Item._id.ToString(),
                IsActive = Item.IsActive,
                IsFooter = Item.IsFooter,
                Title = Item.Title,
                Description = Item.Description,
                HTML = Item.HTML

            };
            return model;
        }

        public async Task<bool> AddHeader(TopHeaderModel model)
        {
            //Do String replace here

            bool bReturn = false;
            var db = DBContext.Instance;
            var modelDB = new TopHeaderDB
            {
                IsActive = model.IsActive,
                IsFooter = model.IsFooter,
                Title = model.Title,
                Description = model.Description,
                HTML = model.HTML

            };
            await db.TopHeaderDB.InsertOneAsync(modelDB);
            bReturn = true;
            return bReturn;
        }

        public async Task<bool> UpdateHeader(TopHeaderModel model)
        {
            bool bReturn = false;

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var filter = Builders<TopHeaderDB>.Filter.Eq(x => x._id, RecordId);
            var db = DBContext.Instance;
            var cursor = await db.TopHeaderDB.FindAsync(filter);
            IList<TopHeaderDB> results = cursor.ToList();
            var modelDB = new TopHeaderDB
            {
                _id = results[0]._id,
                IsActive = model.IsActive,
                IsFooter = model.IsFooter,
                Title = model.Title,
                Description = model.Description,
                HTML = model.HTML

            };

            await db.TopHeaderDB.FindOneAndReplaceAsync(filter, modelDB);
            bReturn = true;
            return bReturn;
        }

        public async Task<bool> Delete(TopHeaderModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = DBContext.Instance;


            var builder = Builders<TopHeaderDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            await db.TopHeaderDB.DeleteOneAsync(filter);

            return true;
        }
    }
}