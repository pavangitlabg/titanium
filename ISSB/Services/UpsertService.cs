using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
    public class UpsertService
    {
        private readonly SemaphoreSlim semaphoreAmendments = new SemaphoreSlim(1, 1);

        public void PostAmendments(int BatchNumber,string GeoCode)
        {
            var srv = new ValidationEngineService();
            var tradeSrv = new TradeDataService();
            var LoggerSrv = new LoggerService();

            var LogModel = new LoggerModel {  Date = DateTime.Now, Message = "Started Amendments" };

            Task.Run(async () =>
            {
                try
                {
                    var sysCont = new SystemControlService();
                    var sysControlModel = await sysCont.GetSystemControl();
                    var PostBatchNumber = sysControlModel.ImportBatch;
                    PostBatchNumber++;
                    sysControlModel.ImportBatch = PostBatchNumber;
                    await sysCont.UpdateControl(sysControlModel);

                    await semaphoreAmendments.WaitAsync();
                    var pendingList = new List<PendingImportModel>();
                    await LoggerSrv.UpdateLogger(LogModel);
                    var amendList = await srv.GetPendingImportAmendments(BatchNumber);

                    foreach (var Item in amendList)
                    {
                        var TradeModel = await tradeSrv.GetTradeDataRecordForAmendments(Item.YEAR, Item.MONTH, Item.SOURCE_COUNTRY_ID, Item.MARKET_COUNTRY_ID, Item.TARIFF_ID);
                        if (TradeModel.BATCH_NO != 0)
                        {
                            double Weight = 0;
                            if (Item.WEIGHT > 0)
                            {
                                Weight = TradeModel.WEIGHT + Item.WEIGHT;
                                TradeModel.WEIGHT = Weight;

                                Weight = TradeModel.YTD_WEIGHT + Item.WEIGHT;
                                TradeModel.YTD_WEIGHT = Weight;
                            }
                            else
                            {
                                Weight = Item.WEIGHT *-1;
                                TradeModel.WEIGHT = (TradeModel.WEIGHT - Weight);

                                TradeModel.YTD_WEIGHT = (TradeModel.YTD_WEIGHT - Weight);
                            }

                            double Money = 0;
                            if (Item.MONETARY_VALUE > 0)
                            {
                                Money = TradeModel.MONETARY_VALUE + Item.MONETARY_VALUE;
                                Item.MONETARY_VALUE = Money;
                                Money = TradeModel.YTD_MONETARY_VALUE + Item.MONETARY_VALUE;
                                Item.YTD_MONETARY_VALUE = Money;
                            }
                            else
                            {
                                Money = Item.MONETARY_VALUE *-1;
                                TradeModel.MONETARY_VALUE = (TradeModel.MONETARY_VALUE - Money);

                                TradeModel.YTD_MONETARY_VALUE = (TradeModel.YTD_MONETARY_VALUE - Money);
                            }

                            string Changed = Item.WEIGHT.ToString() + "|" + Item.MONETARY_VALUE.ToString();
                            TradeModel.APPENDED = Changed;
                            await tradeSrv.Save(TradeModel);
                            await srv.DeleteAmendment(Item);
                        }

                        if (TradeModel.BATCH_NO == 0)
                        {
                            // Add to Validation
                            var importModel = new PendingImportModel
                            {
                                BATCH_NO = (int)PostBatchNumber,
                                SC_GEO = Item.SC_GEO,
                                H_TARIFF = Item.H_TARIFF,
                                COO_GEO_CODE = Item.COO_GEO_CODE,
                                CURRENCY_CODE = "GBP",
                                CWC_GEO_CODE = Item.CWC_GEO_CODE,
                                ESTIMATED = Item.ESTIMATED,
                                ID = Item.ID,
                                IMPORT_TARIFF = Item.IMPORT_TARIFF,
                                IMP_UNIT = Item.IMP_UNIT,
                                MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                                MC_GEO = Item.MC_GEO,
                                MONETARY_VALUE = Item.MONETARY_VALUE,
                                MONTH = Item.MONTH,
                                PORT_ALPHA = Item.PORT_ALPHA,
                                PORT_ID = Item.PORT_ID,
                                QUARTER = Item.QUARTER,
                                SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                                SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                                TARIFF_ID = Item.TARIFF_ID,
                                TIME_ID = Item.TIME_ID,
                                VALUE_PER_TONNE = Item.VALUE_PER_TONNE,
                                WEIGHT = Item.WEIGHT,
                                YEAR = Item.YEAR,
                                YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                                YTD_WEIGHT = Item.YTD_WEIGHT

                            };
                            pendingList.Add(importModel);
                        }

                    }

                    var varSrv = new ValidationEngineService();
                    string status = "Batch: " + PostBatchNumber + " UK Additions";

                    var headModel = new PendingImportHeaderModel
                    {
                        BATCH_NO = (int)PostBatchNumber,
                        DATE = DateTime.Now,
                        END_DATE = DateTime.Now,
                        FAIL = false,
                        IMPORT_TYPE = "UK Additions",
                        POSTED = false,
                        STATUS = status,
                        TIME_TAKEN = "",

                    };

                    await varSrv.AddHeader(headModel);

                    await varSrv.AddForValidation(pendingList);

                    var dataImpSrv = new DataImportService();

                    await dataImpSrv.ProcessValidation((int)PostBatchNumber, "CN");
                }
                catch (Exception e)
                {
                    var LogModelError = new LoggerModel { Date = DateTime.Now, Message = "Error In Amendments" };
                    await LoggerSrv.UpdateLogger(LogModelError);
                    var eSrv = new ErrorService();
                    var eModel = new ErrorModel { Code = "70", Class = "UpsertService Code Line 70", ErrorMessage = e.Message };
                    await eSrv.UpdateError(eModel);
                    semaphoreAmendments.Release();
                }
                finally
                {
                    var LogModelFinished = new LoggerModel { Date = DateTime.Now, Message = "Finished Amendments" };
                    await LoggerSrv.UpdateLogger(LogModelFinished);
                    await srv.DeleteAmendments(GeoCode);
                    semaphoreAmendments.Release();
                }
            });
        }



        public async Task<List<PendingImportHeaderModel>> GetHeaderRecords(bool Posted)
        {
            var builder = Builders<PendingImportHeaderDB>.Filter;
            var filter = builder.Eq("POSTED", Posted);

            var db = new DBContext();
            var cursor = await db.PendingImportHeaderDB.FindAsync(filter);

            IList<PendingImportHeaderDB> results = cursor.ToList();
            var modelList = new List<PendingImportHeaderModel>();

            foreach (var Item in results)
            {

                var dateOne = Item.DATE;
                var dateTwo = Item.END_DATE;

                var diff = dateTwo.Subtract(dateOne);
                var tTaken = String.Format("{0}:{1}:{2}", diff.Hours, diff.Minutes, diff.Seconds);

                var model = new PendingImportHeaderModel
                {
                    _id = Item._id.ToString(),
                    BATCH_NO = Item.BATCH_NO,
                    DATE = Item.DATE,
                    END_DATE = Item.END_DATE,
                    FAIL = Item.FAIL,
                    IMPORT_TYPE = Item.IMPORT_TYPE,
                    POSTED = Item.POSTED,
                    STATUS = Item.STATUS,
                    TIME_TAKEN = tTaken
                };

                modelList.Add(model);
            }

            return modelList.OrderByDescending(x => x._id).ToList();
        }

        public async Task<List<PendingImportModel>> GetPendingRecords(int BatchNo)
        {
            var builder = Builders<PendingImportDB>.Filter;
            var filter = builder.Eq("BATCH_NO", BatchNo);

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
                    MONETARY_VALUE = Math.Round(Item.MONETARY_VALUE, 0),
                    MONTH = Item.MONTH,
                    PORT_ALPHA = Item.PORT_ALPHA,
                    PORT_ID = Item.PORT_ID,
                    QUARTER = Item.QUARTER,
                    SC_GEO = Item.SC_GEO,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                    TARIFF_ID = Item.TARIFF_ID,
                    TIME_ID = Item.TIME_ID,
                    WEIGHT = Item.WEIGHT,
                    YEAR = Item.YEAR,
                    YTD_MONETARY_VALUE = Math.Round(Item.YTD_MONETARY_VALUE, 0),
                    YTD_WEIGHT = Item.YTD_WEIGHT
                };

                modelList.Add(model);
            }

            return modelList;
        }

        public async Task<List<PendingImportModel>> GetPendingRecordsTopOne(int BatchNo)
        {
            var builder = Builders<PendingImportDB>.Filter;
            var filter = builder.Eq("BATCH_NO", BatchNo);

            FindOptions<PendingImportDB> options = new FindOptions<PendingImportDB> { Limit = 1 };

            var db = new DBContext();
            var cursor = await db.PendingImportDB.FindAsync(filter, options);

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
                    MONETARY_VALUE = Math.Round(Item.MONETARY_VALUE, 0),
                    MONTH = Item.MONTH,
                    PORT_ALPHA = Item.PORT_ALPHA,
                    PORT_ID = Item.PORT_ID,
                    QUARTER = Item.QUARTER,
                    SC_GEO = Item.SC_GEO,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                    TARIFF_ID = Item.TARIFF_ID,
                    TIME_ID = Item.TIME_ID,
                    WEIGHT = Item.WEIGHT,
                    YEAR = Item.YEAR,
                    YTD_MONETARY_VALUE = Math.Round(Item.YTD_MONETARY_VALUE, 0),
                    YTD_WEIGHT = Item.YTD_WEIGHT
                };

                modelList.Add(model);
            }

            return modelList;
        }

        public async Task<List<PendingImportModel>> GetHowlerRecords(int BatchNo)
        {

            var cntlSrv = new SystemControlService();
            var cntl = await cntlSrv.GetSystemControl();
            double WeightLimit = cntl.HowlerWeight;
            double ValueLimit = cntl.HowlerValue;

            var builder = Builders<PendingImportDB>.Filter;
            var filter = builder.Eq("BATCH_NO", BatchNo) & builder.Gte("WEIGHT", WeightLimit) & builder.Gte("MONETARY_VALUE", ValueLimit);

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
                    MONETARY_VALUE = Math.Round(Item.MONETARY_VALUE, 0),
                    MONTH = Item.MONTH,
                    PORT_ALPHA = Item.PORT_ALPHA,
                    PORT_ID = Item.PORT_ID,
                    QUARTER = Item.QUARTER,
                    SC_GEO = Item.SC_GEO,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                    TARIFF_ID = Item.TARIFF_ID,
                    TIME_ID = Item.TIME_ID,
                    WEIGHT = Item.WEIGHT,
                    YEAR = Item.YEAR,
                    YTD_MONETARY_VALUE = Math.Round(Item.YTD_MONETARY_VALUE, 0),
                    YTD_WEIGHT = Item.YTD_WEIGHT,
                    VALUE_PER_TONNE = Math.Round(Item.MONETARY_VALUE / (Item.WEIGHT / 1000), 0)
                };

                modelList.Add(model);
            }

            return modelList;
        }

        public async Task<bool> UpsertToProduction(int BatchNo)
        {
            bool bReturn = true;

            try
            {
                var scSrv = new SourceCountryService();

                var builder = Builders<PendingImportDB>.Filter;
                var filter = builder.Eq("BATCH_NO", BatchNo);

                var db = new DBContext();
                var cursor = await db.PendingImportDB.FindAsync(filter);
                IList<PendingImportDB> results = cursor.ToList();
                var modelList = new List<TradeDataDB>();


                //Check if Data has been loaded

                var modelCheck = results[0];
                var builderCheck = Builders<TradeDataDB>.Filter;
                var filterCheck = builderCheck.Eq("TIME_ID", modelCheck.TIME_ID)
                    & builderCheck.Eq("SC_GEO", modelCheck.SC_GEO)
                    & builderCheck.Eq("MC_GEO", modelCheck.MC_GEO)
                    & builderCheck.Eq("IMPORT_TARIFF", modelCheck.IMPORT_TARIFF)
                    & builderCheck.Eq("SIDE_OF_TRADE", modelCheck.SIDE_OF_TRADE);

                var cursorCheck = await db.TradeDataDB.FindAsync(filterCheck);
                IList<TradeDataDB> resultsCheck = cursorCheck.ToList();

                if (resultsCheck.Count.Equals(1))
                {
                    return false;
                }

                var scList = new List<string>();

                foreach (var Item in results)
                {

                    var match = scList.FirstOrDefault(stringToCheck => stringToCheck.Contains(Item.SC_GEO));

                    if (match == null)
                    {
                        scList.Add(Item.SC_GEO);
                    }


                    var model = new TradeDataDB
                    {

                        BATCH_NO = Item.BATCH_NO,
                        COO_GEO_CODE = Item.COO_GEO_CODE,
                        STATE = Item.CURRENCY_CODE,
                        CWC_GEO_CODE = Item.CWC_GEO_CODE,
                        ESTIMATED = Item.ESTIMATED,
                        H_TARIFF = Item.H_TARIFF,
                        ID = Item.ID,
                        IMPORT_TARIFF = Item.IMPORT_TARIFF,
                        IMP_UNIT = Item.IMP_UNIT,
                        MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                        MC_GEO = Item.MC_GEO,
                        MONETARY_VALUE = Math.Round(Item.MONETARY_VALUE, 0),
                        MONTH = Item.MONTH,
                        SITC = Item.PORT_ALPHA,
                        PORT_ID = Item.PORT_ID,
                        QUARTER = Item.QUARTER,
                        SC_GEO = Item.SC_GEO,
                        SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                        SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                        TARIFF_ID = Item.TARIFF_ID,
                        TIME_ID = Item.TIME_ID,
                        WEIGHT = Item.WEIGHT,
                        YEAR = Item.YEAR,
                        YTD_MONETARY_VALUE = Math.Round(Item.YTD_MONETARY_VALUE, 0),
                        YTD_WEIGHT = Item.YTD_WEIGHT,
                        ANZSIC = "NA",
                        APPENDED = "NA",
                        CUSTOMS_DISTRICT = "NA",
                        CUSTOMS_VALUE = 0,
                        ECONOMIC_CATEGORY = "NA",
                        FOB = 0,
                        IMP_QTY = 0,
                        IMP_VALUE = 0,
                        MTH_EUR = 0,
                        MTH_USD = 0,
                        NZ_PORT = "NA",
                        PORT_OF_DISCHARGE = 0,
                        PORT_OF_LANDING = 0,
                        PORT_OF_LOADING = 0,
                        TRADER_LOCATION = "NA",
                        TRANSPORT_MODE = "NA",
                        YTD_EUR = 0,
                        YTD_USD = 0
                    };

                    modelList.Add(model);
                }

                await db.TradeDataDB.InsertManyAsync(modelList);

                //Update last updated Date in Source Country
                foreach (var GEO in scList)
                {
                    var scModel = await scSrv.GetSourceCountryByGeoCode(GEO);
                    scModel.LATEST_DATE = DateTime.Now;
                    await scSrv.SaveSourceCountry(scModel);
                }

            }
            catch (Exception e)
            {
                var eSrv = new ErrorService();
                var eModel = new ErrorModel { Code = "989", Class = "UpsertService Line 173", ErrorMessage = e.Message };
                await eSrv.UpdateError(eModel);
                bReturn = false;
            }

            return bReturn;
        }

        public async Task<PendingImportHeaderModel> GetHeaderById(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<PendingImportHeaderDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.PendingImportHeaderDB.FindAsync(filter);

            IList<PendingImportHeaderDB> results = cursor.ToList();

            var Item = results[0];

            var model = new PendingImportHeaderModel
            {
                _id = Item._id.ToString(),
                BATCH_NO = Item.BATCH_NO,
                DATE = Item.DATE,
                END_DATE = Item.END_DATE,
                FAIL = Item.FAIL,
                IMPORT_TYPE = Item.IMPORT_TYPE,
                POSTED = Item.POSTED,
                STATUS = Item.STATUS

            };

            return model;
        }

        public async Task<PendingImportModel> GetMainById(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<PendingImportDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.PendingImportDB.FindAsync(filter);

            IList<PendingImportDB> results = cursor.ToList();

            var Item = results[0];

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
                MONETARY_VALUE = Math.Round(Item.MONETARY_VALUE, 0),
                MONTH = Item.MONTH,
                PORT_ALPHA = Item.PORT_ALPHA,
                PORT_ID = Item.PORT_ID,
                QUARTER = Item.QUARTER,
                SC_GEO = Item.SC_GEO,
                SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                TARIFF_ID = Item.TARIFF_ID,
                TIME_ID = Item.TIME_ID,
                WEIGHT = Item.WEIGHT,
                YEAR = Item.YEAR,
                YTD_MONETARY_VALUE = Math.Round(Item.YTD_MONETARY_VALUE, 0),
                YTD_WEIGHT = Item.YTD_WEIGHT

            };

            return model;
        }


        public async Task<PendingImportHeaderDB> Save(PendingImportHeaderModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<PendingImportHeaderDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.PendingImportHeaderDB.FindAsync(filter);
            IList<PendingImportHeaderDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new PendingImportHeaderDB
            {
                _id = Item._id,
                BATCH_NO = model.BATCH_NO,
                DATE = model.DATE,
                END_DATE = model.END_DATE,
                FAIL = model.FAIL,
                IMPORT_TYPE = model.IMPORT_TYPE,
                POSTED = model.POSTED,
                STATUS = model.STATUS
            };

            var returnModel = await db.PendingImportHeaderDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;

        }

        public async Task<PendingImportDB> SaveMain(PendingImportModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<PendingImportDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.PendingImportDB.FindAsync(filter);
            IList<PendingImportDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new PendingImportDB
            {
                _id = Item._id,
                BATCH_NO = model.BATCH_NO,
                COO_GEO_CODE = model.COO_GEO_CODE,
                CURRENCY_CODE = model.CURRENCY_CODE,
                CWC_GEO_CODE = model.CWC_GEO_CODE,
                ESTIMATED = model.ESTIMATED,
                H_TARIFF = model.H_TARIFF,
                ID = model.ID,
                IMPORT_TARIFF = model.IMPORT_TARIFF,
                IMP_UNIT = model.IMP_UNIT,
                MARKET_COUNTRY_ID = model.MARKET_COUNTRY_ID,
                MC_GEO = model.MC_GEO,
                MONETARY_VALUE = Math.Round(model.MONETARY_VALUE, 0),
                MONTH = model.MONTH,
                PORT_ALPHA = model.PORT_ALPHA,
                PORT_ID = model.PORT_ID,
                QUARTER = model.QUARTER,
                SC_GEO = model.SC_GEO,
                SIDE_OF_TRADE = model.SIDE_OF_TRADE,
                SOURCE_COUNTRY_ID = model.SOURCE_COUNTRY_ID,
                TARIFF_ID = model.TARIFF_ID,
                TIME_ID = model.TIME_ID,
                WEIGHT = model.WEIGHT,
                YEAR = model.YEAR,
                YTD_MONETARY_VALUE = Math.Round(model.YTD_MONETARY_VALUE, 0),
                YTD_WEIGHT = model.YTD_WEIGHT

            };

            var returnModel = await db.PendingImportDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;

        }

        public async Task<bool> Delete(PendingImportHeaderModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<PendingImportHeaderDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            await db.PendingImportHeaderDB.DeleteOneAsync(filter);

            var builder1 = Builders<PendingImportDB>.Filter;
            var filter1 = builder1.Eq("BATCH_NO", model.BATCH_NO);
            await db.PendingImportDB.DeleteManyAsync(filter1);


            return true;
        }

        public async Task<bool> DeleteProduction(int Batch)
        {
            var db = new DBContext();
            var builder = Builders<TradeDataDB>.Filter;
            var filter = builder.Eq("BATCH_NO", Batch);
            await db.TradeDataDB.DeleteManyAsync(filter);
            return true;
        }

        public async Task<bool> DeleteHowler(PendingImportModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<PendingImportDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            await db.PendingImportDB.DeleteOneAsync(filter);

            return true;
        }


    }
}
