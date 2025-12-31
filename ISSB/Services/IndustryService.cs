using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Data;
using Data.DBModels;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Services
{
    public class IndustryService
    {
        private static IndustryService _instance;

        public static IndustryService Instance
        {
            set { }
            get
            {
                if (_instance == null)
                {
                    _instance = new IndustryService();
                }
                return _instance;
            }
        }

        public async Task<IndustryUserModel> LoginUser(string email, string password)
        {

            password = EncryptionHelper.Encrypt(password);

            //TODO Bug of the password encryption
            var filter = Builders<IndustryUserDB>.Filter.Eq(x => x.Email, email) & Builders<IndustryUserDB>.Filter.Eq(z => z.Password, password);

            var db = DBContext.Instance;
            var cursor = await db.IndustryUserDB.FindAsync(filter);

            IList<IndustryUserDB> results = cursor.ToList();

            if (results.Count == 0)
            {
                var rModel = new IndustryUserModel { IsLoggedIn = false };
                return rModel;
            }

            var ImgURL = string.Empty;
            if (string.IsNullOrEmpty(results[0].ImageURL))
                ImgURL = "../userimages/default.png";

            var LoggedInUser = results[0];
            var model = new IndustryUserModel
            {
                _id = LoggedInUser._id.ToString(),
                Email = LoggedInUser.Email,
                IsLoggedIn = true,
                Password = EncryptionHelper.Decrypt(LoggedInUser.Password),
                UserName = LoggedInUser.UserName,
                IsAdministrator = LoggedInUser.IsAdministrator,
                IsSystemUser = LoggedInUser.IsSystemUser,
                IsAdvancedUser = LoggedInUser.IsAdvancedUser,
                AccountStatus = LoggedInUser.AccountStatus,
                Address1 = LoggedInUser.Address1,
                Address2 = LoggedInUser.Address2,
                Address3 = LoggedInUser.Address3,
                Address4 = LoggedInUser.Address4,
                CompanyName = LoggedInUser.CompanyName,
                Costs = LoggedInUser.Costs,
                DateJoined = LoggedInUser.DateJoined,
                ExpiryDate = LoggedInUser.ExpiryDate,
                FirstName = LoggedInUser.FirstName,
                IsTradeInquiry = LoggedInUser.IsTradeInquiry,
                LastName = LoggedInUser.LastName,
                Mobile = LoggedInUser.Mobile,

                PostalCode = LoggedInUser.PostalCode,
                StartDate = LoggedInUser.StartDate,
                Telephone = LoggedInUser.Telephone,
                TwoFactorAuth = LoggedInUser.TwoFactorAuth,
                ImageURL = ImgURL

            };

            return model;
        }

        public async Task<IndustryUserModel> GetUser(string email)
        {
            var filter = Builders<IndustryUserDB>.Filter.Eq(x => x.Email, email);
            var db = DBContext.Instance;
            var cursor = await db.IndustryUserDB.FindAsync(filter);
            IList<IndustryUserDB> results = cursor.ToList();

            if (results.Count == 0)
            {
                return new IndustryUserModel();
            }

            var ImgURL = string.Empty;
            if (string.IsNullOrEmpty(results[0].ImageURL))
                ImgURL = ImgURL = "../userimages/default.png";

            var model = new IndustryUserModel
            {
                _id = results[0]._id.ToString(),
                IsSystemUser = results[0].IsSystemUser,
                Email = results[0].Email,
                Password = results[0].Password,
                UserName = results[0].UserName,

                DateJoined = results[0].DateJoined,
                IsAdministrator = results[0].IsAdministrator,
                IsTradeInquiry = results[0].IsTradeInquiry,
                IsAdvancedUser = results[0].IsAdvancedUser,
                StartDate = results[0].StartDate,
                ExpiryDate = results[0].ExpiryDate,
                FirstName = results[0].FirstName,
                LastName = results[0].LastName,
                CompanyName = results[0].CompanyName,
                Address1 = results[0].Address1,
                Address2 = results[0].Address2,
                Address3 = results[0].Address3,
                Address4 = results[0].Address4,
                PostalCode = results[0].PostalCode,
                Telephone = results[0].Telephone,
                Mobile = results[0].Mobile,
                Costs = results[0].Costs,
                AccountStatus = results[0].AccountStatus,
                TwoFactorAuth = results[0].TwoFactorAuth,
                EmailNotification = results[0].EmailNotification,
                ImageURL = ImgURL


            };
            return model;
        }

        public async Task<List<IndustryUserModel>> GetUsers()
        {
            var db = DBContext.Instance;
            var cursor = await db.IndustryUserDB.FindAsync(new BsonDocument());
            IList<IndustryUserDB> results = cursor.ToList();
            var modelList = new List<IndustryUserModel>();

            foreach (var Item in results)
            {

                if (string.IsNullOrEmpty(Item.ImageURL))
                    Item.ImageURL = "../userimages/default.png";

                var model = new IndustryUserModel
                {
                    _id = Item._id.ToString(),
                    Email = Item.Email,
                    Password = Item.Password,
                    UserName = Item.UserName,

                    DateJoined = Item.DateJoined,
                    IsAdministrator = Item.IsAdministrator,
                    IsTradeInquiry = Item.IsTradeInquiry,
                    IsSystemUser = Item.IsSystemUser,
                    StartDate = Item.StartDate,
                    ExpiryDate = Item.ExpiryDate,
                    FirstName = Item.FirstName,
                    LastName = Item.LastName,
                    CompanyName = Item.CompanyName,
                    Address1 = Item.Address1,
                    Address2 = Item.Address2,
                    Address3 = Item.Address3,
                    Address4 = Item.Address4,
                    PostalCode = Item.PostalCode,
                    Telephone = Item.Telephone,
                    Mobile = Item.Mobile,
                    Costs = Item.Costs,
                    AccountStatus = Item.AccountStatus,
                    TwoFactorAuth = Item.TwoFactorAuth,
                    EmailNotification = Item.EmailNotification,
                    ImageURL = Item.ImageURL
                };
                modelList.Add(model);
            }
            return modelList;
        }

        public async Task<IndustryUserModel> GetUsersById(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var builder = Builders<IndustryUserDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = DBContext.Instance;
            var cursor = await db.IndustryUserDB.FindAsync(filter);
            IList<IndustryUserDB> results = cursor.ToList();
            var Item = results[0];

            if (string.IsNullOrEmpty(Item.ImageURL))
                Item.ImageURL = "../userimages/default.png";

            var model = new IndustryUserModel
            {
                _id = Item._id.ToString(),
                Email = Item.Email,
                Password = Item.Password,
                UserName = Item.UserName,

                DateJoined = Item.DateJoined,
                IsAdministrator = Item.IsAdministrator,
                IsTradeInquiry = Item.IsTradeInquiry,
                IsSystemUser = Item.IsSystemUser,
                StartDate = Item.StartDate,
                ExpiryDate = Item.ExpiryDate,
                FirstName = Item.FirstName,
                LastName = Item.LastName,
                CompanyName = Item.CompanyName,
                Address1 = Item.Address1,
                Address2 = Item.Address2,
                Address3 = Item.Address3,
                Address4 = Item.Address4,
                PostalCode = Item.PostalCode,
                Telephone = Item.Telephone,
                Mobile = Item.Mobile,
                Costs = Item.Costs,
                AccountStatus = Item.AccountStatus,
                TwoFactorAuth = Item.TwoFactorAuth,
                EmailNotification = Item.EmailNotification,
                ImageURL = Item.ImageURL

            };

            return model;
        }

        public async Task<bool> Delete(IndustryUserModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = DBContext.Instance;
            var builder = Builders<IndustryUserDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var returnModel = await db.IndustryUserDB.DeleteOneAsync(filter);
            return true;
        }

        public async Task<bool> UpdateUser(IndustryUserModel model)
        {
            bool bReturn = false;

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var filter = Builders<IndustryUserDB>.Filter.Eq(x => x._id, RecordId);
            var db = DBContext.Instance;
            var cursor = await db.IndustryUserDB.FindAsync(filter);
            IList<IndustryUserDB> results = cursor.ToList();
            var modelDB = new IndustryUserDB
            {
                _id = results[0]._id,
                DateJoined = model.DateJoined,
                Email = model.Email,
                IsAdministrator = model.IsAdministrator,
                IsSystemUser = model.IsSystemUser,
                IsAdvancedUser = model.IsAdvancedUser,

                Password = model.Password,
                UserName = model.UserName,
                IsTradeInquiry = model.IsTradeInquiry,
                StartDate = model.StartDate,
                ExpiryDate = model.ExpiryDate,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CompanyName = model.CompanyName,
                Address1 = model.Address1,
                Address2 = model.Address2,
                Address3 = model.Address3,
                Address4 = model.Address4,
                PostalCode = model.PostalCode,
                Telephone = model.Telephone,
                Mobile = model.Mobile,
                Costs = model.Costs,
                AccountStatus = model.AccountStatus,
                TwoFactorAuth = model.TwoFactorAuth,

                EmailNotification = model.EmailNotification,
                ImageURL = model.ImageURL,
                BrandLink = model.BrandLink,
                NavBar = model.NavBar,
                SideBar = model.SideBar,

            };
            await db.IndustryUserDB.FindOneAndReplaceAsync(filter, modelDB);
            bReturn = true;
            return bReturn;
        }

        public async Task<bool> DoesExist(IndustryUserModel model)
        {
            bool bReturn = false;
            var filter = Builders<IndustryUserDB>.Filter.Eq(x => x.Email, model.Email);
            var db = DBContext.Instance;
            var cursor = await db.IndustryUserDB.FindAsync(filter);
            IList<IndustryUserDB> results = cursor.ToList();
            if (results.Count > 0)
                bReturn = true;
            return bReturn;
        }

        public async Task<bool> DoesEmailExist(string email)
        {
            bool bReturn = false;
            var filter = Builders<IndustryUserDB>.Filter.Eq(x => x.Email, email);
            var db = DBContext.Instance;
            var cursor = await db.IndustryUserDB.FindAsync(filter);
            IList<IndustryUserDB> results = cursor.ToList();
            if (results.Count > 0)
                bReturn = true;
            return bReturn;
        }

        public async Task<bool> AddUser(IndustryUserModel model)
        {
            bool bReturn = false;
            var db = DBContext.Instance;
            var modelDB = new IndustryUserDB
            {
                DateJoined = model.DateJoined,
                Email = model.Email,
                IsAdministrator = model.IsAdministrator,
                IsSystemUser = model.IsSystemUser,
                IsAdvancedUser = model.IsAdvancedUser,

                Password = EncryptionHelper.Encrypt(model.Password),
                UserName = model.UserName,
                IsTradeInquiry = model.IsTradeInquiry,
                StartDate = model.StartDate,
                ExpiryDate = model.ExpiryDate,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CompanyName = model.CompanyName,
                Address1 = model.Address1,
                Address2 = model.Address2,
                Address3 = model.Address3,
                Address4 = model.Address4,
                PostalCode = model.PostalCode,
                Telephone = model.Telephone,
                Mobile = model.Mobile,
                Costs = model.Costs,
                AccountStatus = model.AccountStatus,
                TwoFactorAuth = model.TwoFactorAuth,
                EmailNotification = model.EmailNotification,
                ImageURL = model.ImageURL,

            };
            await db.IndustryUserDB.InsertOneAsync(modelDB);
            bReturn = true;
            return bReturn;
        }

        public async Task<bool> ChangePassword(string email, string password)
        {
            bool bReturn = false;
            var filter = Builders<IndustryUserDB>.Filter.Eq(x => x.Email, email);
            var db = DBContext.Instance;
            var cursor = await db.IndustryUserDB.FindAsync(filter);
            IList<IndustryUserDB> results = cursor.ToList();
            if (results.Count > 0)
            {
                var newPassword = results[0];
                newPassword.Password = EncryptionHelper.Encrypt(password);
                await db.IndustryUserDB.FindOneAndReplaceAsync(filter, newPassword);
                bReturn = true;
            }
            return bReturn;
        }

        public async Task<int> GetCount()
        {
            var db = DBContext.Instance;
            var cursor = await db.IndustryUserDB.FindAsync(new BsonDocument());
            IList<IndustryUserDB> results = cursor.ToList();
            return results.Count;
        }

        public async Task<bool> AddFormType(FileUploadTypesModel model)
        {
            bool bReturn = false;
            var db = DBContext.Instance;
            var modelDB = new FileUploadTypesDB
            {
                Number = model.Number,
                Description = model.Description,

            };
            await db.FileUploadTypesDB.InsertOneAsync(modelDB);
            bReturn = true;
            return bReturn;
        }

        public async Task<List<FileUploadTypesModel>> GetFormTypes()
        {
            var db = DBContext.Instance;
            var cursor = await db.FileUploadTypesDB.FindAsync(new BsonDocument());

            IList<FileUploadTypesDB> results = cursor.ToList();
            var modelList = new List<FileUploadTypesModel>();

            foreach (var Item in results)
            {
                var model = new FileUploadTypesModel
                {
                    _id = Item._id.ToString(),
                    Description = Item.Description,
                    Number = Item.Number,

                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }

        public async Task<int> GetFormTypesBySource(int source)
        {
            var builder = Builders<FileUploadTypesDB>.Filter;
            var filter = builder.Eq("Number", source);

            var db = DBContext.Instance;
            var cursor = await db.FileUploadTypesDB.FindAsync(filter);

            IList<FileUploadTypesDB> results = cursor.ToList();
      
            return results.Count;
        }

        public async Task<List<IndustryMasterModel>> GetImportedFiles()
        {
            var db = DBContext.Instance;
            var cursor = await db.IndustryMasterDB.FindAsync(new BsonDocument());

            IList<IndustryMasterDB> results = cursor.ToList();
            var modelList = new List<IndustryMasterModel>();

            foreach (var Item in results)
            {
                var model = new IndustryMasterModel
                {
                    _id = Item._id.ToString(),
                    DataType = Item.DataType,
                    dateTime = Item.dateTime,
                    DataName = Item.DataType.ToString(),
                    FileName = Item.FileName,

                    Subject = Item.Subject
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }

        public async Task<IndustryMasterModel> GetImportedFileById(string _id)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var filter = Builders<IndustryMasterDB>.Filter.Eq(x => x._id, RecordId);

            var db = DBContext.Instance;
            var cursor = await db.IndustryMasterDB.FindAsync(filter);

            IList<IndustryMasterDB> results = cursor.ToList();
            var Item = results[0];

            var model = new IndustryMasterModel
            {
                _id = Item._id.ToString(),
                DataType = Item.DataType,
                dateTime = Item.dateTime,
                DataName = Item.DataType.ToString(),
                FileName = Item.FileName,
                Subject = Item.Subject
            };

            return model;
        }

        public async Task<List<IndustryColumnModel>> GetDataColumns()
        {
            var db = DBContext.Instance;
            var cursor = await db.IndustryColumnDB.FindAsync(new BsonDocument());

            IList<IndustryColumnDB> results = cursor.ToList();
            var modelList = new List<IndustryColumnModel>();

            foreach (var Item in results)
            {
                var model = new IndustryColumnModel
                {
                    _id = Item._id.ToString(),
                    Name = Item.Name,
                    Number = Item.Number
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }

        public async Task<List<IndustryRowModel>> GetDataRow()
        {
            var db = DBContext.Instance;
            var cursor = await db.IndustryRowDB.FindAsync(new BsonDocument());

            IList<IndustryRowDB> results = cursor.ToList();
            var modelList = new List<IndustryRowModel>();

            foreach (var Item in results)
            {
                var model = new IndustryRowModel
                {
                    _id = Item._id.ToString(),
                    Description = Item.Description,
                    Number = Item.Number
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }

        public async Task<string> InsertData(IndustryMasterModel model)
        {

            var db = DBContext.Instance;
            var modelDB = new IndustryMasterDB
            {
                dateTime = model.dateTime,
                FileName = model.FileName,

                DataType = model.DataType,
                Subject = model.Subject

            };

            await db.IndustryMasterDB.InsertOneAsync(modelDB);

            return modelDB._id.ToString();
        }

        public async Task<string> UpdateData(IndustryMasterModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var filter = Builders<IndustryMasterDB>.Filter.Eq(x => x._id, RecordId);

            var db = DBContext.Instance;
            var modelDB = new IndustryMasterDB
            {
                 _id = RecordId,
                dateTime = model.dateTime,
                FileName = model.FileName,

                DataType = model.DataType,
                Subject = model.Subject
                
            };

            await db.IndustryMasterDB.FindOneAndReplaceAsync(filter, modelDB);

            return modelDB._id.ToString();
        }

        public async Task<string> InsertTataData(IndDataModel model)
        {

            var dataList = new List<DataColsDB>();

            foreach (var Item in model.Datas)
            {
                var dModel = new DataColsDB
                {
                    Code = Item.Code,
                    Description = Item.Description,
                    ColCode = Item.ColCode,
                    ColValue = Item.ColValue
                };
                dataList.Add(dModel);
            }

            var db = DBContext.Instance;
            var modelDB = new IndDataDB
            {
                UploadDate = DateTime.Now,
                FormLink = model.FormLink,
                FormNumber = model.FormNumber,
                Name = model.Name,
                Number = model.Number,
                ReportingPeriod = model.ReportingPeriod,
                Month = model.Month,
                Year = model.Year,
                TabName = model.TabName,
                Datas = dataList
            };

            await db.IndDataDB.InsertOneAsync(modelDB);

            return modelDB._id.ToString();
        }

        public async Task<bool> Delete(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var db = DBContext.Instance;

            var builder = Builders<IndDataDB>.Filter;
            var filter = builder.Eq("FormLink", _id);
            await db.IndDataDB.DeleteManyAsync(filter);

            var builder1 = Builders<IndustryMasterDB>.Filter;
            var filter1 = builder1.Eq("_id", RecordId);

            await db.IndustryMasterDB.DeleteOneAsync(filter1);

            return true;
        }

        public async Task<bool> DeleteReport(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var db = DBContext.Instance;

            var builder = Builders<IndustrySavedReportsDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            await db.IndustrySavedReportsDB.DeleteOneAsync(filter);

            return true;
        }

        public async Task<List<IndDataModel>> GetIndustryData(string _id)
        {
            // BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var builder = Builders<IndDataDB>.Filter;
            var filter = builder.Eq("FormLink", _id);
            var db = DBContext.Instance;
            var cursor = await db.IndDataDB.FindAsync(filter);
            IList<IndDataDB> results = cursor.ToList();

            var returnList = new List<IndDataModel>();

            foreach (var Item in results)
            {
                var dataList = new List<DataCols>();

                foreach (var cols in Item.Datas)
                {
                    var colModel = new DataCols
                    {
                        Code = cols.Code,
                        ColCode = cols.ColCode,
                        ColValue = cols.ColValue,
                        Description = cols.Description
                    };
                    dataList.Add(colModel);
                }

                var model = new IndDataModel
                {
                    _id = Item._id.ToString(),
                    FormLink = Item.FormLink,
                    FormNumber = Item.FormNumber,
                    Name = Item.Name,
                    Number = Item.Number,
                    ReportingPeriod = Item.ReportingPeriod,
                    TabName = Item.TabName,
                    Datas = dataList,
                };
                returnList.Add(model);
            }
            return returnList;
        }

        public async Task<IndDataModel> GetIndustryDataCols(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var builder = Builders<IndDataDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = DBContext.Instance;
            var cursor = await db.IndDataDB.FindAsync(filter);
            IList<IndDataDB> results = cursor.ToList();

            var Item = results[0];
            var dataList = new List<DataCols>();

            int idx = 1;
            foreach (var cols in Item.Datas)
            {
                var data = _id + "|" + cols.Code + "|" + cols.ColCode + "|" + cols.ColValue + "|" + cols.Description;
                var colModel = new DataCols
                {
                    Index = idx,
                    Code = cols.Code,
                    ColCode = cols.ColCode,
                    ColValue = cols.ColValue,
                    Description = cols.Description,
                    Data = data,
                    _id = _id
                };
                dataList.Add(colModel);
                idx++;
            }

            var model = new IndDataModel
            {
                _id = Item._id.ToString(),
                FormLink = Item.FormLink,
                FormNumber = Item.FormNumber,
                Name = Item.Name,
                Number = Item.Number,
                ReportingPeriod = Item.ReportingPeriod,
                TabName = Item.TabName,
                Datas = dataList,
            };

            return model;
        }

        public async Task<string> UpdateIndustryDataCols(IndDataModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var filter = Builders<IndDataDB>.Filter.Eq(x => x._id, RecordId);

            var db = DBContext.Instance;

            var updatemodel = await GetIndustryDataCols(model._id);

            var dataList = new List<DataColsDB>();

            foreach (var Item in model.Datas)
            {
                var dModel = new DataColsDB
                {
                    Code = Item.Code,
                    ColCode = Item.ColCode,
                    ColValue = Item.ColValue,
                    Description = Item.Description
                };
                dataList.Add(dModel);
            }

            var modelDB = new IndDataDB
            {
                _id = RecordId,
                Datas = dataList,
                FormLink = updatemodel.FormLink,
                FormNumber = updatemodel.FormNumber,
                Month = updatemodel.Month,
                Name = updatemodel.Name,
                Number = updatemodel.Number,
                ReportingPeriod = updatemodel.ReportingPeriod,
                TabName = updatemodel.TabName,
                UploadDate = DateTime.Now,
                Year = model.Year

            };

            await db.IndDataDB.FindOneAndReplaceAsync(filter, modelDB);

            return modelDB._id.ToString();

        }

        public async Task<List<IndDataDB>> GetData(IndustryReportBuilterModel model)
        {
           
            IMongoClient client = DBContext.Instance._database.Client;
            IMongoDatabase database = client.GetDatabase("ISSB_DATA");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("IndDataDB");

            var options = new AggregateOptions()
            {
                AllowDiskUse = true
            };

            var bYear = new BsonArray();
            var yr = (from std in model.YM
                      select std.Year)
                        .Distinct().ToList();

            foreach (var Item in yr)
            {
                bYear.Add(new BsonInt64(Item));
            }

            var bMonth = new BsonArray();
            var mt = (from std in model.YM
                      select std.Month)
                        .Distinct().ToList();

            foreach (var Item in mt)
            {
                bMonth.Add(new BsonInt64(Item));
            }

            var bTariff = new BsonArray();
            
            foreach (var Item in model.TariffCodes)
            {
                bTariff.Add(new BsonString(Item));
            }

            var bNumber = new BsonArray();

            foreach (var Item in model.FormNumber.OrderBy(x => x.Number).ToList())
            {
                bNumber.Add(new BsonString(Item.Number.TrimEnd().ToString()));
            }

            //IDictionary<string, string> numberNames = new Dictionary<string, string>();
            //foreach (var Item in model.FormNumber)
            //{
            //    numberNames.Add("Number",Item.Number);
            //}


            var bForms = new BsonArray();

            foreach (var Item in model.FormNumber)
            {
                bForms.Add(new BsonString(Item.Form));
            }

            var bCols = new BsonArray();

            foreach (var Item in model.ColNumber)
            {
                bCols.Add(new BsonString(Item));
            }

            if (model.IsTariff)
            {
                PipelineDefinition<BsonDocument, BsonDocument> pipeline = new BsonDocument[]
                {
                new BsonDocument("$match", new BsonDocument()
                        .Add("Year", new BsonDocument()
                                .Add("$in", bYear)
                        )
                        .Add("Month", new BsonDocument()
                                .Add("$in", bMonth)
                        )),

                    new BsonDocument("$match", new BsonDocument()

                        .Add("Number", new BsonDocument()
                                    .Add("$in", bNumber)
                    )),
                   new BsonDocument("$match", new BsonDocument()
                    .Add("FormNumber", new BsonDocument()
                            .Add("$in", bForms)
                    )),

                    new BsonDocument("$match", new BsonDocument()
                            .Add("Datas.Code", new BsonDocument()
                                    .Add("$in", bTariff)
                            )),
                    new BsonDocument("$match", new BsonDocument()
                            .Add("Datas.ColCode", new BsonDocument()
                            .Add("$in", bCols)
                    ))
               };

                var data = new List<IndDataDB>();
                using (var cursor = await collection.AggregateAsync(pipeline, options))
                {

                    while (await cursor.MoveNextAsync())
                    {
                        var batch = cursor.Current;
                        foreach (BsonDocument document in batch)
                        {
                            var mod = BsonSerializer.Deserialize<IndDataDB>(document);
                            var nDatas = new List<DataColsDB>();
                            var newList = new List<DataColsDB>();

                            int cnt = 0;
                            foreach (var Item in mod.Datas)
                            {

                                var isTariff = model.TariffCodes.FirstOrDefault(q => q == Item.Code);
                                var isCol = model.ColNumber.FirstOrDefault(q => q == Item.ColCode);
                                bool nPost = false;
                                if (isCol != null)
                                {
                                    nPost = true;
                                }

                                if ((isTariff != null) || (isCol != null))
                                {
                                    if (nPost)
                                        nDatas.Add(Item);
                                }

                                cnt++;
                            }

                            mod.Datas = nDatas;

                            if (mod.Datas.Count > 0)
                            {
                                data.Add(mod);
                            }
                            // Console.WriteLine(document.ToJson());
                        }
                    }
                }

                return data;
            }
            else
            {
                PipelineDefinition<BsonDocument, BsonDocument> pipeline = new BsonDocument[]
                {
                new BsonDocument("$match", new BsonDocument()
                        .Add("Year", new BsonDocument()
                                .Add("$in", bYear)
                        )
                        .Add("Month", new BsonDocument()
                                .Add("$in", bMonth)
                        )),

                    new BsonDocument("$match", new BsonDocument()

                        .Add("Number", new BsonDocument()
                                    .Add("$in", bNumber)
                    )),
                   new BsonDocument("$match", new BsonDocument()
                    .Add("FormNumber", new BsonDocument()
                            .Add("$in", bForms)
                    )),

                    //new BsonDocument("$match", new BsonDocument()
                    //        .Add("Datas.Code", new BsonDocument()
                    //                .Add("$in", bTariff)
                    //        )),
                    new BsonDocument("$match", new BsonDocument()
                            .Add("Datas.ColCode", new BsonDocument()
                            .Add("$in", bCols)
                    ))
               };

                var data = new List<IndDataDB>();
                using (var cursor = await collection.AggregateAsync(pipeline, options))
                {

                    while (await cursor.MoveNextAsync())
                    {
                        var batch = cursor.Current;
                        foreach (BsonDocument document in batch)
                        {
                            var mod = BsonSerializer.Deserialize<IndDataDB>(document);
                            var nDatas = new List<DataColsDB>();
                            var newList = new List<DataColsDB>();

                            int cnt = 0;
                            foreach (var Item in mod.Datas)
                            {

                                var isTariff = model.TariffCodes.FirstOrDefault(q => q == Item.Code);
                                var isCol = model.ColNumber.FirstOrDefault(q => q == Item.ColCode);
                                bool nPost = false;
                                if (isCol != null)
                                {
                                    nPost = true;
                                }

                                if ((isTariff != null) || (isCol != null))
                                {
                                    if (nPost)
                                        nDatas.Add(Item);
                                }

                                cnt++;
                            }

                            mod.Datas = nDatas;

                            if (mod.Datas.Count > 0)
                            {
                                data.Add(mod);
                            }
                            // Console.WriteLine(document.ToJson());
                        }
                    }
                }

                return data;
            }
           
        }

        public async Task<List<IndDataOutModel>> GetFormsByDateRange(IndustryReportBuilterModel model)
        {

            IList<IndDataDB> results;

            var builder = Builders<IndDataDB>.Filter;
            var filter = builder.In("FormNumber", model.FormNumber);
           // & builder.In("Datas.ColCode", model.ColNumber);

            var db = DBContext.Instance;
            var cursor = await db.IndDataDB.FindAsync(filter);
            results = cursor.ToList();
         

            var Items = results;
            var dataList = new List<IndDataOutModel>();

            foreach (var Item in Items)
            {
                var dColsList = new List<DataCols>();
                foreach (var dCol in Item.Datas)
                {
                    var dModel = new DataCols
                    {
                        Code = dCol.Code,
                        ColCode = dCol.ColCode,
                        ColValue = dCol.ColValue,
                        Description = dCol.Description
                    };
                    dColsList.Add(dModel);
                }

                var dataCols = dColsList.Where(x => model.ColNumber.Contains(x.ColCode)).ToList();

                if (dataCols.Count > 0)
                {
                    var rModel = new IndDataOutModel
                    {
                        // _id = Item._id.ToString(),
                        // FormLink = Item.FormLink,
                        FormNumber = Item.FormNumber,
                        Name = Item.Name,
                        Number = Item.Number,
                        ReportingPeriod = Item.ReportingPeriod,
                        TabName = Item.TabName,
                        Datas = dataCols
                    };
                    dataList.Add(rModel);
                }
            }

            

            return dataList;
        }

        public async Task<List<SelectListItem>> GetFormNumbers()
        {
            var ReturnList = new List<IndustryFormNumbersModel>();
            IMongoClient client = DBContext.Instance._database.Client;
            IMongoDatabase database = client.GetDatabase("ISSB_DATA");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("IndDataDB");

            var options = new AggregateOptions()
            {
                AllowDiskUse = true
            };

            PipelineDefinition<BsonDocument, BsonDocument> pipeline = new BsonDocument[]
            {
                new BsonDocument("$group", new BsonDocument()
                        .Add("_id", new BsonDocument()
                                .Add("Name", "$Name")
                                .Add("FormNumber", "$FormNumber")
                        )),
                new BsonDocument("$project", new BsonDocument()
                        .Add("FormNumber", "$_id.FormNumber")
                        .Add("Name", "$_id.Name")
                        .Add("_id", 0)),
                new BsonDocument("$group", new BsonDocument()
                        .Add("_id", BsonNull.Value)
                        .Add("distinct", new BsonDocument()
                                .Add("$addToSet", "$$ROOT")
                        )),
                new BsonDocument("$unwind", new BsonDocument()
                        .Add("path", "$distinct")
                        .Add("preserveNullAndEmptyArrays", new BsonBoolean(false))),
                new BsonDocument("$replaceRoot", new BsonDocument()
                        .Add("newRoot", "$distinct")),
                new BsonDocument("$sort", new BsonDocument()
                        .Add("FormNumber", 1))
            };

            using (var cursor = await collection.AggregateAsync(pipeline, options))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        Console.WriteLine(document.ToJson());

                        var json = document.ToJson();

                        IndustryFormNumbersModel deserializedCountry = JsonConvert.DeserializeObject<IndustryFormNumbersModel>(json);
                        ReturnList.Add(deserializedCountry);

                    }
                }
            }
            var outList = new List<SelectListItem>();

            foreach (var Item in ReturnList)
            {
                var modelLocation = new SelectListItem
                {
                    Value = Item.FormNumber + "-" + Item.Name,
                    Text = Item.FormNumber + " - " + Item.Name,
                    Selected = false
                };
                outList.Add(modelLocation);
            }

            return outList;
        }

        public async Task<List<IndustryFormNumbersModel>> GetForms()
        {
            var ReturnList = new List<IndustryFormNumbersModel>();
            IMongoClient client = DBContext.Instance._database.Client;
            IMongoDatabase database = client.GetDatabase("ISSB_DATA");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("IndDataDB");

            var options = new AggregateOptions()
            {
                AllowDiskUse = true
            };

            PipelineDefinition<BsonDocument, BsonDocument> pipeline = new BsonDocument[]
            {
                new BsonDocument("$group", new BsonDocument()
                        .Add("_id", new BsonDocument()
                                .Add("Number", "$Number")
                                .Add("Name", "$Name")
                                .Add("FormNumber", "$FormNumber")
                        )),
                new BsonDocument("$project", new BsonDocument()
                        .Add("Number", "$_id.Number")
                        .Add("FormNumber", "$_id.FormNumber")
                        .Add("Name", "$_id.Name")
                        .Add("_id", 0)),
                new BsonDocument("$group", new BsonDocument()
                        .Add("_id", BsonNull.Value)
                        .Add("distinct", new BsonDocument()
                                .Add("$addToSet", "$$ROOT")
                        )),
                new BsonDocument("$unwind", new BsonDocument()
                        .Add("path", "$distinct")
                        .Add("preserveNullAndEmptyArrays", new BsonBoolean(false))),
                new BsonDocument("$replaceRoot", new BsonDocument()
                        .Add("newRoot", "$distinct")),
                new BsonDocument("$sort", new BsonDocument()
                        .Add("FormNumber", 1))
            };

            using (var cursor = await collection.AggregateAsync(pipeline, options))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        Console.WriteLine(document.ToJson());

                        var json = document.ToJson();

                        IndustryFormNumbersModel deserializedCountry = JsonConvert.DeserializeObject<IndustryFormNumbersModel>(json);
                        ReturnList.Add(deserializedCountry);

                    }
                }
            }
            var outList = new List<IndustryFormNumbersModel>();

            foreach (var Item in ReturnList)
            {
                var modelLocation = new IndustryFormNumbersModel
                {
                    FormNumber = Item.FormNumber,
                    Name = Item.Name,
                    Number = Item.Number
                };
                outList.Add(modelLocation);
            }

            return outList;
        }

        /// <summary>
        /// Update to Month Year
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public async Task<bool> GetIndustryDataColsAll()
        {
        
            var db = DBContext.Instance;
            var cursor = await db.IndDataDB.FindAsync(new BsonDocument());
            IList<IndDataDB> results = cursor.ToList();
            var modelList = new List<DataCols>();

            foreach (var Item in results)
            {
                var filter = Builders<IndDataDB>.Filter.Eq(x => x._id, Item._id);
                Item.Month = Item.ReportingPeriod.Month;
                Item.Year = Item.ReportingPeriod.Year;

                await db.IndDataDB.FindOneAndReplaceAsync(filter,Item);
                //var dataList = new List<DataCols>();

                //foreach (var cols in Item.Datas)
                //{
                //    var colModel = new DataCols
                //    {
                //        Code = cols.Code,
                //        ColCode = cols.ColCode,
                //        ColValue = cols.ColValue,
                //        Description = cols.Description
                //    };
                //    dataList.Add(colModel);
                //}

                //var model = new IndDataModel
                //{
                //    _id = Item._id.ToString(),
                //    FormLink = Item.FormLink,
                //    FormNumber = Item.FormNumber,
                //    Name = Item.Name,
                //    Number = Item.Number,
                //    ReportingPeriod = Item.ReportingPeriod,
                //    TabName = Item.TabName,
                //    Datas = dataList,
                //    Year = Item.ReportingPeriod.Year,
                //    Month = Item.ReportingPeriod.Month
                //};

            }
            return true;
        }

        public async Task<bool> AddReport(List<IndDataOutDB> model)
        {
            bool bReturn = false;
            var db = DBContext.Instance;

            await db.IndDataOutDB.InsertManyAsync(model);
            bReturn = true;
            return bReturn;
        }

   

        public async Task<bool> SaveReport(IndustrySavedReportsDB model)
        {
           // BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var builder = Builders<IndustrySavedReportsDB>.Filter;
            var filter = builder.Eq("ReportName", model.ReportName);

            var db = DBContext.Instance;
            var cursor = await db.IndustrySavedReportsDB.FindAsync(filter);
            IList<IndustrySavedReportsDB> results = cursor.ToList();

            if (results.Count == 0)
            {
                await db.IndustrySavedReportsDB.InsertOneAsync(model);
            }
            else
            {
                model._id = results[0]._id;
                await db.IndustrySavedReportsDB.ReplaceOneAsync(filter, model);   
            }
            return true;
        }


        public async Task<List<IndustrySavedReportsModel>> GetReports()
        {
            var db = DBContext.Instance;
            var cursor = await db.IndustrySavedReportsDB.FindAsync(new BsonDocument());
            IList<IndustrySavedReportsDB> results = cursor.ToList();
            var modelList = new List<IndustrySavedReportsModel>();

            foreach (var Item in results)
            {
                
                var formsList = new List<IndFormsModel>();

                foreach(var form in Item.Forms)
                {
                    var fModel = new IndFormsModel
                    {
                        Form = form.Form,
                        Name = form.Name,
                        Number = form.Number
                    };

                    formsList.Add(fModel);
                }

                var model = new IndustrySavedReportsModel
                {
                    _id = Item._id.ToString(),
                    Date = Item.Date.ToLocalTime(),
                    ReportName = Item.ReportName,
                    ReportType = Item.ReportType,
                    SaveReport = Item.SaveReport,
                    Forms = formsList,
                    SourceTime = Item.SourceTime
                };

                modelList.Add(model);
            }
            return modelList;
        }


        public async Task<IndustrySavedReportsModel> GetReport(string id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(id));
            var builder = Builders<IndustrySavedReportsDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var db = DBContext.Instance;
            var cursor = await db.IndustrySavedReportsDB.FindAsync(filter);
            IList<IndustrySavedReportsDB> results = cursor.ToList();
       
            var Item = results[0];
            var formsList = new List<IndFormsModel>();
            var tcodeList = new List<string>();
            var colsList = new List<string>();

            foreach (var form in Item.Forms)
            {
                var fModel = new IndFormsModel
                {
                    Form = form.Form,
                    Name = form.Name,
                    Number = form.Number
                };

                formsList.Add(fModel);
            }

            foreach (var code in Item.TCodes)
            {
                tcodeList.Add(code);
            }

            foreach (var code in Item.ReoprtCols)
            {
                colsList.Add(code);
            }

            var model = new IndustrySavedReportsModel
            {
                _id = Item._id.ToString(),
                Date = Item.Date.ToLocalTime(),
                ReportName = Item.ReportName,
                ReportType = Item.ReportType,
                SaveReport = Item.SaveReport,
                Forms = formsList,
                TCodes = tcodeList,
                ReoprtCols = colsList,
                ProductType = Item.ProductType,
                SourceTime = Item.SourceTime
            };

            return model;
        }
    }
}
