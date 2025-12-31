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
    public class QueryService
    {

        public async Task<SavedQueryModel> GetReportByIsVisitorDashboard(string User)
        {

            var filter = Builders<SavedQueryDB>.Filter.Eq(x => x.User, User) & Builders<SavedQueryDB>.Filter.Eq(z => z.SaveToVisitorPageIndex, true);
            var db = new DBContext();
            var cursor = await db.SavedQueryDB.FindAsync(filter);

            IList<SavedQueryDB> results = cursor.ToList();

            if (results.Count().Equals(0))
            {
                return new SavedQueryModel();
            }

            var Record = results[0];
            var sGeos = new List<SaveQuerySourceCountriesModel>();

            foreach (var Item in Record.SourceCountryGEOs)
            {
                var dGeo = new SaveQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                sGeos.Add(dGeo);
            }

            var mGeos = new List<SaveQueryMarketCountriesModel>();

            foreach (var Item in Record.MarketCountryGEOs)
            {
                var dGeo = new SaveQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                mGeos.Add(dGeo);
            }

            var prodList = new List<SaveQueryProductsModel>();
            foreach (var Item in Record.Products)
            {
                var pROD = new SaveQueryProductsModel { Product = Item.Product };
                prodList.Add(pROD);
            }

            var timeList = new List<SaveQueryDateModel>();
            foreach (var Item in Record.TimeIDs)
            {
                var dDate = new SaveQueryDateModel { TimeID = Item.TimeID };
                timeList.Add(dDate);
            }

            var portsList = new List<SaveQueryPortModel>();
            foreach (var Item in Record.Ports)
            {
                var pORT = new SaveQueryPortModel { PortID = Item.PortID };
                portsList.Add(pORT);
            }

            var model = new SavedQueryModel
            {

                Id = Record._id.ToString(),
                Comments = Record.Comments,
                Date = Record.Date,
                Description = Record.Description,
                GroupByMonth = Record.GroupByMonth,
                GroupByQuarter = Record.GroupByQuarter,
                GroupByYear = Record.GroupByYear,
                PortGroup = Record.PortGroup,
                IncludePorts = Record.IncludePorts,
                MarketCountryGroup = Record.MarketCountryGroup,
                Name = Record.Name,
                ProductGroup = Record.ProductGroup,
                ProductGroupType = Record.ProductGroupType,
                SourceCountryGroup = Record.SourceCountryGroup,
                TonnesValuesGroup = Record.TonnesValuesGroup,
                TradeFlowType = Record.TradeFlowType,
                User = Record.User,
                SourceCountryGEOs = sGeos,
                MarketCountryGEOs = mGeos,
                Products = prodList,
                TimeIDs = timeList,
                Ports = portsList,
                Product2DigitCode = Record.Product2DigitCode,
                ProductRegionCode = Record.ProductRegionCode,
                SaveToDashboard = Record.SaveToDashboard,
                SaveToVisitorPageIndex = Record.SaveToVisitorPageIndex,
                SelectedCurrency = Record.SelectedCurrency

            };

            return model;
        }

        public async Task<SavedQueryModel> GeDefaultHomePage()
        {

            var filter = Builders<SavedQueryDB>.Filter.Eq(z => z.SaveToVisitorPageIndex, true);
            var db = new DBContext();
            var cursor = await db.SavedQueryDB.FindAsync(filter);

            IList<SavedQueryDB> results = cursor.ToList();

            if (results.Count().Equals(0))
            {
                return new SavedQueryModel();
            }

            var Record = results[0];
            var sGeos = new List<SaveQuerySourceCountriesModel>();

            foreach (var Item in Record.SourceCountryGEOs)
            {
                var dGeo = new SaveQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                sGeos.Add(dGeo);
            }

            var mGeos = new List<SaveQueryMarketCountriesModel>();

            foreach (var Item in Record.MarketCountryGEOs)
            {
                var dGeo = new SaveQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                mGeos.Add(dGeo);
            }

            var prodList = new List<SaveQueryProductsModel>();
            foreach (var Item in Record.Products)
            {
                var pROD = new SaveQueryProductsModel { Product = Item.Product };
                prodList.Add(pROD);
            }

            var timeList = new List<SaveQueryDateModel>();
            foreach (var Item in Record.TimeIDs)
            {
                var dDate = new SaveQueryDateModel { TimeID = Item.TimeID };
                timeList.Add(dDate);
            }

            var portsList = new List<SaveQueryPortModel>();
            foreach (var Item in Record.Ports)
            {
                var pORT = new SaveQueryPortModel { PortID = Item.PortID };
                portsList.Add(pORT);
            }

            var model = new SavedQueryModel
            {

                Id = Record._id.ToString(),
                Comments = Record.Comments,
                Date = Record.Date,
                Description = Record.Description,
                GroupByMonth = Record.GroupByMonth,
                GroupByQuarter = Record.GroupByQuarter,
                GroupByYear = Record.GroupByYear,
                PortGroup = Record.PortGroup,
                IncludePorts = Record.IncludePorts,
                MarketCountryGroup = Record.MarketCountryGroup,
                Name = Record.Name,
                ProductGroup = Record.ProductGroup,
                ProductGroupType = Record.ProductGroupType,
                SourceCountryGroup = Record.SourceCountryGroup,
                TonnesValuesGroup = Record.TonnesValuesGroup,
                TradeFlowType = Record.TradeFlowType,
                User = Record.User,
                SourceCountryGEOs = sGeos,
                MarketCountryGEOs = mGeos,
                Products = prodList,
                TimeIDs = timeList,
                Ports = portsList,
                Product2DigitCode = Record.Product2DigitCode,
                ProductRegionCode = Record.ProductRegionCode,
                SaveToDashboard = Record.SaveToDashboard,
                SaveToVisitorPageIndex = Record.SaveToVisitorPageIndex,
                SelectedCurrency = Record.SelectedCurrency

            };

            return model;
        }

        public async Task<List<SavedQueryModel>> GetAllReportHeadersByUser(string User)
        {
            var builder = Builders<SavedQueryDB>.Filter;
            var filter = builder.Eq("User", User);

            var db = new DBContext();
            var cursor = await db.SavedQueryDB.FindAsync(filter);

            IList<SavedQueryDB> results = cursor.ToList();
            var modelList = new List<SavedQueryModel>();

            foreach (var Record in results)
            {
                var sGeos = new List<SaveQuerySourceCountriesModel>();

                foreach (var Item in Record.SourceCountryGEOs)
                {
                    var dGeo = new SaveQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                    sGeos.Add(dGeo);
                }

                var mGeos = new List<SaveQueryMarketCountriesModel>();

                foreach (var Item in Record.MarketCountryGEOs)
                {
                    var dGeo = new SaveQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                    mGeos.Add(dGeo);
                }

                var prodList = new List<SaveQueryProductsModel>();
                foreach (var Item in Record.Products)
                {
                    var pROD = new SaveQueryProductsModel { Product = Item.Product };
                    prodList.Add(pROD);
                }

                var timeList = new List<SaveQueryDateModel>();
                foreach (var Item in Record.TimeIDs)
                {
                    var dDate = new SaveQueryDateModel { TimeID = Item.TimeID };
                    timeList.Add(dDate);
                }

                var portsList = new List<SaveQueryPortModel>();
                foreach (var Item in Record.Ports)
                {
                    var pORT = new SaveQueryPortModel { PortID = Item.PortID };
                    portsList.Add(pORT);
                }

                var model = new SavedQueryModel
                {
                    Id = Record._id.ToString(),
                    Comments = Record.Comments,
                    Date = Record.Date,
                    Description = Record.Description,
                    GroupByMonth = Record.GroupByMonth,
                    GroupByQuarter = Record.GroupByQuarter,
                    GroupByYear = Record.GroupByYear,
                    IncludePorts = Record.IncludePorts,
                    PortGroup = Record.PortGroup,
                    MarketCountryGroup = Record.MarketCountryGroup,
                    Name = Record.Name,
                    ProductGroup = Record.ProductGroup,
                    ProductGroupType = Record.ProductGroupType,
                    SourceCountryGroup = Record.SourceCountryGroup,
                    TonnesValuesGroup = Record.TonnesValuesGroup,
                    TradeFlowType = Record.TradeFlowType,
                    User = Record.User,
                    SourceCountryGEOs = sGeos,
                    MarketCountryGEOs = mGeos,
                    Products = prodList,
                    TimeIDs = timeList,
                    Ports = portsList,
                    Product2DigitCode = Record.Product2DigitCode,
                    ProductRegionCode = Record.ProductRegionCode,
                    SaveToDashboard = Record.SaveToDashboard,
                    SaveToVisitorPageIndex = Record.SaveToVisitorPageIndex

                };

                modelList.Add(model);
            }

            return modelList.OrderByDescending(x => x.Date).ToList();
        }

        public async Task<List<SavedQueryModel>> GetSheduledReoports(int day)
        {
            var builder = Builders<SavedQueryDB>.Filter;
            var filter = builder.Eq("IsShedule", true) & builder.Eq("SelectedSheduleDay", day);

            var db = DBContext.Instance;
            var cursor = await db.SavedQueryDB.FindAsync(filter);

            IList<SavedQueryDB> results = cursor.ToList();
            var modelList = new List<SavedQueryModel>();

            foreach (var Record in results)
            {
                var sGeos = new List<SaveQuerySourceCountriesModel>();

                foreach (var Item in Record.SourceCountryGEOs)
                {
                    var dGeo = new SaveQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                    sGeos.Add(dGeo);
                }

                var mGeos = new List<SaveQueryMarketCountriesModel>();

                foreach (var Item in Record.MarketCountryGEOs)
                {
                    var dGeo = new SaveQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                    mGeos.Add(dGeo);
                }

                var prodList = new List<SaveQueryProductsModel>();
                foreach (var Item in Record.Products)
                {
                    var pROD = new SaveQueryProductsModel { Product = Item.Product };
                    prodList.Add(pROD);
                }

                var timeList = new List<SaveQueryDateModel>();
                foreach (var Item in Record.TimeIDs)
                {
                    var dDate = new SaveQueryDateModel { TimeID = Item.TimeID };
                    timeList.Add(dDate);
                }

                var portsList = new List<SaveQueryPortModel>();
                foreach (var Item in Record.Ports)
                {
                    var pORT = new SaveQueryPortModel { PortID = Item.PortID };
                    portsList.Add(pORT);
                }

                var model = new SavedQueryModel
                {
                    Id = Record._id.ToString(),
                    Comments = Record.Comments,
                    Date = Record.Date,
                    Description = Record.Description,
                    GroupByMonth = Record.GroupByMonth,
                    GroupByQuarter = Record.GroupByQuarter,
                    GroupByYear = Record.GroupByYear,
                    IncludePorts = Record.IncludePorts,
                    PortGroup = Record.PortGroup,
                    MarketCountryGroup = Record.MarketCountryGroup,
                    Name = Record.Name,
                    ProductGroup = Record.ProductGroup,
                    ProductGroupType = Record.ProductGroupType,
                    SourceCountryGroup = Record.SourceCountryGroup,
                    TonnesValuesGroup = Record.TonnesValuesGroup,
                    TradeFlowType = Record.TradeFlowType,
                    User = Record.User,
                    SourceCountryGEOs = sGeos,
                    MarketCountryGEOs = mGeos,
                    Products = prodList,
                    TimeIDs = timeList,
                    Ports = portsList,
                    Product2DigitCode = Record.Product2DigitCode,
                    ProductRegionCode = Record.ProductRegionCode,
                    SaveToDashboard = Record.SaveToDashboard,
                    SaveToVisitorPageIndex = Record.SaveToVisitorPageIndex

                };

                modelList.Add(model);
            }

            return modelList.OrderByDescending(x => x.Date).ToList();
        }


        public async Task<bool> Delete(string Id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(Id));
            var builder = Builders<SavedQueryDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();
            await db.SavedQueryDB.DeleteOneAsync(filter);

            return true;
        }


        public async Task<SavedQueryModel> GetReportByID(string User, string Id)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(Id));
            var filter = Builders<SavedQueryDB>.Filter.Eq(x => x.User, User) & Builders<SavedQueryDB>.Filter.Eq(z => z._id, RecordId);
            var db = new DBContext();
            var cursor = await db.SavedQueryDB.FindAsync(filter);

            IList<SavedQueryDB> results = cursor.ToList();

            if (results.Count().Equals(0))
            {
                return new SavedQueryModel();
            }

            var Record = results[0];
            var sGeos = new List<SaveQuerySourceCountriesModel>();

            foreach (var Item in Record.SourceCountryGEOs)
            {
                var dGeo = new SaveQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                sGeos.Add(dGeo);
            }

            var mGeos = new List<SaveQueryMarketCountriesModel>();

            foreach (var Item in Record.MarketCountryGEOs)
            {
                var dGeo = new SaveQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                mGeos.Add(dGeo);
            }

            var prodList = new List<SaveQueryProductsModel>();
            foreach (var Item in Record.Products)
            {
                var pROD = new SaveQueryProductsModel { Product = Item.Product };
                prodList.Add(pROD);
            }

            var timeList = new List<SaveQueryDateModel>();
            foreach (var Item in Record.TimeIDs)
            {
                var dDate = new SaveQueryDateModel { TimeID = Item.TimeID };
                timeList.Add(dDate);
            }

            var portsList = new List<SaveQueryPortModel>();
            foreach (var Item in Record.Ports)
            {
                var pORT = new SaveQueryPortModel { PortID = Item.PortID };
                portsList.Add(pORT);
            }

            var model = new SavedQueryModel
            {

                Id = Record._id.ToString(),
                Comments = Record.Comments,
                Date = Record.Date,
                Description = Record.Description,
                GroupByMonth = Record.GroupByMonth,
                GroupByQuarter = Record.GroupByQuarter,
                GroupByYear = Record.GroupByYear,
                PortGroup = Record.PortGroup,
                IncludePorts = Record.IncludePorts,
                MarketCountryGroup = Record.MarketCountryGroup,
                Name = Record.Name,
                ProductGroup = Record.ProductGroup,
                ProductGroupType = Record.ProductGroupType,
                SourceCountryGroup = Record.SourceCountryGroup,
                TonnesValuesGroup = Record.TonnesValuesGroup,
                TradeFlowType = Record.TradeFlowType,
                User = Record.User,
                SourceCountryGEOs = sGeos,
                MarketCountryGEOs = mGeos,
                Products = prodList,
                TimeIDs = timeList,
                Ports = portsList,
                Product2DigitCode = Record.Product2DigitCode,
                ProductRegionCode = Record.ProductRegionCode,
                SaveToDashboard = Record.SaveToDashboard,
                SaveToVisitorPageIndex = Record.SaveToVisitorPageIndex,
                SelectedCurrency = Record.SelectedCurrency,
                IsShedule = Record.IsShedule,
                SelectedSheduleDay = Record.SelectedSheduleDay

            };

            return model;
        }

        public async Task<string> GetReportIDFromName(string Name)
        {
            var filter = Builders<SavedQueryDB>.Filter.Eq(z => z.Name, Name);
            var db = new DBContext();
            var cursor = await db.SavedQueryDB.FindAsync(filter);

            IList<SavedQueryDB> results = cursor.ToList();

            if(results.Count >0)
            {
                return results[0]._id.ToString();
            }
            else
            {
                return "NotFound";
            }
        }


        public async Task<SavedQueryModel> GetReportByID(string Id)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(Id));
            var filter = Builders<SavedQueryDB>.Filter.Eq(z => z._id, RecordId);
            var db = new DBContext();
            var cursor = await db.SavedQueryDB.FindAsync(filter);

            IList<SavedQueryDB> results = cursor.ToList();

            if (results.Count == 0)
                return new SavedQueryModel();

            var Record = results[0];
            var sGeos = new List<SaveQuerySourceCountriesModel>();

            foreach (var Item in Record.SourceCountryGEOs)
            {
                var dGeo = new SaveQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                sGeos.Add(dGeo);
            }

            var mGeos = new List<SaveQueryMarketCountriesModel>();

            foreach (var Item in Record.MarketCountryGEOs)
            {
                var dGeo = new SaveQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                mGeos.Add(dGeo);
            }

            var prodList = new List<SaveQueryProductsModel>();
            foreach (var Item in Record.Products)
            {
                var pROD = new SaveQueryProductsModel { Product = Item.Product };
                prodList.Add(pROD);
            }

            var timeList = new List<SaveQueryDateModel>();
            foreach (var Item in Record.TimeIDs)
            {
                var dDate = new SaveQueryDateModel { TimeID = Item.TimeID };
                timeList.Add(dDate);
            }

            var portsList = new List<SaveQueryPortModel>();
            foreach (var Item in Record.Ports)
            {
                var pORT = new SaveQueryPortModel { PortID = Item.PortID };
                portsList.Add(pORT);
            }

            if(string.IsNullOrEmpty(Record.SelectedCurrency))
            {
                Record.SelectedCurrency = "GBP";
            }

            var model = new SavedQueryModel
            {
                Id = Record._id.ToString(),
                Comments = Record.Comments,
                Date = Record.Date,
                Description = Record.Description,
                GroupByMonth = Record.GroupByMonth,
                GroupByQuarter = Record.GroupByQuarter,
                GroupByYear = Record.GroupByYear,
                PortGroup = Record.PortGroup,
                IncludePorts = Record.IncludePorts,
                MarketCountryGroup = Record.MarketCountryGroup,
                Name = Record.Name,
                ProductGroup = Record.ProductGroup,
                ProductGroupType = Record.ProductGroupType,
                SourceCountryGroup = Record.SourceCountryGroup,
                TonnesValuesGroup = Record.TonnesValuesGroup,
                TradeFlowType = Record.TradeFlowType,
                User = Record.User,
                SourceCountryGEOs = sGeos,
                MarketCountryGEOs = mGeos,
                Products = prodList,
                TimeIDs = timeList,
                Ports = portsList,
                Product2DigitCode = Record.Product2DigitCode,
                ProductRegionCode = Record.ProductRegionCode,
                SaveToDashboard = Record.SaveToDashboard,
                SaveToVisitorPageIndex = Record.SaveToVisitorPageIndex,
                SelectedCurrency = Record.SelectedCurrency,
                

            };

            return model;
        }

        public async Task<SavedQueryModel> GetReportByNameX(string User, string Name)
        {

            var filter = Builders<SavedQueryDB>.Filter.Eq(x => x.User, User) & Builders<SavedQueryDB>.Filter.Eq(z => z.Name, Name);
            var db = new DBContext();
            var cursor = await db.SavedQueryDB.FindAsync(filter);

            IList<SavedQueryDB> results = cursor.ToList();

            var Record = results[0];
            var sGeos = new List<SaveQuerySourceCountriesModel>();

            foreach (var Item in Record.SourceCountryGEOs)
            {
                var dGeo = new SaveQuerySourceCountriesModel { GeoCode = Item.GeoCode };
                sGeos.Add(dGeo);
            }

            var mGeos = new List<SaveQueryMarketCountriesModel>();

            foreach (var Item in Record.MarketCountryGEOs)
            {
                var dGeo = new SaveQueryMarketCountriesModel { GeoCode = Item.GeoCode };
                mGeos.Add(dGeo);
            }

            var prodList = new List<SaveQueryProductsModel>();
            foreach (var Item in Record.Products)
            {
                var pROD = new SaveQueryProductsModel { Product = Item.Product };
                prodList.Add(pROD);
            }

            var timeList = new List<SaveQueryDateModel>();
            foreach (var Item in Record.TimeIDs)
            {
                var dDate = new SaveQueryDateModel { TimeID = Item.TimeID };
                timeList.Add(dDate);
            }

            var portsList = new List<SaveQueryPortModel>();
            foreach (var Item in Record.Ports)
            {
                var pORT = new SaveQueryPortModel { PortID = Item.PortID };
                portsList.Add(pORT);
            }

            var model = new SavedQueryModel
            {
                Id = Record._id.AsString,
                Comments = Record.Comments,
                Date = Record.Date,
                Description = Record.Description,
                GroupByMonth = Record.GroupByMonth,
                GroupByQuarter = Record.GroupByQuarter,
                GroupByYear = Record.GroupByYear,
                IncludePorts = Record.IncludePorts,
                PortGroup = Record.PortGroup,
                MarketCountryGroup = Record.MarketCountryGroup,
                Name = Record.Name,
                ProductGroup = Record.ProductGroup,
                ProductGroupType = Record.ProductGroupType,
                SourceCountryGroup = Record.SourceCountryGroup,
                TonnesValuesGroup = Record.TonnesValuesGroup,
                TradeFlowType = Record.TradeFlowType,
                User = Record.User,
                SourceCountryGEOs = sGeos,
                MarketCountryGEOs = mGeos,
                Products = prodList,
                TimeIDs = timeList,
                Ports = portsList,
                Product2DigitCode = Record.Product2DigitCode,
                ProductRegionCode = Record.ProductRegionCode,
                SaveToDashboard = Record.SaveToDashboard,
                SaveToVisitorPageIndex = Record.SaveToVisitorPageIndex,
                SelectedCurrency = Record.SelectedCurrency

            };

            return model;
        }
        public async Task<SavedQueryModel> InserNewQuery(SavedQueryModel model)
        {

            BsonObjectId RecordId;


            if (model.Id.Equals(string.Empty))
            {
                RecordId = new BsonObjectId(new ObjectId());
            }
            else
            {
                RecordId = new BsonObjectId(new ObjectId(model.Id));
            }


            var filter = Builders<SavedQueryDB>.Filter.Eq(x => x.User, model.User) & Builders<SavedQueryDB>.Filter.Eq(z => z._id, RecordId);
            var db = new DBContext();
            var cursor = await db.SavedQueryDB.FindAsync(filter);

            IList<SavedQueryDB> results = cursor.ToList();

            var sGeos = new List<SaveQuerySourceCountriesDB>();

            foreach (var Item in model.SourceCountryGEOs)
            {
                var dGeo = new SaveQuerySourceCountriesDB { GeoCode = Item.GeoCode };
                sGeos.Add(dGeo);
            }

            var mGeos = new List<SaveQueryMarketCountriesDB>();

            foreach (var Item in model.MarketCountryGEOs)
            {
                var dGeo = new SaveQueryMarketCountriesDB { GeoCode = Item.GeoCode };
                mGeos.Add(dGeo);
            }

            var prodList = new List<SaveQueryProductsDB>();
            foreach (var Item in model.Products)
            {
                var pROD = new SaveQueryProductsDB { Product = Item.Product };
                prodList.Add(pROD);
            }

            var timeList = new List<SaveQueryDateDB>();
            foreach (var Item in model.TimeIDs)
            {
                var dDate = new SaveQueryDateDB { TimeID = Item.TimeID };
                timeList.Add(dDate);
            }

            var portList = new List<SaveQueryPortDB>();
            foreach (var Item in model.Ports)
            {
                var pORT = new SaveQueryPortDB { PortID = Item.PortID };
                portList.Add(pORT);
            }

            var modelDB = new SavedQueryDB
            {
                _id = RecordId,
                Comments = model.Comments,
                Date = model.Date,
                Description = model.Description,
                GroupByMonth = model.GroupByMonth,
                GroupByQuarter = model.GroupByQuarter,
                GroupByYear = model.GroupByYear,
                IncludePorts = model.IncludePorts,
                PortGroup = model.PortGroup,
                MarketCountryGroup = model.MarketCountryGroup,
                Name = model.Name,
                ProductGroup = model.ProductGroup,
                ProductGroupType = model.ProductGroupType,
                SourceCountryGroup = model.SourceCountryGroup,
                TonnesValuesGroup = model.TonnesValuesGroup,
                TradeFlowType = model.TradeFlowType,
                User = model.User,
                SourceCountryGEOs = sGeos,
                MarketCountryGEOs = mGeos,
                Products = prodList,
                TimeIDs = timeList,
                Ports = portList,
                ProductRegionCode = model.ProductRegionCode,
                Product2DigitCode = model.Product2DigitCode,
                SaveToDashboard = model.SaveToDashboard,
                SaveToVisitorPageIndex = model.SaveToVisitorPageIndex,
                SelectedCurrency = model.SelectedCurrency,
                IsShedule = model.IsShedule,
                SelectedSheduleDay = model.SelectedSheduleDay
                 

            };

            var returnModel = new SavedQueryDB();
            if (results.Count == 0)
            {
                await db.SavedQueryDB.InsertOneAsync(modelDB);
            }
            else
            {

                await db.SavedQueryDB.FindOneAndReplaceAsync(filter, modelDB);

            }

            var rModel = await GetReportByID(modelDB._id.ToString());
            return rModel;
        }
        //Dashboard
        public async Task<bool> AddCacheReport(CachedReportsModel model)
        {
            var db = new DBContext();

            var modelDB = new CachedReportsDB
            {
                IsVisitorPage = model.IsVisitorPage,
                User = model.User,
                Model = model.Model
            };

            int bytesSize = modelDB.ToBson().Length;
            if (bytesSize < 16777216)
                await db.CachedReportsDB.InsertOneAsync(modelDB);

            return true;
        }

        public async Task<CachedReportsModel> GetMainIndexPage(bool IsVisitorPage)
        {

            var builder = Builders<CachedReportsDB>.Filter;

            var filter = builder.Eq("IsVisitorPage", IsVisitorPage);

            var db = new DBContext();
            var cursor = await db.CachedReportsDB.FindAsync(filter);

            IList<CachedReportsDB> results = cursor.ToList();

            var Item = results[0];

            var model = new CachedReportsModel
            {
                _id = Item._id.ToString(),
                IsVisitorPage = Item.IsVisitorPage,
                User = Item.User,
                Model = Item.Model

            };

            return model;
        }

        public async Task<bool> DeleteCache()
        {

            var builder = Builders<CachedReportsDB>.Filter;
            var filter = builder.Eq("IsVisitorPage", true);
            var db = new DBContext();
            await db.CachedReportsDB.DeleteManyAsync(filter);

            return true;
        }

        public async Task<List<DashBoardLinkModel>> GetDashBoardItems(string User)
        {
            var filter = Builders<SavedQueryDB>.Filter.Eq(x => x.User, User) & Builders<SavedQueryDB>.Filter.Eq(z => z.SaveToDashboard, true);
            var db = DBContext.Instance;

            var cursor = await db.SavedQueryDB.FindAsync(filter);
            IList<SavedQueryDB> results = cursor.ToList();
            var modelList = new List<DashBoardLinkModel>();

            foreach (var Record in results)
            {
                var model = new DashBoardLinkModel
                {
                    ReportId = "/ReportEngine/SearchResults?Id=" + Record._id.ToString(),
                    Name = Record.Name
                };

                modelList.Add(model);
            }

            return modelList;
        }

        public async Task<List<ReportCurrencyModel>> CurencyUsed()
        {
            var db = new DBContext();
            var cursor = await db.ReportCurrencyDB.FindAsync(new BsonDocument());

            IList<ReportCurrencyDB> results = cursor.ToList();
            var modelList = new List<ReportCurrencyModel>();

            foreach (var Item in results)
            {

                var model = new ReportCurrencyModel
                {
                    _id = Item._id.ToString(),
                    Sort = Item.Sort,
                    Code = Item.Code,
                    Description = Item.Description
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x.Sort).ToList(); 
        }
    }
}
