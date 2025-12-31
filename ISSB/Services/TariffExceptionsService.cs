using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
    public class TariffExceptionsService
    {
        public async Task<List<TariffExceptionsModel>> GetTariffs()
        {
            TariffExceptionsModel model;

            var db = new DBContext();
            var cursor = await db.TariffExceptionsDB.FindAsync(new BsonDocument());

            IList<TariffExceptionsDB> results = cursor.ToList();
            var modelList = new List<TariffExceptionsModel>();

            foreach (var item in results)
            {
                model = new TariffExceptionsModel
                {
                    _id = item._id.ToString(),
                    TARIFF_ID = item.TARIFF_ID,
                    WTO_CODE = item.WTO_CODE,
                    WTO_CODE_LEGEND = item.WTO_CODE_LEGEND,
                    WTO_ALLOY_CODE = item.WTO_ALLOY_CODE,
                    WTO_ALLOY_LEGEND = item.WTO_ALLOY_LEGEND,
                    ISSB_STORED_CODE = item.ISSB_STORED_CODE,
                    ISSB_STORED_LEGEND = item.ISSB_STORED_LEGEND,
                    HARMONISED_TARIFF_CODE = item.HARMONISED_TARIFF_CODE,
                    HARMONISED_TARIFF_SHORT_LEGEND_BACKUP = item.HARMONISED_TARIFF_SHORT_LEGEND_BACKUP,
                    HARMONISED_TARIFF_LONG_LEGEND = item.HARMONISED_TARIFF_LONG_LEGEND,
                    SOURCE_COUNTRY_TARIFF_CODE = item.SOURCE_COUNTRY_TARIFF_CODE,
                    SOURCE_COUNTRY_TARIFF_SHORT_LEGEND = item.SOURCE_COUNTRY_TARIFF_SHORT_LEGEND,
                    SOURCE_COUNTRY_TARIFF_LONG_LEGEND = item.SOURCE_COUNTRY_TARIFF_LONG_LEGEND,
                    LOWER_VPT = item.LOWER_VPT,
                    UPPER_VPT = item.UPPER_VPT,
                    START_DATE = (DateTime)item.START_DATE,
                    DISCONTINUED_DATE = (DateTime)item.DISCONTINUED_DATE,
                    SIDE_OF_TRADE = item.SIDE_OF_TRADE,
                    TARIFF_CODE_TABLE_CODE = item.TARIFF_CODE_TABLE_CODE,
                    HARMONISED_TARIFF_SHORT_LEGEND = item.HARMONISED_TARIFF_SHORT_LEGEND
                };

                modelList.Add(model);
            }

            return modelList;
        }

        public async Task<bool> Add(TariffExceptionsModel model)
        {
            var db = new DBContext();

            var modelDB = new TariffExceptionsDB
            {
                TARIFF_ID = model.TARIFF_ID,
                WTO_CODE = model.WTO_CODE,
                WTO_CODE_LEGEND = model.WTO_CODE_LEGEND,
                WTO_ALLOY_CODE = model.WTO_ALLOY_CODE,
                WTO_ALLOY_LEGEND = model.WTO_ALLOY_LEGEND,
                ISSB_STORED_CODE = model.ISSB_STORED_CODE,
                ISSB_STORED_LEGEND = model.ISSB_STORED_LEGEND,
                HARMONISED_TARIFF_CODE = model.HARMONISED_TARIFF_CODE,
                HARMONISED_TARIFF_SHORT_LEGEND_BACKUP = model.HARMONISED_TARIFF_SHORT_LEGEND_BACKUP,
                HARMONISED_TARIFF_LONG_LEGEND = model.HARMONISED_TARIFF_LONG_LEGEND,
                SOURCE_COUNTRY_TARIFF_CODE = model.SOURCE_COUNTRY_TARIFF_CODE,
                SOURCE_COUNTRY_TARIFF_SHORT_LEGEND = model.SOURCE_COUNTRY_TARIFF_SHORT_LEGEND,
                SOURCE_COUNTRY_TARIFF_LONG_LEGEND = model.SOURCE_COUNTRY_TARIFF_LONG_LEGEND,
                LOWER_VPT = model.LOWER_VPT,
                UPPER_VPT = model.UPPER_VPT,
                START_DATE = model.START_DATE,
                DISCONTINUED_DATE = model.DISCONTINUED_DATE,
                SIDE_OF_TRADE = model.SIDE_OF_TRADE,
                TARIFF_CODE_TABLE_CODE = model.TARIFF_CODE_TABLE_CODE,
                HARMONISED_TARIFF_SHORT_LEGEND = model.HARMONISED_TARIFF_SHORT_LEGEND

            };

            await db.TariffExceptionsDB.InsertOneAsync(modelDB);

            return true;
        }

        public async Task<TariffExceptionsDB> Save(TariffExceptionsModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<TariffExceptionsDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.TariffExceptionsDB.FindAsync(filter);
            IList<TariffExceptionsDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new TariffExceptionsDB
            {
                _id = Item._id,
                TARIFF_ID = model.TARIFF_ID,
                WTO_CODE = model.WTO_CODE,
                WTO_CODE_LEGEND = model.WTO_CODE_LEGEND,
                WTO_ALLOY_CODE = model.WTO_ALLOY_CODE,
                WTO_ALLOY_LEGEND = model.WTO_ALLOY_LEGEND,
                ISSB_STORED_CODE = model.ISSB_STORED_CODE,
                ISSB_STORED_LEGEND = model.ISSB_STORED_LEGEND,
                HARMONISED_TARIFF_CODE = model.HARMONISED_TARIFF_CODE,
                HARMONISED_TARIFF_SHORT_LEGEND_BACKUP = model.HARMONISED_TARIFF_SHORT_LEGEND_BACKUP,
                HARMONISED_TARIFF_LONG_LEGEND = model.HARMONISED_TARIFF_LONG_LEGEND,
                SOURCE_COUNTRY_TARIFF_CODE = model.SOURCE_COUNTRY_TARIFF_CODE,
                SOURCE_COUNTRY_TARIFF_SHORT_LEGEND = model.SOURCE_COUNTRY_TARIFF_SHORT_LEGEND,
                SOURCE_COUNTRY_TARIFF_LONG_LEGEND = model.SOURCE_COUNTRY_TARIFF_LONG_LEGEND,
                LOWER_VPT = model.LOWER_VPT,
                UPPER_VPT = model.UPPER_VPT,
                START_DATE = model.START_DATE,
                DISCONTINUED_DATE = model.DISCONTINUED_DATE,
                SIDE_OF_TRADE = model.SIDE_OF_TRADE,
                TARIFF_CODE_TABLE_CODE = model.TARIFF_CODE_TABLE_CODE,
                HARMONISED_TARIFF_SHORT_LEGEND = model.HARMONISED_TARIFF_SHORT_LEGEND

            };

            var returnModel = await db.TariffExceptionsDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;
        }

        public async Task<TariffExceptionsModel> GetTariffExceptionsByID(string _id)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<TariffExceptionsDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.TariffExceptionsDB.FindAsync(filter);

            IList<TariffExceptionsDB> results = cursor.ToList();

            var Item = results[0];

            var model = new TariffExceptionsModel
            {
                _id = Item._id.ToString(),
                TARIFF_ID = Item.TARIFF_ID,
                WTO_CODE = Item.WTO_CODE,
                WTO_CODE_LEGEND = Item.WTO_CODE_LEGEND,
                WTO_ALLOY_CODE = Item.WTO_ALLOY_CODE,
                WTO_ALLOY_LEGEND = Item.WTO_ALLOY_LEGEND,
                ISSB_STORED_CODE = Item.ISSB_STORED_CODE,
                ISSB_STORED_LEGEND = Item.ISSB_STORED_LEGEND,
                HARMONISED_TARIFF_CODE = Item.HARMONISED_TARIFF_CODE,
                HARMONISED_TARIFF_SHORT_LEGEND_BACKUP = Item.HARMONISED_TARIFF_SHORT_LEGEND_BACKUP,
                HARMONISED_TARIFF_LONG_LEGEND = Item.HARMONISED_TARIFF_LONG_LEGEND,
                SOURCE_COUNTRY_TARIFF_CODE = Item.SOURCE_COUNTRY_TARIFF_CODE,
                SOURCE_COUNTRY_TARIFF_SHORT_LEGEND = Item.SOURCE_COUNTRY_TARIFF_SHORT_LEGEND,
                SOURCE_COUNTRY_TARIFF_LONG_LEGEND = Item.SOURCE_COUNTRY_TARIFF_LONG_LEGEND,
                LOWER_VPT = Item.LOWER_VPT,
                UPPER_VPT = Item.UPPER_VPT,
                START_DATE = (DateTime)Item.START_DATE,
                DISCONTINUED_DATE = (DateTime)Item.DISCONTINUED_DATE,
                SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                TARIFF_CODE_TABLE_CODE = Item.TARIFF_CODE_TABLE_CODE,
                HARMONISED_TARIFF_SHORT_LEGEND = Item.HARMONISED_TARIFF_SHORT_LEGEND

            };

            return model;

        }

        public async Task<bool> Delete(TariffExceptionsModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<TariffExceptionsDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.TariffExceptionsDB.DeleteOneAsync(filter);

            return true;
        }

        public async Task<int> GetCount()
        {

            var db = new DBContext();
            var cursor = await db.TariffExceptionsDB.FindAsync(new BsonDocument());

            IList<TariffExceptionsDB> results = cursor.ToList();

            return results.Count;
        }
    }
}
