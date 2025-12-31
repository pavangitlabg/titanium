using System;
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
    public class RegisterService
    {
        public async Task<List<RegisterModel>> GetAllRegister()
        {
            var db = new DBContext();
            var cursor = await db.RegisterDB.FindAsync(new BsonDocument());

            IList<RegisterDB> results = cursor.ToList();
            var modelList = new List<RegisterModel>();

            foreach (var Item in results)
            {
                var model = new RegisterModel
                {
                    _id = Item._id.ToString(),
                    RegisterID = Item.RegisterID,
                    FirstName = Item.FirstName,
                    Lastname = Item.Lastname,
                    Company = Item.Company,
                    Info = Item.Info,
                    Address1 = Item.Address1,
                    Address2 = Item.Address2,
                    City = Item.City,
                    State = Item.State,
                    Zip = Item.Zip,
                    Phone = Item.Phone,
                    Email = Item.Email,
                    Comment = Item.Comment,
                    RegisDate = (DateTime)Item.RegisDate,
                    Status = Item.Status,
                    Username = Item.Username,
                    Password = Item.Password,
                    ConfirmPassword = Item.ConfirmPassword,
                    Country = Item.Country
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }    
         
        public async Task<RegisterModel> GetRegisterById(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<RegisterDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.RegisterDB.FindAsync(filter);

            IList<RegisterDB> results = cursor.ToList();

            var Item = results[0];

            var model = new RegisterModel
            {
                _id = Item._id.ToString(),
                RegisterID = Item.RegisterID,
                FirstName = Item.FirstName,
                Lastname = Item.Lastname,
                Company = Item.Company,
                Info = Item.Info,
                Address1 = Item.Address1,
                Address2 = Item.Address2,
                City = Item.City,
                State = Item.State,
                Zip = Item.Zip,
                Phone = Item.Phone,
                Email = Item.Email,
                Comment = Item.Comment,
                RegisDate = (DateTime)Item.RegisDate,
                Status = Item.Status,
                Username = Item.Username,
                Password = Item.Password,
                ConfirmPassword = Item.ConfirmPassword,
                Country = Item.Country

            };

            return model;
        }

        public async Task<bool> Add(RegisterModel model)
        {
            var db = new DBContext();

            var modelDB = new RegisterDB
            {
                RegisterID = model.RegisterID,
                FirstName = model.FirstName,
                Lastname = model.Lastname,
                Company = model.Company,
                Info = model.Info,
                Address1 = model.Address1,
                Address2 = model.Address2,
                City = model.City,
                State = model.State,
                Zip = model.Zip,
                Phone = model.Phone,
                Email = model.Email,
                Comment = model.Comment,
                RegisDate = model.RegisDate,
                Status = model.Status,
                Username = model.Username,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                Country = model.Country

            };

            await db.RegisterDB.InsertOneAsync(modelDB);

            return true;
        }

        public async Task<RegisterDB> Save(RegisterModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<RegisterDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.RegisterDB.FindAsync(filter);
            IList<RegisterDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new RegisterDB
            {
                _id = Item._id,
                RegisterID = model.RegisterID,
                FirstName = model.FirstName,
                Lastname = model.Lastname,
                Company = model.Company,
                Info = model.Info,
                Address1 = model.Address1,
                Address2 = model.Address2,
                City = model.City,
                State = model.State,
                Zip = model.Zip,
                Phone = model.Phone,
                Email = model.Email,
                Comment = model.Comment,
                RegisDate = model.RegisDate,
                Status = model.Status,
                Username = model.Username,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                Country = model.Country

            };

            var returnModel = await db.RegisterDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;

        }

        public async Task<bool> Delete(RegisterModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<RegisterDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.RegisterDB.DeleteOneAsync(filter);

            return true;
        }

        public async Task<int> GetCount()
        {

            var db = new DBContext();
            var cursor = await db.RegisterDB.FindAsync(new BsonDocument());

            IList<RegisterDB> results = cursor.ToList();

            return results.Count;
        }

        public async Task<bool> DoesExist(RegisterModel model)
        {
            bool bReturn = false;
            var builder = Builders<RegisterDB>.Filter;
            var filter = builder.Eq("_id", model.RegisterID);

            var db = new DBContext();

            var cursor = await db.RegisterDB.FindAsync(filter);
            IList<RegisterDB> results = cursor.ToList();
            if (results.Count > 0)
                bReturn = true;

            return bReturn;
        }       

        
     }
}
