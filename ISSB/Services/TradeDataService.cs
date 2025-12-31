using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

namespace Services
{
    public class TradeDataService
    {
        public async Task<TradeDataModel> GetTradeDataRecordForAmendments(int Year,int Month, int Source, int Market, int Tariff)
        {
            var builder = Builders<TradeDataDB>.Filter;

            var filter = builder.Eq("YEAR", Year) &
                         builder.Eq("MONTH", Month) &
                         builder.Eq("SOURCE_COUNTRY_ID", Source) &
                         builder.Eq("MARKET_COUNTRY_ID", Market) &
                         builder.Eq("TARIFF_ID", Tariff);

            var db = new DBContext();
            var cursor = await db.TradeDataDB.FindAsync(filter);

            IList<TradeDataDB> results = cursor.ToList();

            if(results.Count == 0)
            {
                var ReturnEmptyModel = new TradeDataModel { BATCH_NO = 0 };
                return ReturnEmptyModel;
            }

            var Item = results[0];

            var model = new TradeDataModel
            {
                _id = Item._id.ToString(),
                ID = Item.ID,
                ANZSIC = Item.ANZSIC,
                APPENDED = Item.APPENDED,
                BATCH_NO = Item.BATCH_NO,
                COO_GEO_CODE = Item.COO_GEO_CODE,
                CUSTOMS_DISTRICT = Item.CUSTOMS_DISTRICT,
                CUSTOMS_VALUE = Item.CUSTOMS_VALUE,
                CWC_GEO_CODE = Item.CWC_GEO_CODE,
                ECONOMIC_CATEGORY = Item.ECONOMIC_CATEGORY,
                ESTIMATED = Item.ESTIMATED,
                FOB = Item.FOB,
                H_TARIFF = Item.H_TARIFF,
                IMPORT_TARIFF = Item.IMPORT_TARIFF,
                IMP_QTY = Item.IMP_QTY,
                IMP_UNIT = Item.IMP_UNIT,
                IMP_VALUE = Item.IMP_VALUE,
                MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                MC_GEO = Item.MC_GEO,
                MONETARY_VALUE = Item.MONETARY_VALUE,
                MONTH = Item.MONTH,
                MTH_EUR = Item.MTH_EUR,
                MTH_USD = Item.MTH_USD,
                NZ_PORT = Item.NZ_PORT,
                PORT_ID = Item.PORT_ID,
                PORT_OF_DISCHARGE = Item.PORT_OF_DISCHARGE,
                PORT_OF_LANDING = Item.PORT_OF_LANDING,
                PORT_OF_LOADING = Item.PORT_OF_LOADING,
                QUARTER = Item.QUARTER,
                SC_GEO = Item.SC_GEO,
                SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                SITC = Item.SITC,
                SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                STATE = Item.STATE,
                TARIFF_ID = Item.TARIFF_ID,
                TIME_ID = Item.TIME_ID,
                TRADER_LOCATION = Item.TRADER_LOCATION,
                TRANSPORT_MODE = Item.TRANSPORT_MODE,
                WEIGHT = Item.WEIGHT,
                YEAR = Item.YEAR,
                YTD_EUR = Item.YTD_EUR,
                YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                YTD_USD = Item.YTD_USD,
                YTD_WEIGHT = Item.YTD_WEIGHT

            };

            return model;
        }
        public async Task<List<TradeDataModel>> GetTradeData(int Year, int Source, int Market, int Tariff)
        {
            //BsonObjectId RecordId = new BsonObjectId(new ObjectId(Id));
            var filter = Builders<TradeDataDB>.Filter.Eq(x => x.YEAR, Year)
                & Builders<TradeDataDB>.Filter.Eq(z => z.SOURCE_COUNTRY_ID, Source)
                & Builders<TradeDataDB>.Filter.Eq(z => z.MARKET_COUNTRY_ID, Market)
                & Builders<TradeDataDB>.Filter.Eq(z => z.TARIFF_ID, Tariff);
            var db = new DBContext();
            var cursor = await db.TradeDataDB.FindAsync(filter);

            IList<TradeDataDB> results = cursor.ToList();
            var modelList = new List<TradeDataModel>();

            foreach (var Item in results)
            {

                var model = new TradeDataModel
                {
                    _id = Item._id.ToString(),
                    ID = Item.ID,
                    ANZSIC = Item.ANZSIC,
                    APPENDED = Item.APPENDED,
                    BATCH_NO = Item.BATCH_NO,
                    COO_GEO_CODE = Item.COO_GEO_CODE,
                    CUSTOMS_DISTRICT = Item.CUSTOMS_DISTRICT,
                    CUSTOMS_VALUE = Item.CUSTOMS_VALUE,
                    CWC_GEO_CODE = Item.CWC_GEO_CODE,
                    ECONOMIC_CATEGORY = Item.ECONOMIC_CATEGORY,
                    ESTIMATED = Item.ESTIMATED,
                    FOB = Item.FOB,
                    H_TARIFF = Item.H_TARIFF,
                    IMPORT_TARIFF = Item.IMPORT_TARIFF,
                    IMP_QTY = Item.IMP_QTY,
                    IMP_UNIT = Item.IMP_UNIT,
                    IMP_VALUE = Item.IMP_VALUE,
                    MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                    MC_GEO = Item.MC_GEO,
                    MONETARY_VALUE = Item.MONETARY_VALUE,
                    MONTH = Item.MONTH,
                    MTH_EUR = Item.MTH_EUR,
                    MTH_USD = Item.MTH_USD,
                    NZ_PORT = Item.NZ_PORT,
                    PORT_ID = Item.PORT_ID,
                    PORT_OF_DISCHARGE = Item.PORT_OF_DISCHARGE,
                    PORT_OF_LANDING = Item.PORT_OF_LANDING,
                    PORT_OF_LOADING = Item.PORT_OF_LOADING,
                    QUARTER = Item.QUARTER,
                    SC_GEO = Item.SC_GEO,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    SITC = Item.SITC,
                    SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                    STATE = Item.STATE,
                    TARIFF_ID = Item.TARIFF_ID,
                    TIME_ID = Item.TIME_ID,
                    TRADER_LOCATION = Item.TRADER_LOCATION,
                    TRANSPORT_MODE = Item.TRANSPORT_MODE,
                    WEIGHT = Item.WEIGHT,
                    YEAR = Item.YEAR,
                    YTD_EUR = Item.YTD_EUR,
                    YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                    YTD_USD = Item.YTD_USD,
                    YTD_WEIGHT = Item.YTD_WEIGHT

                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x.ID).ToList();
        }

        public async Task<List<TradeDataModel>> GetTradeDataQuery(int Year, int Month, int Source, int Market)
        {

            var filter = Builders<TradeDataDB>.Filter.Eq(x => x.YEAR, Year)
                & Builders<TradeDataDB>.Filter.Eq(z => z.MONTH, Month)
                & Builders<TradeDataDB>.Filter.Eq(z => z.SOURCE_COUNTRY_ID, Source)
                & Builders<TradeDataDB>.Filter.Eq(z => z.MARKET_COUNTRY_ID, Market);

            var db = new DBContext();
            var cursor = await db.TradeDataDB.FindAsync(filter);

            IList<TradeDataDB> results = cursor.ToList();
            var modelList = new List<TradeDataModel>();

            foreach (var Item in results)
            {

                var model = new TradeDataModel
                {
                    _id = Item._id.ToString(),
                    ID = Item.ID,
                    ANZSIC = Item.ANZSIC,
                    APPENDED = Item.APPENDED,
                    BATCH_NO = Item.BATCH_NO,
                    COO_GEO_CODE = Item.COO_GEO_CODE,
                    CUSTOMS_DISTRICT = Item.CUSTOMS_DISTRICT,
                    CUSTOMS_VALUE = Item.CUSTOMS_VALUE,
                    CWC_GEO_CODE = Item.CWC_GEO_CODE,
                    ECONOMIC_CATEGORY = Item.ECONOMIC_CATEGORY,
                    ESTIMATED = Item.ESTIMATED,
                    FOB = Item.FOB,
                    H_TARIFF = Item.H_TARIFF,
                    IMPORT_TARIFF = Item.IMPORT_TARIFF,
                    IMP_QTY = Item.IMP_QTY,
                    IMP_UNIT = Item.IMP_UNIT,
                    IMP_VALUE = Item.IMP_VALUE,
                    MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                    MC_GEO = Item.MC_GEO,
                    MONETARY_VALUE = Item.MONETARY_VALUE,
                    MONTH = Item.MONTH,
                    MTH_EUR = Item.MTH_EUR,
                    MTH_USD = Item.MTH_USD,
                    NZ_PORT = Item.NZ_PORT,
                    PORT_ID = Item.PORT_ID,
                    PORT_OF_DISCHARGE = Item.PORT_OF_DISCHARGE,
                    PORT_OF_LANDING = Item.PORT_OF_LANDING,
                    PORT_OF_LOADING = Item.PORT_OF_LOADING,
                    QUARTER = Item.QUARTER,
                    SC_GEO = Item.SC_GEO,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    SITC = Item.SITC,
                    SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                    STATE = Item.STATE,
                    TARIFF_ID = Item.TARIFF_ID,
                    TIME_ID = Item.TIME_ID,
                    TRADER_LOCATION = Item.TRADER_LOCATION,
                    TRANSPORT_MODE = Item.TRANSPORT_MODE,
                    WEIGHT = Item.WEIGHT,
                    YEAR = Item.YEAR,
                    YTD_EUR = Item.YTD_EUR,
                    YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                    YTD_USD = Item.YTD_USD,
                    YTD_WEIGHT = Item.YTD_WEIGHT

                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x.ID).ToList();
        }

        public async Task<List<TradeDataModel>> GetDataYTD(int Year, int Source, int Market, int Tariff, string Sot)
        {

            var filter = Builders<TradeDataDB>.Filter.Eq(x => x.YEAR, Year)
                & Builders<TradeDataDB>.Filter.Eq(z => z.SOURCE_COUNTRY_ID, Source)
                & Builders<TradeDataDB>.Filter.Eq(z => z.MARKET_COUNTRY_ID, Market)
                & Builders<TradeDataDB>.Filter.Eq(z => z.TARIFF_ID, Tariff)
                & Builders<TradeDataDB>.Filter.Eq(z => z.SIDE_OF_TRADE, Sot);

            var db = new DBContext();
            var cursor = await db.TradeDataDB.FindAsync(filter);

            IList<TradeDataDB> results = cursor.ToList();
            var modelList = new List<TradeDataModel>();

            foreach (var Item in results)
            {

                var model = new TradeDataModel
                {
                    _id = Item._id.ToString(),
                    ID = Item.ID,
                    ANZSIC = Item.ANZSIC,
                    APPENDED = Item.APPENDED,
                    BATCH_NO = Item.BATCH_NO,
                    COO_GEO_CODE = Item.COO_GEO_CODE,
                    CUSTOMS_DISTRICT = Item.CUSTOMS_DISTRICT,
                    CUSTOMS_VALUE = Item.CUSTOMS_VALUE,
                    CWC_GEO_CODE = Item.CWC_GEO_CODE,
                    ECONOMIC_CATEGORY = Item.ECONOMIC_CATEGORY,
                    ESTIMATED = Item.ESTIMATED,
                    FOB = Item.FOB,
                    H_TARIFF = Item.H_TARIFF,
                    IMPORT_TARIFF = Item.IMPORT_TARIFF,
                    IMP_QTY = Item.IMP_QTY,
                    IMP_UNIT = Item.IMP_UNIT,
                    IMP_VALUE = Item.IMP_VALUE,
                    MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                    MC_GEO = Item.MC_GEO,
                    MONETARY_VALUE = Item.MONETARY_VALUE,
                    MONTH = Item.MONTH,
                    MTH_EUR = Item.MTH_EUR,
                    MTH_USD = Item.MTH_USD,
                    NZ_PORT = Item.NZ_PORT,
                    PORT_ID = Item.PORT_ID,
                    PORT_OF_DISCHARGE = Item.PORT_OF_DISCHARGE,
                    PORT_OF_LANDING = Item.PORT_OF_LANDING,
                    PORT_OF_LOADING = Item.PORT_OF_LOADING,
                    QUARTER = Item.QUARTER,
                    SC_GEO = Item.SC_GEO,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    SITC = Item.SITC,
                    SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                    STATE = Item.STATE,
                    TARIFF_ID = Item.TARIFF_ID,
                    TIME_ID = Item.TIME_ID,
                    TRADER_LOCATION = Item.TRADER_LOCATION,
                    TRANSPORT_MODE = Item.TRANSPORT_MODE,
                    WEIGHT = Item.WEIGHT,
                    YEAR = Item.YEAR,
                    YTD_EUR = Item.YTD_EUR,
                    YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                    YTD_USD = Item.YTD_USD,
                    YTD_WEIGHT = Item.YTD_WEIGHT

                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x.MONTH).ToList();
        }

        public async Task<List<TradeDataModel>> GetDataByBatchNumber(int BatchNumber)
        {

            var filter = Builders<TradeDataDB>.Filter.Eq(x => x.BATCH_NO, BatchNumber);
            
            var db = new DBContext();
            var cursor = await db.TradeDataDB.FindAsync(filter);

            IList<TradeDataDB> results = cursor.ToList();
            var modelList = new List<TradeDataModel>();

            if (results.Count == 0)
                return modelList;

            foreach (var Item in results)
            {

                var model = new TradeDataModel
                {
                    _id = Item._id.ToString(),
                    ID = Item.ID,
                    ANZSIC = Item.ANZSIC,
                    APPENDED = Item.APPENDED,
                    BATCH_NO = Item.BATCH_NO,
                    COO_GEO_CODE = Item.COO_GEO_CODE,
                    CUSTOMS_DISTRICT = Item.CUSTOMS_DISTRICT,
                    CUSTOMS_VALUE = Item.CUSTOMS_VALUE,
                    CWC_GEO_CODE = Item.CWC_GEO_CODE,
                    ECONOMIC_CATEGORY = Item.ECONOMIC_CATEGORY,
                    ESTIMATED = Item.ESTIMATED,
                    FOB = Item.FOB,
                    H_TARIFF = Item.H_TARIFF,
                    IMPORT_TARIFF = Item.IMPORT_TARIFF,
                    IMP_QTY = Item.IMP_QTY,
                    IMP_UNIT = Item.IMP_UNIT,
                    IMP_VALUE = Item.IMP_VALUE,
                    MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                    MC_GEO = Item.MC_GEO,
                    MONETARY_VALUE = Item.MONETARY_VALUE,
                    MONTH = Item.MONTH,
                    MTH_EUR = Item.MTH_EUR,
                    MTH_USD = Item.MTH_USD,
                    NZ_PORT = Item.NZ_PORT,
                    PORT_ID = Item.PORT_ID,
                    PORT_OF_DISCHARGE = Item.PORT_OF_DISCHARGE,
                    PORT_OF_LANDING = Item.PORT_OF_LANDING,
                    PORT_OF_LOADING = Item.PORT_OF_LOADING,
                    QUARTER = Item.QUARTER,
                    SC_GEO = Item.SC_GEO,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    SITC = Item.SITC,
                    SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                    STATE = Item.STATE,
                    TARIFF_ID = Item.TARIFF_ID,
                    TIME_ID = Item.TIME_ID,
                    TRADER_LOCATION = Item.TRADER_LOCATION,
                    TRANSPORT_MODE = Item.TRANSPORT_MODE,
                    WEIGHT = Item.WEIGHT,
                    YEAR = Item.YEAR,
                    YTD_EUR = Item.YTD_EUR,
                    YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                    YTD_USD = Item.YTD_USD,
                    YTD_WEIGHT = Item.YTD_WEIGHT

                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x.MONTH).ToList();
        }


        public async Task<TradeDataModel> GetTradeDataRecord(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<TradeDataDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.TradeDataDB.FindAsync(filter);

            IList<TradeDataDB> results = cursor.ToList();

            var Item = results[0];

            var model = new TradeDataModel
            {
                _id = Item._id.ToString(),
                ID = Item.ID,
                ANZSIC = Item.ANZSIC,
                APPENDED = Item.APPENDED,
                BATCH_NO = Item.BATCH_NO,
                COO_GEO_CODE = Item.COO_GEO_CODE,
                CUSTOMS_DISTRICT = Item.CUSTOMS_DISTRICT,
                CUSTOMS_VALUE = Item.CUSTOMS_VALUE,
                CWC_GEO_CODE = Item.CWC_GEO_CODE,
                ECONOMIC_CATEGORY = Item.ECONOMIC_CATEGORY,
                ESTIMATED = Item.ESTIMATED,
                FOB = Item.FOB,
                H_TARIFF = Item.H_TARIFF,
                IMPORT_TARIFF = Item.IMPORT_TARIFF,
                IMP_QTY = Item.IMP_QTY,
                IMP_UNIT = Item.IMP_UNIT,
                IMP_VALUE = Item.IMP_VALUE,
                MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                MC_GEO = Item.MC_GEO,
                MONETARY_VALUE = Item.MONETARY_VALUE,
                MONTH = Item.MONTH,
                MTH_EUR = Item.MTH_EUR,
                MTH_USD = Item.MTH_USD,
                NZ_PORT = Item.NZ_PORT,
                PORT_ID = Item.PORT_ID,
                PORT_OF_DISCHARGE = Item.PORT_OF_DISCHARGE,
                PORT_OF_LANDING = Item.PORT_OF_LANDING,
                PORT_OF_LOADING = Item.PORT_OF_LOADING,
                QUARTER = Item.QUARTER,
                SC_GEO = Item.SC_GEO,
                SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                SITC = Item.SITC,
                SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                STATE = Item.STATE,
                TARIFF_ID = Item.TARIFF_ID,
                TIME_ID = Item.TIME_ID,
                TRADER_LOCATION = Item.TRADER_LOCATION,
                TRANSPORT_MODE = Item.TRANSPORT_MODE,
                WEIGHT = Item.WEIGHT,
                YEAR = Item.YEAR,
                YTD_EUR = Item.YTD_EUR,
                YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                YTD_USD = Item.YTD_USD,
                YTD_WEIGHT = Item.YTD_WEIGHT

            };

            return model;
        }

        public TradeDataDB GetTradeDataRecordByTariffIndex(int ID)
        {
            TradeDataDB rModel;

            var builder = Builders<TradeDataDB>.Filter;

            var filter = builder.Eq("TARIFF_ID", ID);

            var db = new DBContext();
            var cursor =  db.TradeDataDB.Find(filter).Limit(1);

            IList<TradeDataDB> results = cursor.ToList();

            if (results.Count == 0)
            {
                rModel = new TradeDataDB
                {
                    TARIFF_ID = 0
                };
            }
            else
            {
                rModel = results[0];
            }

            return rModel;
        }

        public async Task<TradeDataDB> Save(TradeDataModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<TradeDataDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.TradeDataDB.FindAsync(filter);
            IList<TradeDataDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new TradeDataDB
            {
                _id = Item._id,
                ID = model.ID,
                ANZSIC = model.ANZSIC,
                APPENDED = model.APPENDED,
                BATCH_NO = model.BATCH_NO,
                COO_GEO_CODE = model.COO_GEO_CODE,
                CUSTOMS_DISTRICT = model.CUSTOMS_DISTRICT,
                CUSTOMS_VALUE = model.CUSTOMS_VALUE,
                CWC_GEO_CODE = model.CWC_GEO_CODE,
                ECONOMIC_CATEGORY = model.ECONOMIC_CATEGORY,
                ESTIMATED = model.ESTIMATED,
                FOB = model.FOB,
                H_TARIFF = model.H_TARIFF,
                IMPORT_TARIFF = model.IMPORT_TARIFF,
                IMP_QTY = model.IMP_QTY,
                IMP_UNIT = model.IMP_UNIT,
                IMP_VALUE = model.IMP_VALUE,
                MARKET_COUNTRY_ID = model.MARKET_COUNTRY_ID,
                MC_GEO = model.MC_GEO,
                MONETARY_VALUE = model.MONETARY_VALUE,
                MONTH = model.MONTH,
                MTH_EUR = model.MTH_EUR,
                MTH_USD = model.MTH_USD,
                NZ_PORT = model.NZ_PORT,
                PORT_ID = model.PORT_ID,
                PORT_OF_DISCHARGE = model.PORT_OF_DISCHARGE,
                PORT_OF_LANDING = model.PORT_OF_LANDING,
                PORT_OF_LOADING = model.PORT_OF_LOADING,
                QUARTER = model.QUARTER,
                SC_GEO = model.SC_GEO,
                SIDE_OF_TRADE = model.SIDE_OF_TRADE,
                SITC = model.SITC,
                SOURCE_COUNTRY_ID = model.SOURCE_COUNTRY_ID,
                STATE = model.STATE,
                TARIFF_ID = model.TARIFF_ID,
                TIME_ID = model.TIME_ID,
                TRADER_LOCATION = model.TRADER_LOCATION,
                TRANSPORT_MODE = model.TRANSPORT_MODE,
                WEIGHT = model.WEIGHT,
                YEAR = model.YEAR,
                YTD_EUR = model.YTD_EUR,
                YTD_MONETARY_VALUE = model.YTD_MONETARY_VALUE,
                YTD_USD = model.YTD_USD,
                YTD_WEIGHT = model.YTD_WEIGHT
            };

            var returnModel = await db.TradeDataDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;
        }

        public async Task<bool> Delete(TradeDataModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<TradeDataDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.TradeDataDB.DeleteOneAsync(filter);

            return true;
        }

        public async Task<List<TradeDataModel>> GetDataToMirror(MirrorDataModel mirrorBuilder)
        {
            string[] words = mirrorBuilder.TariffCodes.Split(',');

            var tCodes = new BsonArray();

            foreach (var code in words)
            {
                tCodes.Add(code);
            }
            var db =  DBContext.Instance;

            IMongoCollection<BsonDocument> collection = db.TradeDataDB.Database.GetCollection<BsonDocument>("TradeDataDB");
            
            var options = new AggregateOptions()
            {
                AllowDiskUse = false
            };

            var modelList = new List<TradeDataModel>();

            PipelineDefinition<BsonDocument, BsonDocument> pipeline = new BsonDocument[]
            {

                new BsonDocument("$match", new BsonDocument()
                        .Add("MC_GEO", mirrorBuilder.GeoCode)
                        .Add("YEAR", mirrorBuilder.Year)
                        .Add("MONTH", mirrorBuilder.Month)
                        .Add("SIDE_OF_TRADE", "E" )
                        .Add("H_TARIFF", new BsonDocument()
                        .Add("$in", tCodes)))
            };

            using (var cursor = await collection.AggregateAsync(pipeline, options))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        var model = BsonSerializer.Deserialize<TradeDataDB>(document);
                        var modelDB = new TradeDataModel
                        {
                            _id = model._id.ToString(),
                            ID = model.ID,
                            ANZSIC = model.ANZSIC,
                            APPENDED = model.APPENDED,
                            BATCH_NO = model.BATCH_NO,
                            COO_GEO_CODE = model.COO_GEO_CODE,
                            CUSTOMS_DISTRICT = model.CUSTOMS_DISTRICT,
                            CUSTOMS_VALUE = model.CUSTOMS_VALUE,
                            CWC_GEO_CODE = model.CWC_GEO_CODE,
                            ECONOMIC_CATEGORY = model.ECONOMIC_CATEGORY,
                            ESTIMATED = model.ESTIMATED,
                            FOB = model.FOB,
                            H_TARIFF = model.H_TARIFF,
                            IMPORT_TARIFF = model.IMPORT_TARIFF,
                            IMP_QTY = model.IMP_QTY,
                            IMP_UNIT = model.IMP_UNIT,
                            IMP_VALUE = model.IMP_VALUE,
                            MARKET_COUNTRY_ID = model.MARKET_COUNTRY_ID,
                            MC_GEO = model.MC_GEO,
                            MONETARY_VALUE = model.MONETARY_VALUE,
                            MONTH = model.MONTH,
                            MTH_EUR = model.MTH_EUR,
                            MTH_USD = model.MTH_USD,
                            NZ_PORT = model.NZ_PORT,
                            PORT_ID = model.PORT_ID,
                            PORT_OF_DISCHARGE = model.PORT_OF_DISCHARGE,
                            PORT_OF_LANDING = model.PORT_OF_LANDING,
                            PORT_OF_LOADING = model.PORT_OF_LOADING,
                            QUARTER = model.QUARTER,
                            SC_GEO = model.SC_GEO,
                            SIDE_OF_TRADE = model.SIDE_OF_TRADE,
                            SITC = model.SITC,
                            SOURCE_COUNTRY_ID = model.SOURCE_COUNTRY_ID,
                            STATE = model.STATE,
                            TARIFF_ID = model.TARIFF_ID,
                            TIME_ID = model.TIME_ID,
                            TRADER_LOCATION = model.TRADER_LOCATION,
                            TRANSPORT_MODE = model.TRANSPORT_MODE,
                            WEIGHT = model.WEIGHT,
                            YEAR = model.YEAR,
                            YTD_EUR = model.YTD_EUR,
                            YTD_MONETARY_VALUE = model.YTD_MONETARY_VALUE,
                            YTD_USD = model.YTD_USD,
                            YTD_WEIGHT = model.YTD_WEIGHT
                        };
                        modelList.Add(modelDB);
                    }
                }
            }
            
            return modelList;
        }
    }
}
