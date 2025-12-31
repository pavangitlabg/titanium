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
    public class CurrencyCodesService
    {
        public async Task<List<CurrencyCodesModel>> GetCurrencyCodes()
        {
            CurrencyCodesModel model;

            var db = new DBContext();
            var cursor = await db.CurrencyCodesDB.FindAsync(new BsonDocument());

            IList<CurrencyCodesDB> results = cursor.ToList();
            var modelList = new List<CurrencyCodesModel>();

            foreach (var item in results)
            {
                if(string.IsNullOrEmpty(item.CURRENCY_SYMBOL))
                    item.CURRENCY_SYMBOL = string.Empty;

                if (item.CURRENCY_SYMBOL.Equals("NULL"))
                    item.CURRENCY_SYMBOL = string.Empty;

                model = new CurrencyCodesModel
                {
                    _id = item._id.ToString(),
                    CURRENCY_CODE = item.CURRENCY_CODE,
                    CURRENCY_NAME = item.CURRENCY_NAME,
                    CURRENCY_SYMBOL = item.CURRENCY_SYMBOL
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x.CURRENCY_NAME).ToList();
        }


        public async Task<CurrencyCodesModel> GetCurrencyCodesByID(string _id)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<CurrencyCodesDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.CurrencyCodesDB.FindAsync(filter);

            IList<CurrencyCodesDB> results = cursor.ToList();

            var item = results[0];
            if (string.IsNullOrEmpty(item.CURRENCY_SYMBOL))
                item.CURRENCY_SYMBOL = string.Empty;

            if (item.CURRENCY_SYMBOL.Equals("NULL"))
                item.CURRENCY_SYMBOL = string.Empty;

            var model = new CurrencyCodesModel
            {
                _id = item._id.ToString(),
                CURRENCY_CODE = item.CURRENCY_CODE,
                CURRENCY_NAME = item.CURRENCY_NAME,
                CURRENCY_SYMBOL = item.CURRENCY_SYMBOL

            };

            return model;
        }

        public async Task<bool> Add(CurrencyCodesModel model)
        {
            var db = new DBContext();

            var modelDB = new CurrencyCodesDB
            {
                CURRENCY_CODE = model.CURRENCY_CODE,
                CURRENCY_NAME = model.CURRENCY_NAME,
                CURRENCY_SYMBOL = model.CURRENCY_SYMBOL

            };

            await db.CurrencyCodesDB.InsertOneAsync(modelDB);

            return true;
        }

        public async Task<CurrencyCodesDB> Save(CurrencyCodesModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<CurrencyCodesDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.CurrencyCodesDB.FindAsync(filter);
            IList<CurrencyCodesDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new CurrencyCodesDB
            {
                _id = Item._id,
                CURRENCY_CODE = model.CURRENCY_CODE,
                CURRENCY_NAME = model.CURRENCY_NAME,
                CURRENCY_SYMBOL = model.CURRENCY_SYMBOL
            };

            var returnModel = await db.CurrencyCodesDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;
        }

        public async Task<bool> Delete(CurrencyCodesModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<CurrencyCodesDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.CurrencyCodesDB.DeleteOneAsync(filter);

            return true;
        }

        public async Task<int> GetCount()
        {

            var db = new DBContext();
            var cursor = await db.CurrencyCodesDB.FindAsync(new BsonDocument());

            IList<CurrencyCodesDB> results = cursor.ToList();

            return results.Count;
        }
    }
}
