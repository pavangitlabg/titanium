using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Services
{
    public class ValidationEngineService
    {
        public async Task<bool> AddForValidation(List<PendingImportModel> modelList)
        {
            
            var db = new DBContext();
            var manyDb = new List<PendingImportDB>();
            foreach (var Item in modelList)
            {
                try
                {
                    char pad = '0';
                    string H_TARIFF = Item.IMPORT_TARIFF.Substring(0, 6);
                    string IMPORT_TARIFF = Item.IMPORT_TARIFF.TrimEnd();
                    IMPORT_TARIFF = IMPORT_TARIFF.PadRight(11, pad);

                    int QUARTER = 0;

                    if (Item.MONTH <= 3)
                        QUARTER = 1;
                    if (Item.MONTH >= 4 && Item.MONTH <= 6)
                        QUARTER = 2;
                    if (Item.MONTH >= 7 && Item.MONTH <= 9)
                        QUARTER = 3;
                    if (Item.MONTH >= 10 && Item.MONTH <= 12)
                        QUARTER = 4;

                    int TIME_ID = int.Parse(Item.YEAR.ToString() + QUARTER + Item.MONTH.ToString().PadLeft(2, pad) + "00");

                    var modelDB = new PendingImportDB
                    {
                        BATCH_NO = Item.BATCH_NO,
                        COO_GEO_CODE = Item.COO_GEO_CODE,
                        CURRENCY_CODE = Item.CURRENCY_CODE,
                        CWC_GEO_CODE = Item.CWC_GEO_CODE,
                        ESTIMATED = Item.ESTIMATED,
                        H_TARIFF = H_TARIFF,
                        ID = Item.ID,
                        IMPORT_TARIFF = IMPORT_TARIFF,
                        IMP_UNIT = Item.IMP_UNIT,
                        MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                        MC_GEO = Item.MC_GEO,
                        MONETARY_VALUE = Item.MONETARY_VALUE,
                        MONTH = Item.MONTH,
                        PORT_ID = Item.PORT_ID,
                        PORT_ALPHA = Item.PORT_ALPHA,
                        QUARTER = QUARTER,
                        SC_GEO = Item.SC_GEO,
                        SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                        SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                        TARIFF_ID = Item.TARIFF_ID,
                        TIME_ID = TIME_ID,
                        WEIGHT = Item.WEIGHT,
                        YEAR = Item.YEAR,
                        YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                        YTD_WEIGHT = Item.YTD_WEIGHT
                    };

                    manyDb.Add(modelDB);
                }
                catch(Exception e)
                {
                    var eSrv = new ErrorService();
                    var eModel = new ErrorModel { Code = "989", Class = "ValidationEngineService Line 77", ErrorMessage = e.Message };
                    await eSrv.UpdateError(eModel);
                }
               
            }
            await db.PendingImportDB.InsertManyAsync(manyDb);
            return true;
        }

        public async Task<bool> AddForAmendments(List<PendingImportModel> modelList)
        {

            var db = DBContext.Instance;
            var manyDb = new List<PendingImportAmendmentsDB>();
            foreach (var Item in modelList)
            {
                try
                {
                    char pad = '0';
                    string H_TARIFF = Item.IMPORT_TARIFF.Substring(0, 6);
                    string IMPORT_TARIFF = Item.IMPORT_TARIFF.TrimEnd();
                    IMPORT_TARIFF = IMPORT_TARIFF.PadRight(11, pad);

                    int QUARTER = 0;

                    if (Item.MONTH <= 3)
                        QUARTER = 1;
                    if (Item.MONTH >= 4 && Item.MONTH <= 6)
                        QUARTER = 2;
                    if (Item.MONTH >= 7 && Item.MONTH <= 9)
                        QUARTER = 3;
                    if (Item.MONTH >= 10 && Item.MONTH <= 12)
                        QUARTER = 4;

                    int TIME_ID = int.Parse(Item.YEAR.ToString() + QUARTER + Item.MONTH.ToString().PadLeft(2, pad) + "00");

                    var modelDB = new PendingImportAmendmentsDB
                    {
                        BATCH_NO = Item.BATCH_NO,
                        COO_GEO_CODE = Item.COO_GEO_CODE,
                        CURRENCY_CODE = Item.CURRENCY_CODE,
                        CWC_GEO_CODE = Item.CWC_GEO_CODE,
                        ESTIMATED = Item.ESTIMATED,
                        H_TARIFF = H_TARIFF,
                        ID = Item.ID,
                        IMPORT_TARIFF = IMPORT_TARIFF,
                        IMP_UNIT = Item.IMP_UNIT,
                        MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                        MC_GEO = Item.MC_GEO,
                        MONETARY_VALUE = Item.MONETARY_VALUE,
                        MONTH = Item.MONTH,
                        PORT_ID = Item.PORT_ID,
                        PORT_ALPHA = Item.PORT_ALPHA,
                        QUARTER = QUARTER,
                        SC_GEO = Item.SC_GEO,
                        SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                        SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                        TARIFF_ID = Item.TARIFF_ID,
                        TIME_ID = TIME_ID,
                        WEIGHT = Item.WEIGHT,
                        YEAR = Item.YEAR,
                        YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                        YTD_WEIGHT = Item.YTD_WEIGHT
                    };

                    manyDb.Add(modelDB);
                }
                catch (Exception e)
                {
                    var eSrv = new ErrorService();
                    var eModel = new ErrorModel { Code = "989", Class = "AddForAmendments Line 147", ErrorMessage = e.Message };
                    await eSrv.UpdateError(eModel);
                }

            }
            await db.PendingImportAmendmentsDB.InsertManyAsync(manyDb);
            return true;
        }

        public async Task<bool> DeleteAmendment(PendingImportModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<PendingImportAmendmentsDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.PendingImportAmendmentsDB.DeleteOneAsync(filter);

            return true;
        }


        public async Task<bool> DeleteAmendments(string scGeo)
        {
            var builder = Builders<PendingImportAmendmentsDB>.Filter;
            var filter = builder.Eq("SC_GEO", scGeo);
            var db = DBContext.Instance;

            await db.PendingImportAmendmentsDB.DeleteManyAsync(filter);

            return true;
        }

        //public async Task<bool> DeleteAmendmentsZeroValues(int batchNumber)
        //{
        //    var builder = Builders<PendingImportAmendmentsDB>.Filter;
        //    var filter = builder.Eq("BATCH_NUMBER", batchNumber) & builder.Eq("MONETARY_VALUE", 0) | builder.Eq("WEIGHT", 0);
        //    var db = DBContext.Instance;

        //    await db.PendingImportAmendmentsDB.DeleteManyAsync(filter);

        //    return true;
        //}

        public async Task<List<PendingImportModel>> GetPendingImportAmendments(int BatchNumber)
        {
            var builder = Builders<PendingImportAmendmentsDB>.Filter;

            var filter = builder.Eq("BATCH_NO", BatchNumber);

            var db = new DBContext();

            var cursor = await db.PendingImportAmendmentsDB.FindAsync(filter);


            IList<PendingImportAmendmentsDB> results = cursor.ToList();
            var modelList = new List<PendingImportModel>();

            foreach (var Item in results)
            {

                var model = new PendingImportModel
                {
                    _id = Item._id.ToString(),
                    BATCH_NO = Item.BATCH_NO,
                    COO_GEO_CODE = Item.COO_GEO_CODE,
                    CURRENCY_CODE = Item.CURRENCY_CODE,
                    CWC_GEO_CODE = Item.CWC_GEO_CODE,
                    ESTIMATED = Item.ESTIMATED,
                    H_TARIFF = Item.H_TARIFF,
                    ID = Item.ID,
                    IMPORT_TARIFF = Item.IMPORT_TARIFF,
                    IMP_UNIT = Item.IMP_UNIT,
                    MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                    MC_GEO = Item.MC_GEO,
                    MONETARY_VALUE = Item.MONETARY_VALUE,
                    MONTH = Item.MONTH,
                    PORT_ID = Item.PORT_ID,
                    PORT_ALPHA = Item.PORT_ALPHA,
                    QUARTER = Item.QUARTER,
                    SC_GEO = Item.SC_GEO,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                    TARIFF_ID = Item.TARIFF_ID,
                    TIME_ID = Item.TIME_ID,
                    WEIGHT = Item.WEIGHT,
                    YEAR = Item.YEAR,
                    YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                    YTD_WEIGHT = Item.YTD_WEIGHT,
                };

                modelList.Add(model);
            }

            //await db.PendingImportDB.DeleteManyAsync(filter);


            return modelList;
        }
        public async Task<bool> AddAfterValidation(List<PendingImportModel> modelList)
        {

            var db = new DBContext();
            var manyDb = new List<PendingImportDB>();
            foreach (var Item in modelList)
            {
                try
                {
                    
                    var modelDB = new PendingImportDB
                    {
                        BATCH_NO = Item.BATCH_NO,
                        COO_GEO_CODE = Item.COO_GEO_CODE,
                        CURRENCY_CODE = Item.CURRENCY_CODE,
                        CWC_GEO_CODE = Item.CWC_GEO_CODE,
                        ESTIMATED = Item.ESTIMATED,
                        H_TARIFF = Item.H_TARIFF,
                        ID = Item.ID,
                        IMPORT_TARIFF = Item.IMPORT_TARIFF,
                        IMP_UNIT = Item.IMP_UNIT,
                        MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                        MC_GEO = Item.MC_GEO,
                        MONETARY_VALUE = Item.MONETARY_VALUE,
                        MONTH = Item.MONTH,
                        PORT_ID = Item.PORT_ID,
                        PORT_ALPHA = Item.PORT_ALPHA,
                        QUARTER = Item.QUARTER,
                        SC_GEO = Item.SC_GEO,
                        SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                        SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                        TARIFF_ID = Item.TARIFF_ID,
                        TIME_ID = Item.TIME_ID,
                        WEIGHT = Item.WEIGHT,
                        YEAR = Item.YEAR,
                        YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                        YTD_WEIGHT = Item.YTD_WEIGHT
                    };

                    manyDb.Add(modelDB);
                }
                catch (Exception e)
                {
                    var eSrv = new ErrorService();
                    var eModel = new ErrorModel { Code = "989", Class = "ValidationEngineService Line 128", ErrorMessage = e.Message };
                    await eSrv.UpdateError(eModel);
                }

            }
            await db.PendingImportDB.InsertManyAsync(manyDb);
            return true;
        }

        public async Task<bool> AddHeader(PendingImportHeaderModel model)
        {
            bool bReturn;
          
            var db = new DBContext();
            var modelDB = new PendingImportHeaderDB
            {
                BATCH_NO = model.BATCH_NO,
                DATE  = model.DATE,
                IMPORT_TYPE = model.IMPORT_TYPE,
                STATUS = model.STATUS,
                FAIL = model.FAIL,
                END_DATE = model.END_DATE,
                POSTED = model.POSTED
            };

            await db.PendingImportHeaderDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }


        public async Task<bool> UpdateHeader(PendingImportHeaderModel model)
        {
            bool bReturn;

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<PendingImportHeaderDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var modelDB = new PendingImportHeaderDB
            {
                 _id = RecordId,
                BATCH_NO = model.BATCH_NO,
                DATE = model.DATE,
                IMPORT_TYPE = model.IMPORT_TYPE,
                STATUS = model.STATUS,
                FAIL = model.FAIL,
                END_DATE = model.END_DATE,
                POSTED = model.POSTED
            };

            await db.PendingImportHeaderDB.FindOneAndReplaceAsync(filter, modelDB);
            bReturn = true;

            return bReturn;
        }

        public async Task<PendingImportHeaderModel> GetHeader(int BatchNumber)
        {
            var builder = Builders<PendingImportHeaderDB>.Filter;

            var filter = builder.Eq("BATCH_NO", BatchNumber);

            var db = new DBContext();

            var cursor = await db.PendingImportHeaderDB.FindAsync(filter);


            IList<PendingImportHeaderDB> results = cursor.ToList();

            if (results.Count == 0)
                return new PendingImportHeaderModel();

            var Item = results[0];

            var modelDB = new PendingImportHeaderModel
            {
                 _id = Item._id.ToString(),
                BATCH_NO = Item.BATCH_NO,
                DATE = Item.DATE,
                IMPORT_TYPE = Item.IMPORT_TYPE,
                STATUS = Item.STATUS,
                END_DATE = Item.END_DATE,
                POSTED = Item.POSTED,
                FAIL = Item.FAIL
            };


            return modelDB;
        }

        public async Task<List<PendingImportModel>> ValidateImport(int BatchNumber)
        {
            var builder = Builders<PendingImportDB>.Filter;

            var filter = builder.Eq("BATCH_NO", BatchNumber);

            var db = new DBContext();

            var cursor = await db.PendingImportDB.FindAsync(filter);


            IList<PendingImportDB> results = cursor.ToList();
            var modelList = new List<PendingImportModel>();

            foreach (var Item in results)
            {

                var model = new PendingImportModel
                {
                    _id = Item._id.ToString(),
                    BATCH_NO = Item.BATCH_NO,
                    COO_GEO_CODE = Item.COO_GEO_CODE,
                    CURRENCY_CODE = Item.CURRENCY_CODE,
                    CWC_GEO_CODE = Item.CWC_GEO_CODE,
                    ESTIMATED = Item.ESTIMATED,
                    H_TARIFF = Item.H_TARIFF,
                    ID = Item.ID,
                    IMPORT_TARIFF = Item.IMPORT_TARIFF,
                    IMP_UNIT = Item.IMP_UNIT,
                    MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                    MC_GEO = Item.MC_GEO,
                    MONETARY_VALUE = Item.MONETARY_VALUE,
                    MONTH = Item.MONTH,
                    PORT_ID = Item.PORT_ID,
                    PORT_ALPHA = Item.PORT_ALPHA,
                    QUARTER = Item.QUARTER,
                    SC_GEO = Item.SC_GEO,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                    TARIFF_ID = Item.TARIFF_ID,
                    TIME_ID = Item.TIME_ID,
                    WEIGHT = Item.WEIGHT,
                    YEAR = Item.YEAR,
                    YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                    YTD_WEIGHT = Item.YTD_WEIGHT,
                };

                modelList.Add(model);
            }

            //await db.PendingImportDB.DeleteManyAsync(filter);


            return modelList;
        }

        public async Task<bool> DeleteZeroSourceCountries(int id)
        {
            var builder = Builders<PendingImportDB>.Filter;
            var filter = builder.Eq("SOURCE_COUNTRY_ID", id);
            var db = new DBContext();

            await db.PendingImportDB.DeleteManyAsync(filter);

            return true;
        }

        public async Task<bool> DeleteZeroMarketCountries(int id)
        {
            var builder = Builders<PendingImportDB>.Filter;
            var filter = builder.Eq("MARKET_COUNTRY_ID", id);
            var db = new DBContext();

            await db.PendingImportDB.DeleteManyAsync(filter);

            return true;
        }

        public async Task<bool> DeleteZeroTariffId(int id)
        {
            var builder = Builders<PendingImportDB>.Filter;
            var filter = builder.Eq("TARIFF_ID", id);
            var db = new DBContext();

            await db.PendingImportDB.DeleteManyAsync(filter);

            return true;
        }

        public async Task<bool> DeleteTariffCodes(string TarriffCode)
        {
            var builder = Builders<PendingImportDB>.Filter;
            var filter = builder.Eq("IMPORT_TARIFF", TarriffCode);
            var db = new DBContext();

            await db.PendingImportDB.DeleteManyAsync(filter);

            return true;
        }

        public async Task<bool> DeleteMarketCodes(string MarketCode)
        {
            var builder = Builders<PendingImportDB>.Filter;
            var filter = builder.Eq("MC_GEO", MarketCode);
            var db = new DBContext();

            await db.PendingImportDB.DeleteManyAsync(filter);

            return true;
        }

        public async Task<bool> DeleteByBatchNumber(int BatchNumber)
        {
            var builder = Builders<PendingImportDB>.Filter;
            var filter = builder.Eq("BATCH_NO", BatchNumber);
            var db = new DBContext();

            await db.PendingImportDB.DeleteManyAsync(filter);

            return true;
        }

        public async Task<List<ChinaUnitsofMeasurementModel>> GetChinaUnitsOfMeasure()
        {
            var db = new DBContext();
            var cursor = await db.ChinaUnitsofMeasurementDB.FindAsync(new BsonDocument());

            IList<ChinaUnitsofMeasurementDB> results = cursor.ToList();
            var modelList = new List<ChinaUnitsofMeasurementModel>();

            foreach (var Item in results)
            {

                var model = new ChinaUnitsofMeasurementModel
                {
                    _id = Item._id.ToString(),
                    Codes = Item.Codes,
                    NameOfUnits = Item.NameOfUnits

                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x.Codes).ToList();
        }

        public async Task<YTD_MODEL> CalculateYTD(PendingImportModel model)
        {
            var returnModel = new YTD_MODEL { YTD_MONETARY_VALUE = 0, YTD_WEIGHT = 0 };
            try
            {
                string szStartTimeId = model.YEAR + "10100";
                int SearchTimeID = int.Parse(szStartTimeId);

                var db = new DBContext();
                IMongoCollection<BsonDocument> collection = db._database.GetCollection<BsonDocument>("TradeDataDB");

                var options = new AggregateOptions()
                {
                    AllowDiskUse = true
                };

                PipelineDefinition<BsonDocument, BsonDocument> pipeline = new BsonDocument[]
                {
                new BsonDocument("$match", new BsonDocument()
                        .Add("TIME_ID", new BsonDocument()
                                .Add("$gte", SearchTimeID)
                        )),
                new BsonDocument("$match", new BsonDocument()
                        .Add("SC_GEO", model.SC_GEO)),
                new BsonDocument("$match", new BsonDocument()
                        .Add("MC_GEO", model.MC_GEO)),
                new BsonDocument("$match", new BsonDocument()
                        .Add("IMPORT_TARIFF", model.IMPORT_TARIFF)),
                new BsonDocument("$match", new BsonDocument()
                        .Add("SIDE_OF_TRADE", model.SIDE_OF_TRADE)),
                new BsonDocument("$group", new BsonDocument()
                        .Add("_id", BsonNull.Value)
                        .Add("YTD_WEIGHT", new BsonDocument()
                                .Add("$sum", "$WEIGHT")
                        )
                        .Add("YTD_MONETARY_VALUE", new BsonDocument()
                                .Add("$sum", "$MONETARY_VALUE")
                        ))
                };

                using (var cursor = await collection.AggregateAsync(pipeline, options))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        var batch = cursor.Current;
                        foreach (BsonDocument document in batch)
                        {
                            // Console.WriteLine(document.ToJson());
                            returnModel = JsonConvert.DeserializeObject<YTD_MODEL>(document.ToJson());

                        }
                    }
                }
            }
            catch(Exception e)
            {
                var eSrv = new ErrorService();
                var eModel = new ErrorModel { Code = "988", Class = "ValidationEngineService Line 427", ErrorMessage = e.Message };
                await eSrv.UpdateError(eModel);
            }
            return returnModel;
        }
     }

}

public class YTD_MODEL
{
    public object _id { get; set; }
    public double YTD_WEIGHT { get; set; }
    public double YTD_MONETARY_VALUE { get; set; }
}

