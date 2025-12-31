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

namespace Services
{
    public class ImportFileTypeService
    {
        public async Task<List<ImportFileTypeModel>> GetFileTypes()
        {

            ImportFileTypeModel model;

            var db = new DBContext();
            var cursor = await db.ImportFileTypeDB.FindAsync(new BsonDocument());

            IList<ImportFileTypeDB> results = cursor.ToList();
            var modelList = new List<ImportFileTypeModel>();

            foreach (var item in results)
            {
                model = new ImportFileTypeModel
                {
                    _id = item._id.ToString(),
                    KEY =item.KEY,
                    NAME = item.NAME
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x.KEY).ThenBy(s => s.NAME).ToList();
        }

        public async Task<ImportFileTypeDB> Save(ImportFileTypeModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<ImportFileTypeDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.ImportFileTypeDB.FindAsync(filter);
            IList<ImportFileTypeDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new ImportFileTypeDB
            {
                _id = Item._id,
                KEY = model.KEY,
                NAME = model.NAME
               
            };

            var returnModel = await db.ImportFileTypeDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;

        }

        public async Task<bool> Delete(ImportFileTypeModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<ImportFileTypeDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.ImportFileTypeDB.DeleteOneAsync(filter);

            return true;
        }

        public async Task<ImportFileTypeModel> GeTypetById(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<ImportFileTypeDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.ImportFileTypeDB.FindAsync(filter);

            IList<ImportFileTypeDB> results = cursor.ToList();

            var Item = results[0];

            var model = new ImportFileTypeModel
            {
                _id = Item._id.ToString(),
                KEY = Item.KEY,
                NAME = Item.NAME
            };

            return model;
        }

        public async Task<List<ImportFileTypeModel>> GetFileTypesByKey(string Key)
        {
           
            var builder = Builders<ImportFileTypeDB>.Filter;
            var filter = builder.Eq("KEY", Key);

            ImportFileTypeModel model;

            var db = new DBContext();
            var cursor = await db.ImportFileTypeDB.FindAsync(filter);

            IList<ImportFileTypeDB> results = cursor.ToList();
            var modelList = new List<ImportFileTypeModel>();

            foreach (var item in results)
            {
                model = new ImportFileTypeModel
                {
                    _id = item._id.ToString(),
                    KEY = item.KEY,
                    NAME = item.NAME
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x.NAME).ToList();
        }

        public async Task<IEnumerable<SelectListItem>> GetFileTypesDropdown()
        {
            var db = new DBContext();
            var cursor = await db.ImportFileTypeDB.FindAsync(new BsonDocument());

            IList<ImportFileTypeDB> results = cursor.ToList();
            //var modelList = new List<ImportFileTypeDB>();

            List<SelectListItem> FileTypeitems = new List<SelectListItem>();


            foreach (var Item in results)
            {
                var modelLocation = new SelectListItem
                {
                    Value = Item.NAME,
                    Text = Item.KEY
                };
                FileTypeitems.Add(modelLocation);
            }

            return FileTypeitems;
        }

        public async Task<bool> Add(ImportFileTypeModel model)
        {
            bool bReturn;
          
            var db = new DBContext();

            var modelDB = new ImportFileTypeDB
            {
               
               KEY = model.KEY,
               NAME = model.NAME

            };

            await db.ImportFileTypeDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }
    }
}
