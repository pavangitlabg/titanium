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
    public class ErrorService
    {
        public async Task<List<ErrorModel>> GetErrors()
        {
            ErrorModel model;

            var db = new DBContext();
            var cursor = await db.ErrorDB.FindAsync(new BsonDocument());

            IList<ErrorDB> results = cursor.ToList();
            var modelList = new List<ErrorModel>();

            foreach (var item in results)
            {
                model = new ErrorModel
                {
                    _id = item._id.ToString(),
                    Date = item.Date,
                    Code = item.Code,
                    Class = item.Class,
                    ErrorMessage = item.ErrorMessage

                };

                modelList.Add(model);
            }

            return modelList.OrderByDescending(x => x._id).ToList();
        }

        public async Task<bool> UpdateError(ErrorModel model)
        {
            bool bReturn = false;

            
     
            var db = new DBContext();

            //var cursor = await db.TradeFactDB.FindAsync(filter);
            //IList<TradeFactDB> results = cursor.ToList();

            var modelDB = new ErrorDB
            {
                 Date = DateTime.Now,
                 Code = model.Code,
                 Class = model.Class,
                 ErrorMessage = model.ErrorMessage,
                 
            };

            await db.ErrorDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;


        }

        public async Task<ErrorModel> GetErrorByID(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<ErrorDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.ErrorDB.FindAsync(filter);

            IList<ErrorDB> results = cursor.ToList();

            var Item = results[0];

            var model = new ErrorModel
            {
                _id = Item._id.ToString(),              
                Date = Item.Date,
                Code = Item.Code,
                Class = Item.Class,
                ErrorMessage = Item.ErrorMessage

            };

            return model;
        }

        public async Task<bool> Delete(ErrorModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<ErrorDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.ErrorDB.DeleteOneAsync(filter);

            return true;
        }
    }
}
