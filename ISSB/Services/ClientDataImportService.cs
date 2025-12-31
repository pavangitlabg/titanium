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
    public class ClientDataImportService
    {
        public async Task<List<ClientDataImportModel>> GetImportClients()
        {
            var db = new DBContext();
            var cursor = await db.ClientDataImportDB.FindAsync(new BsonDocument());

            IList<ClientDataImportDB> results = cursor.ToList();
            var modelList = new List<ClientDataImportModel>();

            foreach (var Item in results)
            {
                var modelTransactions = new List<ClientDataImportTransactionsModel>();

                foreach (var ListItem in Item.Transactions)
                {
                    var mListItem = new ClientDataImportTransactionsModel
                    {
                        BatchNumber = ListItem.BatchNumber,
                        Date = ListItem.Date,
                        FileName = ListItem.FileName,
                        FileType = ListItem.FileType,
                        Month = ListItem.Month,
                        Year = ListItem.Year
                    };
                    modelTransactions.Add(mListItem);
                }

                var model = new ClientDataImportModel
                {
                    _id = Item._id.ToString(),
                    Name = Item.Name,
                    Address1 = Item.Address1,
                    Address2 = Item.Address2,
                    Address3 = Item.Address3,
                    Address4 = Item.Address4,
                    Address5 = Item.Address5,
                    Email = Item.Email,
                    Fax = Item.Fax,
                    WebSite = Item.WebSite,
                    Notes = Item.Notes,
                    PostCode = Item.PostCode,
                    SearchString = Item.SearchString,
                    Telephone = Item.Telephone,
                    FileExtenstion = Item.FileExtenstion,
                    Header = Item.Header,
                    Data = Item.Data,
                    Trailer = Item.Trailer,
                    WeightType = Item.WeightType,
                    CountryMapping = Item.CountryMapping,
                    CurrencyCode = Item.CurrencyCode,
                    Transactions = modelTransactions,
                    ImportType = Item.ImportType,
                    SourceGEO = Item.SourceGEO,
                    LastModified = Item.LastModified
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }

        public async Task<ClientDataImportModel> GetClientByID(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var builder = Builders<ClientDataImportDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.ClientDataImportDB.FindAsync(filter);

            IList<ClientDataImportDB> results = cursor.ToList();

            var Item = results[0];

            var modelTransactions = new List<ClientDataImportTransactionsModel>();

            foreach (var ListItem in Item.Transactions)
            {
                var mListItem = new ClientDataImportTransactionsModel
                {
                    BatchNumber = ListItem.BatchNumber,
                    Date = ListItem.Date,
                    FileName = ListItem.FileName,
                    FileType = ListItem.FileType,
                    Month = ListItem.Month,
                    Year = ListItem.Year
                };
                modelTransactions.Add(mListItem);
            }



            var model = new ClientDataImportModel
            {
                _id = Item._id.ToString(),
                Name = Item.Name,
                Address1 = Item.Address1,
                Address2 = Item.Address2,
                Address3 = Item.Address3,
                Address4 = Item.Address4,
                Address5 = Item.Address5,
                Email = Item.Email,
                Fax = Item.Fax,
                WebSite = Item.WebSite,
                Notes = Item.Notes,
                PostCode = Item.PostCode,
                SearchString = Item.SearchString,
                Telephone = Item.Telephone,
                FileExtenstion = Item.FileExtenstion,
                Header = Item.Header,
                Data = Item.Data,
                Trailer = Item.Trailer,
                WeightType = Item.WeightType,
                CurrencyCode = Item.CurrencyCode,
                RegionCode = Item.RegionCode,
                CountryMapping = Item.CountryMapping,
                Transactions = modelTransactions,
                ImportType = Item.ImportType,
                SourceGEO = Item.SourceGEO,
                LastModified = Item.LastModified

            };

            return model;
        }

        public async Task<ClientDataImportModel> GetClientBySearchString(string SearchString)
        {
            
            var builder = Builders<ClientDataImportDB>.Filter;

            var filter = builder.Eq("SearchString", SearchString);

            var db = new DBContext();
            var cursor = await db.ClientDataImportDB.FindAsync(filter);

            IList<ClientDataImportDB> results = cursor.ToList();

            var Item = results[0];

            var modelTransactions = new List<ClientDataImportTransactionsModel>();

            foreach (var ListItem in Item.Transactions)
            {
                var mListItem = new ClientDataImportTransactionsModel
                {
                    BatchNumber = ListItem.BatchNumber,
                    Date = ListItem.Date,
                    FileName = ListItem.FileName,
                    FileType = ListItem.FileType,
                    Month = ListItem.Month,
                    Year = ListItem.Year
                };
                modelTransactions.Add(mListItem);
            }



            var model = new ClientDataImportModel
            {
                _id = Item._id.ToString(),
                Name = Item.Name,
                Address1 = Item.Address1,
                Address2 = Item.Address2,
                Address3 = Item.Address3,
                Address4 = Item.Address4,
                Address5 = Item.Address5,
                Email = Item.Email,
                Fax = Item.Fax,
                WebSite = Item.WebSite,
                Notes = Item.Notes,
                PostCode = Item.PostCode,
                SearchString = Item.SearchString,
                Telephone = Item.Telephone,
                FileExtenstion = Item.FileExtenstion,
                Header = Item.Header,
                Data = Item.Data,
                Trailer = Item.Trailer,
                WeightType = Item.WeightType,
                CurrencyCode = Item.CurrencyCode,
                RegionCode = Item.RegionCode,
                CountryMapping = Item.CountryMapping,
                Transactions = modelTransactions,
                ImportType = Item.ImportType,
                SourceGEO = Item.SourceGEO

            };

            return model;
        }

        public async Task<bool> Add(ClientDataImportModel model)
        {
            var db = new DBContext();

            var modelTransactions = new List<ClientDataImportTransactionsDB>();

            foreach (var ListItem in model.Transactions)
            {
                var mListItem = new ClientDataImportTransactionsDB
                {
                    BatchNumber = ListItem.BatchNumber,
                    Date = ListItem.Date,
                    FileName = ListItem.FileName,
                    FileType = ListItem.FileType,
                    Month = ListItem.Month,
                    Year = ListItem.Year
                };
                modelTransactions.Add(mListItem);
            }

            var modelDB = new ClientDataImportDB
            {
                Name = model.Name,
                Address1 = model.Address1,
                Address2 = model.Address2,
                Address3 = model.Address3,
                Address4 = model.Address4,
                Address5 = model.Address5,
                Email = model.Email,
                Fax = model.Fax,
                WebSite = model.WebSite,
                Notes = model.Notes,
                PostCode = model.PostCode,
                SearchString = model.SearchString,
                Telephone = model.Telephone,
                FileExtenstion = model.FileExtenstion,
                Header = model.Header,
                Data = model.Data,
                Trailer = model.Trailer,
                WeightType = model.WeightType,
                CountryMapping = model.CountryMapping,
                CurrencyCode = model.CurrencyCode,
                RegionCode = model.RegionCode,
                Transactions = modelTransactions,
                LastModified = model.LastModified,
                ImportType = model.ImportType,
                SourceGEO = model.SourceGEO

            };

            await db.ClientDataImportDB.InsertOneAsync(modelDB);

            return true;
        }

        public async Task<ClientDataImportDB> SaveClient(ClientDataImportModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<ClientDataImportDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.ClientDataImportDB.FindAsync(filter);
            IList<ClientDataImportDB> results = cursor.ToList();

            var Item = results[0];

            var modelTransactions = new List<ClientDataImportTransactionsDB>();

            foreach (var ListItem in model.Transactions)
            {
                var mListItem = new ClientDataImportTransactionsDB
                {
                    BatchNumber = ListItem.BatchNumber,
                    Date = ListItem.Date,
                    FileName = ListItem.FileName,
                    FileType = ListItem.FileType,
                    Month = ListItem.Month,
                    Year = ListItem.Year
                };
                modelTransactions.Add(mListItem);
            }

            var modelDB = new ClientDataImportDB
            {
                _id = Item._id,
                Name = model.Name,
                Address1 = model.Address1,
                Address2 = model.Address2,
                Address3 = model.Address3,
                Address4 = model.Address4,
                Address5 = model.Address5,
                Email = model.Email,
                Fax = model.Fax,
                WebSite = model.WebSite,
                Notes = model.Notes,
                PostCode = model.PostCode,
                SearchString = model.SearchString,
                Telephone = model.Telephone,
                FileExtenstion = model.FileExtenstion,
                Header = model.Header,
                Data = model.Data,
                Trailer = model.Trailer,
                Transactions = modelTransactions,
                WeightType = model.WeightType,
                CurrencyCode = model.CurrencyCode,
                RegionCode = model.RegionCode,
                CountryMapping = model.CountryMapping,
                LastModified = model.LastModified,
                ImportType = model.ImportType,
                SourceGEO = model.SourceGEO
                

            };

            var returnModel = await db.ClientDataImportDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;
        }

        public async Task<bool> Delete(ClientDataImportModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<ClientDataImportDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            await db.ClientDataImportDB.DeleteOneAsync(filter);

            return true;
        }
    }
}
