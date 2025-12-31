using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Services
{
    public class GeneralReporstService
    {
        public async Task<TradeDataJsonModel> GetUKPlateReportData(int year)
        {

            var db = new DBContext();
            IMongoCollection<BsonDocument> collection = db._database.GetCollection<BsonDocument>("TradeDataDB");
      
            var options = new AggregateOptions()
            {
                AllowDiskUse = true
            };

            PipelineDefinition<BsonDocument, BsonDocument> pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument()
                        .Add("YEAR", year)),
                new BsonDocument("$match", new BsonDocument()
                        .Add("MONTH", new BsonDocument()
                                .Add("$in", new BsonArray()
                                        .Add(1)
                                        .Add(2)
                                        .Add(3)
                                        .Add(4)
                                        .Add(5)
                                        .Add(6)
                                        .Add(7)
                                        .Add(8)
                                        .Add(9)
                                        .Add(10)
                                        .Add(11)
                                        .Add(12)
                                )
                        )),
                new BsonDocument("$match", new BsonDocument()
                        .Add("IMPORT_TARIFF", new BsonDocument()
                                .Add("$in", new BsonArray()
                                        .Add("72085120000")
                                        .Add("72085191000")
                                        .Add("72085198000")
                                        .Add("72085291000")
                                        .Add("72085299000")
                                        .Add("72254012000")
                                        .Add("72254040000")
                                        .Add("72254060000")
                                        .Add("72254090000")
                                )
                        ))
            };

            var data = new List<TradeDataModel>();
            // var cursor = await collection.AggregateAsync(pipeline, options);
            // var results = cursor as List<TradeDataDB>;

            using (var cursor = await collection.AggregateAsync(pipeline, options))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        var dModel = new TradeDataDB();
                        dModel = BsonSerializer.Deserialize<TradeDataDB>(document);

                        var lModel = new TradeDataModel
                        {
                            _id = dModel._id.ToString(),
                            ANZSIC = dModel.ANZSIC,
                            APPENDED = dModel.APPENDED,
                            BATCH_NO = dModel.BATCH_NO,
                            COO_GEO_CODE = dModel.COO_GEO_CODE,
                            CUSTOMS_DISTRICT = dModel.CUSTOMS_DISTRICT,
                            CUSTOMS_VALUE = dModel.CUSTOMS_VALUE,
                            CWC_GEO_CODE = dModel.CWC_GEO_CODE,
                            ECONOMIC_CATEGORY = dModel.ECONOMIC_CATEGORY,
                            ESTIMATED = dModel.ESTIMATED,
                            FOB = dModel.FOB,
                            H_TARIFF = dModel.H_TARIFF,
                            ID = dModel.ID,
                            IMPORT_TARIFF = dModel.IMPORT_TARIFF,
                            IMP_QTY = dModel.IMP_QTY,
                            IMP_UNIT = dModel.IMP_UNIT,
                            IMP_VALUE = dModel.IMP_VALUE,
                            MARKET_COUNTRY_ID = dModel.MARKET_COUNTRY_ID,
                            MC_GEO = dModel.MC_GEO,
                            MONETARY_VALUE = dModel.MONETARY_VALUE,
                            MONTH = dModel.MONTH,
                            MTH_EUR = dModel.MTH_EUR,
                            MTH_USD = dModel.MTH_USD,
                            NZ_PORT = dModel.NZ_PORT,
                            PORT_ID = dModel.PORT_ID,
                            PORT_OF_DISCHARGE = dModel.PORT_OF_DISCHARGE,
                            PORT_OF_LANDING = dModel.PORT_OF_LANDING,
                            PORT_OF_LOADING = dModel.PORT_OF_LOADING,
                            QUARTER = dModel.QUARTER,
                            SC_GEO = dModel.SC_GEO,
                            SIDE_OF_TRADE = dModel.SIDE_OF_TRADE,
                            SITC = dModel.SITC,
                            SOURCE_COUNTRY_ID = dModel.SOURCE_COUNTRY_ID,
                            STATE = dModel.STATE,
                            TARIFF_ID = dModel.TARIFF_ID,
                            TIME_ID = dModel.TIME_ID,
                            TRADER_LOCATION = dModel.TRADER_LOCATION,
                            TRANSPORT_MODE = dModel.TRANSPORT_MODE,
                            WEIGHT = dModel.WEIGHT,
                            YEAR = dModel.YEAR,
                            YTD_EUR = dModel.YTD_EUR,
                            YTD_MONETARY_VALUE = dModel.YTD_MONETARY_VALUE,
                            YTD_USD = dModel.YTD_USD,
                            YTD_WEIGHT = dModel.YTD_WEIGHT

                        };


                        data.Add(lModel);
                    }
                }
            }

            var dataModel = new TradeDataJsonModel
            {
                TradeData = data

            };

           
            return dataModel;
        }
    }
}
