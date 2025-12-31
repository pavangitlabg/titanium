using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Enums;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Services
{
    public class SystemEventService
    {
        public async Task<bool> UpdateLog(SystemEventLogsModel model)
        {
            bool bReturn;
            var db = new DBContext();

            var modelDB = new SystemEventLogsDB
            {
                DataType = model.DataType,
                TransactionType = model.TransactionType,
                DATE = model.DATE,
                USER = model.USER,
                MESSAGE = model.MESSAGE,
                Model = model.Model
            };

            await db.SystemEventLogsDB.InsertOneAsync(modelDB);
            bReturn = true;
            return bReturn;
        }

        public async Task<List<SystemEventLogsModel>> GetSystemEventLogs()
        {
            SystemEventLogsModel model;

            var projection = Builders<SystemEventLogsDB>.Projection
                  .Exclude(b => b.Model);
            var options = new FindOptions<SystemEventLogsDB, BsonDocument> { Projection = projection };

            var db = new DBContext();
            IList<SystemEventLogsDB> results = new List<SystemEventLogsDB>();

            var cursor = db.SystemEventLogsDB.FindAsync(new BsonDocument(), options).GetAwaiter().GetResult();
            IEnumerable<BsonDocument> batch;

            while (await cursor.MoveNextAsync())
            {
                batch = cursor.Current;

                foreach (BsonDocument document in batch)
                {
                    results.Add(BsonSerializer.Deserialize<SystemEventLogsDB>(document));
                }

            }

            var modelList = new List<SystemEventLogsModel>();
            foreach (var item in results)
            {
                model = new SystemEventLogsModel
                {
                    _id = item._id.ToString(),
                    DataType = item.DataType,
                    TransactionType = item.TransactionType,
                    DataTypeString = Enum.GetName(typeof(DataTypeEnums), item.DataType),
                    TransactionTypeString = Enum.GetName(typeof(TransactionTypeEnums), item.TransactionType),
                    DATE = item.DATE,
                    MESSAGE = item.MESSAGE,
                    USER = item.USER
                };

                modelList.Add(model);
            }

            return modelList.OrderByDescending(x => x._id).ToList();
        }

        public async Task<SystemEventLogsModel> GetSystemEventLogsById(string _id)
        {
            RegisterModels();
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var builder = Builders<SystemEventLogsDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();
            var cursor = await db.SystemEventLogsDB.FindAsync(filter);
            IList<SystemEventLogsDB> results = cursor.ToList();
            var Item = results[0];

            var model = new SystemEventLogsModel
            {
                _id = Item._id.ToString(),
                DataType = Item.DataType,
                TransactionType = Item.TransactionType,
                DATE = Item.DATE,
                MESSAGE = Item.MESSAGE,
                Model = Item.Model,
                USER = Item.USER
            };

            return model;
        }

        public async Task<bool> Delete(SystemEventLogsModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<SystemEventLogsDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.SystemEventLogsDB.DeleteOneAsync(filter);

            return true;
        }

        private void RegisterModels()
        {
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(UserModel)))
            {
                BsonClassMap.RegisterClassMap<UserModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(AssetModel)))
            {
                BsonClassMap.RegisterClassMap<AssetModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(MarketCountryModel)))
            {
                BsonClassMap.RegisterClassMap<MarketCountryModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(SourceCountryModel)))
            {
                BsonClassMap.RegisterClassMap<SourceCountryModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(ClientDataImportTransactionsModel)))
            {
                BsonClassMap.RegisterClassMap<ClientDataImportTransactionsModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(ClientDataImportModel)))
            {
                BsonClassMap.RegisterClassMap<ClientDataImportModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(DashBoardLayOutModel)))
            {
                BsonClassMap.RegisterClassMap<DashBoardLayOutModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(TradeDataModel)))
            {
                BsonClassMap.RegisterClassMap<TradeDataModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            //if (!BsonClassMap.IsClassMapRegistered(typeof(DataFileUploadModel)))
            //{
            //    BsonClassMap.RegisterClassMap<DataFileUploadModel>(cm =>
            //    {
            //        cm.AutoMap();
            //    });
            //}

            if (!BsonClassMap.IsClassMapRegistered(typeof(EmailCredentialsModel)))
            {
                BsonClassMap.RegisterClassMap<EmailCredentialsModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(ErrorModel)))
            {
                BsonClassMap.RegisterClassMap<ErrorModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(ImportFileTypeModel)))
            {
                BsonClassMap.RegisterClassMap<ImportFileTypeModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(MarketCountryExceptionModel)))
            {
                BsonClassMap.RegisterClassMap<MarketCountryExceptionModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(MarketCountryFilterListModel)))
            {
                BsonClassMap.RegisterClassMap<MarketCountryFilterListModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(MarketCountryFilterModel)))
            {
                BsonClassMap.RegisterClassMap<MarketCountryFilterModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(SourceCountryFilterListModel)))
            {
                BsonClassMap.RegisterClassMap<SourceCountryFilterListModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(SourceCountryFilterModel)))
            {
                BsonClassMap.RegisterClassMap<SourceCountryFilterModel>(cm =>
                {
                    cm.AutoMap();
                });
            }           

            if (!BsonClassMap.IsClassMapRegistered(typeof(PortsModel)))
            {
                BsonClassMap.RegisterClassMap<PortsModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(UserDashboardModel)))
            {
                BsonClassMap.RegisterClassMap<UserDashboardModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(ProductAllFilterModel)))
            {
                BsonClassMap.RegisterClassMap<ProductAllFilterModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(RegisterModel)))
            {
                BsonClassMap.RegisterClassMap<RegisterModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(ReportDesignerModel)))
            {
                BsonClassMap.RegisterClassMap<ReportDesignerModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(SalesProfileModel)))
            {
                BsonClassMap.RegisterClassMap<SalesProfileModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(SiteIssuesModel)))
            {
                BsonClassMap.RegisterClassMap<SiteIssuesModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(ReportDesignerModel)))
            {
                BsonClassMap.RegisterClassMap<ReportDesignerModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(SiteVisitorModel)))
            {
                BsonClassMap.RegisterClassMap<SiteVisitorModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(SourceCountryExceptionModel)))
            {
                BsonClassMap.RegisterClassMap<SourceCountryExceptionModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(SourceCountryFilterListModel)))
            {
                BsonClassMap.RegisterClassMap<SourceCountryFilterListModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(SourceCountryMappingModel)))
            {
                BsonClassMap.RegisterClassMap<SourceCountryMappingModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(SystemControlModel)))
            {
                BsonClassMap.RegisterClassMap<SystemControlModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(TariffModel)))
            {
                BsonClassMap.RegisterClassMap<TariffModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(TariffExceptionsModel)))
            {
                BsonClassMap.RegisterClassMap<TariffExceptionsModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(TariffIndexModel)))
            {
                BsonClassMap.RegisterClassMap<TariffIndexModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(PendingImportHeaderModel)))
            {
                BsonClassMap.RegisterClassMap<PendingImportHeaderModel>(cm =>
                {
                    cm.AutoMap();
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(ErrorModel)))
            {
                BsonClassMap.RegisterClassMap<ErrorModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(EmailMessagesModel)))
            {
                BsonClassMap.RegisterClassMap<EmailMessagesModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(SavedQueryModel)))
            {
                BsonClassMap.RegisterClassMap<SavedQueryModel>(cm =>
                {
                    cm.AutoMap();
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(CurrencyCodesModel)))
            {
                BsonClassMap.RegisterClassMap<CurrencyCodesModel>(cm =>
                {
                    cm.AutoMap();
                });
            }

        }

    }
}
