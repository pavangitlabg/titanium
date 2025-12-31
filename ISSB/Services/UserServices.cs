using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
    public class UserServices
    {
        public async Task<UserModel> LoginUser(string email, string password)
        {
         
            password = EncryptionHelper.Encrypt(password);

            //TODO Bug of the password encryption
            var filter = Builders<UserDB>.Filter.Eq(x => x.Email, email) & Builders<UserDB>.Filter.Eq(z => z.Password, password);

            var db = new DBContext();
            var cursor = await db.UserDB.FindAsync(filter);

            IList<UserDB> results = cursor.ToList();

            if (results.Count == 0)
            {
                var rModel = new UserModel { IsLoggedIn = false };
                return rModel;
            }

            var ImgURL = string.Empty;
            if (string.IsNullOrEmpty(results[0].ImageURL))
                ImgURL = "../userimages/default.png";

            var LoggedInUser = results[0];
            var model = new UserModel
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
                //AllowedMarketCountries = LoggedInUser.AllowedMarketCountries,
                //AllowedSourceCountries = LoggedInUser.AllowedSourceCountries,
                //AllowedTariffCodes = LoggedInUser.AllowedTariffCodes,
                CompanyName = LoggedInUser.CompanyName,
                Costs = LoggedInUser.Costs,
                DateJoined = LoggedInUser.DateJoined,
                ExpiryDate = LoggedInUser.ExpiryDate,
                FirstName = LoggedInUser.FirstName,
                IsTradeInquiry = LoggedInUser.IsTradeInquiry,
                LastName = LoggedInUser.LastName,
                Mobile = LoggedInUser.Mobile,
                Permissions = LoggedInUser.Permissions,
                PostalCode = LoggedInUser.PostalCode,
                StartDate = LoggedInUser.StartDate,
                Telephone = LoggedInUser.Telephone,
                TwoFactorAuth = LoggedInUser.TwoFactorAuth,
                ImageURL = ImgURL,
                IsAPI = LoggedInUser.IsAPI

            };

            return model;
        }

        public async Task<UserModel> LoginTokenUser(string email, string token)
        {

           // password = EncryptionHelper.Encrypt(password);

            //TODO Bug of the password encryption
            var filter = Builders<UserDB>.Filter.Eq(x => x.Email, email) & Builders<UserDB>.Filter.Eq(z => z.Password, token);

            var db = new DBContext();
            var cursor = await db.UserDB.FindAsync(filter);

            IList<UserDB> results = cursor.ToList();

            if (results.Count == 0)
            {
                var rModel = new UserModel { IsLoggedIn = false };
                return rModel;
            }

            var ImgURL = string.Empty;
            if (string.IsNullOrEmpty(results[0].ImageURL))
                ImgURL = "../userimages/default.png";

            var LoggedInUser = results[0];
            var model = new UserModel
            {
                _id = LoggedInUser._id.ToString(),
                Email = LoggedInUser.Email,
                IsLoggedIn = true,
               // Password = EncryptionHelper.Decrypt(LoggedInUser.Password),
                UserName = LoggedInUser.UserName,
                IsAdministrator = LoggedInUser.IsAdministrator,
                IsSystemUser = LoggedInUser.IsSystemUser,
                IsAdvancedUser = LoggedInUser.IsAdvancedUser,
                AccountStatus = LoggedInUser.AccountStatus,
                Address1 = LoggedInUser.Address1,
                Address2 = LoggedInUser.Address2,
                Address3 = LoggedInUser.Address3,
                Address4 = LoggedInUser.Address4,
                //AllowedMarketCountries = LoggedInUser.AllowedMarketCountries,
                //AllowedSourceCountries = LoggedInUser.AllowedSourceCountries,
                //AllowedTariffCodes = LoggedInUser.AllowedTariffCodes,
                CompanyName = LoggedInUser.CompanyName,
                Costs = LoggedInUser.Costs,
                DateJoined = LoggedInUser.DateJoined,
                ExpiryDate = LoggedInUser.ExpiryDate,
                FirstName = LoggedInUser.FirstName,
                IsTradeInquiry = LoggedInUser.IsTradeInquiry,
                LastName = LoggedInUser.LastName,
                Mobile = LoggedInUser.Mobile,
                Permissions = LoggedInUser.Permissions,
                PostalCode = LoggedInUser.PostalCode,
                StartDate = LoggedInUser.StartDate,
                Telephone = LoggedInUser.Telephone,
                TwoFactorAuth = LoggedInUser.TwoFactorAuth,
                ImageURL = ImgURL,
                IsAPI = LoggedInUser.IsAPI

            };

            return model;
        }


        public async Task<UserModel> GetUser(string email)
        {
            var filter = Builders<UserDB>.Filter.Eq(x => x.Email, email);
            var db = new DBContext();
            var cursor = await db.UserDB.FindAsync(filter);
            IList<UserDB> results = cursor.ToList();

            if(results.Count == 0)
            {
                return new UserModel();
            }

            var ImgURL = string.Empty;
            if (string.IsNullOrEmpty(results[0].ImageURL))
                ImgURL = ImgURL = "../userimages/default.png";

            var model = new UserModel
            {
                _id = results[0]._id.ToString(),
                IsSystemUser = results[0].IsSystemUser,
                Email = results[0].Email,
                Password = results[0].Password,
                UserName = results[0].UserName,
                Permissions = results[0].Permissions,
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
                ImageURL = ImgURL,
                IsAPI = results[0].IsAPI


            };
            return model;
        }
        public async Task<List<UserModel>> GetUsers()
        {
            var db = new DBContext();
            var cursor = await db.UserDB.FindAsync(new BsonDocument());
            IList<UserDB> results = cursor.ToList();
            var modelList = new List<UserModel>();
            foreach (var Item in results)
            {
               
                if (string.IsNullOrEmpty(Item.ImageURL))
                    Item.ImageURL = "../userimages/default.png";

                var model = new UserModel
                {
                    _id = Item._id.ToString(),
                    Email = Item.Email,
                    Password = Item.Password,
                    UserName = Item.UserName,
                    Permissions = Item.Permissions,
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
                    ImageURL = Item.ImageURL,
                    AllowedYears = Item.AllowedYears,
                    IsAPI = Item.IsAPI
                };
                modelList.Add(model);
            }
            return modelList;
        }

        public async Task<UserColourModel> GetUserColours(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var builder = Builders<UserDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();
            var cursor = await db.UserDB.FindAsync(filter);
            IList<UserDB> results = cursor.ToList();
            var Item = results[0];

            var cModel = new UserColourModel { _id = Item._id.ToString(), BrandLink = Item.BrandLink, NavBar = Item.NavBar, SideBar = Item.SideBar };
            return cModel;
        }

        public async Task<UserModel> GetUsersById(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var builder = Builders<UserDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();
            var cursor = await db.UserDB.FindAsync(filter);
            IList<UserDB> results = cursor.ToList();
            var Item = results[0];

            var ScAllowed = new List<SourceCountryModel>();
            var TcAllowed = new List<ProductAllFilterModel>();
            var CfAllowed = new List<ProductCustomFilterModel>();

            if (Item.AllowedSourceCountries == null)
            {
                Item.AllowedSourceCountries = new List<SourceCountryDB>();
            }

            if (Item.AllowedTariffCodes == null)
            {
                Item.AllowedTariffCodes = new List<ProductAllFilterDB>();
            }

            if (Item.AllowedCustomFilters == null)
            {
                Item.AllowedCustomFilters = new List<ProductCustomFilterDB>();
            }

            foreach (var scItem in Item.AllowedSourceCountries)
            {
                var scModel = new SourceCountryModel
                {
                    _id = scItem._id.ToString(),
                    ACTIVE = scItem.ACTIVE,
                    CURRENCY_CODE = scItem.CURRENCY_CODE,
                    DATA_FORMAT = scItem.DATA_FORMAT,
                    FIRST_DATE = (DateTime)scItem.FIRST_DATE,
                    FREQUENCY = scItem.FREQUENCY,
                    GEO_CODE = scItem.GEO_CODE,
                    GEO_CODE_DISCONTINUED_DATE = (DateTime)scItem.GEO_CODE_DISCONTINUED_DATE,
                    INDUSTRY_REPORTING_CENTRE_ID = scItem.INDUSTRY_REPORTING_CENTRE_ID,
                    LATEST_DATE = (DateTime)scItem.LATEST_DATE,
                    LONG_LEGEND = scItem.LONG_LEGEND,
                    NAME = scItem.NAME,
                    NAME_OLD = scItem.NAME_OLD,
                    OLD_GEO_CODE = scItem.OLD_GEO_CODE,
                    REGION_NAME = scItem.REGION_NAME,
                    SHORT_LEGEND = scItem.SHORT_LEGEND,
                    SIDE_OF_TRADE = scItem.SIDE_OF_TRADE,
                    SOURCE_COUNTRY_ID = scItem.SOURCE_COUNTRY_ID,

                   
                };
                ScAllowed.Add(scModel);
            }

            foreach (var tcItem in Item.AllowedTariffCodes)
            {
                var tcModel = new ProductAllFilterModel
                {
                    //_id = tcItem._id.ToString(),
                    TariffCode = tcItem.TariffCode,
                    Name = tcItem.Name,
                    SortOrder = tcItem.SortOrder,
                    Idx = tcItem.Idx,
                    Cost = tcItem.Cost

                };
                TcAllowed.Add(tcModel);
            }

            foreach (var cfItem in Item.AllowedCustomFilters)
            {
                var cfModel = new ProductCustomFilterModel
                {
                    _id = cfItem._id.ToString(),
                    SortOrder = cfItem.SortOrder,
                    Name = cfItem.Name
                };
                CfAllowed.Add(cfModel);
            }

            if (string.IsNullOrEmpty(Item.ImageURL))
                Item.ImageURL = "../userimages/default.png";

            var model = new UserModel
            {
                _id = results[0]._id.ToString(),
                Email = Item.Email,
                Password = EncryptionHelper.Decrypt(Item.Password),
                UserName = Item.UserName,
                //Permissions = Item.Permissions,
                DateJoined = Item.DateJoined,
                IsAdministrator = Item.IsAdministrator,
                IsTradeInquiry = Item.IsTradeInquiry,
                IsSystemUser = Item.IsSystemUser,
                IsAdvancedUser = Item.IsAdvancedUser,
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
                ImageURL = Item.ImageURL,
                NavBar = Item.NavBar,
                BrandLink = Item.BrandLink,
                SideBar = Item.SideBar,
                AllowedYears = Item.AllowedYears,
                IsAPI = Item.IsAPI

            };

            model.AllowedSourceCountries = ScAllowed.OrderBy(x => x.GEO_CODE).ToList();
            model.AllowedTariffCodes = TcAllowed.OrderBy(x => x.TariffCode).ToList();
            model.AllowedCustomFilters = CfAllowed.OrderBy(x => x.SortOrder).ToList();
            return model;
        }
        public async Task<bool> DeleteUser(string email)
        {
            var filter = Builders<UserDB>.Filter.Eq(x => x.Email, email);
            var db = new DBContext();
            var cursor = await db.UserDB.FindAsync(filter);
            IList<UserDB> results = cursor.ToList();
            bool bReturn = false;
            if (!results[0].IsSystemUser)
            {
                await db.UserDB.FindOneAndDeleteAsync(filter);
                bReturn = true;
            }
            return bReturn;
        }
        public async Task<bool> Delete(UserModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();
            var builder = Builders<UserDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var returnModel = await db.UserDB.DeleteOneAsync(filter);
            return true;
        }

        public async Task<bool> UpdateUser(UserModel model)
        {
            bool bReturn = false;

            var cfAllowed = new List<ProductCustomFilterDB>();

            foreach (var Item in model.AllowedCustomFilters)
            {
                BsonObjectId RecordItemId = new BsonObjectId(new ObjectId(Item._id));
                var tcModel = new ProductCustomFilterDB
                {
                    _id = RecordItemId,
                    SortOrder = Item.SortOrder,
                     Name = Item.Name
                };
                cfAllowed.Add(tcModel);
            }

            var scAllowed = new List<SourceCountryDB>();
            

            foreach (var Item in model.AllowedSourceCountries)
            {
                var scModel = new SourceCountryDB
                {
                    ACTIVE = Item.ACTIVE,
                    CURRENCY_CODE = Item.CURRENCY_CODE,
                    DATA_FORMAT = Item.DATA_FORMAT,
                    FIRST_DATE = Item.FIRST_DATE,
                    FREQUENCY = Item.FREQUENCY,
                    GEO_CODE = Item.GEO_CODE,
                    GEO_CODE_DISCONTINUED_DATE = Item.GEO_CODE_DISCONTINUED_DATE,
                    INDUSTRY_REPORTING_CENTRE_ID = Item.INDUSTRY_REPORTING_CENTRE_ID,
                    LATEST_DATE = Item.LATEST_DATE,
                    LONG_LEGEND = Item.LONG_LEGEND,
                    NAME = Item.NAME,
                    NAME_OLD = Item.NAME_OLD,
                    OLD_GEO_CODE = Item.OLD_GEO_CODE,
                    REGION_NAME = Item.REGION_NAME,
                    SHORT_LEGEND = Item.SHORT_LEGEND,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    SOURCE_COUNTRY_ID = Item.SOURCE_COUNTRY_ID,
                  
                    _id = new BsonObjectId(new ObjectId(Item._id))
                };
                scAllowed.Add(scModel);
            }

            var tcAllowed = new List<ProductAllFilterDB>();

            foreach (var Item in model.AllowedTariffCodes)
            {
                var tcModel = new ProductAllFilterDB
                {
                    TariffCode = Item.TariffCode,
                    Name = Item.Name,
                    Cost = Item.Cost,
                    Idx = Item.Idx,
                    SortOrder = Item.SortOrder,
                };
                tcAllowed.Add(tcModel);
            }

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var filter = Builders<UserDB>.Filter.Eq(x => x._id, RecordId);
            var db = new DBContext();
            var cursor = await db.UserDB.FindAsync(filter);
            IList<UserDB> results = cursor.ToList();
            var modelDB = new UserDB
            {
                _id = results[0]._id,
                DateJoined = model.DateJoined,
                Email = model.Email,
                IsAdministrator = model.IsAdministrator,
                IsSystemUser = model.IsSystemUser,
                IsAdvancedUser = model.IsAdvancedUser,
                Permissions = model.Permissions,
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
                AllowedSourceCountries = scAllowed,
                AllowedTariffCodes = tcAllowed,
                EmailNotification = model.EmailNotification,
                ImageURL = model.ImageURL,
                BrandLink = model.BrandLink,
                NavBar = model.NavBar,
                SideBar = model.SideBar,
                AllowedYears = model.AllowedYears,
                AllowedCustomFilters = cfAllowed,
                IsAPI = model.IsAPI
            };
            await db.UserDB.FindOneAndReplaceAsync(filter, modelDB);
            bReturn = true;
            return bReturn;
        }

        public async Task<bool> DoesExist(UserModel model)
        {
            bool bReturn = false;
            var filter = Builders<UserDB>.Filter.Eq(x => x.Email, model.Email);
            var db = new DBContext();
            var cursor = await db.UserDB.FindAsync(filter);
            IList<UserDB> results = cursor.ToList();
            if (results.Count > 0)
                bReturn = true;
            return bReturn;
        }

        public async Task<bool> DoesEmailExist(string email)
        {
            bool bReturn = false;
            var filter = Builders<UserDB>.Filter.Eq(x => x.Email, email);
            var db = new DBContext();
            var cursor = await db.UserDB.FindAsync(filter);
            IList<UserDB> results = cursor.ToList();
            if (results.Count > 0)
                bReturn = true;
            return bReturn;
        }

        public async Task<bool> AddUser(UserModel model)
        {
            bool bReturn = false;
            var db = new DBContext();
            var modelDB = new UserDB
            {
                DateJoined = model.DateJoined,
                Email = model.Email,
                IsAdministrator = model.IsAdministrator,
                IsSystemUser = model.IsSystemUser,
                IsAdvancedUser = model.IsAdvancedUser,
                Permissions = model.Permissions,
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
                AllowedYears = model.AllowedYears,
                IsAPI = model.IsAPI
            };
            await db.UserDB.InsertOneAsync(modelDB);
            bReturn = true;
            return bReturn;
        }
        public async Task<bool> ChangePassword(string email, string password)
        {
            bool bReturn = false;
            var filter = Builders<UserDB>.Filter.Eq(x => x.Email, email);
            var db = new DBContext();
            var cursor = await db.UserDB.FindAsync(filter);
            IList<UserDB> results = cursor.ToList();
            if (results.Count > 0)
            {
                var newPassword = results[0];
                newPassword.Password = EncryptionHelper.Encrypt(password);
                await db.UserDB.FindOneAndReplaceAsync(filter, newPassword);
                bReturn = true;
            }
            return bReturn;
        }
        public async Task<int> GetCount()
        {
            var db = new DBContext();
            var cursor = await db.UserDB.FindAsync(new BsonDocument());
            IList<UserDB> results = cursor.ToList();
            return results.Count;
        }
    }
}