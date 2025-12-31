using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Services
{
    public class IndustryDataService
    {
        private static IndustryDataService _instance;

        public static IndustryDataService Instance
        {
            set { }
            get
            {
                if (_instance == null)
                {
                    _instance = new IndustryDataService();
                }
                return _instance;
            }
        }

        public IndustryDataService() { }

        public async Task<List<IndustryColumnModel>> GetColumns()
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
            return modelList;
        }

        public async Task<bool> AddCols(IndustryColumnModel model)
        {
            var db = DBContext.Instance;

            var modelDB = new IndustryColumnDB
            {
                Name = model.Name,
                Number = model.Number
            };

            await db.IndustryColumnDB.InsertOneAsync(modelDB);
            return true;
        }

        public async Task<bool> SaveCol(IndustryColumnModel model)
        {
            bool bReturn = false;

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var filter = Builders<IndustryColumnDB>.Filter.Eq(x => x._id, RecordId);
            var db = DBContext.Instance;
            var cursor = await db.IndustryColumnDB.FindAsync(filter);
            IList<IndustryColumnDB> results = cursor.ToList();
            var modelDB = new IndustryColumnDB
            {
                _id = results[0]._id,

                Name = model.Name,
                Number = model.Number

            };
            await db.IndustryColumnDB.FindOneAndReplaceAsync(filter, modelDB);
            bReturn = true;
            return bReturn;
        }

        public async Task<IndustryColumnModel> GetCol(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var db = DBContext.Instance;
            var builder = Builders<IndustryColumnDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var cursor = await db.IndustryColumnDB.FindAsync(filter);
            IList<IndustryColumnDB> results = cursor.ToList();

            var model = new IndustryColumnModel
            {
                Name = results[0].Name,
                Number = results[0].Number,
                _id = results[0]._id.ToString()
            };

            return model;
        }

        public async Task<FileUploadTypesModel> GetSource(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var db = DBContext.Instance;
            var builder = Builders<FileUploadTypesDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var cursor = await db.FileUploadTypesDB.FindAsync(filter);
            IList<FileUploadTypesDB> results = cursor.ToList();

            var model = new FileUploadTypesModel
            {
               Description = results[0].Description,
                Number = results[0].Number,
                _id = results[0]._id.ToString()
            };

            return model;
        }

        public async Task<bool> DeleteCol(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var db = DBContext.Instance;
            var builder = Builders<IndustryColumnDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var returnModel = await db.IndustryColumnDB.DeleteOneAsync(filter);
            return true;
        }

        public async Task<List<IndustryRowModel>> GetRows()
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
            return modelList;
        }


        public async Task<bool> AddRows(IndustryRowModel model)
        {
            var db = DBContext.Instance;

            var modelDB = new IndustryRowDB
            {
                Description = model.Description,
                Number = model.Number
            };

            await db.IndustryRowDB.InsertOneAsync(modelDB);
            return true;
        }

        public async Task<bool> SaveRow(IndustryRowModel model)
        {
            bool bReturn = false;

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var filter = Builders<IndustryRowDB>.Filter.Eq(x => x._id, RecordId);
            var db = DBContext.Instance;
            var cursor = await db.IndustryRowDB.FindAsync(filter);
            IList<IndustryRowDB> results = cursor.ToList();
            var modelDB = new IndustryRowDB
            {
                _id = results[0]._id,

                Description = model.Description,
                Number = model.Number

            };
            await db.IndustryRowDB.FindOneAndReplaceAsync(filter, modelDB);
            bReturn = true;
            return bReturn;
        }

        public async Task<bool> SaveSource(FileUploadTypesModel model)
        {
            bool bReturn = false;

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var filter = Builders<FileUploadTypesDB>.Filter.Eq(x => x._id, RecordId);
            var db = DBContext.Instance;
            var cursor = await db.FileUploadTypesDB.FindAsync(filter);
            IList<FileUploadTypesDB> results = cursor.ToList();
            var modelDB = new FileUploadTypesDB
            {
                _id = results[0]._id,

                Description = model.Description,
                Number = model.Number

            };
            await db.FileUploadTypesDB.FindOneAndReplaceAsync(filter, modelDB);
            bReturn = true;
            return bReturn;
        }

        public async Task<IndustryRowModel> GetRow(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var db = DBContext.Instance;
            var builder = Builders<IndustryRowDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var cursor = await db.IndustryRowDB.FindAsync(filter);
            IList<IndustryRowDB> results = cursor.ToList();

            var model = new IndustryRowModel
            {
                Description = results[0].Description,
                Number = results[0].Number,
                _id = results[0]._id.ToString()
            };

            return model;
        }

        public async Task<bool> DeleteRow(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var db = DBContext.Instance;
            var builder = Builders<IndustryRowDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var returnModel = await db.IndustryRowDB.DeleteOneAsync(filter);
            return true;
        }

        public async Task<bool> DeleteSource(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));
            var db = DBContext.Instance;
            var builder = Builders<FileUploadTypesDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var returnModel = await db.FileUploadTypesDB.DeleteOneAsync(filter);
            return true;
        }
    }
}
