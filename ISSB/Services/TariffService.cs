using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Services
{
    public class TariffService
    {
        /// <summary>Return all tariffs.</summary>
        /// <returns>tariffs</returns>

        public async Task<List<TariffModel>> GetTariffs()
        {
            TariffModel model;

            //var builder = Builders<TariffDB>.Filter;
            //var filter = builder.Eq("TARIFF_ID", "0");

            var db = new DBContext();
            //var cursor = await db.TariffDB.FindAsync(filter);

            //var db = new DBContext();
            var cursor = await db.TariffDB.FindAsync(new BsonDocument());

            IList<TariffDB> results = cursor.ToList();
            var modelList = new List<TariffModel>();

            foreach (var item in results)
            {
                model = new TariffModel
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

      
        public async Task<List<TariffModel>> GetTariffsByRegion(string RegionCode)
        {
            TariffModel model;

            var builder = Builders<TariffDB>.Filter;
            var filter = builder.Eq("TARIFF_CODE_TABLE_CODE", RegionCode);

            var db = new DBContext();
            //var cursor = await db.TariffDB.FindAsync(filter);

            //var db = new DBContext();
            var cursor = await db.TariffDB.FindAsync(filter);

            IList<TariffDB> results = cursor.ToList();
            var modelList = new List<TariffModel>();

            foreach (var item in results)
            {
                model = new TariffModel
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

            return modelList.OrderBy(x => x.HARMONISED_TARIFF_CODE).ToList();
        }

        public async Task<List<TariffSearchModel>> GetLookupTariffsByRegion(string RegionCode)
        {

            var builder = Builders<TariffDB>.Filter;
            var filter = builder.Eq("TARIFF_CODE_TABLE_CODE", RegionCode);
            var db = new DBContext();
            var cursor = await db.TariffDB.FindAsync(filter);

            IList<TariffDB> results = cursor.ToList();

            var ReturnList = new List<TariffSearchModel>();
            
            foreach (var Item in results)
            {
                var tModel = new TariffSearchModel
                {
                    ID = Item.TARIFF_ID,
                    LongTariffCode = Item.SOURCE_COUNTRY_TARIFF_CODE ?? string.Empty,
                    ShortTariffCode = Item.HARMONISED_TARIFF_CODE ?? string.Empty,
                    SideOfTrade = Item.SIDE_OF_TRADE ?? string.Empty,
                    ReigionCode = Item.TARIFF_CODE_TABLE_CODE ?? string.Empty
                };
                ReturnList.Add(tModel);
            }

            return ReturnList.OrderBy(x => x.ID).ToList(); ;


        }

        public async Task<List<string>> GetTariffCodeList()
        {

            var db = new DBContext();
            var cursor = await db.TariffDB.FindAsync(new BsonDocument());

            IList<TariffDB> results = cursor.ToList();
            var modelList = new List<string>();

            foreach (var item in results)
            {

                modelList.Add(item.SOURCE_COUNTRY_TARIFF_CODE);
            }

            return modelList;
        }

        /// <summary>Return tariffs matching products.</summary>
        /// <remarks>Tariffs matching any of the products in the list are returned.</remarks>
        /// <param name="products">products to match</param>
        /// <param name="longType">whether product detail supplied for matching is of long type</param>"
        /// <returns>matching tariffs</returns>

        public async Task<List<TariffModel>> GetTariffs(List<string> products, bool longType)
        {
            IList<TariffDB> results;
            TariffModel model;

            if(products.Count >0)
            {
                var pType = products[0];
                if (pType.Length == 6)
                    longType = false;
            }

            var db = DBContext.Instance;

            var array = new BsonArray();
            var arrayHS = new BsonArray();

            if (longType)
            {
                foreach (string s in products) if (s != null) array = array.Add(int.Parse(s));
            }
            else
            {
                foreach (string t in products)
                {
                    if (t.Length == 6)
                    {
                        if (t != null) arrayHS = arrayHS.Add(t);
                    }
                    else
                    {
                        longType = true;
                        if (t != null) array = array.Add(int.Parse(t));
                    }
                }
            }

            var filter = new BsonDocument();
            if (longType)
            {
                filter.Add("TARIFF_ID", new BsonDocument().Add("$in", array));
            }
            else
            {
                 filter.Add("HARMONISED_TARIFF_CODE", new BsonDocument().Add("$in", arrayHS));

                //BsonDocument doc = new BsonDocument
                //{
                //   // { "HARMONISED_TARIFF_CODE",  arrayHS },
                //    { "TARIFF_ID",  array }
                //};

                //filter.AddRange(doc);

             }

            var modelList = new List<TariffModel>();

            using (var cursor = await db.TariffDB.FindAsync(filter))
            {
                results = cursor.ToList();

                foreach (var item in results)
                {
                    model = new TariffModel
                    {
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
            }

            var outList = new List<TariffModel>();

            
            if (longType)
            {
                foreach(var Item in modelList)
                {
                    if(!string.IsNullOrEmpty(Item.SOURCE_COUNTRY_TARIFF_SHORT_LEGEND))
                    {
                        outList.Add(Item);
                    }
                }
                return outList;
            }

          
            return modelList;
        }

        public async Task<TariffModel> GetTariffByCode(string product)
        {

            var builder = Builders<TariffDB>.Filter;

            var filter = builder.Eq("SOURCE_COUNTRY_TARIFF_CODE", product);

            var db = new DBContext();
            var cursor = await db.TariffDB.FindAsync(filter);

            IList<TariffDB> results = cursor.ToList();

            if (results.Count.Equals(0))
            {
                return new TariffModel { SOURCE_COUNTRY_TARIFF_CODE = string.Empty };
            }

            var Item = results[0];
            var model = new TariffModel
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

        public async Task<TariffModel> GetTariffByCodeAndRegion(string product,string region)
        {

            var builder = Builders<TariffDB>.Filter;

            var filter = builder.Eq("SOURCE_COUNTRY_TARIFF_CODE", product) & builder.Eq("TARIFF_CODE_TABLE_CODE", region);

            var db = new DBContext();
            var cursor = await db.TariffDB.FindAsync(filter);

            IList<TariffDB> results = cursor.ToList();

            if (results.Count.Equals(0))
            {
                return new TariffModel { SOURCE_COUNTRY_TARIFF_CODE = string.Empty };
            }

            var Item = results[0];
            var model = new TariffModel
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

        public async Task<TariffModel> GetHSTariffByCode(string product)
        {

            var builder = Builders<TariffDB>.Filter;

            var filter = builder.Eq("HARMONISED_TARIFF_CODE", product);// & builder.Eq("TARIFF_CODE_TABLE_CODE", "HS");

            var db = new DBContext();
            var cursor = await db.TariffDB.FindAsync(filter);

            IList<TariffDB> results = cursor.ToList();

            if (results.Count.Equals(0))
            {
                return new TariffModel { SOURCE_COUNTRY_TARIFF_CODE = string.Empty };
            }

            var Item = results[0];
            var model = new TariffModel
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
        public async Task<int> GetOnSystemIndex(TariffModel model)
        {
            var builder = Builders<TradeDataDB>.Filter;
            var filter = builder.Eq("SOURCE_COUNTRY_TARIFF_CODE", model.SOURCE_COUNTRY_TARIFF_CODE) & builder.Eq("SC_GEO", model.TARIFF_CODE_TABLE_CODE);
            var options = new FindOptions<TradeDataDB, BsonDocument> { Limit = 1 };

            var db = new DBContext();
            var cursor = db.TradeDataDB.FindAsync(filter, options).GetAwaiter().GetResult();
            IList<TradeDataDB> results = new List<TradeDataDB>();
            IEnumerable<BsonDocument> batch;

            while (await cursor.MoveNextAsync())
            {
                batch = cursor.Current;

                foreach (BsonDocument document in batch)
                {
                    results.Add(BsonSerializer.Deserialize<TradeDataDB>(document));
                }
            }

            if (results.Count == 0)
                return 0;
            

            return results[0].TARIFF_ID;
        }

        public async Task<TariffModel> GetTariffByID(string _id)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<TariffDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.TariffDB.FindAsync(filter);

            IList<TariffDB> results = cursor.ToList();

            var Item = results[0];

            var model = new TariffModel
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
        public async Task<TariffExceptionsModel> GetTariffExceptionByCode(string product)
        {

            var builder = Builders<TariffExceptionsDB>.Filter;

            var filter = builder.Eq("SOURCE_COUNTRY_TARIFF_CODE", product);

            var db = new DBContext();
            var cursor = await db.TariffExceptionsDB.FindAsync(filter);

            IList<TariffExceptionsDB> results = cursor.ToList();

            if (results.Count.Equals(0))
            {
                return new TariffExceptionsModel { SOURCE_COUNTRY_TARIFF_CODE = string.Empty };
            }

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

        public async Task<bool> Add(TariffModel model)
        {
            var db = new DBContext();

            var modelDB = new TariffDB
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

            await db.TariffDB.InsertOneAsync(modelDB);

            return true;
        }

        public async Task<TariffDB> Save(TariffModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<TariffDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.TariffDB.FindAsync(filter);
            IList<TariffDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new TariffDB
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

            var returnModel = await db.TariffDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;
        }

        public async Task<bool> Delete(TariffModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<TariffDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.TariffDB.DeleteOneAsync(filter);

            return true;
        }

        public async Task<int> GetCount()
        {

            var db = new DBContext();
            var cursor = await db.TariffDB.FindAsync(new BsonDocument());

            IList<TariffDB> results = cursor.ToList();

            return results.Count;
        }

        public async Task<List<TariffIndexModel>> GetTariffIndex()
        {
            TariffIndexModel model;

            var db = new DBContext();

            var cursor = await db.TariffIndexDB.FindAsync(new BsonDocument());

            IList<TariffIndexDB> results = cursor.ToList();
            var modelList = new List<TariffIndexModel>();

            foreach (var item in results)
            {
                model = new TariffIndexModel
                {
                    _id = item._id.ToString(),
                    Code = item.Code,
                    Name = item.Name,
                    Description = item.Code + " - " + item.Name

                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x.Code).ToList();

        }

        public async Task<List<Tariff2DigitIndexModel>> GetTariff2DigitIndex()
        {
            Tariff2DigitIndexModel model;

            var db = new DBContext();

            var cursor = await db.Tariff2DigitIndexDB.FindAsync(new BsonDocument());

            IList<Tariff2DigitIndexDB> results = cursor.ToList();
            var modelList = new List<Tariff2DigitIndexModel>();

            foreach (var item in results)
            {
                model = new Tariff2DigitIndexModel
                {
                    _id = item._id.ToString(),
                    Code = item.Code,
                    Name = item.Name,
                    Status = item.Status,
                    Description = item.Code + " - " + item.Name

                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x.Code).ToList();

        }

        public async Task<int> ConvertFromGuidToId(string _id)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<TariffDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.TariffDB.FindAsync(filter);

            IList<TariffDB> results = cursor.ToList();

            var Item = results[0];

            var model = new TariffModel
            {
                _id = Item._id.ToString(),
                TARIFF_ID = Item.TARIFF_ID,

            };

            return model.TARIFF_ID;
        }

        public async Task<IEnumerable<SelectListItem>> GetTariffIndexLookup()
        {

            var db = new DBContext();

            var cursor = await db.TariffIndexDB.FindAsync(new BsonDocument());

            IList<TariffIndexDB> results = cursor.ToList();


            List<SelectListItem> indexitems = new List<SelectListItem>();


            foreach (var Item in results)
            {
                var ModelIndex = new SelectListItem
                {
                    Value = Item.Code,
                    Text = Item.Name
                };
                indexitems.Add(ModelIndex);
            }

            return indexitems;
        }

        public async Task<TariffModel> GetTariffByTariffID(string TariffID)
        {

            var builder = Builders<TariffDB>.Filter;

            var filter = builder.Eq("TARIFF_ID", int.Parse(TariffID));

            var db = new DBContext();
            var cursor = await db.TariffDB.FindAsync(filter);

            IList<TariffDB> results = cursor.ToList();

            var Item = results[0];

            var model = new TariffModel
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

        public async Task<List<TradeDataModel>> GetTop1000Transactions(string TariffID)
        {
            var builder = Builders<TradeDataDB>.Filter;

            var filter = builder.Eq("TARIFF_ID", int.Parse(TariffID));

            var db = new DBContext();
            var cursor = await db.TradeDataDB.FindAsync(filter);

            IList<TradeDataDB> results = cursor.ToList();

            var modelList = new List<TradeDataModel>();

            foreach (var Item in results)
            {
                var model = new TradeDataModel
                {
                    _id = Item._id.ToString(),
                    TARIFF_ID = Item.TARIFF_ID,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    SC_GEO = Item.SC_GEO,
                    MC_GEO = Item.MC_GEO,
                    MONTH = Item.MONTH,
                    YEAR = Item.YEAR,
                    QUARTER = Item.QUARTER,
                    WEIGHT = Item.WEIGHT / 1000,
                    MONETARY_VALUE = Item.MONETARY_VALUE
                };
                modelList.Add(model);

            }

            return modelList.OrderByDescending(x => x.YEAR).ToList(); ;
        }

        public async Task<int> DoesExist(string product,string RegionCode)
        {

            var builder = Builders<TariffDB>.Filter;

            var filter = builder.Eq("SOURCE_COUNTRY_TARIFF_CODE", product) & builder.Eq("TARIFF_CODE_TABLE_CODE", RegionCode);
           // var filter = builder.Eq("HARMONISED_TARIFF_CODE", product) & builder.Eq("TARIFF_CODE_TABLE_CODE", RegionCode);

            var db = new DBContext();
            var cursor = await db.TariffDB.FindAsync(filter);

            IList<TariffDB> results = cursor.ToList();

            return results.Count;
        }

        public async Task<int> GetLastIndex()
        {
            var builder = Builders<TariffDB>.Filter;
            
            var options = new FindOptions<TariffDB, BsonDocument> { Limit = 1, Sort = "{ TARIFF_ID : -1}" };

            var db = new DBContext();
            var cursor = db.TariffDB.FindAsync(new BsonDocument(), options).GetAwaiter().GetResult();
            IList<TariffDB> results = new List<TariffDB>();
            IEnumerable<BsonDocument> batch;

            while (await cursor.MoveNextAsync())
            {
                batch = cursor.Current;

                foreach (BsonDocument document in batch)
                {
                    results.Add(BsonSerializer.Deserialize<TariffDB>(document));
                }
            }

            if (results.Count == 0)
                return 0;


            return results[0].TARIFF_ID + 1;
        }

    }
}