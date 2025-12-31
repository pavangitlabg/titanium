using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;


namespace Services
{
    public class AssetService
    {       
        public async Task<List<AssetModel>> GetAssets()
        {
            var db = new DBContext();
            var cursor = await db.AssetDB.FindAsync(new BsonDocument());

            IList<AssetDB> results = cursor.ToList();
            var modelList = new List<AssetModel>();

            foreach (var Item in results)
            {

                var model = new AssetModel
                {
                    _id = Item._id.ToString(),
                    ID = Item.ID,
                    CHECKED_BY = Item.CHECKED_BY,
                    DATE_CHECKED = (DateTime)Item.DATE_CHECKED,
                    DESCRIPTION = Item.DESCRIPTION,                    
                    LAST_MODIFIED = (DateTime)Item.LAST_MODIFIED,
                    LOCATION = Item.LOCATION,
                    MANUFACTURER = Item.MANUFACTURER,
                    MEMO = Item.MEMO,
                    MODEL_TYPE = Item.MODEL,
                    PASSWORD = Item.PASSWORD,
                    PURCHASE_DATE = (DateTime)Item.PURCHASE_DATE,
                    SERIAL_NO = Item.SERIAL_NO,
                    USERNAME = Item.USERNAME,
                    VALUE = Item.VALUE
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }


        public async Task<AssetModel> GetAssetByID(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var builder = Builders<AssetDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.AssetDB.FindAsync(filter);

            IList<AssetDB> results = cursor.ToList();

            var Item = results[0];

            var model = new AssetModel
            {
                _id = Item._id.ToString(),
                ID = Item.ID,
                CHECKED_BY = Item.CHECKED_BY,
                DATE_CHECKED = (DateTime)Item.DATE_CHECKED,
                DESCRIPTION = Item.DESCRIPTION,
                LAST_MODIFIED = (DateTime)Item.LAST_MODIFIED,
                LOCATION = Item.LOCATION,
                MANUFACTURER = Item.MANUFACTURER,
                MEMO = Item.MEMO,
                MODEL_TYPE = Item.MODEL,
                PASSWORD = Item.PASSWORD,
                PURCHASE_DATE = (DateTime)Item.PURCHASE_DATE,
                SERIAL_NO = Item.SERIAL_NO,
                USERNAME = Item.USERNAME,
                VALUE = Item.VALUE

            };

            return model;
        }

        public async Task<bool> Add(AssetModel model)
        {
            var db = new DBContext();

            var modelDB = new AssetDB
            {
                ID = model.ID,
                CHECKED_BY = model.CHECKED_BY,
                DATE_CHECKED = (DateTime)model.DATE_CHECKED,
                DESCRIPTION = model.DESCRIPTION,                
                LAST_MODIFIED = (DateTime)model.LAST_MODIFIED,
                LOCATION = model.LOCATION,
                MANUFACTURER = model.MANUFACTURER,
                MEMO = model.MEMO,
                MODEL = model.MODEL_TYPE,
                PASSWORD = model.PASSWORD,
                PURCHASE_DATE = (DateTime)model.PURCHASE_DATE,
                SERIAL_NO = model.SERIAL_NO,
                USERNAME = model.USERNAME,
                VALUE = model.VALUE
                
            };

            await db.AssetDB.InsertOneAsync(modelDB);

            return true;
        }

        public async Task<bool> DoesExist(AssetModel model)
        {
            bool bReturn = false;
            var builder = Builders<AssetDB>.Filter;
            var filter = builder.Eq("_ID", model._id);

            var db = new DBContext();

            var cursor = await db.AssetDB.FindAsync(filter);
            IList<AssetDB> results = cursor.ToList();
            if (results.Count > 0)
                bReturn = true;

            return bReturn;
        }

        public async Task<AssetDB> SaveAsset(AssetModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<AssetDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.AssetDB.FindAsync(filter);
            IList<AssetDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new AssetDB
            {
                _id = Item._id,                
                CHECKED_BY = model.CHECKED_BY,
                DATE_CHECKED = (DateTime)model.DATE_CHECKED,
                DESCRIPTION = model.DESCRIPTION,
                ID = model.ID,
                LAST_MODIFIED = (DateTime)model.LAST_MODIFIED,
                LOCATION = model.LOCATION,
                MANUFACTURER = model.MANUFACTURER,
                MEMO = model.MEMO,
                MODEL = model.MODEL_TYPE,
                PASSWORD = model.PASSWORD,
                PURCHASE_DATE = (DateTime)model.PURCHASE_DATE,
                SERIAL_NO = model.SERIAL_NO,
                USERNAME = model.USERNAME,
                VALUE = model.VALUE
            };

            var returnModel = await db.AssetDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;
        }

        public async Task<bool> Delete(AssetModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<AssetDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.AssetDB.DeleteOneAsync(filter);

            return true;
        }

        public async Task<int> GetCount()
        {
         
            var db = new DBContext();
            var cursor = await db.AssetDB.FindAsync(new BsonDocument());

            IList<AssetDB> results = cursor.ToList();

            return results.Count;
        }
    }
}
